using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class grapplingHookSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool grapplingHookEnabled = true;

	public float maxRaycastDistance = 100;
	public LayerMask layerToCheckSurfaces;

	public float minDistanceToAttract = 0.5f;

	public bool applySpeedOnHookStop;
	public float extraSpeedOnHookStopMultiplier = 1;

	public bool rotatePlayerTowardTargetDirection;
	public float rotatePlayerSpeed;
	public float minAngleDifferenceToRotatePlayer;

	[Space]
	[Header ("Attract Objects Settings")]
	[Space]

	public bool attractObjectsEnabled = true;
	public float regularAttractionForce;
	public float increasedAttractionForce;
	public float minDistanceToStopAttractObject;

	public bool addUpForceForAttraction;
	public float upForceForAttraction;
	public float addUpForceForAttractionDuration;

	[Space]
	[Header ("Movement Settings")]
	[Space]

	public float regularMovementSpeed = 6;
	public float increasedMovementSpeed;

	public float inputMovementMultiplier = 3;

	public float airControlAmount = 20;

	public bool useVerticalMovementOnHook;
	public bool ignoreBackWardsMovementOnHook;

	public bool addVerticalFallingSpeed;
	public float verticalFallingSpeed;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public bool checkIfObjectStuck = true;
	public float timeToStopHookIfStuck = 2;
	public float minDistanceToCheckStuck = 1;

	[Space]
	[Header ("Camera Settings")]
	[Space]

	public bool changeCameraStateOnThirdPerson;
	public string cameraStateNameOnGrapplingHookActivate;
	public string cameraStateNameOnGrapplingHookDeactivate;
	public bool keepCameraStateActiveWhileOnAir;

	public bool changeFovOnHookActive;
	public float changeFovSpeed;
	public float regulaFov;
	public float increaseSpeedFov;

	public bool useCameraShake;
	public string regularCameraShakeName;
	public string increaseCamaraShakeName;

	[Space]
	[Header ("Animator Settings")]
	[Space]

	public bool setAnimatorState;
	public string hookStartActionName;
	public string hookEndActionName;

	public int hookStartActionID;
	public int hookEndActionID;

	public string actionActiveAnimatorName = "Action Active";
	public string actionIDAnimatorName = "Action ID";

	public float minDistancePercentageToUseHookEndAction = 0.1f;

	[Space]
	[Header ("Throw Hook Animation Settings")]
	[Space]

	public bool useAnimationToThrowHook;
	public string throwHookAnimationName;
	public float throwHookAnimationDuration;

	public bool throwAnimationInProcess;

	public bool checkIfSurfaceDetectedBeforeAnimation;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool grapplingHookActive;
	public bool grapplingHookUpdateActive;

	public Vector3 currentForceToApply;
	public Vector3 movementDirection;

	public float currentDistance;

	public float currentMovementSpeed;

	public Transform currentGrapplingHookTarget;

	public float angleToTargetDirection;

	public bool attractingObjectActive;

	public bool checkingToRemoveHookActive;

	[Space]
	[Header ("Events Settings")]
	[Space]

	public bool useEventsOnGrapplingHook;
	public UnityEvent eventOnGrapplingHookActivate;
	public UnityEvent eventOnGrapplingHookDeactivate;
	public eventParameters.eventToCallWithVector3 eventWithDirectionOnHookActive;

	public bool useEventsOnChangeCameraView;
	public UnityEvent eventOnSetFirstPersonView;
	public UnityEvent eventOnSetThirdPersonView;

	[Space]
	[Header ("Components")]
	[Space]

	public Transform grapplingHookTipTransform;

	public Transform playerControllerTransform;
	public playerController mainPlayerController;
	public gravitySystem mainGravitySystem;
	public Transform mainCameraTransform;
	public Rigidbody mainRigidbody;
	public playerCamera mainPlayerCamera;

	public Animator mainAnimator;

	RaycastHit hit;

	bool increaseSpeedActive;

	bool checkinGrapplingHookCameraStateAfterDeactivate;

	Vector3 pullForceToApply;

	Vector3 pullForceToApplyNormalize;

	int actionActiveAnimatorID;
	int actionIDAnimatorID;

	bool closeToReachTargetChecked;

	float initialDistanceToTarget;

	bool firstPersonActive;
	bool previoslyFirstPersonActive;

	objectToAttractWithGrapplingHook currentobjectToAttractWithGrapplingHook;

	GameObject currentObjectToAttract;
	Rigidbody currentRigidbodyToAttract;

	float lastTimeHookActive;

	float customMinDistanceToStopAttractObject;

	bool useCustomForceAttractionValues;
	bool customAddUpForceForAttraction;
	float customUpForceForAttraction;
	float customAddUpForceForAttractionDuration;

	bool attractionHookRemovedByDistance;

	Vector3 currentRaycastPosition;
	Vector3 currentRaycastDirection;

	float currentMinDistanceToStopAttractObject;

	Coroutine grapplingHookCoroutine;

	float lastTimeObjectMoving;
	float lastDistanceToObject;

	Coroutine animationCoroutine;


	void Start ()
	{
		actionActiveAnimatorID = Animator.StringToHash (actionActiveAnimatorName);
		actionIDAnimatorID = Animator.StringToHash (actionIDAnimatorName);
	}

	public void stopGrapplingHookCoroutine ()
	{
		if (grapplingHookCoroutine != null) {
			StopCoroutine (grapplingHookCoroutine);
		}

		grapplingHookUpdateActive = false;
	}

	IEnumerator activateGrapplingHookCorouine ()
	{
		var waitTime = new WaitForFixedUpdate ();

		while (true) {
			yield return waitTime;

			updateGrapplingHookState ();
		}
	}

	void updateGrapplingHookState ()
	{
		if (attractingObjectActive) {
			applyAttractionForces ();
		} else {
			if (grapplingHookActive) {
				applyHookForces ();

				if (useEventsOnChangeCameraView) {
					firstPersonActive = mainPlayerCamera.isFirstPersonActive ();

					if (firstPersonActive != previoslyFirstPersonActive) {
						previoslyFirstPersonActive = firstPersonActive;

						if (firstPersonActive) {
							eventOnSetFirstPersonView.Invoke ();
						} else {
							eventOnSetThirdPersonView.Invoke ();
						}
					}
				}
			}

			if (checkinGrapplingHookCameraStateAfterDeactivate) {
				if (mainPlayerController.isPlayerOnGround ()) {
					if (!mainPlayerCamera.isFirstPersonActive () && mainPlayerCamera.isCameraTypeFree ()) {
						mainPlayerCamera.setCameraState (cameraStateNameOnGrapplingHookDeactivate);
					}

					checkinGrapplingHookCameraStateAfterDeactivate = false;

					stopGrapplingHookCoroutine ();
				}
			}
		}
	}

	public void applyHookForces ()
	{
		currentForceToApply = Vector3.zero;
	
		Vector3 playerPosition = playerControllerTransform.position + playerControllerTransform.up;
		
		currentDistance = GKC_Utils.distance (playerPosition, grapplingHookTipTransform.position);
		if (currentDistance > minDistanceToAttract) {
			pullForceToApply = (grapplingHookTipTransform.position - playerPosition).normalized;
		} else {
			removeHook ();
		}

		if (increaseSpeedActive) {
			currentMovementSpeed = increasedMovementSpeed;
		} else {
			currentMovementSpeed = regularMovementSpeed;
		}

		currentForceToApply += pullForceToApply * currentMovementSpeed;

		pullForceToApplyNormalize = pullForceToApply.normalized;

		movementDirection = 
			(mainPlayerController.getHorizontalInput () * Vector3.Cross (playerControllerTransform.up, pullForceToApplyNormalize));

		if (useVerticalMovementOnHook) {
			movementDirection += (mainPlayerController.getVerticalInput () * playerControllerTransform.up);
		} else {
			float verticalInputValue = mainPlayerController.getVerticalInput ();
			if (ignoreBackWardsMovementOnHook) {
				verticalInputValue = Mathf.Clamp (verticalInputValue, 0, 1);
			}

			movementDirection += (verticalInputValue * pullForceToApplyNormalize);
		}

		movementDirection *= inputMovementMultiplier;

		currentForceToApply += movementDirection;

		if (addVerticalFallingSpeed) {
			currentForceToApply -= playerControllerTransform.up * verticalFallingSpeed;
		}

		mainPlayerController.setExternalForceOnAir (currentForceToApply, airControlAmount);

		if (rotatePlayerTowardTargetDirection) {
			pullForceToApplyNormalize -= playerControllerTransform.up * playerControllerTransform.InverseTransformDirection (pullForceToApplyNormalize).y;

			angleToTargetDirection = Vector3.SignedAngle (playerControllerTransform.forward, pullForceToApplyNormalize, playerControllerTransform.up);

			if (Mathf.Abs (angleToTargetDirection) > minAngleDifferenceToRotatePlayer) {
				playerControllerTransform.Rotate (0, (angleToTargetDirection / 2) * rotatePlayerSpeed * Time.deltaTime, 0);
			}
		}

		if (!closeToReachTargetChecked) {
			if (currentDistance < initialDistanceToTarget * minDistancePercentageToUseHookEndAction) {

				if (setAnimatorState) {
					mainAnimator.SetInteger (actionIDAnimatorID, hookEndActionID);
					mainAnimator.CrossFadeInFixedTime (hookEndActionName, 0.1f);
				}

				closeToReachTargetChecked = true;
			}
		}

		checkIfObjectIsMoving ();
	}

	public void applyAttractionForces ()
	{
		currentForceToApply = Vector3.zero;

		Vector3 playerPosition = playerControllerTransform.position + playerControllerTransform.up;

		currentDistance = GKC_Utils.distance (playerPosition, grapplingHookTipTransform.position);

		if (useCustomForceAttractionValues) {
			currentMinDistanceToStopAttractObject = customMinDistanceToStopAttractObject;
		} else {
			currentMinDistanceToStopAttractObject = minDistanceToStopAttractObject;
		}

		if (currentDistance > currentMinDistanceToStopAttractObject) {
			pullForceToApply = (playerPosition - grapplingHookTipTransform.position).normalized;
		} else {
			attractionHookRemovedByDistance = true;

			removeHook ();

			return;
		}

		if (increaseSpeedActive) {
			currentMovementSpeed = increasedAttractionForce;
		} else {
			currentMovementSpeed = regularAttractionForce;
		}

		currentForceToApply += pullForceToApply * currentMovementSpeed;

		movementDirection *= inputMovementMultiplier;

		currentForceToApply += movementDirection;

		if (useCustomForceAttractionValues) {
			if (customAddUpForceForAttraction) {
				if (Time.time < lastTimeHookActive + customAddUpForceForAttractionDuration) {
					currentForceToApply += playerControllerTransform.up * customUpForceForAttraction;
				}
			}
		} else {
			if (addUpForceForAttraction) {
				if (Time.time < lastTimeHookActive + addUpForceForAttractionDuration) {
					currentForceToApply += playerControllerTransform.up * upForceForAttraction;
				}
			}
		}
	
		if (currentRigidbodyToAttract) {
			currentRigidbodyToAttract.velocity = currentForceToApply;
		}

		checkIfObjectIsMoving ();
	}

	public void checkIfObjectIsMoving ()
	{
		if (checkIfObjectStuck) {
			if (lastTimeObjectMoving == 0) {
				currentDistance = GKC_Utils.distance (playerControllerTransform.position, grapplingHookTipTransform.position);

				lastDistanceToObject = currentDistance;

				lastTimeObjectMoving = Time.time;
			}

			if (Time.time > lastTimeObjectMoving + timeToStopHookIfStuck) {
				lastTimeObjectMoving = Time.time;

				if ((currentDistance + minDistanceToCheckStuck) >= lastDistanceToObject) {
					print ("position hasn't changed in " + timeToStopHookIfStuck + " time, stop hook");

					removeHook ();
				}

				lastDistanceToObject = currentDistance;
			}
		}
	}

	public void removeHook ()
	{
		if (grapplingHookActive) {
		
			grapplingHookActive = false;
		
			if (attractingObjectActive) {
				checkEventsOnGrapplingHook (false);

				stopGrapplingHookCoroutine ();
			} else {
				pauseOrResumePlayerState (false);
			}

			increaseSpeedActive = false;

			grapplingHookTipTransform.SetParent (transform);

			if (attractObjectsEnabled) {
				if (currentobjectToAttractWithGrapplingHook) {
					currentobjectToAttractWithGrapplingHook.setAttractionHookRemovedByDistanceState (attractionHookRemovedByDistance);

					currentobjectToAttractWithGrapplingHook.setAttractObjectState (false);

					attractingObjectActive = false;

					currentobjectToAttractWithGrapplingHook = null;

					currentObjectToAttract = null;

					currentRigidbodyToAttract = null;

					attractionHookRemovedByDistance = false;
				}
			}
		}
	}

	public void setGrapplingHookTarget (Transform newTarget)
	{
		currentGrapplingHookTarget = newTarget;
	}

	public void setGrapplingHookEnabledState (bool state)
	{
		grapplingHookEnabled = state;

		if (!grapplingHookEnabled) {
			removeGrapplingHook ();
		}
	}

	void calculateRaycastValues ()
	{
		currentRaycastPosition = mainCameraTransform.position;
		currentRaycastDirection = mainCameraTransform.forward;

		if (!mainPlayerCamera.isCameraTypeFree () && !mainPlayerCamera.isPlayerAiming ()) {
			currentRaycastPosition = playerControllerTransform.position + playerControllerTransform.up * 1.3f;
			currentRaycastDirection = playerControllerTransform.forward;
		}

		if (currentGrapplingHookTarget != null) {
			currentRaycastDirection = currentGrapplingHookTarget.position - currentRaycastPosition;
			currentRaycastDirection.Normalize ();
		}
	}

	public void checkThrowGrapplingHook ()
	{
		if (grapplingHookEnabled) {
			if (!grapplingHookActive) {

				calculateRaycastValues ();

				if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, maxRaycastDistance, layerToCheckSurfaces)) {
					grapplingHookTipTransform.position = hit.point;

					grapplingHookTipTransform.SetParent (hit.collider.gameObject.transform);

					grapplingHookActive = true;

					stopGrapplingHookCoroutine ();

					grapplingHookCoroutine = StartCoroutine (activateGrapplingHookCorouine ());

					grapplingHookUpdateActive = true;

					lastTimeHookActive = Time.time;

					lastTimeObjectMoving = 0;

					if (attractObjectsEnabled) {
						currentObjectToAttract = hit.collider.gameObject;

						GameObject currentVehicle = applyDamage.getVehicle (currentObjectToAttract);

						if (currentVehicle != null) {
							currentObjectToAttract = currentVehicle;
						}

						currentobjectToAttractWithGrapplingHook = currentObjectToAttract.GetComponent<objectToAttractWithGrapplingHook> ();

						if (currentobjectToAttractWithGrapplingHook) {
							attractingObjectActive = currentobjectToAttractWithGrapplingHook.setAttractObjectState (true);

							if (attractingObjectActive) {
								currentRigidbodyToAttract = currentobjectToAttractWithGrapplingHook.getRigidbodyToAttract ();

								if (currentRigidbodyToAttract == null) {
									print ("WARNING: No rigidbody has been configured in the object " + currentobjectToAttractWithGrapplingHook.name);
								
									removeGrapplingHook ();

									return;
								}

								grapplingHookTipTransform.SetParent (currentRigidbodyToAttract.transform);

								grapplingHookTipTransform.position = currentRigidbodyToAttract.position;

								customMinDistanceToStopAttractObject = currentobjectToAttractWithGrapplingHook.customMinDistanceToStopAttractObject;

								useCustomForceAttractionValues = currentobjectToAttractWithGrapplingHook.useCustomForceAttractionValues;
								customAddUpForceForAttraction = currentobjectToAttractWithGrapplingHook.customAddUpForceForAttraction;
								customUpForceForAttraction = currentobjectToAttractWithGrapplingHook.customUpForceForAttraction;
								customAddUpForceForAttractionDuration = currentobjectToAttractWithGrapplingHook.customAddUpForceForAttractionDuration;
							}
						}
					}

					if (attractingObjectActive) {
						checkEventsOnGrapplingHook (true);
					} else {
						pauseOrResumePlayerState (true);
					}

					checkingToRemoveHookActive = false;

					eventWithDirectionOnHookActive.Invoke (currentRaycastDirection);

					closeToReachTargetChecked = false;

					Vector3 playerPosition = playerControllerTransform.position + playerControllerTransform.up;
					initialDistanceToTarget = GKC_Utils.distance (playerPosition, grapplingHookTipTransform.position);
				}
			}
		}
	}

	public void removeGrapplingHook ()
	{
		if (grapplingHookActive) {
			if (checkingToRemoveHookActive) {
				removeHook ();
			}

			checkingToRemoveHookActive = true;
		}
	}

	public void checkRemoveGrapplingHook ()
	{
		if (grapplingHookEnabled) {
			removeGrapplingHook ();
		}
	}

	public void checkEventsOnGrapplingHook (bool state)
	{
		if (useEventsOnGrapplingHook) {
			if (state) {
				eventOnGrapplingHookActivate.Invoke ();
			} else {
				eventOnGrapplingHookDeactivate.Invoke ();
			}
		}
	}

	public void pauseOrResumePlayerState (bool state)
	{
		if (state) {
			if (mainPlayerController.isExternalControllBehaviorActive ()) {
				externalControllerBehavior currentExternalControllerBehavior = mainPlayerController.getCurrentExternalControllerBehavior ();

				if (currentExternalControllerBehavior != null) {
					currentExternalControllerBehavior.disableExternalControllerState ();
				}
			}

			mainPlayerController.setGravityForcePuase (true);

			mainPlayerController.setCheckOnGroungPausedState (true);

			mainPlayerController.setPlayerOnGroundState (false);

			mainGravitySystem.setCameraShakeCanBeUsedState (false);

			mainPlayerController.setJumpLegExternallyActiveState (true);

			mainPlayerController.setIgnoreExternalActionsActiveState (true);
		} else {
			mainPlayerController.setGravityForcePuase (false);

			mainPlayerController.setCheckOnGroungPausedState (false);

			mainGravitySystem.setCameraShakeCanBeUsedState (true);

			mainPlayerController.setJumpLegExternallyActiveState (false);

			mainPlayerController.setLastTimeFalling ();

			mainPlayerController.setIgnoreExternalActionsActiveState (false);
		}

		checkEventsOnGrapplingHook (state);

		bool stopCoroutine = false;

		if (changeCameraStateOnThirdPerson) {
			if (!mainPlayerCamera.isFirstPersonActive ()) {
				if (state) {
					if (mainPlayerCamera.isCameraTypeFree ()) {
						mainPlayerCamera.setCameraState (cameraStateNameOnGrapplingHookActivate);
					}
				} else {
					if (!keepCameraStateActiveWhileOnAir) {
						if (mainPlayerCamera.isCameraTypeFree ()) {
							mainPlayerCamera.setCameraState (cameraStateNameOnGrapplingHookDeactivate);
						}

						stopCoroutine = true;
					} else {
						checkinGrapplingHookCameraStateAfterDeactivate = true;
					}
				}
			} else {
				stopCoroutine = true;
			}
		} else {
			stopCoroutine = true;
		}

		if (changeFovOnHookActive) {
			if (state) {
				mainPlayerCamera.setMainCameraFov (regulaFov, changeFovSpeed);
			} else {
				mainPlayerCamera.setOriginalCameraFov ();
			}
		}

		if (useCameraShake) {
			if (state) {
				mainPlayerCamera.setShakeCameraState (true, regularCameraShakeName);
			} else {
				mainPlayerCamera.setShakeCameraState (false, "");
			}
		}

		if (applySpeedOnHookStop) {
			if (!state) {
				mainPlayerController.addExternalForce (currentForceToApply * extraSpeedOnHookStopMultiplier);
			}
		}

		if (setAnimatorState) {
			if (state) {
				mainAnimator.SetInteger (actionIDAnimatorID, hookStartActionID);
				mainAnimator.CrossFadeInFixedTime (hookStartActionName, 0.1f);
			}

			mainAnimator.SetBool (actionActiveAnimatorID, state);
		}

		if (rotatePlayerTowardTargetDirection) {
			mainPlayerController.setAddExtraRotationPausedState (state);
		}

		if (state) {
			firstPersonActive = mainPlayerCamera.isFirstPersonActive ();
			previoslyFirstPersonActive = !firstPersonActive;
		}

		if (!state) {
			if (stopCoroutine) {
				stopGrapplingHookCoroutine ();
			}
		}
	}

	void stopThrowHookAnimationCoroutine ()
	{
		if (animationCoroutine != null) {
			StopCoroutine (animationCoroutine);
		}

		throwAnimationInProcess = false;
	}

	IEnumerator throwHookAnimationCoroutine ()
	{
		throwAnimationInProcess = true;

		mainAnimator.CrossFade (throwHookAnimationName, 0.1f);

		mainAnimator.SetBool (actionActiveAnimatorID, true);

		yield return new WaitForSeconds (throwHookAnimationDuration);

		mainAnimator.SetBool (actionActiveAnimatorID, false);

		checkThrowGrapplingHook ();

		if (grapplingHookActive) {
			checkingToRemoveHookActive = true;
		}
	
		throwAnimationInProcess = false;
	}

	public void inputThrowGrapplingHook ()
	{
		if (throwAnimationInProcess) {
			return;
		}
			
		if (useAnimationToThrowHook && !mainPlayerCamera.isFirstPersonActive ()) {
			if (!grapplingHookActive) {
				if (checkIfSurfaceDetectedBeforeAnimation) {

					calculateRaycastValues ();

					if (!Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, maxRaycastDistance, layerToCheckSurfaces)) {
						return;
					}
				}

				stopThrowHookAnimationCoroutine ();

				animationCoroutine = StartCoroutine (throwHookAnimationCoroutine ());
			}
		} else {
			checkThrowGrapplingHook ();
		}
	}

	public void inputRemoveGrapplingHook ()
	{
		checkRemoveGrapplingHook ();
	}

	public void inputIncreaseOrDecreaseMovementSpeed (bool state)
	{
		if (grapplingHookEnabled) {
			if (grapplingHookActive) {
				increaseSpeedActive = state;

				if (changeFovOnHookActive) {
					if (increaseSpeedActive) {
						mainPlayerCamera.setMainCameraFov (increaseSpeedFov, changeFovSpeed);
					} else {
						mainPlayerCamera.setMainCameraFov (regulaFov, changeFovSpeed);
					}
				}

				if (useCameraShake) {
					if (increaseSpeedActive) {
						mainPlayerCamera.setShakeCameraState (true, increaseCamaraShakeName);
					} else {
						mainPlayerCamera.setShakeCameraState (true, regularCameraShakeName);
					}
				}
			}
		}
	}

	public void setAttractObjectsEnabled (bool state)
	{
		attractObjectsEnabled = state;
	}
}
