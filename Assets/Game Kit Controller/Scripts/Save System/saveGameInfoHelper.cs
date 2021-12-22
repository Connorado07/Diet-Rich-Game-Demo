using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class saveGameInfoHelper : MonoBehaviour
{
	public saveGameSystem mainSaveGameSystem;

	public UnityEvent eventOnHomeMenuOpened;

	public UnityEvent eventOnGameSavesLocated;
	public UnityEvent eventOnGameSavesNotLocated;

	void Start ()
	{
		checkIfGameCanContinue ();
	}

	public void checkIfGameCanContinue ()
	{
		eventOnHomeMenuOpened.Invoke ();

		mainSaveGameSystem.startGameSystem ();

		List<saveGameSystem.saveStationInfo> saveList = mainSaveGameSystem.loadFile ();

		if (saveList.Count > 0) {
			eventOnGameSavesLocated.Invoke ();
		} else {
			eventOnGameSavesNotLocated.Invoke ();
		}
	}
}
