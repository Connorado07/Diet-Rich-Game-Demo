using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class savePlayerInventoryInfo : saveGameInfo
{
	public inventoryManager mainInventoryManager;

	List<inventoryListElement> persistanceInfoList;

	public override void saveGame (int saveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		saveGameContent (saveNumber, playerID, currentSaveDataPath, showDebugInfo);
	}

	public override void loadGame (int saveNumberToLoad, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		loadGameContent (saveNumberToLoad, playerID, currentSaveDataPath, showDebugInfo);
	}

	public override void saveGameFromEditor (int saveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		saveGameContentFromEditor (saveNumber, playerID, currentSaveDataPath, showDebugInfo);
	}

	public override void initializeValuesOnComponent ()
	{
		initializeValues ();
	}

	public void saveGameContent (int currentSaveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		if (mainInventoryManager == null) {
			return;
		}

		if (!mainInventoryManager.saveCurrentPlayerInventoryToSaveFile) {
			return;
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Saving player inventory");
		}

		bool saveLocated = false;
		bool playerLocated = false;

		int saveSlotIndex = -1;
		int listIndex = -1;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		persistanceInventoryListByPlayerInfo inventoryListToSave = getPersistanceList (playerID, showDebugInfo);

		persistanceInventoryListBySaveSlotInfo newPersistanceInventoryListBySaveSlotInfo = new persistanceInventoryListBySaveSlotInfo ();

		List<persistanceInventoryListBySaveSlotInfo> infoListToSave = new List<persistanceInventoryListBySaveSlotInfo> ();

		if (File.Exists (currentSaveDataPath)) {
			bf = new BinaryFormatter ();
			file = File.Open (currentSaveDataPath, FileMode.Open);
			object currentData = bf.Deserialize (file);
			infoListToSave = currentData as List<persistanceInventoryListBySaveSlotInfo>;

			file.Close ();	
		}

		if (showDebugInfo) {
			print ("Number of save list for player inventory " + infoListToSave.Count);
		}

		int infoListToSaveCount = infoListToSave.Count;

		for (int j = 0; j < infoListToSaveCount; j++) {
			if (saveSlotIndex == -1) {
				persistanceInventoryListBySaveSlotInfo currentSlot = infoListToSave [j];

				if (currentSlot.saveNumber == currentSaveNumber) {
					newPersistanceInventoryListBySaveSlotInfo = currentSlot;
					saveLocated = true;
					saveSlotIndex = j;
				}
			}
		}

		if (saveLocated) {
			int playerInventoryListCount = newPersistanceInventoryListBySaveSlotInfo.playerInventoryList.Count;

			int playerIDToSearch = inventoryListToSave.playerID;

			for (int j = 0; j < playerInventoryListCount; j++) {
				if (listIndex == -1 && newPersistanceInventoryListBySaveSlotInfo.playerInventoryList [j].playerID == playerIDToSearch) {
					playerLocated = true;
					listIndex = j;
				}
			}
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("EXTRA INFO\n");
			print ("PLAYER INVENTORY");
			print ("Number of objects: " + inventoryListToSave.inventoryObjectList.Count);
			print ("Current Save Number " + currentSaveNumber);
			print ("Save Located " + saveLocated);
			print ("Player Located " + playerLocated);
			print ("Player ID " + inventoryListToSave.playerID);

			print ("Save List Index " + listIndex);
		}

		//if the save is located, check if the player id exists
		if (saveLocated) {
			//if player id exists, overwrite it
			if (playerLocated) {
//				infoListToSave [saveSlotIndex].playerInventoryList [listIndex].inventoryObjectList = inventoryListToSave.inventoryObjectList;

				infoListToSave [saveSlotIndex].playerInventoryList [listIndex] = inventoryListToSave;
			} else {
				infoListToSave [saveSlotIndex].playerInventoryList.Add (inventoryListToSave);
			}
		} else {
			newPersistanceInventoryListBySaveSlotInfo.saveNumber = currentSaveNumber;
			newPersistanceInventoryListBySaveSlotInfo.playerInventoryList.Add (inventoryListToSave);
			infoListToSave.Add (newPersistanceInventoryListBySaveSlotInfo);
		}
			
		bf = new BinaryFormatter ();
		file = File.Open (currentSaveDataPath, FileMode.OpenOrCreate); 
		bf.Serialize (file, infoListToSave);

		file.Close ();
	}

	public void loadGameContent (int saveNumberToLoad, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		if (mainInventoryManager == null) {
			return;
		}

		if (!mainInventoryManager.loadCurrentPlayerInventoryFromSaveFile) {
			initializeValues ();

			return;
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("Loading player inventory with PLAYER ID " + playerID);
		}
	
		//need to store and check the current slot saved and the player which is saving, to get that concrete info
		persistanceInfoList = new List<inventoryListElement> ();

		List<persistanceInventoryListBySaveSlotInfo> infoListToLoad = new List<persistanceInventoryListBySaveSlotInfo> ();

		if (File.Exists (currentSaveDataPath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (currentSaveDataPath, FileMode.Open);
			object currentData = bf.Deserialize (file);
			infoListToLoad = currentData as List<persistanceInventoryListBySaveSlotInfo>;

			file.Close ();	
		}

		if (saveNumberToLoad > -1) {

			if (showDebugInfo) {
				print (infoListToLoad.Count);
			}

			persistanceInventoryListBySaveSlotInfo newPersistanceInventoryListBySaveSlotInfo = new persistanceInventoryListBySaveSlotInfo ();
		
			int infoListToLoadCount = infoListToLoad.Count;

			bool slotLocated = false;

			for (int j = 0; j < infoListToLoadCount; j++) {
				if (!slotLocated) {
					persistanceInventoryListBySaveSlotInfo currentSlot = infoListToLoad [j];

					if (currentSlot.saveNumber == saveNumberToLoad) {
						newPersistanceInventoryListBySaveSlotInfo = currentSlot;

						slotLocated = true;
					}
				}
			}

			//print ("number " + newPersistanceInventoryListBySaveSlotInfo.playerInventoryList.Count);

			int listIndex = -1;

			int playerInventoryListCount = newPersistanceInventoryListBySaveSlotInfo.playerInventoryList.Count;

			for (int j = 0; j < playerInventoryListCount; j++) {
//				print (newPersistanceInventoryListBySaveSlotInfo.playerInventoryList [j].playerID + " " +
//				newPersistanceInventoryListBySaveSlotInfo.playerInventoryList [j].inventorySlotAmount);
//				
				if (listIndex == -1 && newPersistanceInventoryListBySaveSlotInfo.playerInventoryList [j].playerID == playerID) {
					listIndex = j;
				}
			}

			//print ("index " + listIndex);

			if (listIndex > -1) {
				persistanceInventoryListByPlayerInfo playerInventoryListToLoad = newPersistanceInventoryListBySaveSlotInfo.playerInventoryList [listIndex];

				int inventoryObjectListCount = playerInventoryListToLoad.inventoryObjectList.Count;

				for (int j = 0; j < inventoryObjectListCount; j++) {
					inventoryListElement newInventoryListElement = new inventoryListElement ();

					persistanceInventoryObjectInfo currentInventorInfo = playerInventoryListToLoad.inventoryObjectList [j];

					newInventoryListElement.Name = currentInventorInfo.Name;
					newInventoryListElement.amount = currentInventorInfo.amount;
					newInventoryListElement.infiniteAmount = currentInventorInfo.infiniteAmount;
					newInventoryListElement.inventoryObjectName = currentInventorInfo.inventoryObjectName;

					newInventoryListElement.categoryIndex = currentInventorInfo.categoryIndex;
					newInventoryListElement.elementIndex = currentInventorInfo.elementIndex;

					newInventoryListElement.isEquipped = currentInventorInfo.isEquipped;

					newInventoryListElement.isWeapon = currentInventorInfo.isWeapon;
					newInventoryListElement.isMeleeWeapon = currentInventorInfo.isMeleeWeapon;

					newInventoryListElement.projectilesInMagazine = currentInventorInfo.projectilesInMagazine;

					persistanceInfoList.Add (newInventoryListElement);
				}

				mainInventoryManager.setInventorySlotAmountValue (playerInventoryListToLoad.inventorySlotAmount);

				mainInventoryManager.setInfiniteSlotsState (playerInventoryListToLoad.infiniteSlots);

				if (showDebugInfo) {
					print ("\n\n");
					print ("Number of inventory slots available: " + playerInventoryListToLoad.inventorySlotAmount);
					print ("\n\n");
				}
			}
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("PLAYER INVENTORY");

			print ("Inventory Loaded in Save Number " + saveNumberToLoad);
			print ("Number of objects: " + persistanceInfoList.Count);

			for (int j = 0; j < persistanceInfoList.Count; j++) {
				inventoryListElement currentElement = persistanceInfoList [j];

				print ("Object Name: " + currentElement.Name + " Amount: " + currentElement.amount);
		
				if (currentElement.isWeapon && !currentElement.isMeleeWeapon) {
					print ("Object is weapon with " + currentElement.projectilesInMagazine + " projectiles in the magazine");
				}
			}
		}

		loadInfoOnMainComponent ();
	}


	public persistanceInventoryListByPlayerInfo getPersistanceList (int playerID, bool showDebugInfo)
	{
		persistanceInventoryListByPlayerInfo newPersistanceInventoryListByPlayerInfo = new persistanceInventoryListByPlayerInfo ();

		newPersistanceInventoryListByPlayerInfo.playerID = playerID;
		newPersistanceInventoryListByPlayerInfo.inventorySlotAmount = mainInventoryManager.inventorySlotAmount;
		newPersistanceInventoryListByPlayerInfo.infiniteSlots = mainInventoryManager.infiniteSlots;

		if (showDebugInfo) {
			print ("\n\n");
			print ("Number of inventory slots available: " + mainInventoryManager.inventorySlotAmount);
			print ("\n\n");
		}

		List<persistanceInventoryObjectInfo> newPersistanceInventoryObjectInfoList = new List<persistanceInventoryObjectInfo> ();

		List<inventoryInfo> inventoryList = mainInventoryManager.inventoryList;

		int inventoryListCount = inventoryList.Count;

		for (int k = 0; k < inventoryListCount; k++) {
			inventoryInfo currentInventoryInfo = inventoryList [k];

			if (currentInventoryInfo.amount > 0) {
				persistanceInventoryObjectInfo newPersistanceInventoryObjectInfo = new persistanceInventoryObjectInfo ();

				newPersistanceInventoryObjectInfo.Name = currentInventoryInfo.Name;
				newPersistanceInventoryObjectInfo.amount = currentInventoryInfo.amount;
				newPersistanceInventoryObjectInfo.infiniteAmount = currentInventoryInfo.infiniteAmount;
				newPersistanceInventoryObjectInfo.inventoryObjectName = currentInventoryInfo.Name;

				newPersistanceInventoryObjectInfo.categoryIndex = currentInventoryInfo.categoryIndex;
				newPersistanceInventoryObjectInfo.elementIndex = currentInventoryInfo.elementIndex;

				newPersistanceInventoryObjectInfo.isEquipped = currentInventoryInfo.isEquiped;

				newPersistanceInventoryObjectInfo.isWeapon = currentInventoryInfo.isWeapon;
				newPersistanceInventoryObjectInfo.isMeleeWeapon = currentInventoryInfo.isMeleeWeapon;

				if (newPersistanceInventoryObjectInfo.isWeapon && !newPersistanceInventoryObjectInfo.isMeleeWeapon) {
					newPersistanceInventoryObjectInfo.projectilesInMagazine = currentInventoryInfo.mainWeaponObjectInfo.getProjectilesInWeaponMagazine ();
				}

				newPersistanceInventoryObjectInfoList.Add (newPersistanceInventoryObjectInfo);
			}
		}	

		if (showDebugInfo) {
			print ("\n\n");

			for (int j = 0; j < newPersistanceInventoryObjectInfoList.Count; j++) {
				persistanceInventoryObjectInfo currentInfo = newPersistanceInventoryObjectInfoList [j];
					
				print ("Object Name: " + currentInfo.Name + " Amount: " + currentInfo.amount + " category index " +
				currentInfo.categoryIndex + " element index " + currentInfo.elementIndex);
		
				if (currentInfo.isWeapon && !currentInfo.isMeleeWeapon) {
					print ("Object is weapon with " + currentInfo.projectilesInMagazine + " projectiles in the magazine");
				}
			}
		}

		newPersistanceInventoryListByPlayerInfo.inventoryObjectList = newPersistanceInventoryObjectInfoList;

		return newPersistanceInventoryListByPlayerInfo;
	}


	void loadInfoOnMainComponent ()
	{
		initializeValues ();

		if (persistanceInfoList != null && persistanceInfoList.Count > 0) {

			mainInventoryManager.setNewInventoryListManagerList (persistanceInfoList);
		}
	}

	void initializeValues ()
	{
		mainInventoryManager.initializeInventoryValues ();
	}


	public void saveGameContentFromEditor (int currentSaveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo)
	{
		if (mainInventoryManager == null) {
			return;
		}

		bool saveLocated = false;
		bool playerLocated = false;

		int saveSlotIndex = -1;
		int listIndex = -1;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		persistanceInventoryListByPlayerInfo inventoryListToSave = getPersistanceInventoryListElement (playerID, showDebugInfo);

		persistanceInventoryListBySaveSlotInfo newPersistanceInventoryListBySaveSlotInfo = new persistanceInventoryListBySaveSlotInfo ();

		List<persistanceInventoryListBySaveSlotInfo> infoListToSave = new List<persistanceInventoryListBySaveSlotInfo> ();

		if (File.Exists (currentSaveDataPath)) {
			bf = new BinaryFormatter ();
			file = File.Open (currentSaveDataPath, FileMode.Open);
			object currentData = bf.Deserialize (file);
			infoListToSave = currentData as List<persistanceInventoryListBySaveSlotInfo>;

			file.Close ();	
		}

		int infoListToSaveCount = infoListToSave.Count;

		for (int j = 0; j < infoListToSaveCount; j++) {
			if (saveSlotIndex == -1) {
				persistanceInventoryListBySaveSlotInfo currentSlot = infoListToSave [j];

				if (currentSlot.saveNumber == currentSaveNumber) {
					newPersistanceInventoryListBySaveSlotInfo = currentSlot;
					saveLocated = true;
					saveSlotIndex = j;
				}
			}
		}

		if (saveLocated) {

			int playerInventoryListCount = newPersistanceInventoryListBySaveSlotInfo.playerInventoryList.Count;

			int playerIDToSearch = inventoryListToSave.playerID;

			for (int j = 0; j < playerInventoryListCount; j++) {
				if (listIndex == -1 && newPersistanceInventoryListBySaveSlotInfo.playerInventoryList [j].playerID == playerIDToSearch) {
					playerLocated = true;
					listIndex = j;
				}
			}
		}

		if (showDebugInfo) {
			print ("\n\n");

			print ("EXTRA INFO\n");
			print ("PLAYER INVENTORY");

			print ("Number of objects: " + inventoryListToSave.inventoryObjectList.Count);
			print ("Current Save Number " + currentSaveNumber);
			print ("Save Located " + saveLocated);
			print ("Player Located " + playerLocated);
			print ("Player ID " + inventoryListToSave.playerID);
		}

		//if the save is located, check if the player id exists
		if (saveLocated) {
			//if player id exists, overwrite it
			if (playerLocated) {
				infoListToSave [saveSlotIndex].playerInventoryList [listIndex].inventoryObjectList = inventoryListToSave.inventoryObjectList;
			} else {
				infoListToSave [saveSlotIndex].playerInventoryList.Add (inventoryListToSave);
			}
		} else {
			newPersistanceInventoryListBySaveSlotInfo.saveNumber = currentSaveNumber;
			newPersistanceInventoryListBySaveSlotInfo.playerInventoryList.Add (inventoryListToSave);
			infoListToSave.Add (newPersistanceInventoryListBySaveSlotInfo);
		}

		bf = new BinaryFormatter ();
		file = File.Create (currentSaveDataPath); 
		bf.Serialize (file, infoListToSave);
		file.Close ();
	}

	public persistanceInventoryListByPlayerInfo getPersistanceInventoryListElement (int playerID, bool showDebugInfo)
	{
		persistanceInventoryListByPlayerInfo newPersistanceInventoryListByPlayerInfo = new persistanceInventoryListByPlayerInfo ();

		newPersistanceInventoryListByPlayerInfo.playerID = playerID;
		
		List<persistanceInventoryObjectInfo> newPersistanceInventoryObjectInfoList = new List<persistanceInventoryObjectInfo> ();

		List<inventoryListElement> inventoryListManagerList = mainInventoryManager.inventoryListManagerList;

		int inventoryListManagerListCount = inventoryListManagerList.Count;

		for (int k = 0; k < inventoryListManagerListCount; k++) {
			persistanceInventoryObjectInfo newPersistanceInventoryObjectInfo = new persistanceInventoryObjectInfo ();
		
			inventoryListElement currentElement = inventoryListManagerList [k];

			newPersistanceInventoryObjectInfo.Name = currentElement.Name;
			newPersistanceInventoryObjectInfo.amount = currentElement.amount;
			newPersistanceInventoryObjectInfo.infiniteAmount = currentElement.infiniteAmount;
			newPersistanceInventoryObjectInfo.inventoryObjectName = currentElement.inventoryObjectName;
		
			newPersistanceInventoryObjectInfo.elementIndex = currentElement.elementIndex;
			newPersistanceInventoryObjectInfo.categoryIndex = currentElement.categoryIndex;
		
			if (showDebugInfo) {
				print (newPersistanceInventoryObjectInfo.Name + " " + newPersistanceInventoryObjectInfo.amount);
			}

			newPersistanceInventoryObjectInfoList.Add (newPersistanceInventoryObjectInfo);
		}	
		
		newPersistanceInventoryListByPlayerInfo.inventoryObjectList = newPersistanceInventoryObjectInfoList;
		
		return newPersistanceInventoryListByPlayerInfo;
	}
}
