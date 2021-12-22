using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor (typeof(buildPlayer))]
public class buildPlayerEditor : Editor
{
	SerializedProperty hitCombat;
	SerializedProperty shootZone;
	SerializedProperty player;
	SerializedProperty playerElementsTransform;
	SerializedProperty grabPhysicalObject;
	SerializedProperty currentCharacterModel;
	SerializedProperty layerToPlaceNPC;
	SerializedProperty buildPlayerType;
	SerializedProperty hasWeaponsEnabled;
	SerializedProperty assignNewModelManually;
	SerializedProperty explanation;
	SerializedProperty newCharacterModel;
	SerializedProperty head;
	SerializedProperty neck;
	SerializedProperty chest;
	SerializedProperty spine;
	SerializedProperty rightLowerArm;
	SerializedProperty leftLowerArm;
	SerializedProperty rightHand;
	SerializedProperty leftHand;
	SerializedProperty rightLowerLeg;
	SerializedProperty leftLowerLeg;
	SerializedProperty rightFoot;
	SerializedProperty leftFoot;
	SerializedProperty rightToes;
	SerializedProperty leftToes;
	SerializedProperty settingsInfoCategoryList;
	SerializedProperty temporalSettingsInfoList;

	SerializedProperty newCharacterSettingsTemplate;

	SerializedProperty characterTemplateDataPath;

	SerializedProperty characterTemplateName;

	SerializedProperty characterTemplateID;

	SerializedProperty characterSettingsTemplateInfoList;

	SerializedProperty applyCharacterSettingsOnManualBuild;

//	SerializedProperty characterBodyElementsList;

	buildPlayer manager;

	bool expanded;

	bool showTemporalSettingList;
	string fieldName;

	bool showElements;

	GUIStyle style = new GUIStyle ();

	Color buttonColor;

	string currentButtonString;

	GUIStyle buttonStyle = new GUIStyle ();

	void OnEnable ()
	{
		hitCombat = serializedObject.FindProperty ("hitCombat");
		shootZone = serializedObject.FindProperty ("shootZone");
		player = serializedObject.FindProperty ("player");
		playerElementsTransform = serializedObject.FindProperty ("playerElementsTransform");
		grabPhysicalObject = serializedObject.FindProperty ("grabPhysicalObject");
		currentCharacterModel = serializedObject.FindProperty ("currentCharacterModel");
		layerToPlaceNPC = serializedObject.FindProperty ("layerToPlaceNPC");
		buildPlayerType = serializedObject.FindProperty ("buildPlayerType");
		hasWeaponsEnabled = serializedObject.FindProperty ("hasWeaponsEnabled");
		assignNewModelManually = serializedObject.FindProperty ("assignNewModelManually");
		explanation = serializedObject.FindProperty ("explanation");
		newCharacterModel = serializedObject.FindProperty ("newCharacterModel");
		head = serializedObject.FindProperty ("head");
		neck = serializedObject.FindProperty ("neck");
		chest = serializedObject.FindProperty ("chest");
		spine = serializedObject.FindProperty ("spine");
		rightLowerArm = serializedObject.FindProperty ("rightLowerArm");
		leftLowerArm = serializedObject.FindProperty ("leftLowerArm");
		rightHand = serializedObject.FindProperty ("rightHand");
		leftHand = serializedObject.FindProperty ("leftHand");
		rightLowerLeg = serializedObject.FindProperty ("rightLowerLeg");
		leftLowerLeg = serializedObject.FindProperty ("leftLowerLeg");
		rightFoot = serializedObject.FindProperty ("rightFoot");
		leftFoot = serializedObject.FindProperty ("leftFoot");
		rightToes = serializedObject.FindProperty ("rightToes");
		leftToes = serializedObject.FindProperty ("leftToes");
		settingsInfoCategoryList = serializedObject.FindProperty ("settingsInfoCategoryList");
		temporalSettingsInfoList = serializedObject.FindProperty ("temporalSettingsInfoList");

		newCharacterSettingsTemplate = serializedObject.FindProperty ("newCharacterSettingsTemplate");

		characterTemplateDataPath = serializedObject.FindProperty ("characterTemplateDataPath");

		characterTemplateName = serializedObject.FindProperty ("characterTemplateName");

		characterTemplateID = serializedObject.FindProperty ("characterTemplateID");

		characterSettingsTemplateInfoList = serializedObject.FindProperty ("characterSettingsTemplateInfoList");

		applyCharacterSettingsOnManualBuild = serializedObject.FindProperty ("applyCharacterSettingsOnManualBuild");

//		characterBodyElementsList = serializedObject.FindProperty ("characterBodyElementsList");

		manager = (buildPlayer)target;
	}

