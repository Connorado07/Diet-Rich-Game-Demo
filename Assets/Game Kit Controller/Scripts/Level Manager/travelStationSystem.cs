using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class travelStationSystem : MonoBehaviour
{
	public int sceneNumberToLoad;

	public int levelManagerIDToLoad;

	public bool allStationsUnlocked;

	public List<travelStationInfo> travelStationInfoList = new List<travelStationInfo> ();

	public bool usingTravelStation;

	GameObject currentPlayer;

	playerComponentsManager mainPlayerComponentsManager;

	travelStationUISystem currentTravelStationUISystem;

	public void setCurrentPlayer (GameObject player)
	{
		currentPlayer = player;

		if (currentPlayer != null) {

			mainPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			currentTravelStationUISystem = mainPlayerComponentsManager.getTravelStationUISystem ();

			currentTravelStationUISystem.setCurrentTravelStationSystem (this, true);
		}
	}

	public void activateTravelStation ()
	{
		usingTravelStation = !usingTravelStation;

		currentTravelStationUISystem.openOrCloseTravelStationMenu (usingTravelStation);
	}

	public void setUsingTravelStationSystemState (bool state)
	{
		usingTravelStation = state;
	}

	public List<travelStationInfo> getTravelStationInfoList ()
	{
		return travelStationInfoList;
	}

	[System.Serializable]
	public class travelStationInfo
	{
		public string Name;

		public int sceneNumberToLoad;

		public int levelManagerIDToLoad;

		public bool zoneFound;
	}
}
