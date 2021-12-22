using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof(vehicleCameraShake))]
[CanEditMultipleObjects]
public class vehicleCameraShakeEditor : Editor
{
	SerializedProperty headBobEnabled;
	SerializedProperty externalForceStateName;
	SerializedProperty shakingActive;
	SerializedProperty playerBobState;
	SerializedProperty bobStatesList;

	string currentState;
	bool expanded;

	void OnEnable ()
	{
		headBobEnabled = serializedObject.FindProperty ("headBobEnabled");
		externalForceStateName = serializedObject.FindProperty ("externalForceStateName");
		shakingActive = serializedObject.FindProperty ("shakingActive");
		playerBobState = serializedObject.FindProperty ("playerBobState");
		bobStatesList = serializedObject.FindProperty ("bobStatesList");
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.BeginVertical (GUILayout.Height (30));

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Main Settings", "window");
		EditorGUILayout.PropertyField (headBobEnabled);
		EditorGUILayout.PropertyField (externalForceStateName);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Camera State", "window");
		GUILayout.Label ("Shaking Active: \t" + shakingActive.boolValue);
		EditorGUILayout.Space ();
		if (shakingActive.boolValue) {
			if (playerBobState != null) {
				currentState = playerBobState.FindPropertyRelative ("Name").stringValue;
			}
		} else {
			currentState = "Idle";
		}
		GUILayout.Label ("Current Shake State: \t" + currentState);
		GUILayout.EndVertical ();

		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		GUILayout.BeginVertical ("Camera Shake List", "window");
		showUpperList (bobStatesList);
		GUILayout.EndVertical ();

		EditorGUILayout.Space ();

		if (GUI.changed) {
			serializedObject.ApplyModifiedProperties ();
		}

		EditorGUILayout.Space ();
	}

	void showListElementInfo (SerializedProperty list)
	{
		GUILayout.BeginVertical ("box");
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("Name"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("eulAmount"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("eulSpeed"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("eulSmooth"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("stateEnabled"));
		EditorGUILayout.PropertyField (list.FindPropertyRelative ("isCurrentState"));
		GUILayout.EndVertical ();
	}

	void showUpperList (SerializedProperty list)
	{
		GUILayout.BeginVertical ();
		EditorGUILayout.PropertyField (list, new GUIContent ("Camera Shake States"), false);
		if (list.isExpanded) {
			GUILayout.Label ("Number Of States: \t" + list.arraySize);

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add State")) {
				list.arraySize++;
			}
			if (GUILayout.Button ("Clear List")) {
				list.arraySize = 0;
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
					EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), false);
					if (list.GetArrayElementAtIndex (i).isExpanded) {
						expanded = true;
						showListElementInfo (list.GetArrayElementAtIndex (i));
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
}
#endif