	public override void OnInspectorGUI ()
	{
		style.fontStyle = FontStyle.Bold;
		style.fontSize = 25;
		style.alignment = TextAnchor.MiddleCenter;

		GUILayout.BeginVertical ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("MAIN SETTINGS", style);

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Place New Character Settings", "window");
		EditorGUILayout.PropertyField (layerToPlaceNPC);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Build Settings", "window");
		EditorGUILayout.PropertyField (buildPlayerType);
		EditorGUILayout.PropertyField (hasWeaponsEnabled);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Build Player Manually", "window");
		EditorGUILayout.PropertyField (assignNewModelManually);
		if (assignNewModelManually.boolValue) {

			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (explanation);

			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (newCharacterModel);

			if (newCharacterModel.objectReferenceValue) {
				GUILayout.Label ("Top Part");
				EditorGUILayout.PropertyField (head);
				EditorGUILayout.PropertyField (neck);
				EditorGUILayout.PropertyField (chest);
				EditorGUILayout.PropertyField (spine);

				EditorGUILayout.Space ();

				GUILayout.Label ("Middle Part");
				EditorGUILayout.PropertyField (rightLowerArm);
				EditorGUILayout.PropertyField (leftLowerArm);
				EditorGUILayout.PropertyField (rightHand);
				EditorGUILayout.PropertyField (leftHand);

				EditorGUILayout.Space ();

				GUILayout.Label ("Lower Part");
				EditorGUILayout.PropertyField (rightLowerLeg);
				EditorGUILayout.PropertyField (leftLowerLeg);
				EditorGUILayout.PropertyField (rightFoot);
				EditorGUILayout.PropertyField (leftFoot);
				EditorGUILayout.PropertyField (rightToes);
				EditorGUILayout.PropertyField (leftToes);
			}

			EditorGUILayout.Space ();

			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (applyCharacterSettingsOnManualBuild);

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Search Bones On New Character")) {
				if (!Application.isPlaying) {
					manager.getCharacterBones ();
				}
			}
		}
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		buttonStyle = new GUIStyle (GUI.skin.button);

		buttonStyle.fontStyle = FontStyle.Bold;
		buttonStyle.fontSize = 16;

		if (GUILayout.Button ("\n BUILD CHARACTER \n", buttonStyle)) {
			if (!Application.isPlaying) {
				manager.buildCharacterByButton ();
			}
		}

		EditorGUILayout.Space ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("CHARACTER SETTINGS", style);

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Settings List", "window");

		showSettingsInfoCategoryList (settingsInfoCategoryList);

		EditorGUILayout.Space ();

		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		buttonColor = GUI.backgroundColor;

		if (showTemporalSettingList) {
			GUI.backgroundColor = Color.gray;
			currentButtonString = "\n Hide Settings List \n";
		} else {
			GUI.backgroundColor = buttonColor;
			currentButtonString = "\n Show Settings List \n";
		}
		if (GUILayout.Button (currentButtonString, buttonStyle)) {
			showTemporalSettingList = !showTemporalSettingList;
		}

		GUI.backgroundColor = buttonColor;

		EditorGUILayout.Space ();

		if (showTemporalSettingList) {

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Update Settings List", buttonStyle)) {
				manager.setTemporalSettingsInfoList ();
			}

			EditorGUILayout.Space ();

