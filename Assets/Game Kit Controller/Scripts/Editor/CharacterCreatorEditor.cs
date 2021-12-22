using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class CharacterCreatorEditor : EditorWindow
{
	public characterType charType = characterType.Player;

	public AICharacterType AIType = AICharacterType.Armed;

	public bool deletePreviousPlayer;

	public bool usePreviousPlayerPrefab;

	public GameObject previousPlayerPrefab;

	public enum characterType
	{
		Player,
		Enemy,
		Friend,
		Neutral
	}

	public enum AICharacterType
	{
		Armed,
		Combat,
		Powers,
		Melee,
		Unarmed
	}

	GUISkin guiSkin;
	Rect windowRect = new Rect ();

	GameObject currentCharacterGameObject;
	Animator characterAnimator;

	Editor characterPreview;
	bool modelIsHumanoid;
	bool correctAnimatorAvatar;
	bool characterSelected;
	public GameObject character;

	bool checkToCreateCharacterActive;

	bool characterCreated;

	GameObject newCharacterModel;

	float timeToBuild = 0.2f;
	float timer;

	string buttonText;

	string prefabsPath = "Assets/Game Kit Controller/Prefabs/";

	string playerPrefabPath = "Player Controller/";
	string playerPrefabName = "Player And Game Management";

	bool useAITemplateByDefault = true;

	string AITemplatePath = "AI/";
	string AITemplatePrefabName = "AI Main Template Prefab";

	string friendPrefabPath = "AI/Friend/";
	string friendArmedPrefabName = "AI Friend Armed";
	string friendCombatPrefabName = "AI Friend Combat";
	string friendPowersPrefabName = "AI Friend Powers";
	string friendMeleePrefabName = "AI Friend Melee";
	string friendUnarmedPrefabName = "AI Friend Unarmed";

	string enemyPrefabPath = "AI/Enemy/";
	string enemyArmedPrefabName = "AI Enemy Armed";
	string enemyCombatPrefabName = "AI Enemy Combat";
	string enemyPowersPrefabName = "AI Enemy Powers";
	string enemyMeleePrefabName = "AI Enemy Melee";
	string enemyUnarmedPrefabName = "AI Enemy Unarmed";

	bool useCustomPrefabName;
	string currentPrefabName;

	bool useCustomPrefab;
	GameObject newCustomPrefab;

	bool characterIsPlayerType;
	bool hasWeaponsEnabled;
	bool isInstantiated = true;

	bool characterCheckResult;

	bool buttonPressed;

	Transform head;
	Transform neck;
	Transform chest;
	Transform spine;

	Transform hips;

	Transform rightLowerArm;
	Transform leftLowerArm;
	Transform rightHand;
	Transform leftHand;

	Transform rightLowerLeg;
	Transform leftLowerLeg;
	Transform rightFoot;
	Transform leftFoot;
	Transform rightToes;
	Transform leftToes;

	Vector2 screenResolution;

	float windowHeightPercentage = 0.7f;

	public Vector2 rectSize = new Vector2 (600, 600);
	Vector2 previewSize = new Vector2 (400, 400);

	float minHeight = 600f;

	Vector2 scrollPos1;
	Vector2 scrollPos2;

	buildPlayer.settingsInfo currentSettingsInfo;
	buildPlayer.settingsInfoCategory currentSettingsInfoCategory;

	public buildPlayer currentBuildPlayer;

	public GameObject currentCharacterInstantiated;

	bool showSettingsList;

	Vector2 previousRectSize;
	Vector2 previousPreviewSize;

	GUIStyle style = new GUIStyle ();

	GameObject previousPlayerPrefabOnSceneFoundOnStart;

	bool newCharacterIsAI;
	bool newCharacterIsEnemy;
	bool newCharacterIsNeutral;

	bool characterHasBeenPreInstantiatedOnEditor;

	GameObject temporalCharacterObject;

	public bool useTemplateList;
	string templateName;
	int templateIndex;
	public string[] templateList;

	void OnEnable ()
	{
		screenResolution = new Vector2 (Screen.currentResolution.width, Screen.currentResolution.height);

		float windowHeight = screenResolution.y * windowHeightPercentage;

		windowHeight = Mathf.Clamp (windowHeight, minHeight, screenResolution.y);

		rectSize = new Vector2 (540, windowHeight);

		previousPlayerPrefabOnSceneFoundOnStart = GameObject.Find (playerPrefabName);

		if (previousPlayerPrefabOnSceneFoundOnStart == null) {
			inputManager currentInputManager = FindObjectOfType<inputManager> ();

			if (currentInputManager) {
				previousPlayerPrefabOnSceneFoundOnStart = currentInputManager.gameObject;
			}
		}
	}

	void OnDisable ()
	{
		showSettingsList = false;

		if (!characterCreated) {
			if (currentCharacterInstantiated != null) {
				DestroyImmediate (currentCharacterInstantiated);

				Debug.Log ("Character instantiated but not built, removing from scene");
			}
		}

		currentBuildPlayer = null;

		characterCreated = false;

		checkToCreateCharacterActive = false;

		previousPlayerPrefabOnSceneFoundOnStart = null;

		newCharacterIsAI = false;
		newCharacterIsEnemy = false;
		newCharacterIsNeutral = false;

		characterHasBeenPreInstantiatedOnEditor = false;

		useTemplateList = false;

		templateIndex = 0;
	}

	void OnGUI ()
	{
		if (!guiSkin) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}
		GUI.skin = guiSkin;

		this.minSize = rectSize;

		this.titleContent = new GUIContent ("Character", null, "Game Kit Controller Character Creator");

		scrollPos2 = EditorGUILayout.BeginScrollView (scrollPos2, false, false);

		GUILayout.BeginVertical (GUILayout.Width (540));

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Character Creator Window", "window");
		//, GUILayout.Width (540));

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("box");

		charType = (characterType)EditorGUILayout.EnumPopup ("Character Type", charType);        

		useCustomPrefab = (bool)EditorGUILayout.Toggle ("Use Custom Prefab", useCustomPrefab);

		if (useCustomPrefab) {
			newCustomPrefab = EditorGUILayout.ObjectField ("New Character Prefab", newCustomPrefab, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;
		} else {
			useCustomPrefabName = (bool)EditorGUILayout.Toggle ("Use Custom Prefab Name", useCustomPrefabName);

			if (useCustomPrefabName) {
				currentPrefabName = (string)EditorGUILayout.TextField ("Prefab Name", currentPrefabName);        
			}
		}
			
		windowRect = GUILayoutUtility.GetLastRect ();
		windowRect.position = new Vector2 (0, windowRect.position.y);
		windowRect.width = this.maxSize.x;

			
		if (charType != characterType.Player) {
			AIType = (AICharacterType)EditorGUILayout.EnumPopup ("AI Type", AIType);       
		}

		if (buttonPressed) {
			if (!characterCheckResult) {
				EditorGUILayout.HelpBox ("The character hasn't a standard skeleton, check the console.\n" +
				"Maybe the system is not able to detect all the bones properly\n." +
				"IMPORTANT: Check the doc to use the manual character creator tool instead, to set those not found bones properly.", MessageType.Error);
			}

			if (currentCharacterGameObject == null) {
				buttonPressed = false;
			}
		} else {
			if (!currentCharacterGameObject) {
				EditorGUILayout.HelpBox ("The FBX model needs to be humanoid", MessageType.Info);
			} else if (!characterSelected) {
				EditorGUILayout.HelpBox ("The object needs an animator component", MessageType.Error);
			} else if (!modelIsHumanoid) {
				EditorGUILayout.HelpBox ("The model is not humanoid", MessageType.Error);
			} else if (!correctAnimatorAvatar) {
				EditorGUILayout.HelpBox (currentCharacterGameObject.name + " is not a valid humanoid", MessageType.Info);
			}
		}

		currentCharacterGameObject = EditorGUILayout.ObjectField ("FBX Model", currentCharacterGameObject, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;
		if (charType == characterType.Player) {
			deletePreviousPlayer = (bool)EditorGUILayout.Toggle ("Remove Previous Player", deletePreviousPlayer);

			if (!deletePreviousPlayer) {
				if (!useCustomPrefabName && !useCustomPrefab) {
					usePreviousPlayerPrefab = (bool)EditorGUILayout.Toggle ("Use Current Settings", usePreviousPlayerPrefab);

					if (usePreviousPlayerPrefab) {
						previousPlayerPrefab = EditorGUILayout.ObjectField ("Previous Prefab", previousPlayerPrefab, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;
					}
				}
			}
		} else {
			if (!useCustomPrefabName && !useCustomPrefab) {
				usePreviousPlayerPrefab = (bool)EditorGUILayout.Toggle ("Use Current Settings", usePreviousPlayerPrefab);

				if (usePreviousPlayerPrefab) {
					previousPlayerPrefab = EditorGUILayout.ObjectField ("Previous Prefab", previousPlayerPrefab, typeof(GameObject), true, GUILayout.ExpandWidth (true)) as GameObject;
				}
			}
		}

		if (GUI.changed && currentCharacterGameObject != null) {
			characterPreview = Editor.CreateEditor (currentCharacterGameObject);
		}

		EditorGUILayout.Space ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Window Height", EditorStyles.boldLabel);

		if (previousRectSize != rectSize) {
			previousRectSize = rectSize;

			this.maxSize = rectSize;
		}

		rectSize.y = EditorGUILayout.Slider (rectSize.y, minHeight, screenResolution.y);

		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Preview Height", EditorStyles.boldLabel);

		if (previousPreviewSize != previewSize) {
			previousPreviewSize = previewSize;
		}
		previewSize.y = EditorGUILayout.Slider (previewSize.y, 200, 375);

		GUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		GUILayout.EndVertical ();

		if (currentCharacterGameObject) {
			characterAnimator = currentCharacterGameObject.GetComponent<Animator> ();
		} else {
			characterAnimator = null;
		}

		if (characterAnimator != null) {
			characterSelected = true;
		} else {
			characterSelected = false;
		}

		if (characterSelected && characterAnimator.isHuman) {
			modelIsHumanoid = true;
		} else {
			modelIsHumanoid = false;
		}

		if (characterSelected && characterAnimator.avatar.isValid) {
			correctAnimatorAvatar = true;
		} else {
			correctAnimatorAvatar = false;
		}

		EditorGUILayout.Space ();

		GUILayout.BeginHorizontal ();

		if (currentBuildPlayer == null) {
			if (currentCharacterGameObject != null) {
				if (GUILayout.Button ("Instantiate Character Prefab (To Show Settings List)")) {
					characterHasBeenPreInstantiatedOnEditor = true;

					createCharacter (false);
				}
			}
		} else {
			if (GUILayout.Button ("Show/Hide Settings List")) {
				showSettingsList = !showSettingsList;
			}
		}

		GUILayout.EndHorizontal ();

		style.fontStyle = FontStyle.Bold;
		style.fontSize = 26;
		style.alignment = TextAnchor.MiddleCenter;

		if (currentBuildPlayer != null && showSettingsList) {
			if (currentBuildPlayer.settingsInfoCategoryList.Count > 0) {
				style.normal.textColor = Color.white;
				style.fontStyle = FontStyle.Bold;
				style.alignment = TextAnchor.MiddleCenter;
				style.fontSize = 18;

				GUILayout.Label ("Character Settings Info List", style);

				GUILayout.BeginVertical ("", "window");

				scrollPos1 = EditorGUILayout.BeginScrollView (scrollPos1, false, false);

				if (charType == characterType.Player) {
					useTemplateList = (bool)EditorGUILayout.Toggle ("Use Template List", useTemplateList);

					if (useTemplateList) {
						templateIndex = EditorGUILayout.Popup ("Character Template", templateIndex, templateList);

						templateName = templateList [templateIndex];  

						EditorGUILayout.Space ();

						if (GUILayout.Button ("Update Settings From Template")) {
							currentBuildPlayer.setTemplateFromListAsCurrentToApplyOnCharacterCreatorEditor (templateIndex);
						
							useTemplateList = false;
						}

						EditorGUILayout.Space ();
					}
				} else {
					useTemplateList = false;
				}

				for (int i = 0; i < currentBuildPlayer.settingsInfoCategoryList.Count; i++) { 

					EditorGUILayout.Space ();

					currentSettingsInfoCategory = currentBuildPlayer.settingsInfoCategoryList [i];

					GUILayout.BeginHorizontal (GUILayout.Width (450));
					GUILayout.Label (currentSettingsInfoCategory.Name.ToUpper (), style);
					GUILayout.EndHorizontal ();

					EditorGUILayout.Space ();

					for (int j = 0; j < currentSettingsInfoCategory.settingsInfoList.Count; j++) { 
						currentSettingsInfo = currentSettingsInfoCategory.settingsInfoList [j];

						GUILayout.BeginHorizontal (GUILayout.Width (450));
						GUILayout.Label (currentSettingsInfo.Name, EditorStyles.boldLabel);

						if (currentSettingsInfo.useBoolState) {
							currentSettingsInfo.boolState = (bool)EditorGUILayout.Toggle ("", currentSettingsInfo.boolState);
						}

						if (currentSettingsInfo.useFloatValue) {
							currentSettingsInfo.floatValue = (float)EditorGUILayout.FloatField ("", currentSettingsInfo.floatValue);
						}

						if (currentSettingsInfo.useStringValue) {
							currentSettingsInfo.stringValue = (string)EditorGUILayout.TextField ("", currentSettingsInfo.stringValue);
						}

						if (currentSettingsInfo.useRegularValue) {
							currentSettingsInfo.regularValue = (bool)EditorGUILayout.Toggle ("", currentSettingsInfo.regularValue);
						}

						GUILayout.EndHorizontal ();

						if (currentSettingsInfo.useFieldExplanation) {
							GUILayout.BeginHorizontal (GUILayout.Width (450));
							EditorGUILayout.HelpBox (currentSettingsInfo.fieldExplanation, MessageType.None);
							GUILayout.EndHorizontal ();
						}
					}

					EditorGUILayout.Space ();
					EditorGUILayout.Space ();
				}   

				EditorGUILayout.Space ();

				EditorGUILayout.EndScrollView ();

				GUILayout.EndVertical ();
			}
		} else {
			
			if (currentCharacterGameObject) {

				EditorGUILayout.Space ();

				GUILayout.FlexibleSpace ();
				if (characterPreview != null) {
					characterPreview.OnInteractivePreviewGUI (GUILayoutUtility.GetRect (previewSize.x, previewSize.y), "window");
				}
			}
		}

		string currentMessage = "If the character prefab is in the hierarchy and the option appears, make sure to press with the right mouse button over " +
		                        "the character prefab and press the option 'Unpack Prefab Completely'.";

		if (useCustomPrefab) {
			currentMessage += "\n\nThe custom prefab is to select a different prefab of the GKC prefab for the character, not a different FBX.";
		}

		showMessage (currentMessage, 10, MessageType.Info);

		EditorGUILayout.Space ();

		GUILayout.FlexibleSpace ();

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Cancel")) {
			this.Close ();
		}

		if (correctAnimatorAvatar && modelIsHumanoid) {
			if (charType == characterType.Player) {
				buttonText = "Create Player";
			} else if (charType == characterType.Enemy) {
				buttonText = "Create Enemy";
			} else if (charType == characterType.Friend) {
				buttonText = "Create Friend";
			} else if (charType == characterType.Neutral) {
				buttonText = "Create Neutral AI";
			}


			if (buttonPressed) {
				buttonText = "Reset Character Creator";
			}

			if (GUILayout.Button (buttonText)) {
				if (!buttonPressed) {
					characterCheckResult = checkCreateCharacter ();

					if (characterCheckResult) {
						createCharacter (true);
					}

					if (temporalCharacterObject != null) {
						DestroyImmediate (temporalCharacterObject);
					}

					if (!characterCheckResult) {
						buttonPressed = true;
					}
				} else {
					currentCharacterGameObject = null;
				}
			}
		}
			
		GUILayout.EndHorizontal ();

		GUILayout.EndVertical ();

		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		EditorGUILayout.EndScrollView ();
	}

	void createCharacter (bool creatingCharacter)
	{
		if (charType == characterType.Player) {
			string characterName = playerPrefabName;

			string prefabPath = prefabsPath + playerPrefabPath;

			if (useCustomPrefabName && currentPrefabName != "") {
				prefabPath += currentPrefabName;
				characterName = currentPrefabName;
			} else {
				prefabPath += characterName;
			}

			prefabPath += ".prefab";

			if (useCustomPrefab) {
				character = newCustomPrefab;
			} else {
				character = (GameObject)AssetDatabase.LoadAssetAtPath (prefabPath, typeof(GameObject));
			}

			bool instantiatePlayerGameObject = true;

			if (creatingCharacter && previousPlayerPrefabOnSceneFoundOnStart != null && !usePreviousPlayerPrefab) {
				if (deletePreviousPlayer) {
					DestroyImmediate (previousPlayerPrefabOnSceneFoundOnStart);
				}
			}

			if (!useCustomPrefabName && usePreviousPlayerPrefab) {
				character = previousPlayerPrefab;
				instantiatePlayerGameObject = false;
				isInstantiated = false;
			}

			if (character) {
				if (!creatingCharacter) {
					instantiatePlayerGameObject = true;
				}

				createCharacterGameObject (characterName, instantiatePlayerGameObject, creatingCharacter);

				characterIsPlayerType = true;
				hasWeaponsEnabled = true;

			} else {
				Debug.Log ("Player prefab not found in path " + prefabPath);
			}
		} else {
			newCharacterIsAI = true;

			newCharacterIsEnemy = (charType == characterType.Enemy);

			newCharacterIsNeutral = (charType == characterType.Neutral);

			string characterName = "";

			string prefabPath = prefabsPath + enemyPrefabPath;

			if (charType == characterType.Friend || charType == characterType.Neutral) {
				prefabPath = prefabsPath + friendPrefabPath;
			}
				
			if (AIType == AICharacterType.Armed) {
				hasWeaponsEnabled = true;
			}

			if (useCustomPrefabName && currentPrefabName != "") {
				prefabPath += currentPrefabName;
				characterName = currentPrefabName;
			} else {
				string newPrefabName = "";

				if (charType == characterType.Friend || charType == characterType.Neutral) {
					if (AIType == AICharacterType.Armed) {
						newPrefabName = friendArmedPrefabName;
					} else if (AIType == AICharacterType.Combat) {
						newPrefabName = friendCombatPrefabName;
					} else if (AIType == AICharacterType.Powers) {
						newPrefabName = friendPowersPrefabName;
					} else if (AIType == AICharacterType.Melee) {
						newPrefabName = friendMeleePrefabName;
					} else if (AIType == AICharacterType.Unarmed) {
						newPrefabName = friendUnarmedPrefabName;
					}
				} else {
					if (AIType == AICharacterType.Armed) {
						newPrefabName = enemyArmedPrefabName;
					} else if (AIType == AICharacterType.Combat) {
						newPrefabName = enemyCombatPrefabName;
					} else if (AIType == AICharacterType.Powers) {
						newPrefabName = enemyPowersPrefabName;
					} else if (AIType == AICharacterType.Melee) {
						newPrefabName = enemyMeleePrefabName;
					} else if (AIType == AICharacterType.Unarmed) {
						newPrefabName = enemyUnarmedPrefabName;
					}
				}

				prefabPath += newPrefabName;
				characterName = newPrefabName;

				if (useAITemplateByDefault) {
					prefabPath = prefabsPath + AITemplatePath + AITemplatePrefabName;
				}
			}

			prefabPath += ".prefab";

			if (useCustomPrefab) {
				character = newCustomPrefab;
			} else {
				character = (GameObject)AssetDatabase.LoadAssetAtPath (prefabPath, typeof(GameObject));
			}

			bool instantiatePlayerGameObject = true;
			if (!useCustomPrefabName && usePreviousPlayerPrefab) {
				character = previousPlayerPrefab;
				instantiatePlayerGameObject = false;
				isInstantiated = false;
			}

			if (character) {
				if (!creatingCharacter) {
					instantiatePlayerGameObject = true;
				}

				createCharacterGameObject (characterName, instantiatePlayerGameObject, creatingCharacter);
			} else {
				Debug.Log ("AI prefab not found in path " + prefabPath);
			}
		}
	}

	void Update ()
	{
		if (checkToCreateCharacterActive) {
			if (timer < timeToBuild) {
				timer += 0.01f;

				if (timer > timeToBuild) {
					string templateToUseName = AIType.ToString ();

					if (useTemplateList) {
						templateToUseName = templateName;
					}

					currentBuildPlayer.setCharacterSettingsStatus (characterHasBeenPreInstantiatedOnEditor, newCharacterIsAI, 
						newCharacterIsEnemy, newCharacterIsNeutral, templateToUseName, useTemplateList);

					currentBuildPlayer.setCharacterVariables (characterIsPlayerType, hasWeaponsEnabled, isInstantiated, newCharacterModel);

					currentBuildPlayer.buildCharacter ();

					checkToCreateCharacterActive = false;

					characterCreated = true;

					timer = 0;

					if (characterCheckResult) {
						Debug.Log ("Character has a standard skeleton, all bones were found properly");

						this.Close ();
					}
				}
			}
		}
	}

	public void createCharacterGameObject (string characterName, bool instantiateGameObject, bool creatingCharacter)
	{
		if (currentBuildPlayer == null) {
			if (instantiateGameObject) {
				if (usePreviousPlayerPrefab) {
					currentCharacterInstantiated = character;
				} else {
					currentCharacterInstantiated = (GameObject)Instantiate (character, Vector3.zero, Quaternion.identity);
				}
			}
		}

		if (creatingCharacter) {
			if (currentBuildPlayer == null && usePreviousPlayerPrefab) {
				currentCharacterInstantiated = character;
			}

			currentCharacterInstantiated.name = characterName;
			Transform playerCOM = currentCharacterInstantiated.GetComponentInChildren<IKSystem> ().IKBodyCOM;
			GameObject previousModel = playerCOM.GetChild (0).gameObject;

			GameObject newModel = GameObject.Instantiate (currentCharacterGameObject, previousModel.transform.position, previousModel.transform.rotation) as GameObject;
			newCharacterModel = newModel;
			newModel.name = currentCharacterGameObject.name;

			characterAnimator = newModel.GetComponentInChildren<Animator> ();

			currentCharacterInstantiated.GetComponentInChildren<Animator> ().avatar = characterAnimator.avatar;
			checkToCreateCharacterActive = true;
		}

		if (currentCharacterInstantiated != null) {
			currentBuildPlayer = currentCharacterInstantiated.GetComponentInChildren<buildPlayer> ();

			if (currentBuildPlayer != null) {
				int characterSettingsTemplateInfoListCount = currentBuildPlayer.characterSettingsTemplateInfoList.Count;

				if (!creatingCharacter) {
					if (characterSettingsTemplateInfoListCount > 0) {
						templateIndex = 0;

						templateList = new string[characterSettingsTemplateInfoListCount];

						for (int i = 0; i < characterSettingsTemplateInfoListCount; i++) { 
							//must convert file path to relative-to-unity path (and watch for '\' character between Win/Mac)
							templateList [i] = currentBuildPlayer.characterSettingsTemplateInfoList [i].Name;
						}
					} else {
						templateList = new string[0];
					}
				}
			}
		}
	}

	public void getCharacterBones ()
	{
		temporalCharacterObject = GameObject.Instantiate (currentCharacterGameObject, Vector3.zero, Quaternion.identity) as GameObject;

		characterAnimator = temporalCharacterObject.GetComponentInChildren<Animator> ();

		if (characterAnimator == null) {
			Debug.Log ("WARNING: There is not an animator component in this model, make sure to attach that component before create a new character");
			return;
		}

		head = characterAnimator.GetBoneTransform (HumanBodyBones.Head);
		neck = characterAnimator.GetBoneTransform (HumanBodyBones.Neck);
		if (neck == null) {
			if (head != null) {
				neck = head.parent;
			} else {
				Debug.Log ("WARNING: no head found, assign it manually to make sure all of them are configured correctly");
			}
		}	

		chest = characterAnimator.GetBoneTransform (HumanBodyBones.Chest);
		spine = characterAnimator.GetBoneTransform (HumanBodyBones.Spine);

		if (spine) {
			if (chest) {
				if (spine != chest.parent) {
					spine = chest.parent;
				}
			} else {
				Debug.Log ("WARNING: no chest found, assign it manually to make sure all of them are configured correctly");
			}
		} else {
			Debug.Log ("WARNING: no spine found, assign it manually to make sure all of them are configured correctly");
		}

		hips = characterAnimator.GetBoneTransform (HumanBodyBones.Hips);

		rightLowerArm = characterAnimator.GetBoneTransform (HumanBodyBones.RightLowerArm);
		leftLowerArm = characterAnimator.GetBoneTransform (HumanBodyBones.LeftLowerArm);
		rightHand = characterAnimator.GetBoneTransform (HumanBodyBones.RightHand);
		leftHand = characterAnimator.GetBoneTransform (HumanBodyBones.LeftHand);

		rightLowerLeg = characterAnimator.GetBoneTransform (HumanBodyBones.RightLowerLeg);
		leftLowerLeg = characterAnimator.GetBoneTransform (HumanBodyBones.LeftLowerLeg);
		rightFoot = characterAnimator.GetBoneTransform (HumanBodyBones.RightFoot);
		leftFoot = characterAnimator.GetBoneTransform (HumanBodyBones.LeftFoot);
		rightToes = characterAnimator.GetBoneTransform (HumanBodyBones.RightToes);
		leftToes = characterAnimator.GetBoneTransform (HumanBodyBones.LeftToes);
	}

	public bool checkAllBonesFound ()
	{
		return 
			(head != null) &&
		(neck != null) &&
		(chest != null) &&
		(spine != null) &&
		(hips != null) &&
		(rightLowerArm != null) &&
		(leftLowerArm != null) &&
		(rightHand != null) &&
		(leftHand != null) &&
		(rightLowerLeg != null) &&
		(leftLowerLeg != null) &&
		(rightFoot != null) &&
		(leftFoot != null) &&
		(rightToes != null) &&
		(leftToes != null);
	}

	bool checkCreateCharacter ()
	{
		getCharacterBones ();

		if (!checkAllBonesFound ()) {

			string skeletonMessage = "WARNING: Not all bones have been found on this model: ";
			if (head == null) {
				skeletonMessage += "\n head not found";
			}

			if (neck == null) {
				skeletonMessage += "\n neck not found";
			}

			if (chest == null) {
				skeletonMessage += "\n chest not found";
			}

			if (spine == null) {
				skeletonMessage += "\n spine not found";
			}

			if (hips == null) {
				skeletonMessage += "\n hips not found";
			}

			if (rightLowerArm == null) {
				skeletonMessage += "\n right lower arm not found";
			}

			if (leftLowerArm == null) {
				skeletonMessage += "\n left lower arm not found";
			}

			if (rightHand == null) {
				skeletonMessage += "\n right hand not found";
			}

			if (leftHand == null) {
				skeletonMessage += "\n left hand not found";
			}

			if (rightLowerLeg == null) {
				skeletonMessage += "\n right lower leg not found";
			}

			if (leftLowerLeg == null) {
				skeletonMessage += "\n left lower leg not found";
			}

			if (rightFoot == null) {
				skeletonMessage += "\n right foot not found";
			}

			if (leftFoot == null) {
				skeletonMessage += "\n left foot not found";
			}

			if (rightToes == null) {
				skeletonMessage += "\n right toes not found";
			}

			if (leftToes == null) {
				skeletonMessage += "\n left toes not found";
			}

			Debug.Log (skeletonMessage);

			return false;
		}

		return true;
	}

	public void showMessage (string messageContent, int fontSize, MessageType messageType)
	{
		GUILayout.BeginHorizontal ();
		EditorGUILayout.HelpBox ("", messageType);

		style = new GUIStyle (EditorStyles.helpBox);
		style.richText = true;

		style.fontStyle = FontStyle.Bold;
		style.fontSize = fontSize;
		EditorGUILayout.LabelField (messageContent, style);

		GUILayout.EndHorizontal ();
	}
}