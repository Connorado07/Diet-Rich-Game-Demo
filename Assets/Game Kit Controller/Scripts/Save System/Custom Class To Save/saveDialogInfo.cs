using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class saveDialogInfo : saveGameInfo
{
	public dialogManager mainDialogManager;

	public string mainDialogManagerName = "Dialog Manager";

	List<persistanceDialogContentInfo> persistanceInfoList;


	public override void saveGame (int saveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		saveGameContent (saveNumber, playerID, currentSaveDataPath, showDebugInfo);
	}

	public override void loadGame (int saveNumberToLoad, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		loadGameContent (saveNumberToLoad, playerID, currentSaveDataPath, showDebugInfo);
	}


	public void saveGameContent (int currentSaveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		getMainManager ();

		if (mainDialogManager == null) {
			return;
		}

		if (!mainDialogManager.dialogSystemEnabled) {
			return;
		}

		if (!mainDialogManager.saveCurrentDialogContentToSaveFile) {
			return;
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Saving dialog");
		}
			
		bool saveLocated = false;
		bool playerLocated = false;

		int saveSlotIndex = -1;
		int listIndex = -1;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		persistancePlayerDialogContentInfo dialogContentToSave = getPersistanceList (playerID, showDebugInfo);

		persistanceDialogContentListBySaveSlotInfo newPersistanceDialogContentListBySaveSlotInfo = new persistanceDialogContentListBySaveSlotInfo ();

		List<persistanceDialogContentListBySaveSlotInfo> infoListToSave = new List<persistanceDialogContentListBySaveSlotInfo> ();

		if (File.Exists (currentSaveDataPath)) {
			bf = new BinaryFormatter ();
			file = File.Open (currentSaveDataPath, FileMode.Open);
			object currentData = bf.Deserialize (file);
			infoListToSave = currentData as List<persistanceDialogContentListBySaveSlotInfo>;

			file.Close ();	
		}

		int infoListToSaveCount = infoListToSave.Count;

		for (int j = 0; j < infoListToSaveCount; j++) {
			if (infoListToSave [j].saveNumber == currentSaveNumber) {
				newPersistanceDialogContentListBySaveSlotInfo = infoListToSave [j];
				saveLocated = true;
				saveSlotIndex = j;
			}
		}

		if (saveLocated) {
			int playerDialogContentListCount = newPersistanceDialogContentListBySaveSlotInfo.playerDialogContentList.Count;

			for (int j = 0; j < playerDialogContentListCount; j++) {
				if (newPersistanceDialogContentListBySaveSlotInfo.playerDialogContentList [j].playerID == dialogContentToSave.playerID) {
					playerLocated = true;
					listIndex = j;
				}
			}
		}

		//if the save is located, check if the player id exists
		if (saveLocated) {
			//if player id exists, overwrite it
			if (playerLocated) {
				infoListToSave [saveSlotIndex].playerDialogContentList [listIndex].dialogContentList = dialogContentToSave.dialogContentList;
			} else {
				infoListToSave [saveSlotIndex].playerDialogContentList.Add (dialogContentToSave);
			}
		} else {
			newPersistanceDialogContentListBySaveSlotInfo.saveNumber = currentSaveNumber;
			newPersistanceDialogContentListBySaveSlotInfo.playerDialogContentList.Add (dialogContentToSave);
			infoListToSave.Add (newPersistanceDialogContentListBySaveSlotInfo);
		}

		bf = new BinaryFormatter ();
		file = File.Open (currentSaveDataPath, FileMode.OpenOrCreate); 
		bf.Serialize (file, infoListToSave);

		file.Close ();
	}

	public void loadGameContent (int saveNumberToLoad, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		getMainManager ();

		if (mainDialogManager == null) {
			return;
		}

		if (!mainDialogManager.dialogSystemEnabled) {
			return;
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Loading dialog");
		}

		persistanceInfoList = new List<persistanceDialogContentInfo> ();

		//need to store and check the current slot saved and the player which is saving, to get that concrete info
		List<persistanceDialogContentListBySaveSlotInfo> infoListToLoad = new List<persistanceDialogContentListBySaveSlotInfo> ();

		if (File.Exists (currentSaveDataPath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (currentSaveDataPath, FileMode.Open);
			object currentData = bf.Deserialize (file);
			infoListToLoad = currentData as List<persistanceDialogContentListBySaveSlotInfo>;

			file.Close ();	
		}

		if (saveNumberToLoad > -1) {
			persistanceDialogContentListBySaveSlotInfo newPersistanceDialogContentListBySaveSlotInfo = new persistanceDialogContentListBySaveSlotInfo ();

			int infoListToLoadCount = infoListToLoad.Count;

			for (int j = 0; j < infoListToLoadCount; j++) {

				if (infoListToLoad [j].saveNumber == saveNumberToLoad) {
					newPersistanceDialogContentListBySaveSlotInfo = infoListToLoad [j];
				}
			}

			int listIndex = -1;

			int playerDialogContentListCount = newPersistanceDialogContentListBySaveSlotInfo.playerDialogContentList.Count;

			for (int j = 0; j < playerDialogContentListCount; j++) {

				if (newPersistanceDialogContentListBySaveSlotInfo.playerDialogContentList [j].playerID == playerID) {
					listIndex = j;
				}
			}

			if (listIndex > -1) {
				persistanceInfoList.AddRange (newPersistanceDialogContentListBySaveSlotInfo.playerDialogContentList [listIndex].dialogContentList);
			}
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Dialogs Loaded in Save Number " + saveNumberToLoad);
			print ("Number of dialog: " + persistanceInfoList.Count);
		}

		loadInfoOnMainComponent ();
	}

	public persistancePlayerDialogContentInfo getPersistanceList (int playerID, bool showDebugInfo)
	{
		persistancePlayerDialogContentInfo newPersistancePlayerDialogContentInfo = new persistancePlayerDialogContentInfo ();

		newPersistancePlayerDialogContentInfo.playerID = playerID;

		List<persistanceDialogContentInfo> newPersistanceDialogContentInfoList = new List<persistanceDialogContentInfo> ();

		List<dialogContentSystem> dialogContentSystemList = mainDialogManager.dialogContentSystemList;

		int dialogContentSystemListCount = dialogContentSystemList.Count;

		for (int k = 0; k < dialogContentSystemListCount; k++) {

			dialogContentSystem currentDialogContentSystem = dialogContentSystemList [k];
			
			if (currentDialogContentSystem != null) {
				persistanceDialogContentInfo newPersistanceDialogContentInfo = new persistanceDialogContentInfo ();
					
				newPersistanceDialogContentInfo.dialogContentID = currentDialogContentSystem.dialogContentID;
				newPersistanceDialogContentInfo.dialogContentScene = currentDialogContentSystem.dialogContentScene;
				newPersistanceDialogContentInfo.currentDialogIndex = currentDialogContentSystem.currentDialogIndex;

				newPersistanceDialogContentInfoList.Add (newPersistanceDialogContentInfo);
			}
		}	

		newPersistancePlayerDialogContentInfo.dialogContentList = newPersistanceDialogContentInfoList;

		return newPersistancePlayerDialogContentInfo;
	}


	void loadInfoOnMainComponent ()
	{
		if (persistanceInfoList != null && persistanceInfoList.Count > 0) {
			int persistanceInfoListCount = persistanceInfoList.Count;

			List<dialogContentSystem> dialogContentSystemList = mainDialogManager.dialogContentSystemList;

			int dialogContentSystemListCount = dialogContentSystemList.Count;

			for (int i = 0; i < persistanceInfoListCount; i++) {

				persistanceDialogContentInfo currentPersistanceDialogContentInfo = persistanceInfoList [i];

				bool dialogContentFound = false;

				for (int j = 0; j < dialogContentSystemListCount; j++) {
					if (!dialogContentFound) {
						dialogContentSystem currentDialogContentSystem = dialogContentSystemList [j];

						if (currentDialogContentSystem != null) {
							if (currentDialogContentSystem.dialogContentScene == currentPersistanceDialogContentInfo.dialogContentScene &&
							    currentDialogContentSystem.dialogContentID == currentPersistanceDialogContentInfo.dialogContentID) {
								currentDialogContentSystem.setCompleteDialogIndex (currentPersistanceDialogContentInfo.currentDialogIndex);

								dialogContentFound = true;
							}
						}
					}
				}
			}
		}
	}


	void getMainManager ()
	{
		if (mainDialogManager == null) {
			GKC_Utils.instantiateMainManagerOnSceneWithType (mainDialogManagerName, typeof(dialogManager));

			mainDialogManager = FindObjectOfType<dialogManager> ();
		}
	}
}