			GUILayout.BeginVertical ("Settings List", "window");
			showTemporalSettingsInfoList (temporalSettingsInfoList);
			GUILayout.EndVertical ();
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("\n APPLY CURRENT SETTINGS \n", buttonStyle)) {
			manager.adjustSettingsFromEditor ();
		}

		EditorGUILayout.Space ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("CHARACTER MANAGEMENT\n SETTINGS", style);

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Save/Load Settings List To File", "window");
		EditorGUILayout.PropertyField (newCharacterSettingsTemplate);	

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Save Settings List To File")) {
			manager.saveSettingsListToFile ();
		}

		if (GUILayout.Button ("Load Settings List From File")) {
			manager.loadSettingsListFromFile ();
		}
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Create New Character Template Data", "window");
		EditorGUILayout.PropertyField (characterTemplateID);	
		EditorGUILayout.PropertyField (characterTemplateDataPath);	
		EditorGUILayout.PropertyField (characterTemplateName);	

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Create Character Template")) {
			manager.createSettingsListTemplate ();
		}
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Character Template List", "window");
		showCharacterSettingsTemplateInfoList (characterSettingsTemplateInfoList);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Player Elements", "window");
		if (GUILayout.Button ("Show Elements")) {
			showElements = !showElements;
		}

		EditorGUILayout.Space ();

		if (showElements) {
			EditorGUILayout.PropertyField (hitCombat);
			EditorGUILayout.PropertyField (shootZone);
			EditorGUILayout.PropertyField (player);	
			EditorGUILayout.PropertyField (playerElementsTransform);	
			EditorGUILayout.PropertyField (grabPhysicalObject);	
			EditorGUILayout.PropertyField (currentCharacterModel);	

//			EditorGUILayout.Space ();
//
//			GUILayout.BeginVertical ("Character Body Elements List", "window");
//			showCharacterBodyElementsList (characterBodyElementsList);
//			GUILayout.EndVertical ();
		}

		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.EndVertical ();

		if (GUI.changed) {
			serializedObject.ApplyModifiedProperties ();
		}
	}

	void showSettingsInfoCategoryList (SerializedProperty list)
	{
		GUILayout.BeginVertical ();
		EditorGUILayout.PropertyField (list, false);
		if (list.isExpanded) {

			EditorGUILayout.Space ();

			GUILayout.Label ("Number Of Categories: \t" + list.arraySize);

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add Category")) {
				list.arraySize++;
			}
			if (GUILayout.Button ("Clear List")) {
				list.arraySize = 0;
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Expand All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = true;
				}
			}
			if (GUILayout.Button ("Collapse All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = false;
				}
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			for (int i = 0; i < list.arraySize; i++) {
				expanded = false;

				GUILayout.BeginHorizontal ();
				GUILayout.BeginHorizontal ("box");

				EditorGUILayout.Space ();

				if (i < list.arraySize && i >= 0) {
					EditorGUILayout.BeginVertical ();

					SerializedProperty currentArrayElement = list.GetArrayElementAtIndex (i);

					EditorGUILayout.PropertyField (currentArrayElement, false);
					if (currentArrayElement.isExpanded) {
						showSettingsInfoCategoryListElement (currentArrayElement);
						expanded = true;
					}

					EditorGUILayout.Space ();

					GUILayout.EndVertical ();
				}
				GUILayout.EndHorizontal ();
				if (expanded) {
					GUILayout.BeginVertical ();
				} else {
					GUILayout.BeginHorizontal ();
				}
				if (GUILayout.Button ("x")) {
					list.DeleteArrayElementAtIndex (i);
				}
				if (GUILayout.Button ("v")) {
					if (i >= 0) {
						list.MoveArrayElement (i, i + 1);
					}
				}
				if (GUILayout.Button ("^")) {
					if (i < list.arraySize) {
						list.MoveArrayElement (i, i - 1);
					}
				}
				if (expanded) {
					GUILayout.EndVertical ();
				} else {
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndHorizontal ();
			}
		}
		GUILayout.EndVertical ();
	}

	void showSettingsInfoCategoryListElement (SerializedProperty list)
	{
		GUILayout.BeginVertical ("box");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));

		EditorGUILayout.Space ();

		showSettingsInfoList (list.FindPropertyRelative ("settingsInfoList"));
		GUILayout.EndVertical ();
	}

	void showSettingsInfoList (SerializedProperty list)
	{
		GUILayout.BeginVertical ();
		EditorGUILayout.PropertyField (list, false);
		if (list.isExpanded) {

			EditorGUILayout.Space ();

			GUILayout.Label ("Number Of Settings: \t" + list.arraySize);

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add Settings")) {
				list.arraySize++;
			}
			if (GUILayout.Button ("Clear List")) {
				list.arraySize = 0;
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Expand All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = true;
				}
			}
			if (GUILayout.Button ("Collapse All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = false;
				}
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			for (int i = 0; i < list.arraySize; i++) {
				expanded = false;

				GUILayout.BeginHorizontal ();
				GUILayout.BeginHorizontal ("box");

				EditorGUILayout.Space ();

				if (i < list.arraySize && i >= 0) {
					EditorGUILayout.BeginVertical ();

					SerializedProperty currentArrayElement = list.GetArrayElementAtIndex (i);

					EditorGUILayout.PropertyField (currentArrayElement, false);
					if (currentArrayElement.isExpanded) {
						showSettingsInfoListElement (currentArrayElement);
						expanded = true;
					}

					EditorGUILayout.Space ();

					GUILayout.EndVertical ();
				}
				GUILayout.EndHorizontal ();
				if (expanded) {
					GUILayout.BeginVertical ();
				} else {
					GUILayout.BeginHorizontal ();
				}
				if (GUILayout.Button ("x")) {
					list.DeleteArrayElementAtIndex (i);
				}
				if (GUILayout.Button ("v")) {
					if (i >= 0) {
						list.MoveArrayElement (i, i + 1);
					}
				}
				if (GUILayout.Button ("^")) {
					if (i < list.arraySize) {
						list.MoveArrayElement (i, i - 1);
					}
				}
				if (expanded) {
					GUILayout.EndVertical ();
				} else {
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndHorizontal ();
			}

			EditorGUILayout.Space ();
		}
		GUILayout.EndVertical ();
	}

	void showSettingsInfoListElement (SerializedProperty list)
	{
		GUILayout.BeginVertical ("box");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));

		GUILayout.BeginVertical ("Bool Values Settings", "window");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useBoolState"));
		if (list.FindPropertyRelative ("useBoolState").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("boolState"));
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("eventToSetBoolState"));
		}
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Float Values Settings", "window");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useFloatValue"));
		if (list.FindPropertyRelative ("useFloatValue").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("floatValue"));
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("eventToSetFloatValue"));
		}
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("String Values Settings", "window");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useStringValue"));
		if (list.FindPropertyRelative ("useStringValue").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("stringValue"));
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("eventToSetStringValue"));
		}
		GUILayout.EndVertical ();

		EditorGUILayout.Space (); 

		GUILayout.BeginVertical ("Regular Values Settings", "window");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useRegularValue"));
		if (list.FindPropertyRelative ("useRegularValue").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("regularValue"));
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("eventToEnableActiveValue"));
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("eventToDisableActiveValue"));
		}
		GUILayout.EndVertical ();

		EditorGUILayout.Space (); 

		GUILayout.BeginVertical ("Field Explanation Settings", "window");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("useFieldExplanation"));
		if (list.FindPropertyRelative ("useFieldExplanation").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("fieldExplanation"));
		}
		GUILayout.EndVertical ();
		GUILayout.EndVertical ();
	}

	void showTemporalSettingsInfoList (SerializedProperty list)
	{
		GUILayout.BeginVertical ();
		EditorGUILayout.PropertyField (list, false);
		if (list.isExpanded) {

			EditorGUILayout.Space ();

			GUILayout.Label ("Number Of Settings: \t" + list.arraySize);

			EditorGUILayout.Space ();

			for (int i = 0; i < list.arraySize; i++) {

				GUILayout.BeginHorizontal ();
				GUILayout.BeginHorizontal ("box");

				if (i < list.arraySize && i >= 0) {
					EditorGUILayout.BeginVertical ();
				
					showTemporalSettingsInfoListElement (list.GetArrayElementAtIndex (i));

					GUILayout.EndVertical ();
				}
				GUILayout.EndHorizontal ();

				GUILayout.EndHorizontal ();
			}
		}
		GUILayout.EndVertical ();
	}

	void showTemporalSettingsInfoListElement (SerializedProperty list)
	{
		fieldName = list.FindPropertyRelative ("Name").stringValue;

		if (list.FindPropertyRelative ("useBoolState").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("boolState"), new GUIContent (fieldName));
		}

		if (list.FindPropertyRelative ("useFloatValue").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("floatValue"), new GUIContent (fieldName));
		}

		if (list.FindPropertyRelative ("useStringValue").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("stringValue"), new GUIContent (fieldName));
		}

		if (list.FindPropertyRelative ("useRegularValue").boolValue) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("regularValue"), new GUIContent (fieldName));
		}
	}

	void showCharacterSettingsTemplateInfoList (SerializedProperty list)
	{
		GUILayout.BeginVertical ();
		EditorGUILayout.PropertyField (list, false);
		if (list.isExpanded) {

			EditorGUILayout.Space ();

			GUILayout.Label ("Number Of Templates: \t" + list.arraySize);

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add Template")) {
				list.arraySize++;
			}
			if (GUILayout.Button ("Clear List")) {
				list.arraySize = 0;
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Expand All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = true;
				}
			}
			if (GUILayout.Button ("Collapse All")) {
				for (int i = 0; i < list.arraySize; i++) {
					list.GetArrayElementAtIndex (i).isExpanded = false;
				}
			}
			GUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			for (int i = 0; i < list.arraySize; i++) {

				GUILayout.BeginHorizontal ();
				GUILayout.BeginHorizontal ("box");

				if (i < list.arraySize && i >= 0) {
					EditorGUILayout.BeginVertical ();

					GUILayout.BeginHorizontal ();
					showCharacterSettingsTemplateInfoListElement (list.GetArrayElementAtIndex (i));

					if (GUILayout.Button ("x")) {
						list.DeleteArrayElementAtIndex (i);
					}
					GUILayout.EndHorizontal ();

					EditorGUILayout.Space ();

					if (GUILayout.Button ("Set as Current Template To Apply")) {
						manager.setTemplateFromListAsCurrentToApply (i);
					}

					GUILayout.EndVertical ();
				}
					
				GUILayout.EndHorizontal ();

				GUILayout.EndHorizontal ();
			}
		}
		GUILayout.EndVertical ();
	}

	void showCharacterSettingsTemplateInfoListElement (SerializedProperty list)
	{
		GUILayout.BeginVertical ("box");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("template"));
		GUILayout.EndVertical ();
	}


