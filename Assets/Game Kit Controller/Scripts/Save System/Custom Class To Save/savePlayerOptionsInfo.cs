using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class savePlayerOptionsInfo : saveGameInfo
{
	public playerOptionsEditorSystem mainPlayerOptionsEditorSystem;

	List<persistanceOptionsInfo> persistanceInfoList;


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
		if (mainPlayerOptionsEditorSystem == null) {
			return;
		}

		if (!mainPlayerOptionsEditorSystem.playerOptionsEditorEnabled) {
			return;
		}

		if (!mainPlayerOptionsEditorSystem.saveCurrentPlayerOptionsToSaveFile) {
			return;
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Saving player options");
		}


		bool saveLocated = false;
		bool playerLocated = false;

		int saveSlotIndex = -1;
		int listIndex = -1;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		persistancePlayerOptionsInfo playerOptionsToSave = getPersistanceList (playerID, showDebugInfo);

		persistancePlayerOptionsListBySaveSlotInfo newPersistancePlayerOptionsListBySaveSlotInfo = new persistancePlayerOptionsListBySaveSlotInfo ();

		List<persistancePlayerOptionsListBySaveSlotInfo> infoListToSave = new List<persistancePlayerOptionsListBySaveSlotInfo> ();

		if (File.Exists (currentSaveDataPath)) {
			bf = new BinaryFormatter ();
			file = File.Open (currentSaveDataPath, FileMode.Open);
			object currentData = bf.Deserialize (file);
			infoListToSave = currentData as List<persistancePlayerOptionsListBySaveSlotInfo>;

			file.Close ();	
		}

		int infoListToSaveCount = infoListToSave.Count;

		for (int j = 0; j < infoListToSaveCount; j++) {
			if (infoListToSave [j].saveNumber == currentSaveNumber) {
				newPersistancePlayerOptionsListBySaveSlotInfo = infoListToSave [j];
				saveLocated = true;
				saveSlotIndex = j;
			}
		}

		if (saveLocated) {
			int playerOptionsListCount = newPersistancePlayerOptionsListBySaveSlotInfo.playerOptionsList.Count;

			for (int j = 0; j < playerOptionsListCount; j++) {
				if (newPersistancePlayerOptionsListBySaveSlotInfo.playerOptionsList [j].playerID == playerOptionsToSave.playerID) {
					playerLocated = true;
					listIndex = j;
				}
			}
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("EXTRA INFO\n");
			print ("Number of options: " + playerOptionsToSave.optionsList.Count);
			print ("Current Save Number " + currentSaveNumber);
			print ("Save Located " + saveLocated);
			print ("Player Located " + playerLocated);
			print ("Player ID " + playerOptionsToSave.playerID);
		}

		//if the save is located, check if the player id exists
		if (saveLocated) {
			//if player id exists, overwrite it
			if (playerLocated) {
				infoListToSave [saveSlotIndex].playerOptionsList [listIndex].optionsList = playerOptionsToSave.optionsList;
			} else {
				infoListToSave [saveSlotIndex].playerOptionsList.Add (playerOptionsToSave);
			}
		} else {
			newPersistancePlayerOptionsListBySaveSlotInfo.saveNumber = currentSaveNumber;
			newPersistancePlayerOptionsListBySaveSlotInfo.playerOptionsList.Add (playerOptionsToSave);
			infoListToSave.Add (newPersistancePlayerOptionsListBySaveSlotInfo);
		}

		bf = new BinaryFormatter ();
		file = File.Open (currentSaveDataPath, FileMode.OpenOrCreate); 
		bf.Serialize (file, infoListToSave);

		file.Close ();
	}

	public void loadGameContent (int saveNumberToLoad, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		if (mainPlayerOptionsEditorSystem == null) {
			return;
		}

		if (!mainPlayerOptionsEditorSystem.playerOptionsEditorEnabled) {
			return;
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Loading player options");
		}
			
		persistanceInfoList = new List<persistanceOptionsInfo> ();

		//need to store and check the current slot saved and the player which is saving, to get that concrete info
		List<persistancePlayerOptionsListBySaveSlotInfo> infoListToLoad = new List<persistancePlayerOptionsListBySaveSlotInfo> ();

		if (File.Exists (currentSaveDataPath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (currentSaveDataPath, FileMode.Open);
			object currentData = bf.Deserialize (file);
			infoListToLoad = currentData as List<persistancePlayerOptionsListBySaveSlotInfo>;

			file.Close ();	
		}
			
		if (saveNumberToLoad > -1) {
			persistancePlayerOptionsListBySaveSlotInfo newPersistancePlayerOptionsListBySaveSlotInfo = new persistancePlayerOptionsListBySaveSlotInfo ();

			int infoListToLoadCount = infoListToLoad.Count;

			for (int j = 0; j < infoListToLoadCount; j++) {

				if (infoListToLoad [j].saveNumber == saveNumberToLoad) {
					newPersistancePlayerOptionsListBySaveSlotInfo = infoListToLoad [j];
				}
			}

			int listIndex = -1;

			int playerOptionsListCount = newPersistancePlayerOptionsListBySaveSlotInfo.playerOptionsList.Count;

			for (int j = 0; j < playerOptionsListCount; j++) {

				if (newPersistancePlayerOptionsListBySaveSlotInfo.playerOptionsList [j].playerID == playerID) {
					listIndex = j;
				}
			}

			if (listIndex > -1) {
				persistanceInfoList.AddRange (newPersistancePlayerOptionsListBySaveSlotInfo.playerOptionsList [listIndex].optionsList);
			}
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Player Options Loaded in Save Number " + saveNumberToLoad);
			print ("Number of Player Options: " + persistanceInfoList.Count);
		}

		loadInfoOnMainComponent ();
	}


	public persistancePlayerOptionsInfo getPersistanceList (int playerID, bool showDebugInfo)
	{
		persistancePlayerOptionsInfo newPersistancePlayerOptionsInfo = new persistancePlayerOptionsInfo ();

		newPersistancePlayerOptionsInfo.playerID = playerID;

		List<persistanceOptionsInfo> newPersistanceOptionsInfoList = new List<persistanceOptionsInfo> ();

		List<playerOptionsEditorSystem.optionInfo> optionInfoList = mainPlayerOptionsEditorSystem.optionInfoList;

		int optionInfoListCount = optionInfoList.Count;

		for (int k = 0; k < optionInfoListCount; k++) {
			persistanceOptionsInfo newPersistanceOptionsInfo = new persistanceOptionsInfo ();

			playerOptionsEditorSystem.optionInfo currentOptionInfo = optionInfoList [k];

			newPersistanceOptionsInfo.currentScrollBarValue = currentOptionInfo.currentScrollBarValue;
			newPersistanceOptionsInfo.currentSliderValue = currentOptionInfo.currentSliderValue;
			newPersistanceOptionsInfo.currentToggleValue = currentOptionInfo.currentToggleValue;

			newPersistanceOptionsInfoList.Add (newPersistanceOptionsInfo);
		}	

		newPersistancePlayerOptionsInfo.optionsList = newPersistanceOptionsInfoList;

		return newPersistancePlayerOptionsInfo;
	}


	void loadInfoOnMainComponent ()
	{
		if (persistanceInfoList != null && persistanceInfoList.Count > 0) {

			List<playerOptionsEditorSystem.optionInfo> optionInfoList = mainPlayerOptionsEditorSystem.optionInfoList;

			int persistanceInfoListCount = persistanceInfoList.Count;

			for (int i = 0; i < persistanceInfoListCount; i++) {

				persistanceOptionsInfo currentPersistanceOptionsInfo = persistanceInfoList [i];

				playerOptionsEditorSystem.optionInfo currentOptionInfo = optionInfoList [i];

				if (currentOptionInfo.useScrollBar) {
					currentOptionInfo.currentScrollBarValue = currentPersistanceOptionsInfo.currentScrollBarValue;
				}

				if (currentOptionInfo.useSlider) {
					currentOptionInfo.currentSliderValue = currentPersistanceOptionsInfo.currentSliderValue;
				}

				if (currentOptionInfo.useToggle) {
					currentOptionInfo.currentToggleValue = currentPersistanceOptionsInfo.currentToggleValue;
				}
			}

			mainPlayerOptionsEditorSystem.isLoadingGame = true;
		}
	}
}