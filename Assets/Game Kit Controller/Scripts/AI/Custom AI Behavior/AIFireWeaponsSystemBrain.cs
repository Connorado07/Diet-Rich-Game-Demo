using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIFireWeaponsSystemBrain : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool weaponsSystemEnabled = true;

	public bool drawWeaponsWhenResumingAI;

	public bool drawWeaponWhenAttackModeSelected;

	public bool keepWeaponsIfNotTargetsToAttack;

	[Space]
	[Header ("Attack Settings")]
	[Space]

	public bool attackEnabled;

	public float fireWeaponAttackRate = 0.17f;

	[Space]
	[Header ("Roll/Dodge Settings")]
	[Space]

	public bool rollEnabled;

	public Vector2 randomRollWaitTime;

	public float minWaitTimeAfterRollActive = 1.3f;

	public List<Vector2> rollMovementDirectionList = new List<Vector2> ();

	[Space]
	[Header ("Random Walk Settings")]
	[Space]

	public bool randomWalkEnabled;

	public Vector2 randomWalkWaitTime;
	public Vector2 randomWalkDuration;
	public Vector2 randomWalkRadius;

	[Space]
	[Header ("Search Weapons On Scene Settings")]
	[Space]

	public bool searchWeaponsPickupsOnLevelIfNoWeaponsAvailable;

	public bool useEventOnNoWeaponToPickFromScene;
	public UnityEvent eventOnNoWeaponToPickFromScene;

	public bool useEventsOnNoWeaponsAvailable;
	public UnityEvent eventOnNoWeaponsAvailable;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;

	public bool weaponsSystemActive;

	public bool weaponEquiped;

	public bool aimingWeapon;

	public bool waitingForAttackActive;
	float currentRandomTimeToAttack;

	public bool walkActive;
	public bool waitingWalkActive;

	public bool waitingRollActive;

	public bool canUseAttackActive;

	public bool attackStatePaused;

	public bool insideMinDistanceToAttack;

	public bool searchingWeapon;
	public bool characterHasWeapons;

	GameObject currentWeaponToGet;

	[Space]
	[Header ("Events Settings")]
	[Space]

	public bool useEventsOnCombatActive;
	public UnityEvent eventOnCombatActive;
	public UnityEvent eventOnCombatDeactivate;

	[Space]
	[Header ("Components")]
	[Space]

	public playerWeaponsManager mainPlayerWeaponsManager;
	public dashSystem mainDashSystem;
	public findObjectivesSystem mainFindObjectivesSystem;
	public AINavMesh mainAINavmeshManager;

	float lastTimeAttack;

	int currentAttackTypeIndex;

	bool weaponInfoStored;

	int currentAttackIndex;

	int currentAttackTypeToAlternateIndex;


	float lastTimeRollActive;

	float lastTimeWaitRollActive;

	float currentRollWaitTime;


	float lastTimeWaitWalkActive;
	float currentWalkTime;
	float lastTimeWalkActive;

	float currentWalkDuration;
	float currentWalkRadius;

	bool rollCoolDownActive;

	float currentPauseAttackStateDuration;
	float lastTimeAttackPauseWithDuration;

	bool checkIfDrawWeaponActive;

	float randomWaitTime;

	float lastTimeFireWeaponAttackAtDistance;

	bool checkIfAICarryingWeapon;

	float currentPathDistanceToTarget;
	float minDistanceToAim;
	float minDistanceToDraw;
	float minDistanceToShoot;

	bool checkIfSearchWeapon;

	bool AIPaused;

	bool cancelCheckAttackState;


	public void updateAI ()
	{
		if (weaponsSystemActive) {
			AIPaused = mainFindObjectivesSystem.isAIPaused ();

			if (!AIPaused) {
				if (!checkIfAICarryingWeapon) {
					if (mainPlayerWeaponsManager.isPlayerCarringWeapon ()) {
						weaponEquiped = true;
					}

					checkIfAICarryingWeapon = true;
				}

				if (walkActive) {
					if (Time.time > lastTimeWalkActive + currentWalkDuration || mainFindObjectivesSystem.getRemainingDistanceToTarget () < 0.5f) {
						resetRandomWalkState ();
					}
				}

				if (searchingWeapon) {
					if (currentWeaponToGet != null) {
						return;
					}

					if (currentWeaponToGet == null) {
						characterHasWeapons = mainPlayerWeaponsManager.checkIfWeaponsAvailable () ||
						mainPlayerWeaponsManager.checkIfUsableWeaponsPrefabListActive ();

						mainFindObjectivesSystem.setSearchigWeaponState (false);

						searchingWeapon = false;

						checkIfSearchWeapon = false;
					}
				}
			}
		}
	}

	public void resetRandomWalkState ()
	{
		mainFindObjectivesSystem.setRandomWalkPositionState (0);

		waitingWalkActive = false;

		walkActive = false;

		lastTimeWalkActive = Time.time;
	}

	public void resetRollState ()
	{
		waitingRollActive = false;

		lastTimeRollActive = Time.time;
	}

	public void resetStates ()
	{
		resetRandomWalkState ();

		resetRollState ();
	}

	public void checkIfResetStatsOnRandomWalk ()
	{
		if (walkActive) {
			resetStates ();
		}
	}

	public void checkRollState ()
	{
		if (rollEnabled) {

			if (walkActive) {
				return;
			}

			if (!insideMinDistanceToAttack) {
				resetRollState ();

				lastTimeRollActive = 0;

				return;
			}

			if (waitingRollActive) {
				if (Time.time > lastTimeWaitRollActive + currentRollWaitTime) {

					int randomRollMovementDirection = Random.Range (0, rollMovementDirectionList.Count - 1);

					mainDashSystem.activateDashStateWithCustomDirection (rollMovementDirectionList [randomRollMovementDirection]);

					resetRollState ();
				}
			} else {
				if (Time.time > lastTimeRollActive + randomWaitTime) {
					currentRollWaitTime = Random.Range (randomRollWaitTime.x, randomRollWaitTime.y);

					lastTimeWaitRollActive = Time.time;

					waitingRollActive = true;

					randomWaitTime = Random.Range (0.1f, 0.5f);
				}
			}
		}
	}

	public void checkWalkState ()
	{
		if (randomWalkEnabled) {

			rollCoolDownActive = Time.time < lastTimeRollActive + 0.7f;

			if (rollCoolDownActive) {
				return;
			}

			if (waitingWalkActive) {
				if (!walkActive) {

					if (Time.time > lastTimeWaitWalkActive + currentWalkTime) {
						mainFindObjectivesSystem.setRandomWalkPositionState (currentWalkRadius);

						lastTimeWalkActive = Time.time;

						walkActive = true;
					}
				}
			} else {
				currentWalkTime = Random.Range (randomWalkWaitTime.x, randomWalkWaitTime.y);

				lastTimeWaitWalkActive = Time.time;

				waitingWalkActive = true;

				currentWalkDuration = Random.Range (randomWalkDuration.x, randomWalkDuration.y);

				currentWalkRadius = Random.Range (randomWalkRadius.x, randomWalkRadius.y);

				walkActive = false;
			}
		}
	}

	public void updateInsideMinDistance (bool newInsideMinDistanceToAttack)
	{
		insideMinDistanceToAttack = newInsideMinDistanceToAttack;

		if (insideMinDistanceToAttack) {
			if (checkIfDrawWeaponActive) {
				if (!mainPlayerWeaponsManager.isPlayerCarringWeapon ()) {
					setDrawOrHolsterWeaponState (true);
				}

				checkIfDrawWeaponActive = false;
			}
		} else {
			if (aimingWeapon) {
				setAimWeaponState (false);
			}
		}
	}

	public void updateMainFireWeaponsBehavior ()
	{
		if (!weaponsSystemActive) {
			return;
		}

		if (AIPaused) {
			return;
		}

		checkWalkState ();

		if (walkActive) {
			return;
		}
			
		checkRollState ();

		if (rollEnabled) {
			if (Time.time < lastTimeRollActive + minWaitTimeAfterRollActive) {
				return;
			}
		}
			
//		if (showDebugPrint) {
//			print ("checking states");
//		}

		if (characterHasWeapons) {
			cancelCheckAttackState = false;

			if (mainFindObjectivesSystem.attackTargetDirectly) {
				mainFindObjectivesSystem.lookingAtTargetPosition = true;

				mainFindObjectivesSystem.lookAtCurrentPlaceToShoot ();

				if (!weaponEquiped) {
					setDrawOrHolsterWeaponState (true);
				} else {
					if (!aimingWeapon) {
						if (mainPlayerWeaponsManager.currentWeaponWithHandsInPosition () && mainPlayerWeaponsManager.isPlayerCarringWeapon () && !mainPlayerWeaponsManager.currentWeaponIsMoving ()) {
							setAimWeaponState (true);
						}
					}

					if (aimingWeapon) {
						if (!mainPlayerWeaponsManager.currentWeaponIsMoving () &&
						    mainPlayerWeaponsManager.reloadingActionNotActive () &&
						    mainPlayerWeaponsManager.isPlayerCarringWeapon () &&
						    mainFindObjectivesSystem.checkIfMinimumAngleToAttack () &&
						    !mainPlayerWeaponsManager.isActionActiveInPlayer () &&
						    mainPlayerWeaponsManager.canPlayerMove ()) {

							shootTarget ();
						}
					}
				}
			} else {
//				if (showDebugPrint) {
//					print ("looking at target");
//				}

				currentPathDistanceToTarget = mainFindObjectivesSystem.currentPathDistanceToTarget;
				minDistanceToAim = mainFindObjectivesSystem.minDistanceToAim;
				minDistanceToDraw = mainFindObjectivesSystem.minDistanceToDraw;
				minDistanceToShoot = mainFindObjectivesSystem.minDistanceToShoot; 

				bool useHalfMinDistance = mainAINavmeshManager.useHalfMinDistance;

				if (useHalfMinDistance) {
					if (aimingWeapon) {
						setAimWeaponState (false);
					}

					mainFindObjectivesSystem.lookingAtTargetPosition = false;

					cancelCheckAttackState = true;
				} else {

					if (currentPathDistanceToTarget <= minDistanceToAim) {
						if (!aimingWeapon) {
							if (mainPlayerWeaponsManager.currentWeaponWithHandsInPosition () && mainPlayerWeaponsManager.isPlayerCarringWeapon () && !mainPlayerWeaponsManager.currentWeaponIsMoving ()) {
								setAimWeaponState (true);
							}
						}

						mainFindObjectivesSystem.lookingAtTargetPosition = true;

						mainFindObjectivesSystem.lookAtCurrentPlaceToShoot ();
					} else {
						if (currentPathDistanceToTarget >= minDistanceToAim + 1.5f) {
							if (aimingWeapon) {
								setAimWeaponState (false);
							}

							mainFindObjectivesSystem.lookingAtTargetPosition = false;

							cancelCheckAttackState = true;
						} else {
							if (mainFindObjectivesSystem.lookingAtTargetPosition) {
								mainFindObjectivesSystem.lookAtCurrentPlaceToShoot ();
							}
						}
					}

					if (!weaponEquiped && currentPathDistanceToTarget <= minDistanceToDraw) {
						setDrawOrHolsterWeaponState (true);
					}
				}
			}
				
			checkAttackState ();
		} else {
			if (!searchingWeapon && !checkIfSearchWeapon) {

				characterHasWeapons = mainPlayerWeaponsManager.checkIfWeaponsAvailable () ||
				mainPlayerWeaponsManager.checkIfUsableWeaponsPrefabListActive ();

				//seach for the closest weapon
				if (!characterHasWeapons) {

					if (useEventsOnNoWeaponsAvailable) {
						eventOnNoWeaponsAvailable.Invoke ();
					}

					if (searchWeaponsPickupsOnLevelIfNoWeaponsAvailable) {
						searchingWeapon = true;

						mainFindObjectivesSystem.setSearchigWeaponState (true);

						bool weaponFound = false;

						pickUpObject[] pickupList = FindObjectsOfType (typeof(pickUpObject)) as pickUpObject[];

						for (int i = 0; i < pickupList.Length; i++) {
							if (!weaponFound) {

								weaponPickup currentWeaponPickup = pickupList [i].gameObject.GetComponent<weaponPickup> ();

								if (currentWeaponPickup != null) {
									if (mainPlayerWeaponsManager.checkIfWeaponCanBePicked (currentWeaponPickup.weaponName)) {
										currentWeaponToGet = pickupList [i].getPickupTrigger ().gameObject;

										mainAINavmeshManager.setTarget (pickupList [i].transform);

										mainAINavmeshManager.setTargetType (false, true);

										weaponFound = true;

										mainAINavmeshManager.lookAtTaget (false);
										//print (pickupList [i].secondaryString);
									}
								}
							}
						}

						if (!weaponFound) {
							if (useEventOnNoWeaponToPickFromScene) {
								eventOnNoWeaponToPickFromScene.Invoke ();
							}
						}
					} else {
						checkIfSearchWeapon = true;
					}

					//it will need to check if the weapon can be seen by the character and if it is can be reached by the navmesh
				}

				mainFindObjectivesSystem.lookingAtTargetPosition = false;
			}
		}
	}

	public void checkAttackState ()
	{
		if (!attackEnabled) {
			return;
		}

		if (!insideMinDistanceToAttack) {
			return;
		}

		if (currentPauseAttackStateDuration > 0) {
			if (Time.time > currentPauseAttackStateDuration + lastTimeAttackPauseWithDuration) {

				attackStatePaused = false;

				currentPauseAttackStateDuration = 0;
			} else {
				return;
			}
		}


		if (!canUseAttackActive) {
			return;
		}
			
		if (!aimingWeapon && !cancelCheckAttackState) {
			setAimWeaponState (true);
		}

//		if (showDebugPrint) {
//			print ("check to fire");
//		}

		if (Time.time > fireWeaponAttackRate + lastTimeFireWeaponAttackAtDistance) {
			if (weaponEquiped &&
			    aimingWeapon &&
			    currentPathDistanceToTarget <= minDistanceToShoot &&
			    !mainPlayerWeaponsManager.currentWeaponIsMoving () &&
			    mainPlayerWeaponsManager.reloadingActionNotActive () &&
			    mainPlayerWeaponsManager.isPlayerCarringWeapon () &&
			    mainFindObjectivesSystem.checkIfMinimumAngleToAttack () &&
			    !mainPlayerWeaponsManager.isActionActiveInPlayer () &&
			    mainPlayerWeaponsManager.canPlayerMove ()) {

				shootTarget ();
			
			}

			lastTimeFireWeaponAttackAtDistance = Time.time;
		}
	}

	public void updateMainFireWeaponsAttack (bool newCanUseAttackActiveState)
	{
		canUseAttackActive = newCanUseAttackActiveState;
	}

	public void setWeaponsSystemActiveState (bool state)
	{
		if (!weaponsSystemEnabled) {
			return;
		}

		weaponsSystemActive = state;

		checkEventsOnCombatStateChange (weaponsSystemActive);

		if (weaponsSystemActive && drawWeaponWhenAttackModeSelected && !mainPlayerWeaponsManager.isPlayerCarringWeapon ()) {
			setDrawOrHolsterWeaponState (true);
		}
	}

	void checkEventsOnCombatStateChange (bool state)
	{
		if (useEventsOnCombatActive) {
			if (state) {
				eventOnCombatActive.Invoke ();
			} else {
				eventOnCombatDeactivate.Invoke ();
			}
		}
	}

	public void pauseAttackDuringXTime (float newDuration)
	{
		currentPauseAttackStateDuration = newDuration;

		lastTimeAttackPauseWithDuration = Time.time;

		attackStatePaused = true;
	}

	public void resetBehaviorStates ()
	{
		resetStates ();

		waitingForAttackActive = false;

		checkIfDrawWeaponActive = true;

		if (keepWeaponsIfNotTargetsToAttack) {
			setDrawOrHolsterWeaponState (false);
		} else {
			setAimWeaponState (false);
		}

		insideMinDistanceToAttack = false;
	}

	public void setDrawOrHolsterWeaponState (bool state)
	{
		if (state) {
			mainPlayerWeaponsManager.drawCurrentWeaponWhenItIsReady (true);
		} else {
			mainPlayerWeaponsManager.drawOrKeepWeapon (false);
		}

		weaponEquiped = state;

		if (!weaponEquiped) {
			aimingWeapon = false;
		}
	}

	public void setAimWeaponState (bool state)
	{
		if (showDebugPrint) {
			print ("setting aim active state " + state);
		}

		if (state) {
			if (!aimingWeapon) {
				mainPlayerWeaponsManager.aimCurrentWeaponWhenItIsReady (true);
				lastTimeFireWeaponAttackAtDistance = Time.time;
			}
		} else {
			if (aimingWeapon) {
				mainPlayerWeaponsManager.stopAimCurrentWeaponWhenItIsReady (true);
			}
		}

		aimingWeapon = state;
	}

	public void setShootWeaponState (bool state)
	{
		mainPlayerWeaponsManager.shootWeapon (state);
	}

	public void dropWeapon ()
	{
		mainPlayerWeaponsManager.dropWeaponByBebugButton ();
	}

	public void shootTarget ()
	{
		setShootWeaponState (true);
	}

	public void resetAttackState ()
	{
		weaponEquiped = false;
		aimingWeapon = false;
	}

	public void checkIfDrawWeaponsWhenResumingAI ()
	{
		if (drawWeaponsWhenResumingAI && !weaponEquiped) {
			setDrawOrHolsterWeaponState (true);
		}
	}

	public void stopAim ()
	{
		if (aimingWeapon) {
			setAimWeaponState (false);
		}
	}

	public void disableOnSpottedState ()
	{

	}

	public void updateWeaponsAvailableState ()
	{
		characterHasWeapons = mainPlayerWeaponsManager.checkIfWeaponsAvailable ();
	}
}