//	void showCharacterBodyElementsList (SerializedProperty list)
//	{
//		GUILayout.BeginVertical ();
//		EditorGUILayout.PropertyField (list, false);
//		if (list.isExpanded) {
//
//			EditorGUILayout.Space ();
//
//			GUILayout.Label ("Number Of Elements: \t" + list.arraySize);
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginHorizontal ();
//			if (GUILayout.Button ("Add Element")) {
//				list.arraySize++;
//			}
//			if (GUILayout.Button ("Clear List")) {
//				list.arraySize = 0;
//			}
//			GUILayout.EndHorizontal ();
//
//			EditorGUILayout.Space ();
//
//			GUILayout.BeginHorizontal ();
//			if (GUILayout.Button ("Expand All")) {
//				for (int i = 0; i < list.arraySize; i++) {
//					list.GetArrayElementAtIndex (i).isExpanded = true;
//				}
//			}
//			if (GUILayout.Button ("Collapse All")) {
//				for (int i = 0; i < list.arraySize; i++) {
//					list.GetArrayElementAtIndex (i).isExpanded = false;
//				}
//			}
//			GUILayout.EndHorizontal ();
//
//			EditorGUILayout.Space ();
//
//			for (int i = 0; i < list.arraySize; i++) {
//				expanded = false;
//
//				GUILayout.BeginHorizontal ();
//				GUILayout.BeginHorizontal ("box");
//
//				EditorGUILayout.Space ();
//
//				if (i < list.arraySize && i >= 0) {
//					EditorGUILayout.BeginVertical ();
//
//					SerializedProperty currentArrayElement = list.GetArrayElementAtIndex (i);
//
//					EditorGUILayout.PropertyField (currentArrayElement, false);
//					if (currentArrayElement.isExpanded) {
//						showCharacterBodyElementsListElement (currentArrayElement);
//						expanded = true;
//					}
//
//					EditorGUILayout.Space ();
//
//					GUILayout.EndVertical ();
//				}
//				GUILayout.EndHorizontal ();
//				if (expanded) {
//					GUILayout.BeginVertical ();
//				} else {
//					GUILayout.BeginHorizontal ();
//				}
//				if (GUILayout.Button ("x")) {
//					list.DeleteArrayElementAtIndex (i);
//				}
//				if (GUILayout.Button ("v")) {
//					if (i >= 0) {
//						list.MoveArrayElement (i, i + 1);
//					}
//				}
//				if (GUILayout.Button ("^")) {
//					if (i < list.arraySize) {
//						list.MoveArrayElement (i, i - 1);
//					}
//				}
//				if (expanded) {
//					GUILayout.EndVertical ();
//				} else {
//					GUILayout.EndHorizontal ();
//				}
//				GUILayout.EndHorizontal ();
//			}
//		}
//		GUILayout.EndVertical ();
//	}
//
//	void showCharacterBodyElementsListElement (SerializedProperty list)
//	{
//		GUILayout.BeginVertical ("box");
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));
//
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("elementGameObject"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("mainBoneToPlaceElement"));
//		EditorGUILayout.PropertyField (list.FindPropertyRelative ("alternativeBoneToPlaceElement"));
//
//		GUILayout.EndVertical ();
//	}
}
#endif