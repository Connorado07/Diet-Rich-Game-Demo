﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedMenuEditor : EditorWindow
{
	[MenuItem ("GameObject/GKC/Objects Options/Enable GameObject", false, 4)]
	static void enableCurrentSelectedGameObject ()
	{
		GameObject currentObjectSelected = Selection.activeGameObject;

		if (currentObjectSelected != null) {
			currentObjectSelected.SetActive (true);
		}
	}

	[MenuItem ("GameObject/GKC/Objects Options/Disable GameObject", false, 4)]
	static void disableCurrentSelectedGameObject ()
	{
		GameObject currentObjectSelected = Selection.activeGameObject;

		if (currentObjectSelected != null) {
			currentObjectSelected.SetActive (false);
		}
	}

	[MenuItem ("GameObject/GKC/Objects Options/Toggle GameObject Active", false, 4)]
	static void toggleActiveCurrentSelectedGameObject ()
	{
		GameObject currentObjectSelected = Selection.activeGameObject;

		if (currentObjectSelected != null) {
			currentObjectSelected.SetActive (!currentObjectSelected.activeSelf);
		}
	}

	[MenuItem ("GameObject/GKC/Create New Character", false, 1)]
	static void createNewPlayer ()
	{
		GetWindow<CharacterCreatorEditor> ();
	}

	[MenuItem ("Game Kit Controller/Create New Character", false, 0)]
	public static void createNewPlayerMain ()
	{
		GetWindow<CharacterCreatorEditor> ();
	}


	[MenuItem ("GameObject/GKC/Create Prefabs Manager", false, 2)]
	static void createPrefabsManager1 ()
	{
		addPrefabsManagerToScene ();
	}

	[MenuItem ("Game Kit Controller/Create Prefabs Manager", false, 3)]
	static void createPrefabsManager2 ()
	{
		addPrefabsManagerToScene ();
	}

	public static void addPrefabsManagerToScene ()
	{
		prefabsManager newPrefabsManager = FindObjectOfType<prefabsManager> ();

		if (newPrefabsManager != null) {
			Debug.Log ("There is already a Prefabs Manager in the scene");

			Selection.activeGameObject = newPrefabsManager.gameObject;
		} else {
			string prefabsPath = "Assets/Game Kit Controller/Prefabs/Prefabs Manager/Prefabs Manager.prefab";

			GameObject prefabsManagerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath (prefabsPath, typeof(GameObject));

			if (prefabsManagerPrefab != null) {
				GameObject newPrefabsManagerGameObject = (GameObject)Instantiate (prefabsManagerPrefab, Vector3.zero, Quaternion.identity);

				newPrefabsManagerGameObject.name = "Prefabs Manager";

				newPrefabsManagerGameObject.transform.position = Vector3.zero;
				newPrefabsManagerGameObject.transform.rotation = Quaternion.identity;

				Selection.activeGameObject = newPrefabsManagerGameObject;

				Debug.Log ("Prefabs Manager added to the scene");
			} else {
				Debug.Log ("WARNING: Prefabs Manager hasn't been found on the project, make sure to configure an object with that name");
			}
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Mission Manager/Add Main Mission Manager", false, 4)]
	static void createMainMissionManager ()
	{
		objectiveManager newObjectiveManager = FindObjectOfType<objectiveManager> ();

		if (newObjectiveManager == null) {
			instantiatePrefabByName ("Main Objective Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newObjectiveManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Mission Manager/Select Main Mission Manager On Scene", false, 4)]
	static void selectMainMissionManager ()
	{
		objectiveManager mainObjectiveManager = FindObjectOfType<objectiveManager> ();

		if (mainObjectiveManager != null) {
			GKC_Utils.setActiveGameObjectInEditor (mainObjectiveManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Dialog Manager/Add Main Dialog Manager", false, 5)]
	static void createMainDialogManager ()
	{
		dialogManager newDialogManager = FindObjectOfType<dialogManager> ();

		if (newDialogManager == null) {
			instantiatePrefabByName ("Main Dialog Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newDialogManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Dialog Manager/Select Main Dialog Manager On Scene", false, 5)]
	static void selectMainDialogManager ()
	{
		dialogManager mainDialogManager = FindObjectOfType<dialogManager> ();

		if (mainDialogManager != null) {
			GKC_Utils.setActiveGameObjectInEditor (mainDialogManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Dialog Manager/Add Dialog System To Current Character", false, 5)]
	static void addDialogSystemToCurrentCharacter ()
	{
		dialogManager newDialogManager = FindObjectOfType<dialogManager> ();

		if (newDialogManager == null) {
			createMainDialogManager ();

			newDialogManager = FindObjectOfType<dialogManager> ();
		}

		if (newDialogManager != null) {
			GameObject currentObjectSelected = Selection.activeGameObject;

			if (currentObjectSelected != null) {
				playerController currentPlayerController = currentObjectSelected.GetComponentInChildren<playerController> ();

				Transform newDialogContentParent = currentObjectSelected.transform;

				if (currentPlayerController != null) {
					if (currentObjectSelected == currentPlayerController.gameObject) {
						Debug.Log ("player controller selected");
						newDialogContentParent = currentObjectSelected.transform.parent;
					} else {
						Debug.Log ("parent selected");
						currentObjectSelected = currentPlayerController.gameObject;
					}
				} else {
					Debug.Log ("no character selected");
				}

				dialogContentSystem newDialogContentSystem = newDialogContentParent.GetComponentInChildren<dialogContentSystem> ();

				if (newDialogContentSystem == null) {
					newDialogManager.addDialogContentToCharacter (newDialogContentParent, currentObjectSelected);
				} else {
					Debug.Log ("The current character selected already has a dialog content system.\n" +
					"Make sure to select a character which hasn't this object already assigned");
				}
			} else {
				Debug.Log ("There is no current character selected in the editor.\n " +
				"Make sure to select the gameObject that is going to have this dialog assigned");
			}
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Level-Scene Manager In-Game/Add Change Level By Button", false, 6)]
	static void createMainLevelManagerIngameByButton ()
	{
		instantiatePrefabByName ("Change Level Trigger (By Button)");
	}

	[MenuItem ("Game Kit Controller/Main Managers/Level-Scene Manager In-Game/Add Change Level By Event", false, 7)]
	static void createMainLevelManagerIngameByEvent ()
	{
		instantiatePrefabByName ("Change Level Trigger (By Event)");
	}

	[MenuItem ("Game Kit Controller/Main Managers/Level-Scene Manager In-Game/Add Change Level By Trigger", false, 8)]
	static void createMainLevelManagerIngameByTrigger ()
	{
		instantiatePrefabByName ("Change Level Trigger (By Trigger)");
	}

	[MenuItem ("Game Kit Controller/Main Managers/Level-Scene Manager In-Game/Add Travel Station System", false, 9)]
	static void createTraveStationSystem ()
	{
		instantiatePrefabByName ("Travel Station");
	}

	[MenuItem ("Game Kit Controller/Main Managers/Action System/Add Action System Trigger By Button", false, 10)]
	static void createActionSystemTriggerByButton ()
	{
		instantiatePrefabByName ("Action System Trigger (Empty Button)");
	}

	[MenuItem ("Game Kit Controller/Main Managers/Action System/Add Action System Trigger Automatic", false, 10)]
	static void createActionSystemTriggerAutomatic ()
	{
		instantiatePrefabByName ("Action System Trigger (Empty Automatic)");
	}

	[MenuItem ("Game Kit Controller/Main Managers/Action System/Add Custom Action System Trigger", false, 10)]
	static void createCustomActionSystemTrigger ()
	{
		instantiatePrefabByName ("Custom Action System Trigger");
	}

	[MenuItem ("Game Kit Controller/Main Managers/Inventory Manager/Add Main Inventory Manager", false, 4)]
	static void createMainInventoryManager ()
	{
		inventoryListManager newInventoryListManager = FindObjectOfType<inventoryListManager> ();

		if (newInventoryListManager == null) {
			instantiatePrefabByName ("Main Inventory Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newInventoryListManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Inventory Manager/Add New Inventory Object", false, 4)]
	static void createNewInventoryObject ()
	{
		inventoryListManager newInventoryListManager = FindObjectOfType<inventoryListManager> ();

		if (newInventoryListManager == null) {
			instantiatePrefabByName ("Main Inventory Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newInventoryListManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Decal Manager/Add Main Decal Manager", false, 4)]
	static void createMainDecalManager ()
	{
		decalManager newDecalManager = FindObjectOfType<decalManager> ();

		if (newDecalManager == null) {
			instantiatePrefabByName ("Decal Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newDecalManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Pickup Manager/Add Main Pickup Manager", false, 4)]
	static void createMainPickupManager ()
	{
		pickUpManager newPickUpManager = FindObjectOfType<pickUpManager> ();

		if (newPickUpManager == null) {
			instantiatePrefabByName ("Pickup Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newPickUpManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Camera Shake Manager/Add Main Camera Shake Manager", false, 4)]
	static void createMaiCameraShakeManager ()
	{
		externalShakeListManager newExternalShakeListManager = FindObjectOfType<externalShakeListManager> ();

		if (newExternalShakeListManager == null) {
			instantiatePrefabByName ("External Shake List Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newExternalShakeListManager.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Faction Manager/Add Main Faction Manager", false, 4)]
	static void createMaiFactionManager ()
	{
		factionSystem newFactionSystem = FindObjectOfType<factionSystem> ();

		if (newFactionSystem == null) {
			instantiatePrefabByName ("Faction System");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newFactionSystem.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Damage On Screen Info Manager/Add Main Damage On Screen Info Manager", false, 4)]
	static void createMaiDamageOnScreenInfoManager ()
	{
		damageOnScreenInfoSystem newDamageOnScreenInfoSystem = FindObjectOfType<damageOnScreenInfoSystem> ();

		if (newDamageOnScreenInfoSystem == null) {
			instantiatePrefabByName ("Damage On Screen Info Manager");
		} else {
			GKC_Utils.setActiveGameObjectInEditor (newDamageOnScreenInfoSystem.gameObject);
		}
	}

	[MenuItem ("Game Kit Controller/Main Managers/Add All Main Managers Into Scene", false, 20)]
	static void addAllMainManagersIntoScene ()
	{
		GKC_Utils.addAllMainManagersToScene ();
	}

	public static void instantiatePrefabByName (string prefabName)
	{
		prefabsManager newPrefabsManager = FindObjectOfType<prefabsManager> ();

		if (newPrefabsManager == null) {
			addPrefabsManagerToScene ();

			newPrefabsManager = FindObjectOfType<prefabsManager> ();
		}

		if (newPrefabsManager != null) {
			newPrefabsManager.spawnPrefabByName (prefabName);

			newPrefabsManager.selectLastPrefabSpawned ();
		}
	}


	//Custom Toolbar Functions
	[MenuItem ("Game Kit Controller/Others/Select Main Player", false, 10)]
	public static void selectMainPlayerOnScene ()
	{
		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			GKC_Utils.setActiveGameObjectInEditor (currentPlayer);
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Main Player Inventory", false, 10)]
	public static void SelectMainPlayerInventory ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			CollapseInspectorEditor.CollapseAllComponentsButOne (currentPlayer, typeof(inventoryManager));
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Main Player Controller", false, 10)]
	public static void SelectMainPlayerController ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			CollapseInspectorEditor.CollapseAllComponentsButOne (currentPlayer, typeof(playerController));
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Main Player Controller Input", false, 10)]
	public static void SelectMainPlayerInput ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			CollapseInspectorEditor.CollapseAllComponentsButOne (currentPlayer, typeof(playerInputManager));
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Main Input Manager", false, 10)]
	public static void SelectMainInput ()
	{
		gameManager currentGameManager = FindObjectOfType<gameManager> ();

		if (currentGameManager != null) {
			GKC_Utils.setActiveGameObjectInEditor (currentGameManager.gameObject);

			CollapseInspectorEditor.CollapseAllComponentsButOne (currentGameManager.gameObject, typeof(inputManager));
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Main Player Camera", false, 10)]
	public static void SelectMainPlayerCamera ()
	{
		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				playerCamera currentPlayerCamera = currentPlayerComponentsManager.getPlayerCamera ();

				if (currentPlayerCamera != null) {
					GKC_Utils.setActiveGameObjectInEditor (currentPlayerCamera.gameObject);

					CollapseInspectorEditor.CollapseAllComponentsButOne (currentPlayerCamera.gameObject, typeof(playerCamera));
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Toggle Main Player Camera View", false, 10)]
	public static void ToggleMainPlayerCameraView ()
	{
		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				playerCamera currentPlayerCamera = currentPlayerComponentsManager.getPlayerCamera ();

				if (currentPlayerCamera != null) {
					currentPlayerCamera.toggleViewOnEditor ();
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Main Player Weapons Manager", false, 10)]
	public static void SelectMainPlayerWeaponsManager ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			CollapseInspectorEditor.CollapseAllComponentsButOne (currentPlayer, typeof(playerWeaponsManager));
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Player Weapons", false, 10)]
	public static void SelectPlayerWeapons ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				playerWeaponsManager currentPlayerWeaponsManager = currentPlayerComponentsManager.getPlayerWeaponsManager ();

				if (currentPlayerWeaponsManager != null) {
					currentPlayerWeaponsManager.selectFirstWeaponGameObjectOnEditor ();
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Player Melee Weapons", false, 10)]
	public static void SelectPlayerMeleeWeapons ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				grabbedObjectMeleeAttackSystem currentGrabbedObjectMeleeAttackSystem = currentPlayerComponentsManager.getGrabbedObjectMeleeAttackSystem ();

				if (currentGrabbedObjectMeleeAttackSystem != null) {
					GKC_Utils.setActiveGameObjectInEditor (currentGrabbedObjectMeleeAttackSystem.gameObject);

					CollapseInspectorEditor.CollapseAllComponentsButOne (currentGrabbedObjectMeleeAttackSystem.gameObject, typeof(grabbedObjectMeleeAttackSystem));
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Collapse Components On Current Object", false, 10)]
	public static void CollapseAllComponentsOnCurrentSelectedObject ()
	{
		GameObject currentObject = GKC_Utils.getActiveGameObjectInEditor ();

		if (currentObject != null) {
			CollapseInspectorEditor.SetAllInspectorsExpanded (currentObject, false);
		}
	}

	[MenuItem ("Game Kit Controller/Others/Move Current Object Selected On Camera Editor", false, 10)]
	public static void PlaceObjectInCameraEditorPositionSystem ()
	{
		gameManager currentGameManager = FindObjectOfType<gameManager> ();

		if (currentGameManager != null) {
			placeObjectInCameraEditorPositionSystem currentPlaceObjectInCameraEditorPositionSystem = currentGameManager.GetComponent<placeObjectInCameraEditorPositionSystem> ();

			if (currentPlaceObjectInCameraEditorPositionSystem != null) {
				currentPlaceObjectInCameraEditorPositionSystem.moveObjects ();
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Player Manual Character Creator", false, 10)]
	public static void SelectPlayerManualCharacterCreator ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				buildPlayer currentBuildPlayer = currentPlayerComponentsManager.getBuildPlayer ();

				if (currentBuildPlayer != null) {
					GKC_Utils.setActiveGameObjectInEditor (currentBuildPlayer.gameObject);

					CollapseInspectorEditor.CollapseAllComponentsButOne (currentBuildPlayer.gameObject, typeof(buildPlayer));
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Player Menu Pause", false, 10)]
	public static void SelectPlayerMenuPause ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				menuPause currentMenuPause = currentPlayerComponentsManager.getPauseManager ();

				if (currentMenuPause != null) {
					GKC_Utils.setActiveGameObjectInEditor (currentMenuPause.gameObject);

					CollapseInspectorEditor.CollapseAllComponentsButOne (currentMenuPause.gameObject, typeof(menuPause));
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Map System", false, 10)]
	public static void SelectMapSystem ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				mapSystem currentMapSystem = currentPlayerComponentsManager.getMapSystem ();

				if (currentMapSystem != null) {
					GKC_Utils.setActiveGameObjectInEditor (currentMapSystem.gameObject);

					CollapseInspectorEditor.CollapseAllComponentsButOne (currentMapSystem.gameObject, typeof(mapSystem));
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Player Health", false, 10)]
	public static void SelectPlayerHealth ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				health currentHealth = currentPlayerComponentsManager.getHealth ();

				if (currentHealth != null) {
					GKC_Utils.setActiveGameObjectInEditor (currentHealth.gameObject);

					CollapseInspectorEditor.CollapseAllComponentsButOne (currentHealth.gameObject, typeof(health));
				}
			}
		}
	}

	[MenuItem ("Game Kit Controller/Others/Select Player Custom Character Manager", false, 10)]
	public static void SelectPlayerCustomCharacterManager ()
	{
		selectMainPlayerOnScene ();

		GameObject currentPlayer = GKC_Utils.findMainPlayerOnScene ();

		if (currentPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				customCharacterControllerManager currentCustomCharacterControllerManager = currentPlayerComponentsManager.getCustomCharacterControllerManager ();

				if (currentCustomCharacterControllerManager != null) {
					GKC_Utils.setActiveGameObjectInEditor (currentCustomCharacterControllerManager.gameObject);

					CollapseInspectorEditor.CollapseAllComponentsButOne (currentCustomCharacterControllerManager.gameObject, typeof(customCharacterControllerManager));
				}
			}
		}
	}


}
