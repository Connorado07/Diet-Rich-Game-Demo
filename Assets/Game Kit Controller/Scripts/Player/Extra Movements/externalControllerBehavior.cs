using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class externalControllerBehavior : MonoBehaviour
{
	[Header ("Behavior Main Settings")]
	[Space]

	public bool externalControllerJumpEnabled;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool behaviorCurrentlyActive;

	public virtual void updateControllerBehavior ()
	{

	}

	public virtual bool isCharacterOnGround ()
	{

		return false;
	}

	public virtual bool isBehaviorActive ()
	{

		return false;
	}

	public virtual void setExternalForceActiveState (bool state)
	{
		
	}

	public virtual void setExternalForceEnabledState (bool state)
	{

	}

	public virtual void updateExternalForceActiveState (Vector3 forceDirection, float forceAmount)
	{

	}

	public virtual void checkIfActivateExternalForce ()
	{

	}

	public virtual void setJumpActiveForExternalForce ()
	{

	}

	public virtual void setExtraImpulseForce (Vector3 forceAmount, bool useCameraDirection)
	{
		
	}

	public virtual void disableExternalControllerState ()
	{
		
	}

	public virtual void checkIfResumeExternalControllerState ()
	{

	}
}
