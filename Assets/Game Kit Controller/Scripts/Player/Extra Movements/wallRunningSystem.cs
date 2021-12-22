using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class wallRunningSystem : externalControllerBehavior
{
	[Header ("Main Settings")]
	[Space]

	public bool wallRunningEnabled = true;

	public LayerMask raycastLayermask;

	public float wallRunningSpeed;
	public float wallSprintSpeed;
	public float wallRunningRotationSpeed;
	public bool wallRunningCanBeUsed = true;

	public Vector3 wallRunningImpulseOnJump;
	public Vector3 wallRunningStopMovemmentImpulse;
	public Vector3 wallRunningEndOfSurfaceImpulse;
	public Vector3 wallRunningImpulseOnJumpCameraDirection;

	public float raycastDistance = 0.6f;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public bool useDownMovementOnWallRunning;
	public float downMovementOnWallRunningAmount;
	public float delayDownMovementOnWallRunning;

	public bool useUpAndDownMovementOnWallRunning;
	public float upAndDownMovementOnWallRunningDelay;
	public float upAndDownMovementOnWallRunningAmount;

	public bool useStopWallRunnigAfterDelay;
	public float stopWallRunningDelay;

	public bool useSurfaceAngleDiffToJumpInCameraDirection;
	public Vector2 surfaceAngleDiffRangeToWallRunning;

	public float maxVelocityChangeWallRunning;

	public bool keepWeaponsOnWallRunning;
	public bool drawWeaponsIfCarriedPreviously;

	[Space]
	[Header ("Third Person Settings")]
	[Space]

	public string wallRunningRightActionName = "Wall Running Right";
	public string wallRunnigLeftActionName = "Wall Running Left";

	public string actionActiveAnimatorName = "Action Active";

	public bool useCharacterCOMOffset = true;
	public float rotationOffsetOnCharacterCOM = 10;
	public float positionOffsetOnCharacterCOM = 0.15f;
	public float offsetAdjustmentSpeed = 5;

	public float playerRotationToSurfaceSpeedThirdPerson = 5;

	[Space]
	[Header ("Third Person Camera State Settings")]
	[Space]

	public bool setNewCameraStateOnThirdPerson;

	public string newCameraStateOnThirdPersonRightSide;
	public string newCameraStateOnThirdPersonLeftSide;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;

	public bool wallRunningActive;

	public bool isFirstPersonActive;

	public bool wallRunningOnRightSide;
	public bool wallRunningOnLeftSide;

	public bool carryingWeaponsPreviously;

	public Vector3 currentMovementDirection;

	public float extraYPosition;

	[Space]
	[Header ("First Person Events Settings")]
	[Space]

	public UnityEvent eventOnWallRunningStartFirstPerson;
	public UnityEvent eventOnWallRunningToRightFirstPerson;
	public UnityEvent eventOnWallRunningToLeftFirstPerson;
	public UnityEvent eventOnWallRunningEndFirstPerson;

	[Space]
	[Header ("Third Person Events Settings")]
	[Space]

	public UnityEvent eventOnWallRunningStartThirdPerson;
	public UnityEvent eventOnWallRunningToRightThirdPerson;
	public UnityEvent eventOnWallRunningToLeftThirdPerson;
	public UnityEvent eventOnWallRunningEndThirdPerson;

	[Space]
	[Header ("Components")]
	[Space]

	public playerController mainPlayerController;
	public Transform playerTransform;
	public Rigidbody mainRigidbody;
	public Transform playerCameraTransform;
	public playerCamera mainPlayerCamera;
	public playerWeaponsManager mainPlayerWeaponsManager;

	public Animator mainAnimator;

	public Transform COM;

	float lastTimeWallRunningActive;

	float lastTimeWallRunningDisabled;

	bool previouslyWallRunningOnRightSide;
	bool previouslyWallRunningOnLeftSide;
	float lastTimeWallRunningOnRightSide;
	RaycastHit wallRunningHit;

	bool autoUseDownMovementOnWallRunningActive;
	bool autoUseStopWallRunningAfterDelay;
	bool originalWallRunningEnabled;

	Vector3 playerTransformUp;
	Vector3 playerTransformForward;
	Vector3 playerTransformRight;

	RaycastHit hit;

	Vector3 currentNormal;

	Quaternion quaternionIdentity = Quaternion.identity;

	float half = 0.5f;

	Vector3 velocityChange;

	int actionActiveAnimatorID;

	float originalCOMYPosition;

	Coroutine resetCOMCoroutine;

	float currentRotationTarget;
	float currentPositionTarget;

	string previousCameraState;

	float playerYPositionOnWallRunningStart;


	void Start ()
	{
		originalWallRunningEnabled = wallRunningEnabled;

		actionActiveAnimatorID = Animator.StringToHash (actionActiveAnimatorName);

		originalCOMYPosition = COM.localPosition.y;
	}

	public override void updateControllerBehavior ()
	{
		playerTransformUp = playerTransform.up;

		playerTransformForward = playerTransform.forward;

		playerTransformRight = playerTransform.right;

		Vector3 currentRaycastPosition = playerTransform.position + playerTransformUp;
		Vector3 currentRaycastDirection = playerTransformRight;

		if (!wallRunningOnRightSide) {
			currentRaycastDirection = -playerTransformRight;
		}

		if (mainPlayerController.isPlayerMovingVertical (0.7f)) {
			if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, 1, raycastLayermask)) {
				wallRunningHit = hit;

				float currentWallRunningSpeed = wallRunningSpeed;

				if (mainPlayerController.isPlayerRunning ()) {
					currentWallRunningSpeed = wallSprintSpeed;
				}

				currentMovementDirection = wallRunningHit.point + playerTransformForward;

				currentMovementDirection = currentMovementDirection - playerTransformUp * playerTransform.InverseTransformDirection (currentMovementDirection).y;

				currentMovementDirection += playerTransformUp * (playerYPositionOnWallRunningStart + extraYPosition);

				if (useDownMovementOnWallRunning || autoUseDownMovementOnWallRunningActive) {
					if (Time.time > delayDownMovementOnWallRunning + lastTimeWallRunningActive) {
						extraYPosition -= downMovementOnWallRunningAmount;
					}
				} else {
					if (useUpAndDownMovementOnWallRunning && Time.time > upAndDownMovementOnWallRunningDelay + lastTimeWallRunningActive) {
						float wallRunningInput = 0;

						float horizontalInput = mainPlayerController.getHorizontalInput ();

						if (horizontalInput > half) {
							wallRunningInput = upAndDownMovementOnWallRunningAmount * horizontalInput;
						} else if (horizontalInput < -half) {
							wallRunningInput = upAndDownMovementOnWallRunningAmount * horizontalInput;
						}

						if (!wallRunningOnRightSide) {
							wallRunningInput *= (-1);
						}

						extraYPosition += wallRunningInput;
					}
				}

				if (useStopWallRunnigAfterDelay || autoUseStopWallRunningAfterDelay) {
					if (Time.time > stopWallRunningDelay + lastTimeWallRunningActive) {
						setWallRunningActiveState (false);

						setWallRunningImpulseForce (wallRunningStopMovemmentImpulse, false);
					}
				}

				float currentFixedUpdateDeltaTime = mainPlayerController.getCurrentDeltaTime ();

				mainRigidbody.position = Vector3.MoveTowards (mainRigidbody.position, currentMovementDirection, currentFixedUpdateDeltaTime * currentWallRunningSpeed);
			} else {
				setWallRunningActiveState (false);

				setWallRunningImpulseForce (wallRunningEndOfSurfaceImpulse, false);
			}
		} else {
			setWallRunningActiveState (false);

			setWallRunningImpulseForce (wallRunningStopMovemmentImpulse, false);
		}

		mainPlayerController.setCurrentVelocityValue (mainRigidbody.velocity);
	

		//Manage player rotation to adjust to wall normal

		Vector3 wallNormal = -wallRunningHit.normal;

		currentNormal = mainPlayerController.getCurrentNormal ();

		Quaternion playerTargetRotation = Quaternion.LookRotation (wallNormal, currentNormal);

		Vector3 playerTargetEulerAngles = playerTargetRotation.eulerAngles;

		playerTargetEulerAngles = Vector3.Scale (currentNormal, playerTargetEulerAngles);

		Quaternion extraRotation = quaternionIdentity;

		if (wallRunningOnRightSide) {
			extraRotation = Quaternion.AngleAxis (-90, playerTransformUp);
		} else {
			extraRotation = Quaternion.AngleAxis (90, playerTransformUp);
		}

		if (isFirstPersonActive) {
			playerTransform.eulerAngles = playerTargetEulerAngles + extraRotation.eulerAngles;
		} else {
			Quaternion targetRotation = Quaternion.Euler (playerTargetEulerAngles + extraRotation.eulerAngles);

			playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, targetRotation,
				Time.deltaTime * playerRotationToSurfaceSpeedThirdPerson);
		}

		//Set foot step state and head bob
		mainPlayerController.setCurrentFootStepsState ();

		if (mainPlayerController.updateHeadbobState) {
			mainPlayerController.setCurrentHeadBobState ();
		}

		if (useCharacterCOMOffset) {
			if (wallRunningOnRightSide) {
				currentRotationTarget = -rotationOffsetOnCharacterCOM;
				currentPositionTarget = -positionOffsetOnCharacterCOM;
			} else {
				currentRotationTarget = rotationOffsetOnCharacterCOM;
				currentPositionTarget = positionOffsetOnCharacterCOM;
			}

			Quaternion COMTargetRotation = Quaternion.Euler (Vector3.forward * currentRotationTarget);
		
			COM.localRotation = Quaternion.Lerp (COM.localRotation, COMTargetRotation,	offsetAdjustmentSpeed * Time.fixedDeltaTime);

			Vector3 COMTargetPosition = Vector3.right * currentPositionTarget + Vector3.up * originalCOMYPosition;

			COM.localPosition = Vector3.Lerp (COM.localPosition, COMTargetPosition, offsetAdjustmentSpeed * Time.fixedDeltaTime);
		}
	}

	public override void checkIfActivateExternalForce ()
	{
		if (wallRunningEnabled && wallRunningCanBeUsed && !mainPlayerController.useFirstPersonPhysicsInThirdPersonActive) {
			if (!wallRunningActive && !mainPlayerController.pauseAllPlayerDownForces && !mainPlayerController.ignoreExternalActionsActiveState) {
//
//				if (showDebugPrint) {
//					print ("Checking if activate external force");
//				}

				bool wallRunningSurfaceFound = false;

				if (mainPlayerController.getVerticalInput () > 0 && !mainPlayerController.isCrouching () && Time.time > lastTimeWallRunningActive + 0.05f) {
					playerTransformUp = playerTransform.up;

					playerTransformRight = playerTransform.right;

					Vector3 currentRaycastPosition = playerTransform.position + playerTransformUp;
					Vector3 currentRaycastDirection = playerTransformRight;

					if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, raycastDistance, raycastLayermask)) {

						if (previouslyWallRunningOnRightSide != wallRunningOnRightSide) {
							previouslyWallRunningOnRightSide = wallRunningOnRightSide;

							lastTimeWallRunningOnRightSide = Time.time;

							if (showDebugPrint) {
								print ("Running Right Side");
							}
						}

						wallRunningOnRightSide = true;
						wallRunningOnLeftSide = false;
						previouslyWallRunningOnLeftSide = false;

						if (!previouslyWallRunningOnRightSide || Time.time > lastTimeWallRunningOnRightSide + 0.3f) {
							setWallRunningActiveState (true);

							previouslyWallRunningOnRightSide = false;
						}

						wallRunningSurfaceFound = true;
					} else {
						currentRaycastDirection = -playerTransformRight;

						if (Physics.Raycast (currentRaycastPosition, currentRaycastDirection, out hit, raycastDistance, raycastLayermask)) {
							if (previouslyWallRunningOnLeftSide != wallRunningOnLeftSide) {
								previouslyWallRunningOnLeftSide = wallRunningOnLeftSide;

								lastTimeWallRunningOnRightSide = Time.time;

								if (showDebugPrint) {
									print ("Running Left Side");
								}
							}

							wallRunningOnRightSide = false;
							wallRunningOnLeftSide = true;
							previouslyWallRunningOnRightSide = false;

							if (!previouslyWallRunningOnLeftSide || Time.time > lastTimeWallRunningOnRightSide + 0.3f) {
								setWallRunningActiveState (true);

								previouslyWallRunningOnLeftSide = false;
							}

							wallRunningSurfaceFound = true;
						}
					}
				}

				if (!wallRunningSurfaceFound && Time.time > lastTimeWallRunningDisabled + half) {
					previouslyWallRunningOnLeftSide = false;
					previouslyWallRunningOnRightSide = false;
					wallRunningOnRightSide = false;
					wallRunningOnLeftSide = false;
				}
			}
		}
	}

	public override void setExtraImpulseForce (Vector3 forceAmount, bool useCameraDirection)
	{
		setWallRunningImpulseForce (forceAmount, useCameraDirection);
	}

	public void setWallRunningImpulseForce (Vector3 forceAmount, bool useCameraDirection)
	{
		Transform mainImpulseTransform = playerTransform;

		if (useCameraDirection) {
			mainImpulseTransform = playerCameraTransform; 
		}

		Vector3 impulseForce = mainImpulseTransform.forward * forceAmount.z + mainImpulseTransform.up * forceAmount.y;

		if (wallRunningOnRightSide) {
			impulseForce -= mainImpulseTransform.right * forceAmount.x;
		} else {
			impulseForce += mainImpulseTransform.right * forceAmount.x;
		}

		if (maxVelocityChangeWallRunning > 0) {
			velocityChange = impulseForce - mainRigidbody.velocity;

			velocityChange = Vector3.ClampMagnitude (velocityChange, maxVelocityChangeWallRunning);

		} else {
			velocityChange = impulseForce;
		}

		mainPlayerController.setVelocityChangeValue (velocityChange);

		mainRigidbody.AddForce (velocityChange, ForceMode.VelocityChange);
	}

	public override void setJumpActiveForExternalForce ()
	{
		setJumpActive ();
	}

	public void setJumpActive ()
	{
		if (wallRunningActive) {
			if (useSurfaceAngleDiffToJumpInCameraDirection) {
				float angleSurface = Vector3.SignedAngle (playerCameraTransform.forward, playerTransform.forward, playerTransform.up);

				if (Mathf.Abs (angleSurface) > surfaceAngleDiffRangeToWallRunning.x && Mathf.Abs (angleSurface) < surfaceAngleDiffRangeToWallRunning.y) {
					setWallRunningImpulseForce (wallRunningImpulseOnJumpCameraDirection, true);
				} else {
					setWallRunningImpulseForce (wallRunningImpulseOnJump, false);
				}
			} else {
				setWallRunningImpulseForce (wallRunningImpulseOnJump, false);
			}

			mainPlayerController.setCurrentVelocityValue (mainRigidbody.velocity);

			lastTimeWallRunningActive = Time.time;

			mainPlayerController.setWallRunningActiveValue (false);

			setWallRunningActiveState (false);
		}
	}

	public override void setExternalForceActiveState (bool state)
	{
		setWallRunningActiveState (state);
	}

	public void setWallRunningActiveState (bool state)
	{
		if (!wallRunningEnabled) {
			return;
		}

		if (wallRunningActive == state) {
			return;
		}

		if (!state) {
			lastTimeWallRunningDisabled = Time.time;
		}

		wallRunningActive = state;

		behaviorCurrentlyActive = state;

		mainPlayerController.setWallRunningActiveValue (state);

		if (showDebugPrint) {
			print ("Wall running active state " + state);
		}

		isFirstPersonActive = mainPlayerController.isPlayerOnFirstPerson ();

		if (wallRunningActive) {
			if (isFirstPersonActive) {
				eventOnWallRunningStartFirstPerson.Invoke ();
				if (wallRunningOnRightSide) {
					eventOnWallRunningToRightFirstPerson.Invoke ();
				} else {
					eventOnWallRunningToLeftFirstPerson.Invoke ();
				}

			} else {
				eventOnWallRunningStartThirdPerson.Invoke ();

				if (wallRunningOnRightSide) {
					eventOnWallRunningToRightThirdPerson.Invoke ();
				} else {
					eventOnWallRunningToLeftThirdPerson.Invoke ();
				}

				if (wallRunningOnRightSide) {
					mainAnimator.CrossFadeInFixedTime (wallRunningRightActionName, 0.1f);
				} else {
					mainAnimator.CrossFadeInFixedTime (wallRunnigLeftActionName, 0.1f);
				}

				mainAnimator.SetBool (actionActiveAnimatorID, state);
			}

			extraYPosition = 0;

			playerYPositionOnWallRunningStart = playerTransform.position.y;

			mainPlayerController.setJumpsAmountValue (0);

			lastTimeWallRunningActive = Time.time;

			mainPlayerCamera.enableOrDisableChangeCameraView (false);

			if (useCharacterCOMOffset) {
				stopResetCOMRotationCoroutine ();
			}

			if (!isFirstPersonActive) {
				if (keepWeaponsOnWallRunning) {
					carryingWeaponsPreviously = mainPlayerWeaponsManager.isPlayerCarringWeapon ();

					if (carryingWeaponsPreviously) {
						mainPlayerWeaponsManager.checkIfDisableCurrentWeapon ();
					}
					
					mainPlayerWeaponsManager.setGeneralWeaponsInputActiveState (false);
				}
			}
		} else {
			if (isFirstPersonActive) {
				eventOnWallRunningEndFirstPerson.Invoke ();
			} else {
				eventOnWallRunningEndThirdPerson.Invoke ();

				mainAnimator.SetBool (actionActiveAnimatorID, state);
			}

			mainPlayerCamera.setOriginalchangeCameraViewEnabledValue ();

			if (useCharacterCOMOffset) {
				resetCOMRotation ();
			}

			if (keepWeaponsOnWallRunning) {
				mainPlayerWeaponsManager.setGeneralWeaponsInputActiveState (true);
			}

			if (carryingWeaponsPreviously) {
				if (!drawWeaponsIfCarriedPreviously) {
					carryingWeaponsPreviously = false;
				}
			}
		}

		mainPlayerCamera.setCameraPositionMouseWheelEnabledState (!wallRunningActive);

		mainPlayerController.stepManager.setWallRunningState (wallRunningActive, wallRunningOnRightSide);

		mainPlayerController.setLastTimeFalling ();

		if (setNewCameraStateOnThirdPerson && !isFirstPersonActive) {
			if (state) {
				previousCameraState = mainPlayerCamera.getCurrentStateName ();

				if (wallRunningOnRightSide) {
					mainPlayerCamera.setCameraStateOnlyOnThirdPerson (newCameraStateOnThirdPersonRightSide);
				} else {
					mainPlayerCamera.setCameraStateOnlyOnThirdPerson (newCameraStateOnThirdPersonLeftSide);
				}
			} else {
				
				if (previousCameraState != "") {
					if (wallRunningOnRightSide) {
						if (previousCameraState != newCameraStateOnThirdPersonRightSide) {
							mainPlayerCamera.setCameraStateOnlyOnThirdPerson (previousCameraState);
						}
					} else {
						if (previousCameraState != newCameraStateOnThirdPersonLeftSide) {
							mainPlayerCamera.setCameraStateOnlyOnThirdPerson (previousCameraState);
						}
					}

					previousCameraState = "";
				}
			}
		}
	}

	public override void setExternalForceEnabledState (bool state)
	{
		setWallRunningEnabledState (state);
	}

	public void setWallRunningEnabledState (bool state)
	{
		if (!state) {
			if (wallRunningActive) {
				setWallRunningActiveState (false);

				setWallRunningImpulseForce (wallRunningStopMovemmentImpulse, false);

				previouslyWallRunningOnLeftSide = false;
				previouslyWallRunningOnRightSide = false;
				wallRunningOnRightSide = false;
				wallRunningOnLeftSide = false;
			}
		}

		wallRunningEnabled = state;
	}

	public void setWallRunningCanBeUsedState (bool state)
	{
		wallRunningCanBeUsed = state;
	}

	public void setOriginalWallRunningEnabledState ()
	{
		setWallRunningEnabledState (originalWallRunningEnabled);
	}

	public void setAutoUseDownMovementOnWallRunningActiveState (bool state)
	{
		autoUseDownMovementOnWallRunningActive = state;
	}

	public void setAutoUseStopWallRunningAfterDelayState (bool state)
	{
		autoUseStopWallRunningAfterDelay = state;
	}

	public void resetCOMRotation ()
	{
		stopResetCOMRotationCoroutine ();

		resetCOMCoroutine = StartCoroutine (resetCOMRotationCoroutine ());
	}

	void stopResetCOMRotationCoroutine ()
	{
		if (resetCOMCoroutine != null) {
			StopCoroutine (resetCOMCoroutine);
		}
	}

	public IEnumerator resetCOMRotationCoroutine ()
	{
		bool targetReached = false;

		float movementTimer = 0;

		float t = 0;

		float duration = 0.5f;

		float angleDifference = 0;

		float positionDifference = 0;

		Vector3 targetPosition = Vector3.up * originalCOMYPosition;
		Quaternion targetRotation = Quaternion.identity;

		while (!targetReached) {
			t += Time.deltaTime / duration; 
			COM.localPosition = Vector3.Slerp (COM.localPosition, targetPosition, t);
			COM.localRotation = Quaternion.Slerp (COM.localRotation, targetRotation, t);

			angleDifference = Quaternion.Angle (COM.localRotation, targetRotation);

			positionDifference = GKC_Utils.distance (COM.localPosition, targetPosition);

			movementTimer += Time.deltaTime;

			if ((positionDifference < 0.01f && angleDifference < 0.2f) || movementTimer > (duration + 1)) {
				targetReached = true;
			}
			yield return null;
		}

		if (drawWeaponsIfCarriedPreviously) {
			yield return new WaitForSeconds (1);

			if (carryingWeaponsPreviously) {
				if (!mainPlayerController.isPlayerOnFirstPerson () && !wallRunningActive && mainPlayerController.canPlayerMove ()) {
					mainPlayerWeaponsManager.checkIfDrawSingleOrDualWeapon ();
				}

				carryingWeaponsPreviously = false;
			}
		}
	}
}
