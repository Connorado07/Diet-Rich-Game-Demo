﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class meleeWeaponsGrabbedManager : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool meleeWeaponsGrabbedManagerEnabled = true;
	public bool meleeWeaponsGrabbedManagerActive;

	public bool storeEachGrabbedWeapon;

	public bool storeOnlyOneWeapon;

	public bool canSpawnAnyWeaponStoredInfiniteTimes;

	public bool storePickedWeaponsOnInventory;

	public bool numberKeysToChangeBetweenMeleeWeaponsEnabled = true;

	[Space]
	[Header ("Start Game With Melee Weapon Settings")]
	[Space]

	public bool startGameWithWeapon;
	public string weaponNameToStartGame;
	public bool startWithWeaponOnlyIfAlreadyAvailable;

	public bool drawWeaponAtStartIfConfiguredExternally;

	[Space]
	[Header ("Start Game With Melee Shield Settings")]
	[Space]

	public bool startGameWithShield;
	public string shieldNameToStartGame;

	[Space]
	[Header ("Weapon List")]
	[Space]

	public List<meleeWeaponGrabbedInfo> meleeWeaponGrabbedInfoList = new List<meleeWeaponGrabbedInfo> ();

	public List<meleeWeaponPrefabInfo> meleeWeaponPrefabInfoList = new List<meleeWeaponPrefabInfo> ();

	[Space]
	[Header ("Shield List")]
	[Space]

	public List<shieldPrefabInfo> shieldPrefabInfoList = new List<shieldPrefabInfo> ();

	public List<shieldGrabbedInfo> shieldGrabbedInfoList = new List<shieldGrabbedInfo> ();

	[Space]
	[Header ("Events Settings")]
	[Space]

	public bool useEventsOnStateChange;
	public UnityEvent eventOnStateActive;
	public UnityEvent eventOnStateDeactivate;

	public bool useEventsOnMeleeWeaponEquipped;
	public UnityEvent eventOnMeleeWeaponEquipped;

	[Space]
	[Header ("Save Settings")]
	[Space]

	public bool saveCurrentMeleeWeaponListToSaveFile = true;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;

	public int currentNumberOfWeaponsAvailable;

	public int currentWeaponIndex = 0;

	public bool equipMeleeWeaponPaused;

	public bool currentMeleeWeaponSheathedOrCarried;

	[Space]
	[Header ("Components")]
	[Space]

	public GameObject playerGameObject;
	public playerInputManager playerInput;
	public grabObjects mainGrabObjects;
	public grabbedObjectMeleeAttackSystem mainGrabbedObjectMeleeAttackSystem;
	public inventoryManager mainInventoryManager;

	bool startWithWeaponChecked;

	bool startWithShieldChecked;

	bool drawMeleeWeaponsPaused;

	shieldGrabbedInfo currentShieldGrabbedInfo;


	void Start ()
	{
		currentNumberOfWeaponsAvailable = meleeWeaponGrabbedInfoList.Count;

		if (playerGameObject == null) {
			playerGameObject = mainGrabObjects.gameObject;
		}
	}

	void Update ()
	{
		if (meleeWeaponsGrabbedManagerActive) {
			if (!storePickedWeaponsOnInventory) {
				if (numberKeysToChangeBetweenMeleeWeaponsEnabled) {
					if (!mainGrabbedObjectMeleeAttackSystem.isObjectThrownTravellingToTarget ()) {
						int currentNumberInput = playerInput.checkNumberInput (currentNumberOfWeaponsAvailable + 1);

						if (currentNumberInput > -1) {
							if (currentNumberInput == 0) {
								currentNumberInput = 9;
							} else {
								currentNumberInput--;
							}

							if (meleeWeaponGrabbedInfoList.Count == 0 || currentNumberInput >= meleeWeaponGrabbedInfoList.Count) {
								return;
							}

							if (isGrabObjectsEnabled ()) {
								checkWeaponByNumber (currentNumberInput);
							}
						}
					}
				}
			}

			if (!startWithShieldChecked) {
				if (startGameWithShield && isGrabObjectsEnabled ()) {
					setShieldActiveState (true, shieldNameToStartGame);

					startWithShieldChecked = true;
				}
			}

			if (!startWithWeaponChecked) {

				if (startGameWithWeapon) {
					if (isGrabObjectsEnabled ()) {
						if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
							checkMeleeWeaponToUse (weaponNameToStartGame, startWithWeaponOnlyIfAlreadyAvailable);
						}

						startWithWeaponChecked = true;
					}
				} else {
					if (drawWeaponAtStartIfConfiguredExternally) {
						if (isGrabObjectsEnabled ()) {
							if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
								if (meleeWeaponGrabbedInfoList.Count > 0) {
									checkMeleeWeaponToUse (meleeWeaponGrabbedInfoList [0].Name, false);

									if (showDebugPrint) {
										print ("checking to draw weapon when starting game and checking for grabbed object list");
									}
								}
							}
						}

						startWithWeaponChecked = true;
					}
				}
			}
		}
	}

	public bool checkMeleeWeaponToUse (string weaponNameToSearch, bool checkIfWeaponNotFound)
	{
		bool canSearchWeapon = true;

		if (showDebugPrint) {
			print (weaponNameToSearch);
		}

		int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

		if (checkIfWeaponNotFound) {
			if (weaponIndex == -1) {
				canSearchWeapon = false;
			}
		}

		if (canSearchWeapon) {
			
			if (weaponIndex > -1 && meleeWeaponGrabbedInfoList [weaponIndex].weaponInstantiated) {
				if (showDebugPrint) {
					print ("check weapon by number");
				}

				checkWeaponByNumber (weaponIndex);

				return true;
			} else {
				if (showDebugPrint) {
					print ("instantiate new weapon");
				}

				int weaponPrefabIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == weaponNameToSearch);

				if (weaponPrefabIndex > -1) {
					GameObject newWeaponToCarry = (GameObject)Instantiate (meleeWeaponPrefabInfoList [weaponPrefabIndex].weaponPrefab, Vector3.up * 1000, Quaternion.identity);

					grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = newWeaponToCarry.GetComponent<grabPhysicalObjectSystem> ();

					currentGrabPhysicalObjectSystem.setCurrentPlayer (playerGameObject);

					mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

					mainGrabObjects.grabPhysicalObjectExternally (newWeaponToCarry);

					mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

					return true;
				}
			}
		}

		return false;
	}

	public void checkIfDrawWeapon ()
	{
		if (meleeWeaponsGrabbedManagerActive) {
			checkWeaponByNumber (currentWeaponIndex);

			mainGrabbedObjectMeleeAttackSystem.checkEventWhenKeepingOrDrawingMeleeWeapon (false);
		}
	}

	void checkWeaponByNumber (int currentNumberInput)
	{
		if (currentNumberInput >= meleeWeaponGrabbedInfoList.Count || currentNumberInput < 0) {
			return;
		}

		meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [currentNumberInput];

		if (!currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {

			currentMeleeWeaponGrabbedInfo.isCurrentWeapon = true;

			currentMeleeWeaponGrabbedInfo.carryingWeapon = true;

			currentMeleeWeaponSheathedOrCarried = true;

			currentWeaponIndex = currentNumberInput;

			bool weaponToKeepFound = false;

			for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
				if (k != currentNumberInput) {
					if (meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
						keepWeapon (k);

						weaponToKeepFound = true;
					}

					meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;

					meleeWeaponGrabbedInfoList [k].carryingWeapon = false;
				} 
			}

			if (!weaponToKeepFound && !storeEachGrabbedWeapon) {
				if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
					mainGrabObjects.dropObject ();

					mainGrabObjects.clearPhysicalObjectToGrabFoundList ();
				}
			}

			bool weaponInstantiated = false;

			if (!currentMeleeWeaponGrabbedInfo.weaponInstantiated || currentMeleeWeaponGrabbedInfo.canBeSpawnedInfiniteTimes || canSpawnAnyWeaponStoredInfiniteTimes) {
				if (currentMeleeWeaponGrabbedInfo.weaponStored == null) {

					currentMeleeWeaponGrabbedInfo.weaponInstantiated = true;

					meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = getWeaponPrefabByName (currentMeleeWeaponGrabbedInfo.Name);

					if (currentMeleeWeaponPrefabInfo != null) {
						currentMeleeWeaponGrabbedInfo.weaponStored = (GameObject)Instantiate (currentMeleeWeaponPrefabInfo.weaponPrefab, Vector3.up * 1000, Quaternion.identity);
			
						currentMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

						grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectSystem> ();

						currentGrabPhysicalObjectSystem.setCurrentPlayer (playerGameObject);

						mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

						mainGrabObjects.grabPhysicalObjectExternally (currentMeleeWeaponGrabbedInfo.weaponStored);

						mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

						weaponInstantiated = true;
					} else {
						print ("WARNING: melee weapon prefab with the name " + currentMeleeWeaponGrabbedInfo.Name + " not found, make sure to configure a weapon" +
						" with that info.");

						return;
					}
				}
			} 

			if (!weaponInstantiated) {
				currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);

				if (!currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed) {
					checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
				}

				grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectSystem> ();

				currentGrabPhysicalObjectSystem.setCurrentPlayer (playerGameObject);

				mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

				mainGrabObjects.grabPhysicalObjectExternally (currentMeleeWeaponGrabbedInfo.weaponStored);

				mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);
			}
		}

		mainGrabbedObjectMeleeAttackSystem.drawOrSheatheShield (true);
	}

	public void enableOrDisableMeleeWeaponsGrabbedManager (bool state)
	{
		if (!meleeWeaponsGrabbedManagerEnabled) {
			return;
		}
			
		if (meleeWeaponsGrabbedManagerActive == state) {
			return;
		}

		meleeWeaponsGrabbedManagerActive = state;

		if (meleeWeaponsGrabbedManagerActive) {
			if (!drawMeleeWeaponsPaused) {
				bool canDrawWeapon = true;

				if (startGameWithWeapon && !startWithWeaponChecked) {
					canDrawWeapon = false;
				}

				if (canDrawWeapon) {
					if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
						checkWeaponByNumber (currentWeaponIndex);

						if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject () && currentWeaponIndex >= 0 && currentWeaponIndex < meleeWeaponGrabbedInfoList.Count) {
							updateQuickAccesSlotOnInventory (meleeWeaponGrabbedInfoList [currentWeaponIndex].Name);
						}
					}
				}
			}
		} else {
			for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
				if (meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
					keepWeapon (k);
				}

				meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;
			} 
		}

		if (useEventsOnStateChange) {
			if (meleeWeaponsGrabbedManagerActive) {
				eventOnStateActive.Invoke ();
			} else {
				eventOnStateDeactivate.Invoke ();
			}
		}

		if (storePickedWeaponsOnInventory) {
			mainInventoryManager.checkToEnableOrDisableWeaponSlotsParentOutOfInventory (meleeWeaponsGrabbedManagerActive);
		}

		drawMeleeWeaponsPaused = false;

		if (!meleeWeaponsGrabbedManagerActive) {
			currentMeleeWeaponSheathedOrCarried = false; 
		}
	}

	public void setDrawMeleeWeaponsPausedState (bool state)
	{
		drawMeleeWeaponsPaused = state;
	}

	void keepWeapon (int weaponIndex)
	{
		if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
			return;
		}

		if (meleeWeaponGrabbedInfoList.Count > weaponIndex) {

			meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

			if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
				mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

				mainGrabbedObjectMeleeAttackSystem.checkEventWhenKeepingOrDrawingMeleeWeapon (true);

				mainGrabObjects.grabbed = true;

				mainGrabObjects.checkIfDropObject (currentMeleeWeaponGrabbedInfo.weaponStored);

				mainGrabObjects.removeCurrentPhysicalObjectToGrabFound (currentMeleeWeaponGrabbedInfo.weaponStored);


				currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);

				mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

				if (!currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed) {
					checkObjectMeshToEnableOrDisable (true, currentMeleeWeaponGrabbedInfo);
				}

				mainGrabbedObjectMeleeAttackSystem.drawOrSheatheShield (false);
			}

			currentMeleeWeaponGrabbedInfo.isCurrentWeapon = false;
		}
	}

	public void disableCurrentMeleeWeapon (string weaponName)
	{
		if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
			return;
		}

		int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

		if (meleeWeaponGrabbedInfoList.Count > weaponIndex && weaponIndex > -1) {

			meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

			if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
				if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
					mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

					mainGrabObjects.grabbed = true;

					mainGrabObjects.checkIfDropObject (currentMeleeWeaponGrabbedInfo.weaponStored);

					mainGrabObjects.removeCurrentPhysicalObjectToGrabFound (currentMeleeWeaponGrabbedInfo.weaponStored);

					currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);

					mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

					if (!currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed) {
						checkObjectMeshToEnableOrDisable (true, currentMeleeWeaponGrabbedInfo);
					}
				}

				currentMeleeWeaponGrabbedInfo.isCurrentWeapon = false;

				currentMeleeWeaponGrabbedInfo.carryingWeapon = false;
			}
		}
	}

	public void checkObjectMeshToEnableOrDisable (bool enableWeaponMesh, meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo)
	{
		grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

		if (currentMeleeWeaponGrabbedInfo.weaponMesh == null) {
			GameObject weaponMeshToInstantiate = currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh;

			if (currentGrabPhysicalObjectMeleeAttackSystem.useCustomWeaponMeshToInstantiate) {
				weaponMeshToInstantiate = currentGrabPhysicalObjectMeleeAttackSystem.customWeaponMeshToInstantiate;
			}

			currentMeleeWeaponGrabbedInfo.weaponMesh = (GameObject)Instantiate (weaponMeshToInstantiate, Vector3.zero, Quaternion.identity);
		
			currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localScale = currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh.transform.localScale;
		}

		if (currentMeleeWeaponGrabbedInfo.weaponMesh != null) {
			currentMeleeWeaponGrabbedInfo.weaponMesh.SetActive (enableWeaponMesh);
	
			if (enableWeaponMesh) {
				grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectSystem> ();

				bool setWeaponMeshOnPhysicalWeaponPlace = false;

				if (enableWeaponMesh) {
					currentMeleeWeaponGrabbedInfo.objectThrown = currentGrabPhysicalObjectMeleeAttackSystem.isObjectThrown ();

					if (currentMeleeWeaponGrabbedInfo.objectThrown) {
						setWeaponMeshOnPhysicalWeaponPlace = true;
					}
				}

				Transform newParent = null;

				bool useMountPointToKeepObject = currentGrabPhysicalObjectSystem.useMountPointToKeepObject;

				if (useMountPointToKeepObject) {
					newParent = GKC_Utils.getMountPointTransformByName (currentGrabPhysicalObjectSystem.mountPointTokeepObjectName, playerGameObject.transform);
				} 

				if (!useMountPointToKeepObject || newParent == null) {
					newParent = mainGrabbedObjectMeleeAttackSystem.getCharacterHumanBone (currentGrabPhysicalObjectSystem.boneToKeepObject);
				}

				Vector3 targetPosition = currentGrabPhysicalObjectMeleeAttackSystem.referencePositionToKeepObjectMesh.localPosition;
				Quaternion targetRotation = currentGrabPhysicalObjectMeleeAttackSystem.referencePositionToKeepObjectMesh.localRotation;

				if (currentGrabPhysicalObjectMeleeAttackSystem.useCustomReferencePositionToKeepObjectMesh) {
					targetPosition = currentGrabPhysicalObjectMeleeAttackSystem.customReferencePositionToKeepObjectMesh.localPosition;
					targetRotation = currentGrabPhysicalObjectMeleeAttackSystem.customReferencePositionToKeepObjectMesh.localRotation;
				}

				if (setWeaponMeshOnPhysicalWeaponPlace) {
					Transform lastParentAssigned = currentGrabPhysicalObjectSystem.getLastParentAssigned ();

					if (lastParentAssigned != null) {
						currentMeleeWeaponGrabbedInfo.weaponStored.transform.SetParent (lastParentAssigned);
					}

					currentMeleeWeaponGrabbedInfo.weaponMesh.transform.SetParent (currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh.transform);
				
					currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localPosition = Vector3.zero;
					currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localRotation = Quaternion.identity;

					newParent = currentGrabPhysicalObjectSystem.transform.parent;

					currentMeleeWeaponGrabbedInfo.weaponMesh.transform.SetParent (newParent);
				} else {

					currentMeleeWeaponGrabbedInfo.weaponMesh.transform.SetParent (newParent);

					currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localPosition = targetPosition;
					currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localRotation = targetRotation;
				}

				currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localScale = Vector3.one;
			}
		}
	}

	public meleeWeaponPrefabInfo getWeaponPrefabByName (string weaponName)
	{
		for (int k = 0; k < meleeWeaponPrefabInfoList.Count; k++) {
			if (meleeWeaponPrefabInfoList [k].Name.Equals (weaponName)) {
				return meleeWeaponPrefabInfoList [k];
			}
		}

		return null;
	}

	public grabPhysicalObjectMeleeAttackSystem getWeaponGrabbedByName (string weaponName)
	{
		for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
			if (meleeWeaponGrabbedInfoList [k].Name.Equals (weaponName)) {
				if (meleeWeaponGrabbedInfoList [k].weaponStored != null) {

					return meleeWeaponGrabbedInfoList [k].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

				}
			}
		}

		return null;
	}

	public void checkWeaponToStore (string weaponName, GameObject weaponGameObject)
	{
		if (!storeEachGrabbedWeapon) {
			return;
		}

		int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

		if (weaponIndex == -1) {
			meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = getWeaponPrefabByName (weaponName);

			if (currentMeleeWeaponPrefabInfo != null) {
				meleeWeaponGrabbedInfo newMeleeWeaponGrabbedInfo = new meleeWeaponGrabbedInfo ();

				newMeleeWeaponGrabbedInfo.Name = weaponName;
				newMeleeWeaponGrabbedInfo.isCurrentWeapon = true;
				newMeleeWeaponGrabbedInfo.carryingWeapon = true;

				currentMeleeWeaponSheathedOrCarried = true;

				newMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

				newMeleeWeaponGrabbedInfo.weaponStored = weaponGameObject;

				grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = newMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

				newMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed = currentGrabPhysicalObjectMeleeAttackSystem.hideWeaponMeshWhenNotUsed;

				newMeleeWeaponGrabbedInfo.weaponInstantiated = true;

				meleeWeaponGrabbedInfoList.Add (newMeleeWeaponGrabbedInfo);

				currentNumberOfWeaponsAvailable = meleeWeaponGrabbedInfoList.Count;

				currentWeaponIndex = meleeWeaponGrabbedInfoList.Count - 1;

				if (storeOnlyOneWeapon) {
					for (int i = meleeWeaponGrabbedInfoList.Count - 1; i >= 0; i--) {
						if (currentWeaponIndex != i) {
							meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [i];

							if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {

								if (!currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed) {
									checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
								}

								currentMeleeWeaponGrabbedInfo.weaponStored.transform.position = mainGrabObjects.transform.position + mainGrabObjects.transform.up + mainGrabObjects.transform.forward;

								currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);
							}

							meleeWeaponGrabbedInfoList.RemoveAt (i);
						} 
					} 

					currentNumberOfWeaponsAvailable = meleeWeaponGrabbedInfoList.Count;

					currentWeaponIndex = meleeWeaponGrabbedInfoList.Count - 1;
				}

				if (storePickedWeaponsOnInventory && !equipMeleeWeaponPaused) {
					if (showDebugPrint) {
						print ("ADD MELEE WEAPON TO INVENTORY " + weaponName);
					}

					mainInventoryManager.addObjectAmountToInventoryByName (weaponName, 1);

					if (!meleeWeaponsGrabbedManagerActive) {
						mainInventoryManager.checkQuickAccessSlotToSelectByName (weaponName);

//						if (useEventsOnMeleeWeaponEquipped) {
//							eventOnMeleeWeaponEquipped.Invoke ();
//						}
					}
				}

				equipMeleeWeaponPaused = false;
			} else {
				print ("WARNING: melee weapon prefab with the name " + weaponName + " not found, make sure to configure a weapon" +
				" with that info.");
			}
		} else {
			for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
				if (weaponIndex == k) {
					meleeWeaponGrabbedInfoList [k].isCurrentWeapon = true;
					meleeWeaponGrabbedInfoList [k].carryingWeapon = true;

					currentMeleeWeaponSheathedOrCarried = true;
				} else {
					meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;
					meleeWeaponGrabbedInfoList [k].carryingWeapon = false;
				}
			} 

			currentWeaponIndex = weaponIndex;

			meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [currentWeaponIndex];

			if (currentMeleeWeaponGrabbedInfo.weaponStored != weaponGameObject) {

				if (!currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed) {
					checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
				}

				currentMeleeWeaponGrabbedInfo.weaponStored.transform.position = mainGrabObjects.transform.position + mainGrabObjects.transform.up + mainGrabObjects.transform.forward;

				currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);

				currentMeleeWeaponGrabbedInfo.weaponStored = weaponGameObject;
			}
		}
	}

	public void checkToDropWeaponFromList (string weaponName)
	{
		for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
			meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [k];

			if (currentMeleeWeaponGrabbedInfo.Name.Equals (weaponName)) {

				bool objectIsThrown = false;
				
				if (storePickedWeaponsOnInventory && mainInventoryManager != null) {
					if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
						grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = 
							currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();
						
						if (currentGrabPhysicalObjectMeleeAttackSystem.isObjectThrown ()) {
							objectIsThrown = true;
						} else {
							currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);
						}
					}
				}

				if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
					currentMeleeWeaponSheathedOrCarried = false;
				}

				meleeWeaponGrabbedInfoList.RemoveAt (k);

				if (meleeWeaponGrabbedInfoList.Count == 0) {
					currentMeleeWeaponSheathedOrCarried = false;
				}

				if (storePickedWeaponsOnInventory && mainInventoryManager != null) {
					if (objectIsThrown) {
						if (showDebugPrint) {
							print ("object is thrown, remove the weapon from inventory without instantiate a pickup");
						}

						mainInventoryManager.dropEquipByName (weaponName, 1, false);
					} else {
						if (showDebugPrint) {
							print ("object is not thrown, remove the weapon from inventory and instantiate a pickup");
						}

						bool objectLocatedOnInventory = false;

						if (showDebugPrint) {
							print (mainInventoryManager.getInventoryObjectAmountByName (weaponName));
						}

						if (mainInventoryManager.getInventoryObjectAmountByName (weaponName) >= 1) {
							objectLocatedOnInventory = true;
						}

						mainInventoryManager.dropEquipByName (weaponName, 1, true);

						if (!objectLocatedOnInventory) {
							currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);
						}
					}
				}

				return;
			}
		} 
	}

	public void disableIsCurrentWeaponStateOnAllWeapons ()
	{
		for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
			meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;
		} 
	}

	public bool isGrabObjectsEnabled ()
	{
		return mainGrabObjects.isGrabObjectsEnabled ();
	}

	public void enableOrDisableMeleeWeaponMeshesOnCharacterBody (bool state)
	{
		for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
			if (!meleeWeaponGrabbedInfoList [k].objectThrown) {
				if (meleeWeaponGrabbedInfoList [k].weaponMesh != null) {
					meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (state);
				}
			}
		}
	}

	public void enableOrDisableAllMeleeWeaponMeshesOnCharacterBody (bool state)
	{
		for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
			if (!meleeWeaponGrabbedInfoList [k].objectThrown) {
				if (meleeWeaponGrabbedInfoList [k].weaponMesh != null) {
					if (state) {
						if (!meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
							meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (state);
						}
					} else {
						meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (state);
					}
				}
			}
		}
	}

	public bool characterIsCarryingWeapon ()
	{
		return meleeWeaponsGrabbedManagerActive && mainGrabbedObjectMeleeAttackSystem.isCarryingObject ();
	}

	//INVENTORY FUNCTIONS
	public bool equipMeleeWeapon (string weaponNameToSearch, bool checkIfWeaponNotFound)
	{
		int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

		equipMeleeWeaponPaused = true;

		if (weaponIndex > -1) {
			meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

			if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
				return true;
			} else {
				if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {

				} else {
					checkWeaponByNumber (weaponIndex);

					if (!meleeWeaponsGrabbedManagerActive) {
//						mainInventoryManager.checkQuickAccessSlotToSelectByName (weaponNameToSearch);

//						if (useEventsOnMeleeWeaponEquipped) {
//							eventOnMeleeWeaponEquipped.Invoke ();
//						}
					}

					updateQuickAccesSlotOnInventory (weaponNameToSearch);

					return true;
				}
			}
		}

		bool weaponEquippedCorrectly = false;

		//checkMeleeWeaponToUse (weaponNameToSearch, checkIfWeaponNotFound);

		bool canSearchWeapon = true;

		if (showDebugPrint) {
			print (weaponNameToSearch);
		}

		weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

		if (checkIfWeaponNotFound) {
			if (weaponIndex == -1) {
				canSearchWeapon = false;
			}
		}

		if (canSearchWeapon) {
			if (weaponIndex > -1 && meleeWeaponGrabbedInfoList [weaponIndex].weaponInstantiated) {
				if (showDebugPrint) {
					print ("check weapon by number");
				}

//				checkWeaponByNumber (weaponIndex);

				weaponEquippedCorrectly = true;
			} else {
				if (showDebugPrint) {
					print ("instantiate new weapon");
				}

				int weaponPrefabIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == weaponNameToSearch);

				if (weaponPrefabIndex > -1) {
					meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = meleeWeaponPrefabInfoList [weaponPrefabIndex];

					GameObject newWeaponToCarry = (GameObject)Instantiate (currentMeleeWeaponPrefabInfo.weaponPrefab, Vector3.up * 1000, Quaternion.identity);

					meleeWeaponGrabbedInfo newMeleeWeaponGrabbedInfo = new meleeWeaponGrabbedInfo ();
					newMeleeWeaponGrabbedInfo.Name = weaponNameToSearch;

					newMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

					newMeleeWeaponGrabbedInfo.weaponStored = newWeaponToCarry;

					newWeaponToCarry.SetActive (false);

					newMeleeWeaponGrabbedInfo.weaponInstantiated = true;

					grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = newMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

					newMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed = currentGrabPhysicalObjectMeleeAttackSystem.hideWeaponMeshWhenNotUsed;

					meleeWeaponGrabbedInfoList.Add (newMeleeWeaponGrabbedInfo);

					weaponEquippedCorrectly = true;

//					checkWeaponByNumber (meleeWeaponGrabbedInfoList.Count - 1);
				}
			}
		}
			

		equipMeleeWeaponPaused = false;

		if (weaponEquippedCorrectly) {
			if (!meleeWeaponsGrabbedManagerActive) {
//				mainInventoryManager.checkQuickAccessSlotToSelectByName (weaponNameToSearch);

//				if (useEventsOnMeleeWeaponEquipped) {
//					eventOnMeleeWeaponEquipped.Invoke ();
//				}
			}

			updateQuickAccesSlotOnInventory (weaponNameToSearch);
		}

		return weaponEquippedCorrectly;
	}

	void updateQuickAccesSlotOnInventory (string weaponNameToSearch)
	{
		if (storePickedWeaponsOnInventory) {
			mainInventoryManager.showWeaponSlotsParentWhenWeaponSelectedByName (weaponNameToSearch);
		}
	}

	public bool unEquipMeleeWeapon (string weaponNameToSearch, bool dropWeaponObject)
	{
		int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

		if (weaponIndex > -1) {
			meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

			if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
				if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
					mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

					mainGrabObjects.grabbed = true;

					mainGrabObjects.checkIfDropObject (currentMeleeWeaponGrabbedInfo.weaponStored);
				} else {
					currentMeleeWeaponGrabbedInfo.weaponStored.transform.position = mainGrabObjects.transform.position + mainGrabObjects.transform.up + mainGrabObjects.transform.forward;
				}

				if (!currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed) {
					checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
				}

				mainGrabObjects.removeCurrentPhysicalObjectToGrabFound (currentMeleeWeaponGrabbedInfo.weaponStored);

				currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (dropWeaponObject);

				mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);
			}

			currentMeleeWeaponGrabbedInfo.isCurrentWeapon = false;

			currentMeleeWeaponGrabbedInfo.carryingWeapon = false;

			currentMeleeWeaponSheathedOrCarried = false;

			meleeWeaponGrabbedInfoList.RemoveAt (weaponIndex);

			currentNumberOfWeaponsAvailable = meleeWeaponGrabbedInfoList.Count;

			currentWeaponIndex = meleeWeaponGrabbedInfoList.Count - 1;

			if (currentWeaponIndex < 0) {
				currentWeaponIndex = 0;
			}

			return true;
		}

		return false;
	}

	public bool checkWeaponToSelectOnQuickAccessSlots (string weaponName)
	{
		int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

		if (weaponIndex > -1) {
			meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

			checkWeaponByNumber (weaponIndex);

			if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
				return true;
			}
		}

		return false;
	}

	public bool isMeleeWeaponsGrabbedManagerActive ()
	{
		return meleeWeaponsGrabbedManagerActive;
	}

	public string getCurrentWeaponName ()
	{
		for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
			if (meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
				return meleeWeaponGrabbedInfoList [k].Name;
			}
		} 

		return "";
	}

	public GameObject getCurrentWeaponMeshByName (string weaponName)
	{
		for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
			if (meleeWeaponGrabbedInfoList [k].Name.Equals (weaponName)) {
				return meleeWeaponGrabbedInfoList [k].weaponMesh;
			}
		} 

		return null;
	}

	public string getCurrentWeaponActiveName ()
	{
		if (meleeWeaponGrabbedInfoList.Count > 0 && currentWeaponIndex < meleeWeaponGrabbedInfoList.Count) {
			return meleeWeaponGrabbedInfoList [currentWeaponIndex].Name;
		}

		return "";
	}

	public bool equipShield (string shieldName)
	{
		return setShieldActiveState (true, shieldName);
	}

	public bool unequipeShield (string shieldName)
	{
		return setShieldActiveState (false, shieldName);
	}
	//END INVENTORY FUNCTIONS


	//START SHIELD FUNCTIONS
	public bool setShieldActiveState (bool state, string shieldName)
	{
		int shieldInfoIndex = shieldGrabbedInfoList.FindIndex (s => s.Name == shieldName);

		int shieldPrefabIndex = shieldPrefabInfoList.FindIndex (s => s.Name == shieldName);

		if (shieldInfoIndex == -1) {
			if (shieldPrefabIndex > -1) {

				shieldPrefabInfo currentShieldPrefabInfo = shieldPrefabInfoList [shieldPrefabIndex];

				GameObject newWeaponToCarry = (GameObject)Instantiate (currentShieldPrefabInfo.shieldPrefab, Vector3.up * 1000, Quaternion.identity);

				shieldGrabbedInfo newShieldGrabbedInfo = new shieldGrabbedInfo ();
				newShieldGrabbedInfo.Name = shieldName;

				newShieldGrabbedInfo.shieldInstantiated = true;

				newShieldGrabbedInfo.shieldPrefabIndex = currentShieldPrefabInfo.shieldPrefabIndex;

				newShieldGrabbedInfo.shieldStored = newWeaponToCarry;

				newShieldGrabbedInfo.equipShieldWhenPickedIfNotShieldEquippedPrevioulsy = currentShieldPrefabInfo.equipShieldWhenPickedIfNotShieldEquippedPrevioulsy;

				shieldGrabbedInfoList.Add (newShieldGrabbedInfo);

				shieldInfoIndex = shieldGrabbedInfoList.Count - 1;
			}
		}

		if (shieldInfoIndex > -1) {
			for (int k = 0; k < shieldGrabbedInfoList.Count; k++) {
				if (k == shieldInfoIndex) {
					currentShieldGrabbedInfo = shieldGrabbedInfoList [k];

					if (!currentShieldGrabbedInfo.isCurrentShield) {
						if (currentShieldGrabbedInfo.useEventsOnEquipShieldChangeState) {
							if (state) {
								currentShieldGrabbedInfo.eventOnUnequippShield.Invoke ();
							} else {
								currentShieldGrabbedInfo.eventOnEquipShield.Invoke ();
							}
						}
					}

					currentShieldGrabbedInfo.isCurrentShield = true;

					if (!currentShieldGrabbedInfo.shieldStored.activeSelf) {
						currentShieldGrabbedInfo.shieldStored.SetActive (state);
					}
				} else {
					if (shieldGrabbedInfoList [k].isCurrentShield) {
						if (shieldGrabbedInfoList [k].useEventsOnEquipShieldChangeState) {
							shieldGrabbedInfoList [k].eventOnUnequippShield.Invoke ();
						}
					}

					shieldGrabbedInfoList [k].isCurrentShield = false;

					if (shieldGrabbedInfoList [k].shieldStored.activeSelf) {
						shieldGrabbedInfoList [k].shieldStored.SetActive (false);
					}
				}
			}

			mainGrabbedObjectMeleeAttackSystem.setShieldInfo (currentShieldGrabbedInfo.Name, currentShieldGrabbedInfo.shieldStored, 
				shieldPrefabInfoList [shieldPrefabIndex].shieldHandMountPointTransformReference, shieldPrefabInfoList [shieldPrefabIndex].shieldBackMountPointTransformReference, state);

			mainGrabbedObjectMeleeAttackSystem.setShieldActiveState (state);

			return true;
		}

		return false;
	}

	public void checkEquipShieldIfNotCarryingPreviously ()
	{
		if (!mainGrabbedObjectMeleeAttackSystem.carryingShield &&
		    (meleeWeaponsGrabbedManagerActive || mainGrabbedObjectMeleeAttackSystem.shieldCanBeUsedWithoutMeleeWeapon)) {

			string lastInventoryObjectPickedName = mainInventoryManager.getLastInventoryObjectPickedName ();

			if (lastInventoryObjectPickedName != "") {
				for (int k = 0; k < shieldPrefabInfoList.Count; k++) {
					if (shieldPrefabInfoList [k].equipShieldWhenPickedIfNotShieldEquippedPrevioulsy &&
					    shieldPrefabInfoList [k].Name.Equals (lastInventoryObjectPickedName)) {

						mainInventoryManager.equipObjectByName (lastInventoryObjectPickedName);

						return;
					}
				}
			}
		}
	}

	public void toggleDrawOrSheatheShield (string shieldName)
	{
		int shieldInfoIndex = shieldGrabbedInfoList.FindIndex (s => s.Name == shieldName);

		bool shieldState = false;

		if (shieldInfoIndex > -1) {
//			if (meleeWeaponsGrabbedManagerActive) {
			if (shieldGrabbedInfoList [shieldInfoIndex].isCurrentShield) {
				shieldState = !mainGrabbedObjectMeleeAttackSystem.shieldActive;
			} else {
				shieldState = true;
			} 
//			} else {
//				print ("trying to draw the shield when not melee mode active, sending signal to quick access slots");
//
//				mainInventoryManager.changeToMeleeWeapons (mainGrabbedObjectMeleeAttackSystem.getEmptyWeaponToUseOnlyShield ());
//			}
		} else {
			shieldState = true;
		}

		drawOrSheatheShield (shieldState, shieldName);

//		print (shieldState + " " + shieldName);
	}

	public void drawOrSheatheShield (bool state, string shieldName)
	{
		setShieldActiveState (true, shieldName);

		mainGrabbedObjectMeleeAttackSystem.drawOrSheatheShield (state);

		if (state) {
			if (!mainGrabbedObjectMeleeAttackSystem.shieldActive && mainGrabbedObjectMeleeAttackSystem.carryingShield) {
				mainGrabbedObjectMeleeAttackSystem.setShieldParentState (false);
			}
		} else {
			mainGrabbedObjectMeleeAttackSystem.setShieldActiveFieldValueDirectly (false);
		}
	}
	//END SHIELD FUNCTIONS

	public void checkToKeepWeapon ()
	{
		keepWeaponExternally (true);
	}

	public void checkToKeepWeaponWithoutCheckingInputActive ()
	{
		keepWeaponExternally (false);
	}

	void keepWeaponExternally (bool checkIfInputActive)
	{
		if (!meleeWeaponsGrabbedManagerActive) {
			return;
		}

		if (!isGrabObjectsEnabled ()) {
			return;
		}

		if ((mainGrabbedObjectMeleeAttackSystem.canUseWeaponsInput () || !checkIfInputActive) &&
		    !mainGrabbedObjectMeleeAttackSystem.isObjectThrownTravellingToTarget ()) {
			if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
				keepWeapon (currentWeaponIndex);
			}
		}
	}

	public void addNewMeleeWeaponPrefab (GameObject newWeaponPrefab, string newWeaponName)
	{
		if (newWeaponPrefab != null) {
			int weaponPrefabIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == newWeaponName);

			if (weaponPrefabIndex < 0) {
				
				meleeWeaponPrefabInfo newMeleeWeaponPrefabInfo = new meleeWeaponPrefabInfo ();

				newMeleeWeaponPrefabInfo.Name = newWeaponName;

				newMeleeWeaponPrefabInfo.weaponPrefab = newWeaponPrefab;

				newMeleeWeaponPrefabInfo.weaponPrefabIndex = meleeWeaponPrefabInfoList.Count;

				meleeWeaponPrefabInfoList.Add (newMeleeWeaponPrefabInfo);

				updateComponent ();
			}
		}
	}

	public void addNewMeleeShieldPrefab (GameObject newShieldPrefab, string newShieldName)
	{
		if (newShieldPrefab != null) {
			shieldPrefabInfo newShieldPrefabInfo = new shieldPrefabInfo ();

			newShieldPrefabInfo.Name = newShieldName;

			newShieldPrefabInfo.shieldPrefab = newShieldPrefab;

			newShieldPrefabInfo.shieldPrefabIndex = shieldPrefabInfoList.Count + 1;

			if (shieldPrefabInfoList.Count > 0) {
				newShieldPrefabInfo.shieldBackMountPointTransformReference = shieldPrefabInfoList [0].shieldBackMountPointTransformReference;
				newShieldPrefabInfo.shieldHandMountPointTransformReference = shieldPrefabInfoList [0].shieldHandMountPointTransformReference;
			}

			shieldPrefabInfoList.Add (newShieldPrefabInfo);

			updateComponent ();
		}
	}

	//INPUT FUNCTIONS
	public void inputDrawOrKeepMeleeWeapon ()
	{
		drawOrKeepMeleeWeapon (true);
	}

	public void drawOrKeepMeleeWeaponWithoutCheckingInputActive ()
	{
		drawOrKeepMeleeWeapon (false);
	}

	void drawOrKeepMeleeWeapon (bool checkIfInputActive)
	{
		if (!meleeWeaponsGrabbedManagerActive) {
			return;
		}

		if (!isGrabObjectsEnabled ()) {
			return;
		}

		if (mainGrabbedObjectMeleeAttackSystem.isAttackInProcess ()) {
			return;
		}

		if ((mainGrabbedObjectMeleeAttackSystem.canUseWeaponsInput () || !checkIfInputActive) &&
		    !mainGrabbedObjectMeleeAttackSystem.isObjectThrownTravellingToTarget ()) {
			if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
				keepWeapon (currentWeaponIndex);
			} else {
				checkIfDrawWeapon ();
			}
		}
	}

	void updateComponent ()
	{
		GKC_Utils.updateComponent (this);

		GKC_Utils.updateDirtyScene ("Add Shield", gameObject);
	}

	[System.Serializable]
	public class meleeWeaponGrabbedInfo
	{
		public string Name;
		public bool weaponInstantiated;
		public GameObject weaponStored;
		public bool canBeSpawnedInfiniteTimes;
		public bool isCurrentWeapon;
		public bool carryingWeapon;
		public int weaponPrefabIndex;
		public bool hideWeaponMeshWhenNotUsed;

		public bool objectThrown;

		public GameObject weaponMesh;
	}

	[System.Serializable]
	public class meleeWeaponPrefabInfo
	{
		public string Name;
		public GameObject weaponPrefab;
		public int weaponPrefabIndex;
	}

	[System.Serializable]
	public class shieldPrefabInfo
	{
		public string Name;
		public GameObject shieldPrefab;
		public Transform shieldHandMountPointTransformReference;
		public Transform shieldBackMountPointTransformReference;
		public int shieldPrefabIndex;

		public bool equipShieldWhenPickedIfNotShieldEquippedPrevioulsy;
	}

	[System.Serializable]
	public class shieldGrabbedInfo
	{
		public string Name;
		public bool shieldInstantiated;
		public GameObject shieldStored;

		public bool isCurrentShield;
		public int shieldPrefabIndex;
		public bool hideWeaponMeshWhenNotUsed;

		public bool equipShieldWhenPickedIfNotShieldEquippedPrevioulsy;

		public bool useEventsOnEquipShieldChangeState;
		public UnityEvent eventOnEquipShield;
		public UnityEvent eventOnUnequippShield;
	}
}
