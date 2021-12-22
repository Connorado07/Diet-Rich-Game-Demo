using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class addSliceSystemToCharacterCreatorEditor : EditorWindow
{
	GUISkin guiSkin;
	Rect windowRect = new Rect ();

	Vector2 rectSize = new Vector2 (550, 600);

	bool sliceSystemAdded;

	float timeToBuild = 0.2f;
	float timer;

	GUIStyle style = new GUIStyle ();

	float windowHeightPercentage = 0.3f;

	Vector2 screenResolution;

	playerController currentPlayerController;

	bool characterSelected;

	string prefabsPath = "Assets/Game Kit Controller/Prefabs/Others/Slice System";

	string sliceRagdollName = "Ragdoll (With Slice System)";

	public GameObject currentRagdollPrefab;

	public Material sliceMaterial;

	public GameObject characterMeshForRagdollPrefab;

	public bool setTagOnSkeletonRigidbodies = true;

	public string tagOnSkeletonRigidbodies = "box";

	bool ragdollPrefabCreated;

	[MenuItem ("Game Kit Controller/Others/Add Slice System To Character", false, 10)]
	public static void addSliceSystemToCharacter ()
	{
		GetWindow<addSliceSystemToCharacterCreatorEditor> ();
	}

	void OnEnable ()
	{
		screenResolution = new Vector2 (Screen.currentResolution.width, Screen.currentResolution.height);

		//		Debug.Log (screenResolution + " " + partsHeight + " " + settingsHeight + " " + previewHeight);

		float totalHeight = screenResolution.y * windowHeightPercentage;

		if (totalHeight < 400) {
			totalHeight = 400;
		}

		rectSize = new Vector2 (550, totalHeight);

		resetCreatorValues ();

		checkCurrentCharacterSelected (Selection.activeGameObject);
	}

	void checkCurrentCharacterSelected (GameObject currentCharacterSelected)
	{
		if (currentCharacterSelected) {
			if (!Directory.Exists (prefabsPath)) {
				Debug.Log ("WARNING: " + prefabsPath + " path doesn't exist, make sure the path is from an existing folder in the project");

				return;
			}

			string pathForRagdoll = prefabsPath + "/" + sliceRagdollName + ".prefab";

			currentRagdollPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath (pathForRagdoll, typeof(GameObject)) as GameObject;
		
			currentPlayerController = currentCharacterSelected.GetComponentInChildren<playerController> ();

			if (currentPlayerController != null) {
				Debug.Log ("Character Selected on creator opened");
				characterSelected = true;
			} else {
				characterSelected = false;
				Debug.Log ("No Character Selected on creator opened");
			}
		}
	}

	void OnDisable ()
	{
		resetCreatorValues ();
	}

	void resetCreatorValues ()
	{
		currentPlayerController = null;

		characterSelected = false;

		sliceSystemAdded = false;

		ragdollPrefabCreated = false;

		Debug.Log ("Slice System window closed");
	}

	void OnGUI ()
	{
		if (!guiSkin) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}
		GUI.skin = guiSkin;

		this.minSize = rectSize;

		this.titleContent = new GUIContent ("Slice System", null, "Add Slice System To Character");

		GUILayout.BeginVertical ("Add Slice System Window", "window");

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		windowRect = GUILayoutUtility.GetLastRect ();
		windowRect.position = new Vector2 (0, windowRect.position.y);
		windowRect.width = this.maxSize.x;

		if (!characterSelected) {
			GUILayout.BeginHorizontal ();

			EditorGUILayout.HelpBox ("", MessageType.Info);

			style = new GUIStyle (EditorStyles.helpBox);
			style.richText = true;

			style.fontStyle = FontStyle.Bold;
			style.fontSize = 15;

			EditorGUILayout.LabelField ("If not character is selected in the hierarchy, select one and press the button 'Check Current Object Selected'.\n\n", style);
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			EditorGUILayout.HelpBox ("", MessageType.Warning);

			style = new GUIStyle (EditorStyles.helpBox);
			style.richText = true;
			style.fontStyle = FontStyle.Bold;
			style.fontSize = 15;

			EditorGUILayout.LabelField ("WARNING: No Character was found, make sure to select the player or an " +
			"humanoid AI to add the slice system to it.", style);

			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button ("Check Current Character Selected")) {
				checkCurrentCharacterSelected (Selection.activeGameObject);
			}
		} else {
			GUILayout.BeginHorizontal ();

			EditorGUILayout.HelpBox ("", MessageType.Info);

			style = new GUIStyle (EditorStyles.helpBox);
			style.richText = true;

			style.fontStyle = FontStyle.Bold;
			style.fontSize = 15;

			EditorGUILayout.LabelField ("You can create the ragdoll prefab for this character, which will be used" +
			" when each slice is applied, to instantiate a new ragdoll with those settings and mesh.\n\n" +
			"Just select a prefab character mesh on the field 'Character Mesh For Ragdoll' and press the button 'Create Ragdoll Prefab'.\n\n" +
			"By default, there is already one selected, but you may want to create a new ragdoll prefab if you are using a new character mesh.", style);
			GUILayout.EndHorizontal ();

			GUILayout.FlexibleSpace ();

			currentRagdollPrefab = EditorGUILayout.ObjectField ("Ragdoll Prefab", currentRagdollPrefab, 
				typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;

			sliceMaterial = EditorGUILayout.ObjectField ("Slice Material", sliceMaterial, 
				typeof(Material), true, GUILayout.ExpandWidth (true)) as Material;

			characterMeshForRagdollPrefab = EditorGUILayout.ObjectField ("Character Mesh For Ragdoll Prefab", characterMeshForRagdollPrefab, 
				typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;

			EditorGUILayout.Space ();

			setTagOnSkeletonRigidbodies = EditorGUILayout.Toggle ("Set Tag On Skeleton", setTagOnSkeletonRigidbodies);

			if (setTagOnSkeletonRigidbodies) {
				tagOnSkeletonRigidbodies = EditorGUILayout.TextField ("Tag On Skeleton", tagOnSkeletonRigidbodies);
			}

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			if (characterMeshForRagdollPrefab != null && !ragdollPrefabCreated) {
				if (GUILayout.Button ("Create Ragdoll Prefab")) {
					createRagdollPrefab ();
				}
			}

			if (GUILayout.Button ("Add Slice System To Character")) {
				addSliceSystem ();
			}

			if (GUILayout.Button ("Cancel")) {
				this.Close ();
			}
		}

		GUILayout.EndVertical ();
	}

	void addSliceSystem ()
	{
		if (currentRagdollPrefab) {
			GameObject playerControllerGameObject = currentPlayerController.gameObject;

			surfaceToSlice currentSurfaceToSlice = playerControllerGameObject.GetComponent<surfaceToSlice> ();

			if (currentSurfaceToSlice == null || currentSurfaceToSlice.getMainSimpleSliceSystem () == null) {

				if (currentSurfaceToSlice == null) {
					currentSurfaceToSlice = playerControllerGameObject.AddComponent<surfaceToSlice> ();
				}

				GameObject characterMesh = currentPlayerController.getCharacterMeshGameObject ().transform.parent.gameObject;

				simpleSliceSystem currentSimpleSliceSystem = characterMesh.AddComponent<simpleSliceSystem> ();

				currentSimpleSliceSystem.searchBodyParts ();

				currentSimpleSliceSystem.mainSurfaceToSlice = currentSurfaceToSlice;
				currentSimpleSliceSystem.objectToSlice = characterMesh;

				currentSurfaceToSlice.setMainSimpleSliceSystem (currentSimpleSliceSystem.gameObject);
				currentSurfaceToSlice.objectIsCharacter = true;

				currentSimpleSliceSystem.objectToSlice = characterMesh;

				currentSimpleSliceSystem.alternatePrefab = currentRagdollPrefab;

				currentSimpleSliceSystem.infillMaterial = sliceMaterial;

				GKC_Utils.updateComponent (currentSimpleSliceSystem);

				GKC_Utils.updateDirtyScene ("Set slice system info", currentSimpleSliceSystem.gameObject);

				Debug.Log ("Slice System added to character");

				GKC_Utils.updateDirtyScene ("Add Slice To Character", characterMesh);
			} else {
				Debug.Log ("Slice System was already configured in this character");
			}

			sliceSystemAdded = true;
		} else {
			Debug.Log ("WARNING: no prefab for ragdoll found on path " + prefabsPath + "/" + sliceRagdollName);
		}
	}

	void createRagdollPrefab ()
	{
//		GameObject newCharacterMeshForRagdollPrefab = (GameObject)Instantiate (characterMeshForRagdollPrefab, Vector3.zero, Quaternion.identity);
//
//		string prefabName = characterMeshForRagdollPrefab.name + " Dummy Ragdoll (With Slice System)";
//		newCharacterMeshForRagdollPrefab.name = prefabName;
//
//		surfaceToSlice currentSurfaceToSlice = newCharacterMeshForRagdollPrefab.GetComponent<surfaceToSlice> ();
//
//		if (currentSurfaceToSlice == null) {
//			ragdollBuilder currentRagdollBuilder = newCharacterMeshForRagdollPrefab.AddComponent<ragdollBuilder> ();
//
//			Animator mainAnimator = newCharacterMeshForRagdollPrefab.GetComponent<Animator> ();
//			currentRagdollBuilder.getAnimator (mainAnimator);
//			currentRagdollBuilder.createRagdoll ();
//
//			DestroyImmediate (currentRagdollBuilder);
//
//			currentSurfaceToSlice = newCharacterMeshForRagdollPrefab.AddComponent<surfaceToSlice> ();
//
//			GameObject characterMesh = newCharacterMeshForRagdollPrefab;
//
//			simpleSliceSystem currentSimpleSliceSystem = characterMesh.AddComponent<simpleSliceSystem> ();
//
//			currentSimpleSliceSystem.searchBodyParts ();
//
//			currentSimpleSliceSystem.mainSurfaceToSlice = currentSurfaceToSlice;
//			currentSimpleSliceSystem.objectToSlice = characterMesh;
//			currentSimpleSliceSystem.alternatePrefab = newCharacterMeshForRagdollPrefab;
//
//			currentSurfaceToSlice.setMainSimpleSliceSystem (currentSimpleSliceSystem.gameObject);
//			currentSurfaceToSlice.objectIsCharacter = true;
//
//			currentSimpleSliceSystem.objectToSlice = characterMesh;
//
//			currentSimpleSliceSystem.infillMaterial = sliceMaterial;
//
//			if (setTagOnSkeletonRigidbodies) {
//				currentSimpleSliceSystem.setTagOnBodyParts (tagOnSkeletonRigidbodies);
//			}
//
//			GKC_Utils.updateComponent (currentSimpleSliceSystem);
//
//			GKC_Utils.updateDirtyScene ("Set slice system info", currentSimpleSliceSystem.gameObject);
//
//			ragdollPrefabCreated = true;
//
//			Debug.Log ("Ragdoll prefab created ");
//		} else {
//			Debug.Log ("Ragdoll was already configured for this prefab");
//		}
//
//		currentRagdollPrefab = GKC_Utils.createPrefab (prefabsPath, prefabName, newCharacterMeshForRagdollPrefab);
//
//		GKC_Utils.updateDirtyScene ("Create Slice Ragdoll", newCharacterMeshForRagdollPrefab);
//
//		DestroyImmediate (newCharacterMeshForRagdollPrefab);

		currentRagdollPrefab = GKC_Utils.createSliceRagdollPrefab (characterMeshForRagdollPrefab, prefabsPath, sliceMaterial, setTagOnSkeletonRigidbodies, tagOnSkeletonRigidbodies);
	
		ragdollPrefabCreated = currentRagdollPrefab != null;
	}


	void Update ()
	{
		if (sliceSystemAdded) {
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
