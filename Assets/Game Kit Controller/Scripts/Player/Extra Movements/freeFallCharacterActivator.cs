using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeFallCharacterActivator : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool checkCharacterFallEnabled = true;

	public float minTimeOnAirToActivateFreeFall = 2;

	public int regularAirID = -1;
	public int freeFallID = 3;

	public bool setNewCameraStateOnFreeFallActive;

	public string newCameraStateOnFreeFall;

	public Vector3 capsuleColliderCenter = new Vector3 (0, 1, 0);

	[Space]
	[Header ("Debug")]
	[Space]

	public bool checkingFreeFall;

	public bool freeFallActive;

	public bool freeFallPaused;

	[Space]
	[Header ("Components")]
	[Space]

	public playerCamera mainPlayerCamera;

	public playerController mainPlayerController;

	float lastTimeFalling;

	float lastTimeJump;

	string previousCameraState;

	void Update ()
	{
		if (checkCharacterFallEnabled) {
			if (freeFallPaused) {
				return;
			}

			if (!checkingFreeFall) {
				if (!mainPlayerController.isPlayerOnGround () &&
				    mainPlayerController.getCurrentAirID () == regularAirID &&
				    !mainPlayerController.isExternalControlBehaviorForAirTypeActive () &&
				    !mainPlayerController.isPlayerDriving ()) {

					checkingFreeFall = true;

					lastTimeFalling = Time.time;
				}
			} else {
				if (!freeFallActive) {
					if (Time.time > minTimeOnAirToActivateFreeFall + lastTimeFalling) {

						if (mainPlayerController.getCurrentAirID () == regularAirID) {
							freeFallActive = true;

							mainPlayerController.setCurrentAirIDValue (freeFallID);

							mainPlayerController.setPlayerCapsuleColliderDirection (2);

							mainPlayerController.setPlayerColliderCapsuleCenter (capsuleColliderCenter);

							lastTimeJump = mainPlayerController.getLastDoubleJumpTime ();

							if (setNewCameraStateOnFreeFallActive) {
								previousCameraState = mainPlayerCamera.getCurrentStateName ();

								mainPlayerCamera.setCameraStateOnlyOnThirdPerson (newCameraStateOnFreeFall);
							}
						} else {
							checkingFreeFall = false;
						}
					} else {
						if (mainPlayerController.isPlayerOnGround () ||
						    mainPlayerController.isActionActive () ||
						    mainPlayerController.isGravityPowerActive () ||
						    mainPlayerController.isPlayerOnFFOrZeroGravityModeOn () ||
						    mainPlayerController.isChoosingGravityDirection () ||
						    mainPlayerController.isGravityForcePaused () ||
						    mainPlayerController.isWallRunningActive () ||
						    mainPlayerController.isSwimModeActive () ||
						    mainPlayerController.isSphereModeActive () ||
						    mainPlayerController.isExternalControlBehaviorForAirTypeActive () ||
						    mainPlayerController.isPlayerDriving ()) {

							checkingFreeFall = false;

							freeFallActive = false;
						}
					}
				} else {
					if (mainPlayerController.isPlayerOnGround () ||
					    mainPlayerController.isPlayerAiming () ||
					    lastTimeJump != mainPlayerController.getLastDoubleJumpTime () ||
					    mainPlayerController.isExternalControlBehaviorForAirTypeActive ()) {

						if (mainPlayerController.getCurrentAirID () == freeFallID) {
							mainPlayerController.setCurrentAirIDValue (regularAirID);

							mainPlayerController.setPlayerCapsuleColliderDirection (1);

							mainPlayerController.setOriginalPlayerColliderCapsuleScale ();
						}

						disableFreeFallActiveState ();
					}

					if (mainPlayerController.getCurrentAirID () != freeFallID) {
						disableFreeFallActiveState ();
					}
				}
			}
		}
	}

	public void disableFreeFallActiveState ()
	{
		checkingFreeFall = false;

		freeFallActive = false;

		if (setNewCameraStateOnFreeFallActive) {
			if (previousCameraState != "") {
				mainPlayerCamera.setCameraStateOnlyOnThirdPerson (previousCameraState);

				previousCameraState = "";
			}
		}
	}

	public void setFreeFallPausedState (bool state)
	{
		if (!state) {
			if (freeFallActive) {
				disableFreeFallActiveState ();
			}
		}

		freeFallPaused = state;
	}
}
