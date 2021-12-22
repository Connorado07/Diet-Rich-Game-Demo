using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class addMeleeWeaponToCharacterEditor : EditorWindow
{
	GUISkin guiSkin;
	Rect windowRect = new Rect ();
	Event currentEvent;

	Vector2 rectSize = new Vector2 (550, 600);

	bool weaponAdded;

	float timeToBuild = 0.2f;
	float timer;

	string prefabsPath = "Assets/Game Kit Controller/Prefabs/Melee Combat System/Melee Weapons/";

	GUIStyle style = new GUIStyle ();

	float windowHeightPercentage = 0.38f;

	Vector2 screenResolution;

	playerComponentsManager mainPlayerComponentsManager;
	meleeWeaponsGrabbedManager mainMeleeWeaponsGrabbedManager;

	public string[] weaponList;
	public int weaponIndex;
	string newWeaponName;

	bool weaponListAssigned;

	[MenuItem ("Game Kit Controller/Create New Weapon/Add Melee Weapon To Character", false, 2)]
	public static void addMeleeWeaponToCharacter ()
	{
		GetWindow<addMeleeWeaponToCharacterEditor> ();
	}

	void OnEnable ()
	{
		weaponListAssigned = false;

		newWeaponName = "";

		screenResolution = new Vector2 (Screen.currentResolution.width, Screen.currentResolution.height);

		//		Debug.Log (screenResolution + " " + partsHeight + " " + settingsHeight + " " + previewHeight);

		float totalHeight = screenResolution.y * windowHeightPercentage;

		if (totalHeight < 500) {
			totalHeight = 500;
		}

		rectSize = new Vector2 (550, totalHeight);

		resetCreatorValues ();

		checkCurrentWeaponSelected (Selection.activeGameObject);
	}

	void checkCurrentWeaponSelected (GameObject currentCharacterSelected)
	{
		if (currentCharacterSelected) {
			mainPlayerComponentsManager = currentCharacterSelected.GetComponentInChildren<playerComponentsManager> ();

			if (mainPlayerComponentsManager != null) {
				mainMeleeWeaponsGrabbedManager = mainPlayerComponentsManager.getMeleeWeaponsGrabbedManager ();

				if (!Directory.Exists (prefabsPath)) {
					Debug.Log ("WARNING: " + prefabsPath + " path doesn't exist, make sure the path is from an existing folder in the project");

					return;
				}

				string[] search_results = null;

				search_results = System.IO.Directory.GetFiles (prefabsPath, "*.prefab");

				if (search_results.Length > 0) {
					weaponList = new string[search_results.Length];
					int currentWeaponIndex = 0;

					foreach (string file in search_results) {
						//must convert file path to relative-to-unity path (and watch for '\' character between Win/Mac)
						GameObject currentPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath (file, typeof(GameObject)) as GameObject;

						if (currentPrefab) {
							string currentWeaponName = currentPrefab.name;
							weaponList [currentWeaponIndex] = currentWeaponName;
							currentWeaponIndex++;
						} else {
							Debug.Log ("WARNING: something went wrong when trying to get the prefab in the path " + file);
						}
					}

					weaponListAssigned = true;
				} else {
					Debug.Log ("Weapon prefab not found in path " + prefabsPath);

					weaponList = new string[0];
				}
			}
		}
	}

	void OnDisable ()
	{
		resetCreatorValues ();
	}

	void resetCreatorValues ()
	{
		if (weaponAdded) {

		} else {

		}

		mainMeleeWeaponsGrabbedManager = null;

		weaponAdded = false;

		weaponListAssigned = false;

		mainPlayerComponentsManager = null;

		Debug.Log ("Weapon window closed");
	}

	void OnGUI ()
	{
		if (!guiSkin) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}
		GUI.skin = guiSkin;

		this.minSize = rectSize;

		this.titleContent = new GUIContent ("Weapons", null, "Add Melee Weapon To Character");

		GUILayout.BeginVertical ("Add Melee Weapon To Character", "window");

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		windowRect = GUILayoutUtility.GetLastRect ();
		windowRect.position = new Vector2 (0, windowRect.position.y);
		windowRect.width = this.maxSize.x;

		GUILayout.BeginHorizontal ();

		EditorGUILayout.HelpBox ("", MessageType.Info);

		style = new GUIStyle (EditorStyles.helpBox);
		style.richText = true;

		style.fontStyle = FontStyle.Bold;
		style.fontSize = 17;

		EditorGUILayout.LabelField ("Select a weapon from the 'Melee Weapon To Add' list and press the button 'Add Melee Weapon To Character'. \n\n" +
		"If not character is selected in the hierarchy, select one and press the button 'Check Current Object Selected'.\n\n", style);
		GUILayout.EndHorizontal ();

		if (mainMeleeWeaponsGrabbedManager == null) {

			GUILayout.FlexibleSpace ();

			GUILayout.BeginHorizontal ();
			EditorGUILayout.HelpBox ("", MessageType.Warning);

			style = new GUIStyle (EditorStyles.helpBox);
			style.richText = true;

			style.fontStyle = FontStyle.Bold;
			style.fontSize = 17;
			EditorGUILayout.LabelField ("WARNING: No Character was found, make sure to select the player or an " +
			"humanoid AI to add a weapon to it.", style);

			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Check Current Object Selected")) {
				checkCurrentWeaponSelected (Selection.activeGameObject);
			}
		} else {
			if (weaponListAssigned) {
				if (weaponList.Length > 0) {

					GUILayout.FlexibleSpace ();

					EditorGUILayout.Space ();

					if (weaponIndex < weaponList.Length) {
						weaponIndex = EditorGUILayout.Popup ("Weapon To Add", weaponIndex, weaponList);

						newWeaponName = weaponList [weaponIndex];  
					}

					EditorGUILayout.Space ();

					if (GUILayout.Button ("Add Melee Weapon To Character")) {
						addWeapon ();
					}

					if (GUILayout.Button ("Cancel")) {
						this.Close ();
					}
				}
			} else {
				GUILayout.FlexibleSpace ();

				EditorGUILayout.Space ();

				GUILayout.BeginHorizontal ();
				EditorGUILayout.HelpBox ("", MessageType.Warning);

				style = new GUIStyle (EditorStyles.helpBox);
				style.richText = true;

				style.fontStyle = FontStyle.Bold;
				style.fontSize = 17;
				EditorGUILayout.LabelField ("WARNING: No weapons prefabs where found on the path " + prefabsPath, style);

				GUILayout.EndHorizontal ();
			}
		}

		GUILayout.EndVertical ();
	}

	void addWeapon ()
	{
		string pathForWeapon = prefabsPath + newWeaponName + ".prefab";

		GameObject currentPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath (pathForWeapon, typeof(GameObject)) as GameObject;

		if (currentPrefab != null) {
			grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = currentPrefab.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

			if (currentGrabPhysicalObjectMeleeAttackSystem != null) {

				string currentWeaponName = currentGrabPhysicalObjectMeleeAttackSystem.weaponName;

				mainMeleeWeaponsGrabbedManager.addNewMeleeWeaponPrefab (currentPrefab, currentWeaponName);

				GKC_Utils.updateDirtyScene ("Create Object To Grab", mainMeleeWeaponsGrabbedManager.gameObject);
			}

			weaponAdded = true;
		} else {
			Debug.Log ("WARNING: no prefab found on path " + prefabsPath + newWeaponName);
		}
	}

	void Update ()
	{
		if (weaponAdded) {
			if (timer < timeToBuild) {
				timer += 0.01f;

				if (timer > timeToBuild) {
					timer = 0;

					this.Close ();
				}
			}
		}
	}
}
#endif