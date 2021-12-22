using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setMissionSubObjectiveStateRemotelySystem : MonoBehaviour
{
	public int missionID;

	public string subObjectiveName;

	public void addSubObjectiveCompleteRemotely (string customSubObjectiveName)
	{
		sendMissionInfo (customSubObjectiveName);
	}

	public void addSubObjectiveCompleteRemotely ()
	{
		sendMissionInfo (subObjectiveName);
	}

	public void sendMissionInfo (string newSubObjectiveName)
	{
		objectiveManager mainObjectiveManager = FindObjectOfType<objectiveManager> ();

		if (mainObjectiveManager != null) {
			mainObjectiveManager.addSubObjectiveCompleteRemotely (newSubObjectiveName, missionID);
		}
	}

	public void setMissionID (int newValue)
	{
		missionID = newValue;
	}

	public void setSubObjectiveName (string newValue)
	{
		subObjectiveName = newValue;
	}
}
