using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class weaponCreatorEditor : EditorWindow
{
	GUISkin guiSkin;
	Rect windowRect = new Rect ();

	public GameObject currentWeapon;

	bool weaponCreated;

	Vector2 scrollPos1;
	Vector2 scrollPos2;
	Vector2 scrollPos3;

	float timeToBuild = 0.2f;
	float timer;
	string assetsPath;

	string prefabsPath = "Assets/Game Kit Controller/Prefabs/Weapons/Usable Weapons/";

	string currentWeaponName = "New Weapon";

	bool buttonPressed;

	public GameObject weaponGameObject;

	public weaponBuilder currentWeaponBuilder;

	weaponBuilder.partInfo currentWeaponPartInfo;

	weaponBuilder.settingsInfo currentWeaponSettingsInfo;

	bool showGizmo = true;
	bool useHandle = true;

	bool mainUseWeaponPartState;

	GUIStyle style = new GUIStyle ();

	float windowHeightPercentage = 0.65f;

	Vector2 rectSize = new Vector2 (580, 600);

	float minHeight = 600f;

	Vector2 screenResolution;

	bool selectingWeapon;

	bool weaponRemovedFromCreator;

	playerWeaponsManager currentPlayerWeaponsManager;

	public string[] weaponList;
	public int weaponIndex;
	string newWeaponName;

	GameObject currentPlayer;

	bool weaponSelectedAtStart;

	Vector2 previousRectSize;

	[Range (0.01f, 5)] float newWeaponScale = 1;

	public Texture weaponIconTexture;


	[MenuItem ("Game Kit Controller/Create New Weapon/Create New Fire Weapon", false, 2)]
	public static void createNewWeapon ()
	{
		GetWindow<weaponCreatorEditor> ();
	}

	void OnEnable ()
	{
		selectingWeapon = false;

		newWeaponName = "";

		screenResolution = new Vector2 (Screen.currentResolution.width, Screen.currentResolution.height);

		float windowHeight = screenResolution.y * windowHeightPercentage;

		windowHeight = Mathf.Clamp (windowHeight, minHeight, screenResolution.y);

		rectSize = new Vector2 (580, windowHeight);

		resetCreatorValues ();

		currentWeapon = Selection.activeGameObject;

		if (currentWeapon != null) {
			if (currentWeapon.GetComponent<weaponBuilder> ()) {
				weaponSelectedAtStart = true;
			} else {
				weaponSelectedAtStart = true;
			}
		}

		checkCurrentWeaponSelected ();
	}

	void checkCurrentWeaponSelected ()
	{
		currentPlayer = GameObject.FindWithTag ("Player");

		bool weaponFound = false;

		if (currentWeapon != null) {
			currentWeaponBuilder = currentWeapon.GetComponent<weaponBuilder> ();

			if (currentWeaponBuilder != null) {
				GameObject newWeaponObject = (GameObject)Instantiate (currentWeapon, Vector3.zero, Quaternion.identity);
				Transform newWeaponObjectTransform = newWeaponObject.transform;

				currentWeapon = newWeaponObject;
		
				newWeaponObjectTransform.SetParent (currentWeaponBuilder.weaponParent);
				newWeaponObjectTransform.localPosition = Vector3.zero;
				newWeaponObjectTransform.localRotation = Quaternion.identity;

				currentWeaponBuilder = currentWeapon.GetComponent<weaponBuilder> ();

				currentWeaponBuilder.weaponMeshParent.transform.position = Vector3.up * 1000;

				currentWeaponBuilder.weaponViewTransform.SetParent (currentWeaponBuilder.transform);

				currentWeaponBuilder.weaponMeshParent.transform.rotation = Quaternion.identity;
				currentWeaponBuilder.weaponMeshParent.transform.localScale = Vector3.one * 10;

				currentWeaponBuilder.alignViewWithWeaponCameraPosition ();

				currentWeaponName = "New Weapon";

				currentWeapon.name = currentWeaponName;

				setExpandOrCollapsePartElementsListState (false);

				setExpandOrCollapseSettingsElementsListState (false);

				weaponFound = true;

				GKC_Utils.setActiveGameObjectInEditor (currentWeapon);
			}
		}

		if (!weaponFound) {
			currentWeapon = null;

			if (currentPlayer != null) {
				currentPlayerWeaponsManager = currentPlayer.GetComponent<playerWeaponsManager> ();

				if (currentPlayerWeaponsManager != null) {
					if (newWeaponName.Equals ("")) {

						if (currentPlayerWeaponsManager.weaponsList.Count > 0) {
							int weaponListCount = 0;

							for (int i = 0; i < currentPlayerWeaponsManager.weaponsList.Count; i++) {
								IKWeaponSystem currentIKWeaponSystem = currentPlayerWeaponsManager.weaponsList [i];

								if (currentIKWeaponSystem != null) {
									weaponListCount++;
								}
							}

							weaponList = new string[weaponListCount];

							int currentWeaponIndex = 0;

							for (int i = 0; i < currentPlayerWeaponsManager.weaponsList.Count; i++) {
								IKWeaponSystem currentIKWeaponSystem = currentPlayerWeaponsManager.weaponsList [i];

								if (currentIKWeaponSystem != null) {
									string name = currentIKWeaponSystem.getWeaponSystemName ();
									weaponList [currentWeaponIndex] = name;
									currentWeaponIndex++;
								}
							}
						} else {
							weaponList = new string[0];
						}

						weaponIndex = 0;

						selectingWeapon = true;
					} else {
						selectingWeapon = false;

						currentWeapon = currentPlayerWeaponsManager.getIKWeaponSystem (newWeaponName).gameObject;

						checkCurrentWeaponSelected ();
					}
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
		if (weaponCreated) {

		} else {
			if (currentWeaponBuilder != null) {
				DestroyImmediate (currentWeaponBuilder.gameObject);

				Debug.Log ("destroy instantiated weapon");
			}
		}

		currentWeapon = null;

		if (currentWeaponBuilder != null) {
			currentWeaponBuilder.removeTemporalWeaponParts ();

			currentWeaponBuilder.showGizmo = false;
			currentWeaponBuilder.useHandle = false;
		}

		currentWeaponBuilder = null;

		weaponCreated = false;

		mainUseWeaponPartState = false;

		weaponRemovedFromCreator = false;

		newWeaponName = "";

		newWeaponScale = 1;

		weaponSelectedAtStart = false;

		Debug.Log ("Creator window closed");
	}

	void OnGUI ()
	{
		if (guiSkin == null) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}

		GUI.skin = guiSkin;

		this.minSize = rectSize;

		this.titleContent = new GUIContent ("Weapon", null, "Game Kit Controller Weapon Creator");

		scrollPos3 = EditorGUILayout.BeginScrollView (scrollPos3, false, false);

		GUILayout.BeginVertical (GUILayout.Width (580));

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Weapon Creator Window", "window");

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("box");

		currentWeaponName = (string)EditorGUILayout.TextField ("Weapon Name", currentWeaponName); 

		windowRect = GUILayoutUtility.GetLastRect ();
		windowRect.position = new Vector2 (0, windowRect.position.y);
		windowRect.width = this.maxSize.x;
	
		currentWeapon = EditorGUILayout.ObjectField ("Current Weapon", currentWeapon, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Weapon Icon", EditorStyles.boldLabel);

		weaponIconTexture = EditorGUILayout.ObjectField (weaponIconTexture, typeof(Texture), true, GUILayout.ExpandWidth (true)) as Texture;
		GUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		GUILayout.BeginHorizontal ();
		showGizmo = (bool)EditorGUILayout.Toggle ("Show Gizmo", showGizmo);
		useHandle = (bool)EditorGUILayout.Toggle ("Show Handle", useHandle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Window Height", EditorStyles.boldLabel);

		if (previousRectSize != rectSize) {
			previousRectSize = rectSize;

			this.maxSize = rectSize;
		}

		rectSize.y = EditorGUILayout.Slider (rectSize.y, minHeight, screenResolution.y);

		GUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		if (currentWeapon != null && currentWeaponBuilder == null) {
			if (weaponRemovedFromCreator || !weaponSelectedAtStart) {

				weaponRemovedFromCreator = false;

				checkCurrentWeaponSelected ();

				Debug.Log ("Weapon reassigned on creator");
				
				setExpandOrCollapsePartElementsListState (false);

				GKC_Utils.setActiveGameObjectInEditor (currentWeapon);
			}
		}

		if (currentWeaponBuilder == null) {
			GUILayout.FlexibleSpace ();
		}

		GUILayout.EndVertical ();

		if (currentWeaponBuilder != null) {

			EditorGUILayout.Space ();

			style.normal.textColor = Color.white;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 18;

			GUILayout.Label ("Weapon Settings Info List", style);

			GUILayout.BeginVertical ("", "window");

//			GUILayout.FlexibleSpace ();

			scrollPos2 = EditorGUILayout.BeginScrollView (scrollPos2, false, false);

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Replace Arms", EditorStyles.boldLabel);
			currentWeaponBuilder.replaceArmsModel = (bool)EditorGUILayout.Toggle ("", currentWeaponBuilder.replaceArmsModel);
			GUILayout.EndHorizontal ();

			if (currentWeaponBuilder.replaceArmsModel) {
				currentWeaponBuilder.newArmsModel = EditorGUILayout.ObjectField ("New Arms", currentWeaponBuilder.newArmsModel, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;
			}
				
			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Weapon Scale", EditorStyles.boldLabel);
			newWeaponScale = EditorGUILayout.Slider (newWeaponScale, 0.01f, 5);
			GUILayout.EndHorizontal ();

			currentWeaponBuilder.newWeaponMeshParent.localScale = Vector3.one * newWeaponScale;

			EditorGUILayout.Space ();

			for (int i = 0; i < currentWeaponBuilder.weaponSettingsInfoList.Count; i++) { 

				currentWeaponSettingsInfo = currentWeaponBuilder.weaponSettingsInfoList [i];

				GUILayout.BeginHorizontal ();

				GUILayout.BeginHorizontal ();
				GUILayout.Label (currentWeaponSettingsInfo.Name, EditorStyles.boldLabel);
				if (currentWeaponSettingsInfo.useBoolState) {
					currentWeaponSettingsInfo.boolState = (bool)EditorGUILayout.Toggle ("", currentWeaponSettingsInfo.boolState, GUILayout.MaxWidth (50));
				}

				if (currentWeaponSettingsInfo.useFloatValue) {
					currentWeaponSettingsInfo.floatValue = (float)EditorGUILayout.FloatField ("", currentWeaponSettingsInfo.floatValue, GUILayout.MaxWidth (50));
				}
				GUILayout.EndHorizontal ();

				GUILayout.EndHorizontal ();
			}   

			EditorGUILayout.EndScrollView ();

			GUILayout.EndVertical ();


			style.normal.textColor = Color.white;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 18;

			GUILayout.Label ("Weapon Parts Info List", style);


			GUILayout.BeginVertical ("", "window");

//			GUILayout.FlexibleSpace ();

			scrollPos1 = EditorGUILayout.BeginScrollView (scrollPos1, false, false);

			for (int i = 0; i < currentWeaponBuilder.weaponPartInfoList.Count; i++) { 

				currentWeaponPartInfo = currentWeaponBuilder.weaponPartInfoList [i];

				currentWeaponPartInfo.expandElement = EditorGUILayout.Foldout (currentWeaponPartInfo.expandElement, currentWeaponPartInfo.Name);

				if (currentWeaponPartInfo.expandElement) {
					GUILayout.BeginVertical ("label");

					currentWeaponPartInfo.newWeaponMesh = 
							EditorGUILayout.ObjectField ("New Weapon Mesh", currentWeaponPartInfo.newWeaponMesh, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;

					EditorGUILayout.Space ();

					if (currentWeaponPartInfo.newWeaponMesh == null) {
						GUILayout.BeginHorizontal ();
						GUILayout.Label ("Remove Weapon Part If No New Mesh Added", EditorStyles.boldLabel);
					
						currentWeaponPartInfo.removeWeaponPartIfNoNewMesh = (bool)EditorGUILayout.Toggle ("", currentWeaponPartInfo.removeWeaponPartIfNoNewMesh, GUILayout.MaxWidth (50));
						GUILayout.EndHorizontal ();
					} else {
						currentWeaponPartInfo.removeWeaponPartIfNoNewMesh = false;

						if (currentWeaponPartInfo.temporalWeaponMesh != null) {
							if (!currentWeaponBuilder.useHandle) {
								currentWeaponPartInfo.newWeaponMeshPositionOffset = (Vector3)EditorGUILayout.Vector3Field ("Position Offset", currentWeaponPartInfo.newWeaponMeshPositionOffset);
								currentWeaponPartInfo.newWeaponMeshEulerOffset = (Vector3)EditorGUILayout.Vector3Field ("Euler Offset", currentWeaponPartInfo.newWeaponMeshEulerOffset);
							}

							if (GUILayout.Button ("O")) {
								GKC_Utils.setActiveGameObjectInEditor (currentWeaponPartInfo.temporalWeaponMesh);
							}
						}
					}

					GUILayout.EndVertical ();
				}

				if (!currentWeaponPartInfo.removeWeaponPartIfNoNewMesh) {
					if (currentWeaponPartInfo.newWeaponMesh != null) {
						if (!currentWeaponPartInfo.temporalWeaponMeshInstantiated) {
							currentWeaponPartInfo.temporalWeaponMeshInstantiated = true;

							if (currentWeaponPartInfo.currentWeaponMesh != null) {
								currentWeaponPartInfo.currentWeaponMesh.SetActive (false);
							}

							if (currentWeaponPartInfo.extraWeaponPartMeshesList.Count > 0) {
								for (int j = 0; j < currentWeaponPartInfo.extraWeaponPartMeshesList.Count; j++) { 
									if (currentWeaponPartInfo.extraWeaponPartMeshesList [j] != null) {
										currentWeaponPartInfo.extraWeaponPartMeshesList [j].SetActive (false);
									}
								}
							}
						}

						if (currentWeaponPartInfo.newWeaponMesh != currentWeaponPartInfo.temporalNewWeaponMesh) {

							if (currentWeaponPartInfo.temporalWeaponMesh != null) {
								DestroyImmediate (currentWeaponPartInfo.temporalWeaponMesh);
							}

							currentWeaponPartInfo.temporalNewWeaponMesh = currentWeaponPartInfo.newWeaponMesh;

							GameObject newWeaponPart = (GameObject)Instantiate (currentWeaponPartInfo.newWeaponMesh, Vector3.zero, Quaternion.identity);

							Transform newWeaponPartTransform = newWeaponPart.transform;

							newWeaponPartTransform.SetParent (currentWeaponPartInfo.weaponMeshParent);
							newWeaponPartTransform.localPosition = Vector3.zero;
							newWeaponPartTransform.localRotation = Quaternion.identity;
							newWeaponPartTransform.localScale = Vector3.one;

							currentWeaponPartInfo.temporalWeaponMesh = newWeaponPart;
						}
					} else {
						if (currentWeaponPartInfo.currentWeaponMesh != null && !currentWeaponPartInfo.currentWeaponMesh.activeSelf) {
							currentWeaponPartInfo.currentWeaponMesh.SetActive (true);
						}

						if (currentWeaponPartInfo.extraWeaponPartMeshesList.Count > 0) {
							for (int j = 0; j < currentWeaponPartInfo.extraWeaponPartMeshesList.Count; j++) { 
								if (currentWeaponPartInfo.extraWeaponPartMeshesList [j] != null && !currentWeaponPartInfo.extraWeaponPartMeshesList [j].activeSelf) {
									currentWeaponPartInfo.extraWeaponPartMeshesList [j].SetActive (true);
								}
							}
						}

						if (currentWeaponPartInfo.temporalWeaponMeshInstantiated) {

							if (currentWeaponPartInfo.temporalWeaponMesh != null) {
								DestroyImmediate (currentWeaponPartInfo.temporalWeaponMesh);
							}

							currentWeaponPartInfo.temporalWeaponMeshInstantiated = false;
						}
					}

					if (currentWeaponPartInfo.temporalWeaponMesh != null) {
						if (!currentWeaponBuilder.useHandle) {
							currentWeaponPartInfo.temporalWeaponMesh.transform.localPosition = Vector3.zero + currentWeaponPartInfo.newWeaponMeshPositionOffset;
							currentWeaponPartInfo.temporalWeaponMesh.transform.localEulerAngles = Vector3.zero + currentWeaponPartInfo.newWeaponMeshEulerOffset;
						}
					}
				} else {
					if (currentWeaponPartInfo.currentWeaponMesh != null && currentWeaponPartInfo.currentWeaponMesh.activeSelf) {
						currentWeaponPartInfo.currentWeaponMesh.SetActive (false);
					}

					if (currentWeaponPartInfo.extraWeaponPartMeshesList.Count > 0) {
						for (int j = 0; j < currentWeaponPartInfo.extraWeaponPartMeshesList.Count; j++) { 
							if (currentWeaponPartInfo.extraWeaponPartMeshesList [j] != null && currentWeaponPartInfo.extraWeaponPartMeshesList [j].activeSelf) {
								currentWeaponPartInfo.extraWeaponPartMeshesList [j].SetActive (false);
							}
						}
					}
				}

				EditorGUILayout.Space ();
			}   

			EditorGUILayout.EndScrollView ();

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Expand All")) {
				setExpandOrCollapsePartElementsListState (true);
			}
			if (GUILayout.Button ("Collapse All")) {
				setExpandOrCollapsePartElementsListState (false);
			}
			if (GUILayout.Button ("Toggle All Parts")) {
				setUseWeaponPartsListState (!mainUseWeaponPartState);
			}
			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();

			currentWeaponBuilder.showGizmo = showGizmo;
			currentWeaponBuilder.useHandle = useHandle;

			if (currentWeapon == null) {
				resetCreatorValues ();

				weaponRemovedFromCreator = true;

				Debug.Log ("Weapon removed from creator");

				selectingWeapon = true;

				checkCurrentWeaponSelected ();
			}

		} else {
			if (selectingWeapon) {
				if (weaponList.Length > 0) {
					if (weaponIndex < weaponList.Length) {
						weaponIndex = EditorGUILayout.Popup ("Weapon To Build", weaponIndex, weaponList);

						newWeaponName = weaponList [weaponIndex];  
					}

					EditorGUILayout.Space ();

					if (GUILayout.Button ("Select Weapon Type")) {
						currentWeapon = currentPlayer;

						checkCurrentWeaponSelected ();

						return;
					}

					if (GUILayout.Button ("Cancel")) {
						this.Close ();
					}
				} else {
					GUILayout.BeginHorizontal ();
					EditorGUILayout.HelpBox ("", MessageType.Info);

					style = new GUIStyle (EditorStyles.helpBox);
					style.richText = true;

					style.fontStyle = FontStyle.Bold;
					style.fontSize = 16;

					EditorGUILayout.LabelField ("There are not weapons configured on the current player on the scene. " +
					"Go to Assets -> Game Kit Controller -> Prefabs -> Weapons -> Usable Weapons and drop the weapon template you want to use on the scene.\n" +
					"Then, select that weapon on the hierarchy and close and open the weapon creator wizard again or assign it on the Current Weapon field to use that weapon as template.\n" +
					"Finally, after creating the weapon, follow the steps of the tutorial video to add a new weapon to any character, selecting the new weapon created for it.", style);
					GUILayout.EndHorizontal ();

					EditorGUILayout.Space ();

					EditorGUILayout.Space ();
				}
			}
			GUILayout.BeginHorizontal ();

			EditorGUILayout.HelpBox ("", MessageType.Info);

			style = new GUIStyle (EditorStyles.helpBox);
			style.richText = true;

			style.fontStyle = FontStyle.Bold;
			style.fontSize = 17;

			EditorGUILayout.LabelField ("Select a weapon from the 'Weapon To Build' list and press the button Select Weapon Type. \n" +
			"Or, drop a weapon on 'Current Weapon' field from one of the weapons located in player's body, to build one of similar characteristics.\n\n" +
			"Once that is done, drop the Weapon mesh prefab in the scene and assign its parts into the fields you need.\n\n" +
			"If the 'Weapon To Build' list is empty, add first a weapon to the character using the 'Add Fire Weapon To Character' wizard, " +
			"and add the weapon type that is closest to the configuration that you want to use for the new weapon. \n\n" +
			"Then, open this wizard again and select the new type configured, and customize the new weapon as you need.", style);
			GUILayout.EndHorizontal ();

			if (currentWeapon != null && currentWeaponBuilder == null) {

				GUILayout.BeginHorizontal ();
				EditorGUILayout.HelpBox ("", MessageType.Warning);

				style = new GUIStyle (EditorStyles.helpBox);
				style.richText = true;

				style.fontStyle = FontStyle.Bold;
				style.fontSize = 17;
				EditorGUILayout.LabelField ("WARNING: The object placed in the field 'Current Weapon' is not a Weapon." +
				" Please, make sure to assign a GKC Weapon prefab on that field.", style);

				GUILayout.EndHorizontal ();
			}
		}

		if (currentWeapon != null && currentWeaponBuilder != null) {
			GUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Cancel")) {
				this.Close ();
			}

			if (GUILayout.Button ("Align View")) {
				if (currentWeaponBuilder != null) {
					currentWeaponBuilder.alignViewWithWeaponCameraPosition ();

					GKC_Utils.setActiveGameObjectInEditor (currentWeapon);
				}
			}

			if (GUILayout.Button ("Reset Weapon")) {
				if (currentWeaponBuilder != null) {
					currentWeaponBuilder.removeTemporalWeaponParts ();
				}
			}

			if (GUILayout.Button ("Create Weapon")) {
				createWeapon ();
			}

			GUILayout.EndHorizontal ();
		}

		GUILayout.EndVertical ();

		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		EditorGUILayout.EndScrollView ();
	}

	void createWeapon ()
	{
		string prefabPath = prefabsPath + currentWeaponName;

		prefabPath += ".prefab";

		bool instantiateGameObject = false;

		if (currentWeapon != null) {
			weaponGameObject = currentWeapon;
		} else {
			instantiateGameObject = true;

			weaponGameObject = (GameObject)AssetDatabase.LoadAssetAtPath (prefabPath, typeof(GameObject));
		}

		if (weaponGameObject != null) {
			if (instantiateGameObject) {

			} else {
				currentWeaponBuilder.weaponMeshParent.transform.localScale = Vector3.one;

				currentWeaponBuilder.weaponMeshParent.transform.localRotation = Quaternion.identity;

				currentWeaponBuilder.weaponViewTransform.SetParent (currentWeaponBuilder.weaponMeshParent.transform);

				currentWeaponBuilder.weaponMeshParent.transform.localPosition = Vector3.zero;

				currentWeaponBuilder.setNewWeaponIconTexture (weaponIconTexture);
			}

			currentWeaponBuilder.setNewWeaponName (currentWeaponName);

			weaponCreated = true;
		} else {
			Debug.Log ("Weapon prefab not found in path " + prefabPath);
		}
	}

	void Update ()
	{
		if (weaponCreated) {
			if (timer < timeToBuild) {
				timer += 0.01f;

				if (timer > timeToBuild) {
					currentWeaponBuilder.buildWeapon ();

					timer = 0;

					this.Close ();
				}
			}
		}
	}

	public void setExpandOrCollapsePartElementsListState (bool state)
	{
		if (currentWeaponBuilder != null) {
			for (int i = 0; i < currentWeaponBuilder.weaponPartInfoList.Count; i++) {
				currentWeaponPartInfo = currentWeaponBuilder.weaponPartInfoList [i];
				currentWeaponPartInfo.expandElement = state;
			}
		}
	}

	public void setUseWeaponPartsListState (bool state)
	{
		if (currentWeaponBuilder != null) {
			mainUseWeaponPartState = state;

			for (int i = 0; i < currentWeaponBuilder.weaponPartInfoList.Count; i++) {
				currentWeaponPartInfo = currentWeaponBuilder.weaponPartInfoList [i];
				currentWeaponPartInfo.removeWeaponPartIfNoNewMesh = mainUseWeaponPartState;
			}
		}
	}

	public void setExpandOrCollapseSettingsElementsListState (bool state)
	{
		if (currentWeaponBuilder != null) {
			for (int i = 0; i < currentWeaponBuilder.weaponSettingsInfoList.Count; i++) {
				currentWeaponSettingsInfo = currentWeaponBuilder.weaponSettingsInfoList [i];
				currentWeaponSettingsInfo.expandElement = state;
			}
		}
	}
}
#endif