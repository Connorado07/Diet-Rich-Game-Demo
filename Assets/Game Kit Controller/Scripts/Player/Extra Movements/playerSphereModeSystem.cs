using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerSphereModeSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool sphereModeEnabled;

	public string defaultVehicleStateName = "Sphere";

	public List<vehicleInfo> vehicleInfoList = new List<vehicleInfo> ();

	vehicleInfo currentVehicleInfo;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool usingSphereMode;

	[Space]
	[Header ("Components")]
	[Space]

	public playerController playerManager;
	public gravitySystem gravityManager;
	public usingDevicesSystem usingDevicesManager;
	public Collider playerCollider;

	public overrideElementControlSystem mainOverrideElementControlSystem;

	GameObject currentObjectToControl;
	Collider sphereVehicleCollider;
	vehicleHUDManager vehicleManager;
	GameObject vehicleCamera;
	vehicleGravityControl vehicleGravityManager;
	IKDrivingSystem mainIKDrivingSystem;

	bool initialized;


	void checkCurrentVehicleInfo ()
	{
		if (currentVehicleInfo == null || !initialized) {
			initialized = true;

			setCurrentVehicleInfo (defaultVehicleStateName);
		}
	}

	public void setCurrentVehicleInfo (string newVehicleInfoName)
	{
		for (int i = 0; i < vehicleInfoList.Count; i++) {
			if (vehicleInfoList [i].Name.Equals (newVehicleInfoName)) {
				currentVehicleInfo = vehicleInfoList [i];

				if (!currentVehicleInfo.isCurrentVehicle) {
					if (currentVehicleInfo.useEventOnVehicleInfoState) {
						currentVehicleInfo.eventOnVehicleInfoState.Invoke ();
					}
				}

				currentVehicleInfo.isCurrentVehicle = true;
			} else {
				if (vehicleInfoList [i].isCurrentVehicle) {
					setSphereModeActiveState (false);
				}

				vehicleInfoList [i].isCurrentVehicle = false;
			}
		}
	}

	public void toggleSphereModeActiveState ()
	{
		setSphereModeActiveState (!usingSphereMode);
	}

	public void setSphereModeActiveState (bool state)
	{
		if (!sphereModeEnabled) {
			return;
		}
			
		if (playerManager.canUseSphereMode) {
			StartCoroutine (setVehicleState (state));
		}
	}

	IEnumerator setVehicleState (bool state)
	{
		checkCurrentVehicleInfo ();

		if (state) {
			if (currentVehicleInfo.controlWithOverrideSystem) {
				if (currentVehicleInfo.currentVehicleObject != null) {
					if (!mainOverrideElementControlSystem.checkIfTemporalObjectOnList (currentVehicleInfo.currentVehicleObject)) {
						currentVehicleInfo.currentVehicleObject = null;
					}
				}
			}

			if (currentVehicleInfo.currentVehicleObject == null) {
				currentVehicleInfo.currentVehicleObject = (GameObject)Instantiate (currentVehicleInfo.vehiclePrefab, Vector3.one * 1000, Quaternion.identity);

				yield return new WaitForSeconds (0.00001f);

				getCurrentVehicleComponents ();

				if (vehicleGravityManager != null) {
					vehicleGravityManager.pauseDownForce (true);
				}

				currentObjectToControl.SetActive (false);

				if (sphereVehicleCollider != null) {
					sphereVehicleCollider.enabled = false;
				}

				yield return new WaitForSeconds (0.00001f);

				if (vehicleGravityManager != null) {
					vehicleGravityManager.pauseDownForce (false);
				}

				yield return null;
			}

			if (currentVehicleInfo.currentVehicleObject != null) {
				getCurrentVehicleComponents ();
			}

			if (currentObjectToControl != null) {
				if (!currentObjectToControl.activeSelf) {
					if (vehicleGravityManager != null) {
						vehicleGravityManager.setCustomNormal (gravityManager.getCurrentNormal ());
					}

					Vector3 vehiclePosition = playerManager.transform.position + playerManager.transform.up;

					currentObjectToControl.transform.position = vehiclePosition;

					if (currentVehicleInfo.setVehicleRotationWheGetOn) {
						currentObjectToControl.transform.rotation = playerManager.transform.rotation;
					}

					if (vehicleCamera != null) {
						vehicleCamera.transform.position = vehiclePosition;
					}

					currentObjectToControl.SetActive (true);

					if (vehicleManager != null) {
						vehicleManager.OnTriggerEnter (playerCollider);
					}

					yield return null;
				}
			}
		} else {
			if (currentObjectToControl != null) {
				if (currentObjectToControl.activeSelf) {
					if (vehicleCamera != null) {
						if (currentVehicleInfo.setVehicleRotationWheGetOn) {
							currentObjectToControl.transform.rotation = vehicleCamera.transform.rotation;
						}
					}
				} 
			}
		}

		if (currentObjectToControl != null) {
			if (currentVehicleInfo.controlWithOverrideSystem) {
				if (state) {
					mainOverrideElementControlSystem.overrideElementControl (currentObjectToControl);

					mainOverrideElementControlSystem.addNewTemporalObject (currentObjectToControl);
				} else {
					mainOverrideElementControlSystem.inputStopOverrideControl ();

					if (mainOverrideElementControlSystem.checkIfTemporalObjectOnList (currentObjectToControl)) {
						currentObjectToControl.SetActive (false);
					}
				}
			} else {
				if (state) {
					usingDevicesManager.clearDeviceList ();

					usingDevicesManager.addDeviceToList (currentObjectToControl);

					usingDevicesManager.setCurrentVehicle (currentObjectToControl);

					usingDevicesManager.useCurrentDevice (currentObjectToControl);

					usingDevicesManager.setUseDeviceButtonEnabledState (false);
				} else {
					if (currentVehicleInfo.currentVehicleObject != null) {
						usingDevicesManager.useDevice ();

						usingDevicesManager.checkTriggerInfo (sphereVehicleCollider, false);

						usingDevicesManager.removeCurrentVehicle (currentObjectToControl);
					}

					currentObjectToControl.SetActive (false);

					usingDevicesManager.setUseDeviceButtonEnabledState (true);
				}
			}
		}

		if (state) {
			if (currentVehicleInfo != null) {
				playerManager.enableOrDisableSphereMode (true);

				playerManager.setCheckOnGroungPausedState (true);

				playerManager.setPlayerOnGroundState (false);

				playerManager.setPlayerOnGroundAnimatorStateOnOverrideOnGroundWithTime (false);

				playerManager.overrideOnGroundAnimatorValue (0);

				playerManager.setPlayerOnGroundAnimatorStateOnOverrideOnGround (false);

				playerManager.setOnGroundAnimatorIDValue (false);
			}
		} else {
			if (currentVehicleInfo != null) {
				if (currentVehicleInfo.isCurrentVehicle) {

					currentVehicleInfo.isCurrentVehicle = false;

					playerManager.enableOrDisableSphereMode (false);

					playerManager.setCheckOnGroungPausedState (false);

					playerManager.setPlayerOnGroundState (false);

					playerManager.setPlayerOnGroundAnimatorStateOnOverrideOnGroundWithTime (true);

					playerManager.disableOverrideOnGroundAnimatorValue ();

					playerManager.setPauseResetAnimatorStateFOrGroundAnimatorState (true);

					if (playerManager.getCurrentSurfaceBelowPlayer () != null) {
						playerManager.setPlayerOnGroundState (true);

						playerManager.setOnGroundAnimatorIDValue (true);
					}
				}
			}
		}
	}

	void getCurrentVehicleComponents ()
	{
		if (currentVehicleInfo.controlWithOverrideSystem) {
			currentObjectToControl = currentVehicleInfo.currentVehicleObject;
		} else {
			mainIKDrivingSystem = currentVehicleInfo.currentVehicleObject.GetComponent<IKDrivingSystem> ();

			if (mainIKDrivingSystem != null) {
				vehicleManager = mainIKDrivingSystem.getHUDManager ();
			}

			if (vehicleManager != null) {
				vehicleGravityManager = vehicleManager.getVehicleGravityControl ();
			}

			if (mainIKDrivingSystem != null) {
				mainIKDrivingSystem.setPlayerVisibleInVehicleState (currentVehicleInfo.playerVisibleInVehicle);

				mainIKDrivingSystem.setEjectPlayerWhenDestroyedState (currentVehicleInfo.ejectPlayerWhenDestroyed);

				mainIKDrivingSystem.setResetCameraRotationWhenGetOnState (currentVehicleInfo.resetCameraRotationWhenGetOn);
			}

			if (vehicleManager != null) {
				currentObjectToControl = vehicleManager.gameObject;

				vehicleCamera = vehicleManager.getVehicleCameraController ().gameObject;
			}

			sphereVehicleCollider = currentObjectToControl.GetComponent<Collider> ();

			tutorialActivatorSystem currentTutorialActivatorSystem = currentVehicleInfo.currentVehicleObject.GetComponent<tutorialActivatorSystem> ();

			if (currentTutorialActivatorSystem != null) {
				currentTutorialActivatorSystem.setTutorialEnabledState (false);
			}
		}
	}

	[System.Serializable]
	public class vehicleInfo
	{
		public string Name;
		public bool isCurrentVehicle;

		public bool playerVisibleInVehicle;

		public bool ejectPlayerWhenDestroyed = true;

		public bool controlWithOverrideSystem;

		public bool setVehicleRotationWheGetOn = true;

		public bool resetCameraRotationWhenGetOn;

		public GameObject vehiclePrefab;

		public GameObject currentVehicleObject;

		public bool useEventOnVehicleInfoState;
		public UnityEvent eventOnVehicleInfoState;
	}
}
