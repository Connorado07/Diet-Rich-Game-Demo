using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class swimSystem : externalControllerBehavior
{
	[Header ("Main Settings")]
	[Space]

	public bool swimSystemEnabled = true;

	public LayerMask layerForGroundDetection;

	public float groundDetectionRaycastDistance = 1;
	public float groundDetectionRaycastDistanceOnWater = 0.1f;

	public float maxDistanceToWalkOnWater = 1;

	[Space]
	[Header ("Animation Settings")]
	[Space]

	public int swimID;

	public int regularID = 1;

	public int swimStateID = 0;
	public int diveStateID = 1;

	public int movingUpStateID = 1;
	public int movingDownStateID = 1;

	[Space]
	[Header ("Physics Settings")]
	[Space]

	public bool useForceMode;

	public ForceMode swimForceMode;

	public bool moveUpAndDownEnabled;

	public bool turboEnabled;

	public float waterFriction;

	public float maxSwimVelocity;

	public float swimTurboSpeed;

	public float swimForce;

	public float swimMoveUpSpeed;

	public float swimMoveDownSpeed;

	public float swimSpeedOnAimMultiplier;

	public float autoAdjustDistanceToSwimSurfaceSpeed = 2;

	public float autoAdjustDistanceToSwimSurface = 1.6f;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public float minDistanceForSwimSurface = 0.2f;

	public bool setInitialStateOnSurfaceOnEnterWater = true;

	public bool useMaxSpeedToMoveToSurfaceOnSwimEnabled;

	public float maxSpeedToMoveToSurfaceOnSwimEnabled = 20;

	public float minSpeedToSetSwimOnSurfaceState = 5;

	public bool cancelReturnToSurfaceIfInputPressed;

	[Space]
	[Header ("Jump Settings")]
	[Space]

	public bool canJumpWhenSwimmingOnSurface;
	public float jumpForceOnSurface;

	public bool onlyClimbOnCloseSurfaces;

	public float maxAngleDifferenceOnSurfaceToClimb = 90;

	public float maxClimbSurfaceHeight = 4;

	public bool allowJumpOnSwimIfNoCloseSurfaceToClimbFound;

	public Transform climbSurfaceActionSystemTransform;

	[Space]

	public eventParameters.eventToCallWithGameObject eventToActivateClimbSurfaceActionSystem;

	[Space]
	[Header ("Swim Settings")]
	[Space]

	public float swimVerticalSpeedMultiplier;

	public float swimHorizontalSpeedMultiplier;

	[Space]
	[Header ("Dive Settings")]
	[Space]

	public float diveVerticalSpeedMultiplier;
	public float diveHorizontalSpeedMultiplier;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;

	public bool swimSystemActive;

	public bool turboActive;

	public bool movingUp;
	public bool movingDown;

	public bool currentlyOnSurface;
	public bool currentlyDiving;

	public bool overridingMovementToSurfaceActive;

	public Vector3 targetDirection;

	public float currentHorizontalMovement;
	public float currentVerticalMovement;

	public bool checkIfCloseToSurface;

	public bool playerIsMoving;

	public float currentDistanceToSurface;

	public bool checkingToActivateMovementToSurfaceAutomaticallyActive;

	public int currentStatID;

	public bool walkingOnWater;

	public Vector3 totalForce;

	public Vector3 extraForce;

	public bool cameraBelowWater;

	[Space]
	[Header ("Events Settings")]
	[Space]

	public UnityEvent eventOnStateEnabled;
	public UnityEvent eventOnStateDisabled;

	public bool useEventsOnDiveStateChange;
	public UnityEvent eventOnStartDive;
	public UnityEvent eventOnEndDive;

	public UnityEvent eventBeforeActivatingSwimSystem;
	public UnityEvent eventBeforeDeactivatingSwimSystem;

	public bool useEventOnCameraUnderwaterEffect;
	public UnityEvent eventOnCameraUnderWaterStart;
	public UnityEvent eventOnCameraUnderWaterEnd;

	public bool useEventUseTurboOnSwim;
	public UnityEvent eventOnStarTurboOnSwim;
	public UnityEvent eventOnEndTurboOnSwim;

	[Space]
	[Header ("Components")]
	[Space]

	public playerController mainPlayerController;

	public Transform playerTransform;

	public Transform mainCameraTransform;

	public Transform playerCameraTransform;

	public Rigidbody mainRigidbody;

	public playerInputManager playerInput;

	public headTrack mainHeadTrack;

	public Collider mainPlayerCollider;

	public Collider swimCollider;

	float currentAimSpeedMultipler;

	float velocityMagnitude;

	Transform currentSwimZoneTransform;

	float lastTimeSwimActive;

	bool lookingInCameraDirection;

	bool turnAndForwardAnimatorValuesPaused;

	bool checkingDivingDirection;

	bool enteringOnWaterFromWalking;

	float lastTimeWalkingOnWaterActive;

	bool setMovementToSurfaceOnEnterWaterActive;

	bool canUseTurboPausedState;


	public override void updateControllerBehavior ()
	{
		//if the player is swimming
		if (swimSystemActive) {
			if (mainPlayerController.isPlayerDead ()) {
				setSwimSystemActivestate (false);

				return;
			}

			currentDistanceToSurface = Mathf.Abs (playerTransform.InverseTransformPoint (currentSwimZoneTransform.position).y);

			if (walkingOnWater) {
				if (!mainPlayerController.isPlayerOnGround () && (lastTimeWalkingOnWaterActive == 0 || Time.time > lastTimeWalkingOnWaterActive + 1)) {
					enableOrDisableSwimState (true);

					walkingOnWater = false;

					lastTimeWalkingOnWaterActive = 0;
				} else {
					if (currentDistanceToSurface > maxDistanceToWalkOnWater) {
						enteringOnWaterFromWalking = true;

						walkingOnWater = false;

						enableOrDisableSwimState (true);
					}
				}
			} else {
				updateSwimActiveState ();
					
				checkIfGroundDetectedOnSwimActiveState ();

				if (useEventOnCameraUnderwaterEffect) {

					if (mainCameraTransform.position.y < currentSwimZoneTransform.position.y) {
						if (!cameraBelowWater) {
							eventOnCameraUnderWaterStart.Invoke ();

							cameraBelowWater = true;
						}
					} else {
						if (cameraBelowWater) {
							eventOnCameraUnderWaterEnd.Invoke ();

							cameraBelowWater = false;
						}
					}
				}
			}
		}
	}

	void updateSwimActiveState ()
	{
		targetDirection = Vector3.zero;

		currentVerticalMovement = mainPlayerController.getVerticalInput ();
		currentHorizontalMovement = mainPlayerController.getHorizontalInput ();

		if (overridingMovementToSurfaceActive) {
			currentVerticalMovement = 0;
			currentHorizontalMovement = 0;

			checkIfCloseToSurface = true;

			playerInput.overrideInputValues (new Vector2 (0, 1), true);

			if (cancelReturnToSurfaceIfInputPressed) {
				if (playerInput.getRealMovementAxisInputAnyType () != Vector2.zero) {
					enableOrDisableVerticalMovementUp (false);

					overridingMovementToSurfaceActive = false;

					checkIfCloseToSurface = false;

					setDivingState (true);

					if (showDebugPrint) {
						print ("cancel movement to surface");
					}
				}
			}
		} else {
			if (movingUp) {
				checkIfCloseToSurface = true;

				currentVerticalMovement = 0;
				currentHorizontalMovement = 0;

				if (lookingInCameraDirection) {
					playerInput.overrideInputValues (new Vector2 (0, 1), true);
				} else {
					playerInput.overrideInputValues (new Vector2 (0, -1), true);
				}
			} else {
				checkIfCloseToSurface = false;
			}

			if (movingDown) {
				currentVerticalMovement = 0;
				currentHorizontalMovement = 0;

				if (lookingInCameraDirection) {
					playerInput.overrideInputValues (new Vector2 (0, 1), true);
				} else {
					playerInput.overrideInputValues (new Vector2 (0, -1), true);
				}
			}
		}

		if (checkIfCloseToSurface) {
			bool canCheckDistance = true;

			if (checkingToActivateMovementToSurfaceAutomaticallyActive) {
				if (Time.time > lastTimeSwimActive + 1) {
					checkingToActivateMovementToSurfaceAutomaticallyActive = false;
				} else {
					canCheckDistance = false;

//					if (showDebugPrint) {
//						print ("waiting to check close to surface");
//					}
				}
			}

			if (canCheckDistance && currentDistanceToSurface < minDistanceForSwimSurface) {
				if (showDebugPrint) {
					print ("close to surface reached");
				}

				enableOrDisableVerticalMovementUp (false);

				overridingMovementToSurfaceActive = false;

				checkIfCloseToSurface = false;

				setDivingState (false);
			}
		}

		if (currentlyDiving) {
			targetDirection = 
				mainCameraTransform.forward * (currentVerticalMovement * diveVerticalSpeedMultiplier) +
			mainCameraTransform.right * (currentHorizontalMovement * diveHorizontalSpeedMultiplier);
		} 

		if (currentlyOnSurface) {
			targetDirection = 
				playerCameraTransform.forward * (currentVerticalMovement * swimVerticalSpeedMultiplier) +
			playerCameraTransform.right * (currentHorizontalMovement * swimHorizontalSpeedMultiplier);
		}

		if (targetDirection.magnitude > 1) {
			targetDirection.Normalize ();
		}

		playerIsMoving = mainPlayerController.isPlayerMoving (0.1f);

		if (!mainPlayerController.isPlayerOnFirstPerson () && !mainPlayerController.isPlayerRotatingToSurface ()) {
			if (mainPlayerController.isPlayerMovingOn3dWorld ()) {
				if (playerIsMoving) {
					float currentLookAngle = 0;

					if (!mainPlayerController.isPlayerAiming ()) {
						if (currentlyDiving && !movingDown && !movingUp) {
							currentLookAngle = Vector3.SignedAngle (playerCameraTransform.forward, mainCameraTransform.forward, playerCameraTransform.right);

							if (currentLookAngle > 45) {
								currentStatID = movingDownStateID;
							} else if (currentLookAngle < -45) {
								currentStatID = movingUpStateID;
							} else {
								currentStatID = diveStateID;
							}

							mainPlayerController.setCurrentIdleIDValue (currentStatID);

							checkingDivingDirection = true;
						} else {
							if (currentlyOnSurface && movingUp) {
								mainPlayerController.setCurrentIdleIDValue (movingUpStateID);

								checkingDivingDirection = true;
							}
						}
					}
				} else {
					if (currentlyDiving) {
						if (checkingDivingDirection) {
							updateCurrentStateIDValue ();
						}
					}

					checkingDivingDirection = false;
				}
			}
		}

		currentAimSpeedMultipler = 1;

		if (!mainPlayerController.isPlayerOnFirstPerson () && mainPlayerController.isPlayerAiming ()) {
			currentAimSpeedMultipler = swimSpeedOnAimMultiplier;

			if (!turnAndForwardAnimatorValuesPaused) {
				mainPlayerController.setTurnAndForwardAnimatorValuesPausedState (true);

				turnAndForwardAnimatorValuesPaused = true;
			}
		} else {
			if (turnAndForwardAnimatorValuesPaused) {
				mainPlayerController.setTurnAndForwardAnimatorValuesPausedState (false);

				turnAndForwardAnimatorValuesPaused = false;
			}
		}

		totalForce = targetDirection * (swimForce * currentAimSpeedMultipler);

		if (movingUp) {
			totalForce += playerTransform.up * swimMoveUpSpeed;

			playerIsMoving = true;
		}

		if (movingDown) {
			totalForce -= playerTransform.up * swimMoveDownSpeed;

			playerIsMoving = true;
		}

		extraForce = Vector3.zero;

		if (currentlyOnSurface) {
			if (setMovementToSurfaceOnEnterWaterActive) {
				if (Time.time < lastTimeSwimActive + 1) {
					mainRigidbody.velocity = Vector3.MoveTowards (mainRigidbody.velocity, Vector3.zero, Time.deltaTime * 10);
				} else {
					setMovementToSurfaceOnEnterWaterActive = false;
				}
			} else {
				if (currentDistanceToSurface < autoAdjustDistanceToSwimSurface) {
					extraForce = -playerTransform.up * autoAdjustDistanceToSwimSurfaceSpeed;
				} else {
					if (Time.time > lastTimeSwimActive + 3) {
						if (currentDistanceToSurface > autoAdjustDistanceToSwimSurface + 0.2f) {
							extraForce = playerTransform.up * autoAdjustDistanceToSwimSurfaceSpeed;
						}
					}
				}
			}
		}

		if (playerIsMoving) {
			totalForce += extraForce;

			if (turboActive) {
				totalForce *= swimTurboSpeed;
			}

			velocityMagnitude = totalForce.magnitude;

			if (velocityMagnitude > maxSwimVelocity) {
				totalForce = Vector3.ClampMagnitude (totalForce, maxSwimVelocity);
			}

			velocityMagnitude = totalForce.magnitude;

			if (useForceMode) {
				mainRigidbody.AddForce (totalForce, swimForceMode);
			} else {
				mainRigidbody.AddForce (totalForce);
			}
		} else {
			velocityMagnitude = mainRigidbody.velocity.magnitude;

			if (velocityMagnitude > 0) {
				totalForce = mainRigidbody.velocity * (-1 * waterFriction);
			}

			totalForce += extraForce;

			mainRigidbody.AddForce (totalForce, swimForceMode);
		}
	}

	void checkIfGroundDetectedOnSwimActiveState ()
	{
		if (currentDistanceToSurface < maxDistanceToWalkOnWater) {
			if (Physics.Raycast (playerTransform.position + (playerTransform.up * 0.05f), -playerTransform.up, groundDetectionRaycastDistanceOnWater, layerForGroundDetection)) {
				lastTimeWalkingOnWaterActive = Time.time;

				enteringOnWaterFromWalking = true;

				walkingOnWater = true;

				enableOrDisableSwimState (false);
			}
		}
	}

	public void setSwimSystemActivestate (bool state)
	{
		if (!swimSystemEnabled) {
			return;
		}

		if (swimSystemActive == state) {
			return;
		}

		if (state && mainPlayerController.isPlayerDead ()) {
			return;
		}

		bool swimModeActivePrevioulsy = swimSystemActive;

		swimSystemActive = state;

		behaviorCurrentlyActive = state;

		if (swimSystemActive) {
			mainPlayerController.setExternalControllerBehavior (this);
		} else {
			if (swimModeActivePrevioulsy) {
				externalControllerBehavior currentExternalControllerBehavior = mainPlayerController.getCurrentExternalControllerBehavior ();

				if (currentExternalControllerBehavior == null || currentExternalControllerBehavior == this) {
					mainPlayerController.setExternalControllerBehavior (null);
				}
			}
		}

		bool canSetSwimState = true;

		walkingOnWater = false;

		setMovementToSurfaceOnEnterWaterActive = false;

		if (state) {
			if (Physics.Raycast (playerTransform.position, -playerTransform.up, groundDetectionRaycastDistance, layerForGroundDetection)) {
				canSetSwimState = false;

				walkingOnWater = true;
			}
		} else {
			canSetSwimState = true;
		}

		if (canSetSwimState) {
			enableOrDisableSwimState (state);
		}

		if (swimSystemActive) {
			Physics.IgnoreCollision (mainPlayerCollider, swimCollider, true);
		} 

		swimCollider.enabled = state;
	}

	void enableOrDisableSwimState (bool state)
	{		
		if (state) {
			eventBeforeActivatingSwimSystem.Invoke ();
		} else {
			eventBeforeDeactivatingSwimSystem.Invoke ();
		}

		mainPlayerController.enableOrDisableSwimMode (state);

		if (!state) {
			mainPlayerController.setLastTimeFalling ();
		}

		if (state) {
			mainPlayerController.setCurrentAirIDValue (swimID);
		} else {
			mainPlayerController.setCurrentAirIDValue (regularID);

			mainPlayerController.setCurrentAirSpeedValue (1);
		}

		if (swimSystemActive) {
			eventOnStateEnabled.Invoke ();

			if (mainHeadTrack != null) {
				mainHeadTrack.setSmoothHeadTrackDisableState (true);
			}
		} else {
			eventOnStateDisabled.Invoke ();
		
			if (turboActive) {
				enableOrDisableTurbo (false);
			}

			enableOrDisableVerticalMovementUp (false);

			enableOrDisableVerticalMovementDown (false);

			if (mainHeadTrack != null) {
				mainHeadTrack.setSmoothHeadTrackDisableState (false);
			}
		}

		mainPlayerController.stopShakeCamera ();

		movingUp = false;

		movingDown = false;

		if (currentlyDiving) {
			setDivingState (false);
		}

		if (swimSystemActive) {
			bool checkIfDivingState = true;

			float lastTimeResurrect = mainPlayerController.getLastTimeResurrect ();

			bool ignoreDivingCheck = false;

			if (lastTimeResurrect > 0 && Time.time < lastTimeResurrect + 3f) {
				enteringOnWaterFromWalking = false;

				ignoreDivingCheck = true;
			}

			currentDistanceToSurface = Mathf.Abs (playerTransform.InverseTransformPoint (currentSwimZoneTransform.position).y);

			float verticalSpeed = playerTransform.InverseTransformDirection (mainRigidbody.velocity).y;

			if (currentDistanceToSurface > 2 && Mathf.Abs (verticalSpeed) < 1) {
				enteringOnWaterFromWalking = false;

				ignoreDivingCheck = true;

				if (showDebugPrint) {
					print ("entering on deep water on low speed, setting dive state");
				}
			}

			if (enteringOnWaterFromWalking) {
				overridingMovementToSurfaceActive = false;

				checkIfCloseToSurface = false;

				setDivingState (false);

				checkIfDivingState = false;

				if (showDebugPrint) {
					print ("entering on water while walking");
				}
			} 

			if (setInitialStateOnSurfaceOnEnterWater && checkIfDivingState && !ignoreDivingCheck) {
				if (useMaxSpeedToMoveToSurfaceOnSwimEnabled) {
					verticalSpeed = playerTransform.InverseTransformDirection (mainRigidbody.velocity).y;

					float distanceToSurfaceWithSign = playerTransform.InverseTransformPoint (currentSwimZoneTransform.position).y;

					if (showDebugPrint) {
						print (distanceToSurfaceWithSign);
					}

					currentDistanceToSurface = Mathf.Abs (distanceToSurfaceWithSign);

					if (showDebugPrint) {
						print ("vertical speed " + verticalSpeed);
					}

					if (Mathf.Abs (verticalSpeed) < minSpeedToSetSwimOnSurfaceState) {
						overridingMovementToSurfaceActive = false;

						checkIfCloseToSurface = false;

						setDivingState (false);

						checkIfDivingState = false;

						if (showDebugPrint) {
							print ("starting on suface " + verticalSpeed);
						}

						setMovementToSurfaceOnEnterWaterActive = true;
					} else {

						if (Mathf.Abs (verticalSpeed) < maxSpeedToMoveToSurfaceOnSwimEnabled &&
						    currentDistanceToSurface < 5) {

							if (showDebugPrint) {
								print ("return to surface");
							}

							checkingToActivateMovementToSurfaceAutomaticallyActive = true;

							overridingMovementToSurfaceActive = true;

							enableOrDisableVerticalMovementUp (true);

							checkIfDivingState = false;
						} 
					}
				}
			}
		
			if (checkIfDivingState) {
				verticalSpeed = playerTransform.InverseTransformDirection (mainRigidbody.velocity).y;

				if (Mathf.Abs (verticalSpeed) > maxSpeedToMoveToSurfaceOnSwimEnabled) {
					setDivingState (true);

					if (showDebugPrint) {
						print ("setting diving state");
					}
				} else {
					currentDistanceToSurface = Mathf.Abs (playerTransform.InverseTransformPoint (currentSwimZoneTransform.position).y);

					if (currentDistanceToSurface > 3) {
						setDivingState (true);

						if (showDebugPrint) {
							print ("setting diving state");
						}
					}
				}
			}

			lastTimeSwimActive = Time.time;
		} else {
			overridingMovementToSurfaceActive = false;

			currentlyOnSurface = false;
		}

		if (turnAndForwardAnimatorValuesPaused) {
			mainPlayerController.setTurnAndForwardAnimatorValuesPausedState (false);

			turnAndForwardAnimatorValuesPaused = false;
		}

		updateCurrentStateIDValue ();

		enteringOnWaterFromWalking = false;

		if (!swimSystemActive) {
			if (useEventOnCameraUnderwaterEffect) {
				if (cameraBelowWater) {
					eventOnCameraUnderWaterEnd.Invoke ();

					cameraBelowWater = false;
				}
			}
		}
	}

	public void enableOrDisableTurbo (bool state)
	{
		if (turboActive == state) {
			return;
		}

		if (state && canUseTurboPausedState) {
			return;
		}

		turboActive = state;

		mainPlayerController.enableOrDisableFlyModeTurbo (turboActive);

		if (turboActive) {
			mainPlayerController.setCurrentAirSpeedValue (2);
		} else {
			mainPlayerController.setCurrentAirSpeedValue (1);
		}

		if (useEventUseTurboOnSwim) {
			if (state) {
				eventOnStarTurboOnSwim.Invoke ();
			} else {
				eventOnEndTurboOnSwim.Invoke ();
			}
		}
	}

	public void setCanUseTurboPausedState (bool state)
	{
		if (!state && turboActive) {
			enableOrDisableTurbo (false);
		}

		canUseTurboPausedState = state;
	}

	public override void disableExternalControllerState ()
	{
		setSwimSystemActivestate (false);
	}

	public void inputChangeTurboState (bool state)
	{
		if (overridingMovementToSurfaceActive) {
			return;
		}

		if (swimSystemActive && turboEnabled) {
			enableOrDisableTurbo (state);
		}
	}

	public void inputMoveUp (bool state)
	{
		if (overridingMovementToSurfaceActive) {
			return;
		}

		if (swimSystemActive && moveUpAndDownEnabled) {
			if (currentlyOnSurface && state) {
				if (canJumpWhenSwimmingOnSurface) {
					bool activateRegularJump = true;

					bool surfaceToLedgeDetected = false;

					if (onlyClimbOnCloseSurfaces) {
						Vector3 raycastPosition = playerTransform.position + (playerTransform.up * 1.5f);
						Vector3 raycastDirection = playerTransform.forward;

						Vector3 climbSurfaceTargetPosition = Vector3.zero;

						RaycastHit hit = new RaycastHit ();

						if (Physics.Raycast (raycastPosition, raycastDirection, out hit, 0.5f, layerForGroundDetection)) {

							float angleWithSurface = Vector3.SignedAngle (playerTransform.up, hit.normal, playerTransform.right);

							bool surfaceAngleNotValid = false;

							float angleWithSurfaceAux = Mathf.Abs (Mathf.Abs (angleWithSurface) - 90);

							if (showDebugPrint) {
								print ("angle with surface " + angleWithSurface + " " + angleWithSurfaceAux);
							}

							if (angleWithSurfaceAux > maxAngleDifferenceOnSurfaceToClimb) {
								surfaceAngleNotValid = true;
							}
								
							if (!surfaceAngleNotValid) {
								//if not surface is found, then
								Debug.DrawRay (raycastPosition, raycastDirection, Color.green);

								//search for the closest point surface of that ledge, by lowering the raycast position until a surface is found
								surfaceToLedgeDetected = false;

								RaycastHit newHit = new RaycastHit ();

								int numberOfLoops = 0;

								Vector3 newRaycastPosition = raycastPosition;

								while (!surfaceToLedgeDetected && numberOfLoops < 100) {

									Debug.DrawRay (newRaycastPosition, raycastDirection, Color.blue, 4);

									if (Physics.Raycast (newRaycastPosition, raycastDirection, out newHit, 2, layerForGroundDetection)) {
										hit = newHit;

										newRaycastPosition += playerTransform.up * 0.04f;
									} else {
										climbSurfaceTargetPosition = hit.point + playerTransform.up * 0.04f;

										surfaceToLedgeDetected = true;
									}

									numberOfLoops++;
								}

								if (GKC_Utils.distance (raycastPosition, climbSurfaceTargetPosition) > maxClimbSurfaceHeight) {
									surfaceToLedgeDetected = false;

									if (showDebugPrint) {
										print ("surface to climb detected too far away");
									}
								}

								if (surfaceToLedgeDetected) {
									climbSurfaceActionSystemTransform.position = climbSurfaceTargetPosition;
									climbSurfaceActionSystemTransform.rotation = Quaternion.LookRotation (-hit.normal);

									climbSurfaceActionSystemTransform.gameObject.SetActive (true);

									eventToActivateClimbSurfaceActionSystem.Invoke (playerTransform.gameObject);

									if (showDebugPrint) {
										print ("surface to climb detected");
									}
								} else {
									if (showDebugPrint) {
										print ("surface to climb not detected");
									}
								}
							} 
						} else {
							if (showDebugPrint) {
								print ("surface to climb not detected");
							}
						}

						if (surfaceToLedgeDetected) {
							activateRegularJump = false;
						} else {
							if (!allowJumpOnSwimIfNoCloseSurfaceToClimbFound) {
								activateRegularJump = false;
							}
						}
					}

					if (activateRegularJump) {
						mainPlayerController.useJumpPlatform (playerTransform.up * jumpForceOnSurface, ForceMode.Impulse);

						return;
					}
				}
			}

			enableOrDisableVerticalMovementUp (state);
		}
	}

	public void inputMoveDown (bool state)
	{
		if (overridingMovementToSurfaceActive) {
			return;
		}

		if (swimSystemActive && moveUpAndDownEnabled) {
			enableOrDisableVerticalMovementDown (state);

			if (!currentlyDiving) {
				setDivingState (true);
			}
		}
	}

	void enableOrDisableVerticalMovementUp (bool state)
	{
		if (movingUp && !state) {
			playerInput.overrideInputValues (Vector3.zero, false);
		}

		movingUp = state;

		if (movingUp) {
			checkIfLookingInCameraDirection ();
		
			movingDown = false;
		}

		updateCurrentStateIDValue ();
	}

	void enableOrDisableVerticalMovementDown (bool state)
	{
		if (movingDown && !state) {
			playerInput.overrideInputValues (Vector3.zero, false);
		}

		movingDown = state;

		if (movingDown) {
			checkIfLookingInCameraDirection ();

			movingUp = false;
		}

		updateCurrentStateIDValue ();
	}

	void checkIfLookingInCameraDirection ()
	{
		float angleWithCamera = Vector3.Angle (playerTransform.forward, playerCameraTransform.forward);

		if (angleWithCamera < 90) {
			lookingInCameraDirection = true;
		} else {
			lookingInCameraDirection = false;
		}

		if (showDebugPrint) {
			print (angleWithCamera + " " + lookingInCameraDirection);
		}
	}

	public void setSwimZoneTransform (Transform newSwimZoneTransform)
	{
		currentSwimZoneTransform = newSwimZoneTransform;
	}

	public void setDivingState (bool state)
	{
		currentlyDiving = state;

		currentlyOnSurface = !currentlyDiving;

		if (useEventsOnDiveStateChange) {
			if (currentlyDiving) {
				eventOnStartDive.Invoke ();
			} else {
				eventOnEndDive.Invoke ();
			}
		}

		updateCurrentStateIDValue ();
	}

	void updateCurrentStateIDValue ()
	{
		currentStatID = -1;

		if (swimSystemActive) {
			if (currentlyDiving) {
				if (movingUp) {
					currentStatID = movingUpStateID;
				} else if (movingDown) {
					currentStatID = movingDownStateID;
				} else {
					currentStatID = diveStateID;
				}
			} 

			if (currentlyOnSurface) {
				currentStatID = swimStateID;
			}
		} else {
			currentStatID = 0;
		}

		mainPlayerController.setCurrentIdleIDValue (currentStatID);
	}
}
