using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

//a simple editor to add a button in the features manager script inspector
[CustomEditor (typeof(customCharacterControllerManager))]
public class customCharacterControllerManagerEditor : Editor
{
	customCharacterControllerManager manager;

	Vector3 curretPositionHandle;
	Quaternion currentRotationHandle;

	void OnEnable ()
	{
		manager = (customCharacterControllerManager)target;
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Enable Current Generic Model")) {
			manager.toggleCharacterModelMeshOnEditor (true);
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Enable Humanoid Model")) {
			manager.toggleCharacterModelMeshOnEditor (false);
		}

		EditorGUILayout.Space ();
	}
}
#endif