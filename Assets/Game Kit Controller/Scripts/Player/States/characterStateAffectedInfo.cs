using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStateAffectedInfo : MonoBehaviour
{
	public bool stateEnabled = true;

	public string stateAffectedName;

	public bool stateAffectedActive;

	public virtual void activateStateAffected (float stateDuration, float stateAmount)
	{

	}

	public bool isStateAffectedActive ()
	{
		return stateAffectedActive;
	}
}
