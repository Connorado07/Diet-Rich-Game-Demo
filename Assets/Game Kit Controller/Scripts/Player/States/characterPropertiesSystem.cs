using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterPropertiesSystem : MonoBehaviour
{
	public bool characeterStatesAffectedEnabled = true;

	public List<characterStateAffectedInfo> characterStateAffectedInfoList = new List<characterStateAffectedInfo> ();

	public void activateStateAffected (string stateName, float stateDuration, float stateAmount)
	{
		if (!characeterStatesAffectedEnabled) {
			return;
		}

		for (int i = 0; i < characterStateAffectedInfoList.Count; i++) {
			if (characterStateAffectedInfoList [i].stateAffectedName.Equals (stateName) && characterStateAffectedInfoList [i].stateEnabled) {
				characterStateAffectedInfoList [i].activateStateAffected (stateDuration, stateAmount);
			}
		}
	}

	public characterStateAffectedInfo getCharacterStateAffectedInfoByName (string stateName)
	{
		for (int i = 0; i < characterStateAffectedInfoList.Count; i++) {
			if (characterStateAffectedInfoList [i].stateAffectedName.Equals (stateName)) {
				return characterStateAffectedInfoList [i];
			}
		}

		return null;
	}

	public GameObject getCharacterStateAffectedInfoGameObjectByName (string stateName)
	{
		for (int i = 0; i < characterStateAffectedInfoList.Count; i++) {
			if (characterStateAffectedInfoList [i].stateAffectedName.Equals (stateName)) {
				return characterStateAffectedInfoList [i].gameObject;
			}
		}

		return null;
	}
}
