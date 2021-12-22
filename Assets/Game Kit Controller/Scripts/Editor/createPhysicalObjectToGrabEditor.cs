using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class createPhysicalObjectToGrabEditor : EditorWindow
{
	GUISkin guiSkin;
	Rect windowRect = new Rect ();

	Vector2 rectSize = new Vector2 (550, 600);

	bool objectToGrabCreated;

	float timeToBuild = 0.2f;
	float timer;
	string assetsPath;

	GUIStyle style = new GUIStyle ();

	float windowHeightPercentage = 0.3f;

	Vector2 screenResolution;

	grabPhysicalObjectSystem currentGrabPhysicalObjectSystem;

	public string[] objectToGrabList;
	public int objectToGrabIndex;
	string newObjectToGrabName;

	bool objectToGrabListAssigned;

	GameObject newObjectToGrabMesh;

	public bool configureObjectColliderAutomatically = true;
	public bool useBoxCollider;
	public bool useMeshCollider;

	[Range (0.01f, 5)] float newObjectMeshScale = 1;


	static string prefabsPath = "Assets/Game Kit Controller/Prefabs/Objects To Grab Physically/Templates/Regular Objects Templates/";

	static string editorTitle = "Grab Objects";

	static string editorDescription = "Create New Object To Grab";

	static string editorSecondaryTitle = "Create Object To Grab Physically";

	static string editorInstructions = "Select an Object To Grab Type from the 'Object To Grab Type' list and press the button 'Create Object'. \n\n" +
	                                   "After that, make sure to adjust the collider size and shape for the new object created as you consider better.\n\n";


	public string temporalPrefabsPath;

	public string temporalEditorTitle;

	public string temporalEditorDescription;

	public string temporalEditorSecondaryTitle;

	public string temporalEditorInstructions;

	bool creatingMeleeWeapon;

	bool addMeleeWeaponToInventory;

	public string relativePathWeaponsMesh = "Assets/Game Kit Controller/Prefabs/Inventory/Mesh/Melee Weapons";

	public string relativePathMeleeWeapons = "Assets/Game Kit Controller/Prefabs/Melee Combat System/Melee Weapons";

	Texture weaponIconTexture;

	Vector2 previousRectSize;

	float minHeight;

	string currentWeaponName = "New Weapon";

	public bool useNewPathForMeleeWeapon;
	[TextArea (3, 5)] public string newPathForMeleeWeapon;

	[MenuItem ("Game Kit Controller/Create New Object To Grab Physically", false, 2)]
	public static void createPhysicalObjectToGrab ()
	{
		createPhysicalObjectToGrabEditor editorWindow = EditorWindow.GetWindow<createPhysicalObjectToGrabEditor> ();

		editorWindow.setTemporalPrefabsPath (prefabsPath, editorTitle, editorDescription, editorSecondaryTitle, editorInstructions, false);
	}

	public void setTemporalPrefabsPath (string newPrefabsPath, string newEditorTitle, string newEditorDescription, 
	                                    string newEditorSecondaryTitle, string newEditorInstructions, bool creatingMeleeWeaponValue)
	{
		temporalPrefabsPath = newPrefabsPath;

		temporalEditorTitle = newEditorTitle;

		temporalEditorDescription = newEditorDescription;

		temporalEditorSecondaryTitle = newEditorSecondaryTitle;

		temporalEditorInstructions = newEditorInstructions;

		creatingMeleeWeapon = creatingMeleeWeaponValue;


		objectToGrabListAssigned = false;

		newObjectToGrabName = "";

		screenResolution = new Vector2 (Screen.currentResolution.width, Screen.currentResolution.height);

		//		Debug.Log (screenResolution + " " + partsHeight + " " + settingsHeight + " " + previewHeight);

		float totalHeight = screenResolution.y * windowHeightPercentage;

		if (totalHeight < 400) {
			totalHeight = 400;
		}

		if (creatingMeleeWeapon) {
			totalHeight += 150;
		}

		minHeight = totalHeight;

		rectSize = new Vector2 (550, totalHeight);

		resetCreatorValues ();

		getGrabObjectPrefabsList ();
	}

	void getGrabObjectPrefabsList ()
	{
		if (!Directory.Exists (temporalPrefabsPath)) {
			Debug.Log ("WARNING: " + temporalPrefabsPath + " path doesn't exist, make sure the path is from an existing folder in the project");

			return;
		}

		string[] search_results = null;

		search_results = System.IO.Directory.GetFiles (temporalPrefabsPath, "*.prefab");

		if (search_results.Length > 0) {
			objectToGrabList = new string[search_results.Length];
			int currentObjectToGrabIndex = 0;

			foreach (string file in search_results) {
				//must convert file path to relative-to-unity path (and watch for '\' character between Win/Mac)
				GameObject currentPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath (file, typeof(GameObject)) as GameObject;

				if (currentPrefab) {
					string currentObjectToGrabName = currentPrefab.name;
					objectToGrabList [currentObjectToGrabIndex] = currentObjectToGrabName;
					currentObjectToGrabIndex++;
				} else {
					Debug.Log ("WARNING: something went wrong when trying to get the prefab in the path " + file);
				}
			}

			objectToGrabListAssigned = true;
		} else {
			Debug.Log ("Object To Grab prefab not found in path " + temporalPrefabsPath);

			objectToGrabList = new string[0];
		}

		useNewPathForMeleeWeapon = false;

		newPathForMeleeWeapon = relativePathMeleeWeapons;
	}

	void OnDisable ()
	{
		resetCreatorValues ();
	}

	void resetCreatorValues ()
	{
		if (objectToGrabCreated) {

		} else {

		}

		currentGrabPhysicalObjectSystem = null;

		objectToGrabCreated = false;

		objectToGrabListAssigned = false;

		newObjectMeshScale = 1;

		configureObjectColliderAutomatically = true;

		currentWeaponName = "New Weapon";

		Debug.Log ("Object To Grab window closed");
	}

	void OnGUI ()
	{
		if (!guiSkin) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}
		GUI.skin = guiSkin;

		this.minSize = rectSize;

		this.titleContent = new GUIContent (temporalEditorTitle, null, temporalEditorDescription);

		GUILayout.BeginVertical (temporalEditorSecondaryTitle, "window");

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

		EditorGUILayout.LabelField (temporalEditorInstructions, style);
		GUILayout.EndHorizontal ();

		if (objectToGrabListAssigned) {
			if (objectToGrabList.Length > 0) {

				GUILayout.FlexibleSpace ();

				EditorGUILayout.Space ();

				if (objectToGrabIndex < objectToGrabList.Length) {
					objectToGrabIndex = EditorGUILayout.Popup ("Object Type", objectToGrabIndex, objectToGrabList);

					newObjectToGrabName = objectToGrabList [objectToGrabIndex];  
				}

				newObjectToGrabMesh = EditorGUILayout.ObjectField ("New Object Mesh", newObjectToGrabMesh, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;

				EditorGUILayout.Space ();

				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Configure Collider Automatically", EditorStyles.boldLabel);
				configureObjectColliderAutomatically = (bool)EditorGUILayout.Toggle ("", configureObjectColliderAutomatically);
				GUILayout.EndHorizontal ();

				EditorGUILayout.Space ();

				if (!configureObjectColliderAutomatically) {
					useBoxCollider = (bool)EditorGUILayout.Toggle ("Use Box Collider", useBoxCollider);
					useMeshCollider = (bool)EditorGUILayout.Toggle ("Use Mesh Collider", useMeshCollider);
				}

				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Object Mesh Scale", EditorStyles.boldLabel);
				newObjectMeshScale = EditorGUILayout.Slider (newObjectMeshScale, 0.01f, 5);
				GUILayout.EndHorizontal ();

				EditorGUILayout.Space ();

				if (creatingMeleeWeapon) {

					currentWeaponName = (string)EditorGUILayout.TextField ("Melee Weapon Name", currentWeaponName); 

					EditorGUILayout.Space ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Add Melee Weapon To Inventory", EditorStyles.boldLabel);
					addMeleeWeaponToInventory = (bool)EditorGUILayout.Toggle ("", addMeleeWeaponToInventory);
					GUILayout.EndHorizontal ();

					if (addMeleeWeaponToInventory) {
						EditorGUILayout.Space ();

						EditorGUILayout.Space ();

						GUILayout.BeginHorizontal ();
						GUILayout.Label ("Weapon Icon", EditorStyles.boldLabel);

						weaponIconTexture = EditorGUILayout.ObjectField (weaponIconTexture, typeof(Texture), true, GUILayout.ExpandWidth (true)) as Texture;
						GUILayout.EndHorizontal ();
					}

					EditorGUILayout.Space ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Use New Path For Melee Weapon Prefab", EditorStyles.boldLabel);
					useNewPathForMeleeWeapon = (bool)EditorGUILayout.Toggle ("", useNewPathForMeleeWeapon);
					GUILayout.EndHorizontal ();

					if (useNewPathForMeleeWeapon) {
						newPathForMeleeWeapon = (string)EditorGUILayout.TextField ("New Path", newPathForMeleeWeapon); 
					}

					EditorGUILayout.Space ();

					GUILayout.Label ("Window Height", EditorStyles.boldLabel);

					if (previousRectSize != rectSize) {
						previousRectSize = rectSize;

						this.maxSize = rectSize;
					}

					rectSize.y = EditorGUILayout.Slider (rectSize.y, minHeight, screenResolution.y);				
				}
				EditorGUILayout.Space ();

				if (newObjectToGrabMesh != null) {
					if (GUILayout.Button ("Create Object")) {
						createObjectToGrab ();
					}
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
			EditorGUILayout.LabelField ("WARNING: No Object prefabs where found on the path " + temporalPrefabsPath, style);

			GUILayout.EndHorizontal ();
		}

		GUILayout.EndVertical ();
	}

	void createObjectToGrab ()
	{
		string pathForObject = temporalPrefabsPath + newObjectToGrabName + ".prefab";

		GameObject currentPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath (pathForObject, typeof(GameObject)) as GameObject;

		if (currentPrefab != null) {
			GameObject newObjectToGrabCreated = (GameObject)Instantiate (currentPrefab, Vector3.zero, Quaternion.identity);
			newObjectToGrabCreated.name = newObjectToGrabName;

			objectToGrabCreated = true;

			currentGrabPhysicalObjectSystem = newObjectToGrabCreated.GetComponent<grabPhysicalObjectSystem> ();

			if (currentGrabPhysicalObjectSystem != null) {
				if (!configureObjectColliderAutomatically) {
					currentGrabPhysicalObjectSystem.useMeshCollider = useMeshCollider;
					currentGrabPhysicalObjectSystem.useBoxCollider = useBoxCollider;
				}

				if (newObjectToGrabMesh != null) {
					if (currentGrabPhysicalObjectSystem.objectMeshInsideMainParent) {
						GameObject newObjectToGrabMeshCreated = (GameObject)Instantiate (newObjectToGrabMesh, Vector3.zero, Quaternion.identity);

						newObjectToGrabMeshCreated.transform.SetParent (currentGrabPhysicalObjectSystem.objectMeshMainParent);
						newObjectToGrabMeshCreated.transform.localPosition = Vector3.zero;
						newObjectToGrabMeshCreated.transform.localRotation = Quaternion.identity;

						newObjectToGrabMeshCreated.transform.localScale = Vector3.one * newObjectMeshScale;

						grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = newObjectToGrabCreated.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();
					
						if (currentGrabPhysicalObjectMeleeAttackSystem != null) {

							currentGrabPhysicalObjectMeleeAttackSystem.weaponName = currentWeaponName;

							if (currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh != null) {
								Transform weaponMeshChild = currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh.transform.GetChild (0);

								if (weaponMeshChild != null) {
									if (weaponMeshChild != newObjectToGrabMeshCreated.transform) {
										DestroyImmediate (weaponMeshChild.gameObject);
									}
								}
							}

							if (creatingMeleeWeapon && addMeleeWeaponToInventory) {
								newObjectToGrabCreated.name = currentWeaponName + " (to grab physically with Melee Attacks)";

								GameObject weaponMesh = newObjectToGrabMeshCreated;

								GKC_Utils.createInventoryWeapon (currentWeaponName, "Melee Weapons", weaponMesh, weaponIconTexture, relativePathWeaponsMesh, true);

								string currentPrefabPath = relativePathMeleeWeapons;

								if (useNewPathForMeleeWeapon) {
									currentPrefabPath = newPathForMeleeWeapon;
								}
									
								GameObject newMeleeWeaponPrefab = GKC_Utils.createPrefab (currentPrefabPath, currentWeaponName, newObjectToGrabCreated);

								if (newMeleeWeaponPrefab != null) {
									meleeWeaponsGrabbedManager[] meleeWeaponsGrabbedManagerList = FindObjectsOfType<meleeWeaponsGrabbedManager> ();

									foreach (meleeWeaponsGrabbedManager currentMeleeWeaponsGrabbedManager in meleeWeaponsGrabbedManagerList) {
										currentMeleeWeaponsGrabbedManager.addNewMeleeWeaponPrefab (newMeleeWeaponPrefab, currentWeaponName);
									}

									GKC_Utils.updateDirtyScene ("Create Object To Grab", newObjectToGrabCreated);

									DestroyImmediate (newObjectToGrabCreated);
								}
							}
						}
					} else {
						MeshRenderer newObjectToGrabMeshRenderer = newObjectToGrabMesh.GetComponent<MeshRenderer> ();
						MeshRenderer newObjectToGrabCreatedMeshRenderer = newObjectToGrabCreated.GetComponent<MeshRenderer> ();

						if (newObjectToGrabMeshRenderer && newObjectToGrabCreatedMeshRenderer) {
							newObjectToGrabCreatedMeshRenderer.sharedMaterials = newObjectToGrabMeshRenderer.sharedMaterials;
						}

						MeshFilter newObjectToGrabMeshFilter = newObjectToGrabMesh.GetComponent<MeshFilter> ();
						MeshFilter newObjectToGrabCreatedMeshFilter = newObjectToGrabCreated.GetComponent<MeshFilter> ();

						if (newObjectToGrabMeshFilter != null && newObjectToGrabCreatedMeshFilter != null) {
							newObjectToGrabCreatedMeshFilter.sharedMesh = newObjectToGrabMeshFilter.sharedMesh;
						}

						if (currentGrabPhysicalObjectSystem.useMeshCollider) {
							MeshCollider newObjectToGrabCreatedMeshCollider = newObjectToGrabCreated.GetComponent<MeshCollider> ();

							if (newObjectToGrabCreatedMeshCollider == null) {
								newObjectToGrabCreatedMeshCollider = newObjectToGrabCreated.AddComponent<MeshCollider> ();
							}

							newObjectToGrabCreatedMeshCollider.convex = true;
						}

						if (currentGrabPhysicalObjectSystem.useBoxCollider) {
							BoxCollider newObjectToGrabCreatedBoxCollider = newObjectToGrabCreated.GetComponent<BoxCollider> ();

							if (newObjectToGrabCreatedBoxCollider == null) {
								newObjectToGrabCreatedBoxCollider = newObjectToGrabCreated.AddComponent<BoxCollider> ();
							}
						}
					}
				}
			}
				
			if (newObjectToGrabCreated != null) {
				GKC_Utils.setActiveGameObjectInEditor (newObjectToGrabCreated);

				Camera currentCameraEditor = GKC_Utils.getCameraEditor ();

				if (currentCameraEditor != null) {
					Vector3 editorCameraPosition = currentCameraEditor.transform.position;
					Vector3 editorCameraForward = currentCameraEditor.transform.forward;

					RaycastHit hit;

					if (Physics.Raycast (editorCameraPosition, editorCameraForward, out hit, Mathf.Infinity)) {
						newObjectToGrabCreated.transform.position = hit.point + Vector3.up * 0.2f;
					}
				}

				GKC_Utils.updateDirtyScene ("Create Object To Grab", newObjectToGrabCreated);
			}
		} else {
			Debug.Log ("WARNING: no prefab found on path " + temporalPrefabsPath + newObjectToGrabName);
		}
	}

	void Update ()
	{
		if (objectToGrabCreated) {
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