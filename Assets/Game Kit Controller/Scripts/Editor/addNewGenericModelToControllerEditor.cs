using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class addNewGenericModelToControllerEditor : EditorWindow
{
	GUISkin guiSkin;
	Rect windowRect = new Rect ();
	Event currentEvent;

	Vector2 rectSize = new Vector2 (550, 420);

	float timeToBuild = 0.2f;
	float timer;

	GUIStyle style = new GUIStyle ();

	float windowHeightPercentage = 0.38f;

	Vector2 screenResolution;

	playerComponentsManager mainPlayerComponentsManager;
	customCharacterControllerManager mainCustomCharacterControllerManager;

	public GameObject newGenericModel;
	public string newGenericName;
	public Animator genericModelAnimator;
	public Avatar genericModelAvatar;
	public RuntimeAnimatorController originalAnimatorController;

	public bool characterIsAI;

	bool componentsLocated;

	bool characterAdded;

	[MenuItem ("Game Kit Controller/Generic Models/Add New Generic Model To Controller", false, 2)]
	public static void addNewGenericModelToController ()
	{
		GetWindow<addNewGenericModelToControllerEditor> ();
	}

	void OnEnable ()
	{
		screenResolution = new Vector2 (Screen.currentResolution.width, Screen.currentResolution.height);

		//		Debug.Log (screenResolution + " " + partsHeight + " " + settingsHeight + " " + previewHeight);

		float totalHeight = screenResolution.y * windowHeightPercentage;

		if (totalHeight < 420) {
			totalHeight = 420;
		}

		rectSize = new Vector2 (550, totalHeight);

		resetCreatorValues ();

		checkCurrentCharacterSelected (Selection.activeGameObject);
	}

	void checkCurrentCharacterSelected (GameObject currentCharacterSelected)
	{
		if (currentCharacterSelected) {
			mainPlayerComponentsManager = currentCharacterSelected.GetComponentInChildren<playerComponentsManager> ();

			if (mainPlayerComponentsManager != null) {
				mainCustomCharacterControllerManager = mainPlayerComponentsManager.getCustomCharacterControllerManager ();

				if (mainCustomCharacterControllerManager != null) {
					componentsLocated = true;
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
		newGenericModel = null;
		newGenericName = "";
		genericModelAnimator = null;
		genericModelAvatar = null;
		originalAnimatorController = null;

		mainCustomCharacterControllerManager = null;

		mainPlayerComponentsManager = null;

		componentsLocated = false;

		characterAdded = false;

		characterIsAI = false;

		Debug.Log ("Generic Model window closed");
	}

	void OnGUI ()
	{
		if (!guiSkin) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}
		GUI.skin = guiSkin;

		this.minSize = rectSize;

		this.titleContent = new GUIContent ("Generic", null, "Add Generic Model To Controller");

		GUILayout.BeginVertical ("Add Generic Model To Controller", "window");

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

		EditorGUILayout.LabelField ("Configure the generic model elements and press the button 'Create Character'. \n\n" +
		"If not character is selected in the hierarchy, select one and press the button 'Check Current Object Selected'.\n\n", style);
		GUILayout.EndHorizontal ();

		if (mainCustomCharacterControllerManager == null) {

			GUILayout.FlexibleSpace ();

			GUILayout.BeginHorizontal ();
			EditorGUILayout.HelpBox ("", MessageType.Warning);

			style = new GUIStyle (EditorStyles.helpBox);
			style.richText = true;

			style.fontStyle = FontStyle.Bold;
			style.fontSize = 17;
			EditorGUILayout.LabelField ("WARNING: No Character was found, make sure to select the player or an " +
			"AI to add new generic model to it.", style);

			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Check Current Object Selected")) {
				checkCurrentCharacterSelected (Selection.activeGameObject);
			}
		} else {
			if (componentsLocated) {

				GUILayout.FlexibleSpace ();

				EditorGUILayout.Space ();

				GUILayout.BeginVertical ("box");

				newGenericName = (string)EditorGUILayout.TextField ("Character Name", newGenericName); 

				windowRect = GUILayoutUtility.GetLastRect ();
				windowRect.position = new Vector2 (0, windowRect.position.y);
				windowRect.width = this.maxSize.x;

				newGenericModel = EditorGUILayout.ObjectField ("Generic Model", newGenericModel, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;

//				genericModelAnimator = EditorGUILayout.ObjectField ("Generic Animator", genericModelAnimator, typeof(Animator), true, GUILayout.ExpandWidth (true)) as Animator;
				genericModelAvatar = EditorGUILayout.ObjectField ("Generic Avatar", genericModelAvatar, typeof(Avatar), true, GUILayout.ExpandWidth (true)) as Avatar;

				originalAnimatorController = EditorGUILayout.ObjectField ("Animator Controller", originalAnimatorController, typeof(RuntimeAnimatorController), true, GUILayout.ExpandWidth (true)) as RuntimeAnimatorController;

				characterIsAI = (bool)EditorGUILayout.Toggle ("Character is AI", characterIsAI);

				GUILayout.EndVertical ();

				EditorGUILayout.Space ();

				EditorGUILayout.Space ();

				GUILayout.BeginHorizontal ();

				if (GUILayout.Button ("Cancel")) {
					this.Close ();
				}

				if (GUILayout.Button ("Create Character")) {
					if (newGenericModel != null//&& genericModelAnimator != null
					    && originalAnimatorController != null
					    && genericModelAvatar != null
					    && newGenericName != "") {

						addCharacter ();

						return;
					} else {
						Debug.Log ("WARNING: Not all elements for the new character have been assigned, make sure to add the proper fileds to it");
					}
				}

				GUILayout.EndHorizontal ();
			}
		}

		GUILayout.EndVertical ();
	}

	void addCharacter ()
	{
		GameObject newCustomCharacterObject = (GameObject)Instantiate (mainCustomCharacterControllerManager.customCharacterPrefab, Vector3.zero, Quaternion.identity);

		newCustomCharacterObject.name = "Custom Character Controller " + newGenericName;
			
		customCharacterControllerBase currentCustomCharacterControllerBase = newCustomCharacterObject.GetComponent<customCharacterControllerBase> ();

		GameObject genericModelCreated = (GameObject)Instantiate (newGenericModel, Vector3.zero, Quaternion.identity, currentCustomCharacterControllerBase.gameObject.transform);


		Animator genericModelAnimator = genericModelCreated.GetComponent<Animator> ();

		genericModelAnimator.enabled = false;


		currentCustomCharacterControllerBase.originalAnimatorController = originalAnimatorController;

		currentCustomCharacterControllerBase.originalAvatar = genericModelAvatar;


		currentCustomCharacterControllerBase.characterGameObject = genericModelCreated;

		currentCustomCharacterControllerBase.characterMeshesList.Add (genericModelCreated);

		genericModelCreated.SetActive (false);

		currentCustomCharacterControllerBase.setNewCameraStates = true;

		currentCustomCharacterControllerBase.newCameraStateThirdPerson = newGenericName + " View Third Person";

		currentCustomCharacterControllerBase.newCameraStateFirstPerson = newGenericName + " View First Person";

		currentCustomCharacterControllerBase.customRagdollInfoName = newGenericName + " Ragdoll";


		playerController mainPlayerController = mainPlayerComponentsManager.getPlayerController ();

		GameObject playerControllerParentGameObject = mainPlayerController.transform.parent.gameObject;

		currentCustomCharacterControllerBase.mainAnimator = mainPlayerController.getCharacterAnimator ();

		currentCustomCharacterControllerBase.setAIValues = characterIsAI;

		newCustomCharacterObject.transform.SetParent (mainCustomCharacterControllerManager.transform);

		mainCustomCharacterControllerManager.addNewCustomCharacterController (currentCustomCharacterControllerBase, newGenericName);


		genericRagdollBuilder currentGenericRagdollBuilder = newCustomCharacterObject.GetComponent<genericRagdollBuilder> ();

		if (currentGenericRagdollBuilder != null) {
			currentGenericRagdollBuilder.ragdollName = newGenericName + " Ragdoll";

			currentGenericRagdollBuilder.characterBody = currentCustomCharacterControllerBase.characterGameObject;

			currentGenericRagdollBuilder.mainRagdollActivator = mainPlayerComponentsManager.getRagdollActivator ();

			GKC_Utils.updateComponent (currentGenericRagdollBuilder);

			GKC_Utils.updateDirtyScene ("Update Generic Ragdoll elements", currentGenericRagdollBuilder.gameObject);
		}


		followObjectPositionSystem currentFollowObjectPositionSystem = newCustomCharacterObject.GetComponentInChildren<followObjectPositionSystem> ();

		if (currentFollowObjectPositionSystem != null) {
			currentFollowObjectPositionSystem.objectToFollow = mainPlayerController.transform;

			GKC_Utils.updateComponent (currentFollowObjectPositionSystem);

			GKC_Utils.updateDirtyScene ("Update Follow Object Position elements", currentFollowObjectPositionSystem.gameObject);
		}

		characterAdded = true;
	}

	void Update ()
	{
		if (characterAdded) {
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