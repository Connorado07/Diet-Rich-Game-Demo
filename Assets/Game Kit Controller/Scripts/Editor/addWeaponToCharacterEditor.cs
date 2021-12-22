using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class addWeaponToCharacterEditor : EditorWindow
{
	GUISkin guiSkin;
	Rect windowRect = new Rect ();
	Event currentEvent;

	Vector2 rectSize = new Vector2 (550, 600);

	bool weaponAdded;

	float timeToBuild = 0.2f;
	float timer;

	string prefabsPath = "Assets/Game Kit Controller/Prefabs/Weapons/Usable Weapons/";

	public GameObject weaponGameObject;

	GUIStyle style = new GUIStyle ();

	float windowHeightPercentage = 0.38f;

	Vector2 screenResolution;

	playerWeaponsManager currentPlayerWeaponsManager;

	Transform currentPlayerWeaponsParent;

	public string[] weaponList;
	public int weaponIndex;
	string newWeaponName;

	bool weaponListAssigned;

	public bool removeAttachmentSystemFromWeapon;
	public bool removeWeapon3dHudPanel;
	public bool weaponUsedOnAI;
	public float newWeaponDamage;
	public int newAmmoAmount;
	public float newFireRate;

	[MenuItem ("Game Kit Controller/Create New Weapon/Add Fire Weapon To Character", false, 2)]
	public static void addWeaponToCharacter ()
	{
		GetWindow<addWeaponToCharacterEditor> ();
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
			currentPlayerWeaponsManager = currentCharacterSelected.GetComponentInChildren<playerWeaponsManager> ();
		
			if (currentPlayerWeaponsManager) {
				currentPlayerWeaponsParent = currentPlayerWeaponsManager.getWeaponsParent ();

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

		currentPlayerWeaponsManager = null;

		weaponAdded = false;

		weaponListAssigned = false;

		removeAttachmentSystemFromWeapon = false;

		removeWeapon3dHudPanel = false;

		weaponUsedOnAI = false;

		newWeaponDamage = 0;

		newAmmoAmount = 0;

		newFireRate = 0;

		Debug.Log ("Weapon window closed");
	}

	void OnGUI ()
	{
		if (!guiSkin) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}
		GUI.skin = guiSkin;

		this.minSize = rectSize;

		this.titleContent = new GUIContent ("Weapons", null, "Add Weapon To Character");

		GUILayout.BeginVertical ("Add Weapon To Character Creator Window", "window");

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

		EditorGUILayout.LabelField ("Select a weapon from the 'Weapon To Add' list and press the button 'Add Weapon To Character'. \n\n" +
		"If not character is selected in the hierarchy, select one and press the button 'Check Current Object Selected'.\n\n", style);
		GUILayout.EndHorizontal ();

		if (currentPlayerWeaponsManager == null) {

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

						removeAttachmentSystemFromWeapon = (bool)EditorGUILayout.Toggle ("Remove Attachments", removeAttachmentSystemFromWeapon);

						removeWeapon3dHudPanel = (bool)EditorGUILayout.Toggle ("Remove HUD 3d Panel", removeWeapon3dHudPanel);

						weaponUsedOnAI = (bool)EditorGUILayout.Toggle ("Weapon Used On AI", weaponUsedOnAI);

						newWeaponDamage = (float)EditorGUILayout.FloatField ("New Weapon Damage", newWeaponDamage);

						newAmmoAmount = (int)EditorGUILayout.IntField ("New Ammo Amount", newAmmoAmount);

						newFireRate = (float)EditorGUILayout.FloatField ("New Fire Rate", newFireRate);
					}

					EditorGUILayout.Space ();

					if (GUILayout.Button ("Add Weapon To Character")) {
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
		if (currentPrefab) {
			GameObject newWeaponCreated = (GameObject)Instantiate (currentPrefab, Vector3.zero, Quaternion.identity);
			newWeaponCreated.name = newWeaponName;

			newWeaponCreated.transform.SetParent (currentPlayerWeaponsParent);
			newWeaponCreated.transform.localPosition = Vector3.zero;
			newWeaponCreated.transform.localRotation = Quaternion.identity;

			weaponAdded = true;

			if (removeAttachmentSystemFromWeapon) {
				weaponAttachmentSystem currentWeaponAttachmentSystem = newWeaponCreated.GetComponentInChildren<weaponAttachmentSystem> ();

				if (currentWeaponAttachmentSystem != null) {
					DestroyImmediate (currentWeaponAttachmentSystem.gameObject);
				}
			}

			playerWeaponSystem currentPlayerWeaponSystem = newWeaponCreated.GetComponentInChildren<playerWeaponSystem> ();

			if (currentPlayerWeaponSystem != null) {
				if (removeWeapon3dHudPanel) {
				
					GameObject weaponHUDGameObject = currentPlayerWeaponSystem.getWeaponHUDGameObject ();

					if (weaponHUDGameObject != null) {
						DestroyImmediate (weaponHUDGameObject);
					}
				}

				if (newWeaponDamage > 0) {
					currentPlayerWeaponSystem.setProjectileDamage (newWeaponDamage);
				}

				if (newAmmoAmount > 0) {
					currentPlayerWeaponSystem.setRemainAmmoAmountFromEditor (newAmmoAmount);
				}

				if (newFireRate > 0) {
					currentPlayerWeaponSystem.setFireRateFromEditor (newFireRate);
				}
			}

			if (weaponUsedOnAI) {
				weaponBuilder currentWeaponBuilder = newWeaponCreated.GetComponent<weaponBuilder> ();

				if (currentWeaponBuilder != null) {
					currentWeaponBuilder.checkWeaponsPartsToRemoveOnAI ();
				}

				IKWeaponSystem currentIKWeaponSystem = newWeaponCreated.GetComponent<IKWeaponSystem> ();

				if (currentIKWeaponSystem != null) {
					currentIKWeaponSystem.setWeaponEnabledState (true);
				}
			}

			currentPlayerWeaponsManager.setWeaponList ();
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