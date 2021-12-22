using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof(playerOptionsEditorSystem))]
public class playerOptionsEditorSystemEditor : Editor
{
	SerializedProperty playerOptionsEditorEnabled;
	SerializedProperty initializeOptionsOnlyWhenLoadingGame;
	SerializedProperty saveCurrentPlayerOptionsToSaveFile;
	SerializedProperty valuesInitialized;
	SerializedProperty optionInfoList;

	bool optionEnabled;
	string isEnabled;

	void OnEnable ()
	{
		playerOptionsEditorEnabled = serializedObject.FindProperty ("playerOptionsEditorEnabled");
		initializeOptionsOnlyWhenLoadingGame = serializedObject.FindProperty ("initializeOptionsOnlyWhenLoadingGame");
		saveCurrentPlayerOptionsToSaveFile = serializedObject.FindProperty ("saveCurrentPlayerOptionsToSaveFile");
		valuesInitialized = serializedObject.FindProperty ("valuesInitialized");
		optionInfoList = serializedObject.FindProperty ("optionInfoList");
	}

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.Space ();
	
		GUILayout.BeginVertical ("Main Settings", "window");
		EditorGUILayout.PropertyField (playerOptionsEditorEnabled);
		EditorGUILayout.PropertyField (initializeOptionsOnlyWhenLoadingGame);
		EditorGUILayout.PropertyField (saveCurrentPlayerOptionsToSaveFile);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Debug State", "window");
		EditorGUILayout.PropertyField (valuesInitialized);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Option Info List", "window");
		showOptionInfoList (optionInfoList);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		if (GUI.changed) {
			serializedObject.ApplyModifiedProperties ();
		}
	}

	void showOptionInfoList (SerializedProperty list)
	{
		GUILayout.BeginVertical ();
		EditorGUILayout.PropertyField (list, false);

		if (list.isExpanded) {

			EditorGUILayout.Space ();

			GUILayout.Label ("Number of Options: " + list.arraySize);

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add Option")) {
				list.arraySize++;
			}
			if (GUILayout.Button ("Clear")) {
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
				bool expanded = false;
				GUILayout.BeginHorizontal ();
				GUILayout.BeginHorizontal ("box");

				EditorGUILayout.Space ();

				if (i < list.arraySize && i >= 0) {
					EditorGUILayout.BeginVertical ();

					optionEnabled = list.GetArrayElementAtIndex (i).FindPropertyRelative ("optionEnabled").boolValue;
					isEnabled = " +";
					if (!optionEnabled) {
						isEnabled = " -";
					}
					EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), new GUIContent (list.GetArrayElementAtIndex (i).displayName + isEnabled), false);
				
					if (list.GetArrayElementAtIndex (i).isExpanded) {
						showOptionInfoListElement (list.GetArrayElementAtIndex (i));
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


	void showOptionInfoListElement (SerializedProperty list)
	{
		GUILayout.BeginVertical ("box");

		GUILayout.BeginVertical ("Main Settings", "window");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("optionEnabled"));
		GUILayout.EndVertical ();

		if (list.FindPropertyRelative ("optionEnabled").boolValue) {
			EditorGUILayout.Space ();

			GUILayout.BeginVertical ("ScrollBar Settings", "window");
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useScrollBar"));
			if (list.FindPropertyRelative ("useScrollBar").boolValue) {
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("scrollBar"));
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("currentScrollBarValue"));
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("defaultScrollerbarValue"));
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("useOppositeBoolValue"));

				EditorGUILayout.Space ();

				EditorGUILayout.PropertyField (list.FindPropertyRelative ("optionEvent"), new GUIContent ("Bool Option Event"));
			}
			GUILayout.EndVertical ();

			EditorGUILayout.Space ();

			GUILayout.BeginVertical ("Slider Settings", "window");
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useSlider"));
			if (list.FindPropertyRelative ("useSlider").boolValue) {
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("slider"));
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("currentSliderValue"));
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("defaultSliderValue"));

				EditorGUILayout.Space ();

				EditorGUILayout.PropertyField (list.FindPropertyRelative ("showSliderText"));
				if (list.FindPropertyRelative ("showSliderText").boolValue) {
					EditorGUILayout.PropertyField (list.FindPropertyRelative ("sliderText"));
				}

				EditorGUILayout.Space ();

				EditorGUILayout.PropertyField (list.FindPropertyRelative ("floatOptionEvent"));
			}
			GUILayout.EndVertical ();

			EditorGUILayout.Space ();

			GUILayout.BeginVertical ("Toggle Settings", "window");
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("useToggle"));
			if (list.FindPropertyRelative ("useToggle").boolValue) {
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("toggle"));
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("currentToggleValue"));
				EditorGUILayout.PropertyField (list.FindPropertyRelative ("defaultToggleValue"));

				EditorGUILayout.PropertyField (list.FindPropertyRelative ("useOppositeBoolValue"));

				EditorGUILayout.Space ();

				EditorGUILayout.PropertyField (list.FindPropertyRelative ("optionEvent"), new GUIContent ("Bool Option Event"));
			}
			GUILayout.EndVertical ();
		}

		GUILayout.EndVertical ();
	}

}
#endif