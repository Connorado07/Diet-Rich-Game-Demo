using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class mainManagerAdministrator : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public List<mainManagerInfo> mainManagerInfoList = new List<mainManagerInfo> ();

	[Space]

	[TextArea (3, 15)]public string explanation = 
		"This component stores the a list of prefabs which are the main managers of the game, as these objects are now separated objects" +
		"from the player it self. \n\n" +

		"This includes elements like the main inventory list manager, faction system, mission manager, dialog manager, etc....\n\n" +

		"Just press the button Add Main Managers to Scene and they will be spawned on the scene, " +
		"so you can configure the values on these manager and use the button Update Main Managers Info to Prefabs, to update the new info " +
		"configured. ";


	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;


	public void updateMainManagersInfoToPrefabs ()
	{
		#if UNITY_EDITOR

		for (int i = 0; i < mainManagerInfoList.Count; i++) {
			if (mainManagerInfoList [i].mainManagerOnScene != null) {
				GameObject newPrefab = GameObject.Find (mainManagerInfoList [i].mainManagerOnScene.name);

				PrefabUtility.ReplacePrefab (newPrefab, mainManagerInfoList [i].mainManagerPrefab, ReplacePrefabOptions.ReplaceNameBased);
			}
		}

		updateComponent ();

		#endif
	}

	public void addAllMainManagersToScene ()
	{
		for (int i = 0; i < mainManagerInfoList.Count; i++) {
			addMainManagerToScene (mainManagerInfoList [i].Name);
		}
	}

	public void addMainManagerToScene (string managerName)
	{
		int currentIndex = mainManagerInfoList.FindIndex (s => s.Name == managerName);

		if (currentIndex > -1) {
			mainManagerInfo currentMainManagerInfo = mainManagerInfoList [currentIndex];

			if (currentMainManagerInfo.mainManagerOnScene == null) {
				GameObject managerPrefab = currentMainManagerInfo.mainManagerPrefab;

				if (managerPrefab != null) {
					GameObject newManagerOnScene = (GameObject)Instantiate (managerPrefab, Vector3.zero, Quaternion.identity);

					newManagerOnScene.name = managerPrefab.name;

					currentMainManagerInfo.mainManagerOnScene = newManagerOnScene as UnityEngine.Object;

					updateComponent ();

					if (showDebugPrint) {
						print ("Main Manager " + managerName + " added on scene");
					}
				}
			}
		}
	}

	public void addMainManagerToSceneWithType (string managerName, Type typeToSearch)
	{
		int currentIndex = mainManagerInfoList.FindIndex (s => s.Name == managerName);

		if (currentIndex > -1) {
			mainManagerInfo currentMainManagerInfo = mainManagerInfoList [currentIndex];

//			print (currentMainManagerInfo.Name);

			if (currentMainManagerInfo.mainManagerOnScene == null) {
//				print (typeToSearch.Name);

				UnityEngine.Object typeObject = UnityEngine.Object.FindObjectOfType (typeToSearch);

				if (typeObject != null) {
//					print (typeObject.name);

					currentMainManagerInfo.mainManagerOnScene = typeObject;

					updateComponent ();

					if (showDebugPrint) {
						print ("Main Manager " + managerName + " located on scene");
					}
				} else {

					GameObject managerPrefab = currentMainManagerInfo.mainManagerPrefab;

					if (managerPrefab != null) {
						GameObject newManagerOnScene = (GameObject)Instantiate (managerPrefab, Vector3.zero, Quaternion.identity);

						newManagerOnScene.name = managerPrefab.name;

						currentMainManagerInfo.mainManagerOnScene = newManagerOnScene as UnityEngine.Object;

						updateComponent ();

						if (showDebugPrint) {
							print ("Main Manager " + managerName + " added on scene");
						}
					}
				}
			}
		}
	}

	void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}

	[System.Serializable]
	public class mainManagerInfo
	{
		public string Name;
		public GameObject mainManagerPrefab;

		public UnityEngine.Object mainManagerOnScene;
	}
}
