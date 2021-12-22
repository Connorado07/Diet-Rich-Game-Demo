using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powersAIBehavior : AIBehaviorInfo
{
	public otherPowers mainOtherPowers;

	public override void updateAI ()
	{
		if (!behaviorEnabled) {
			return;
		}

//		mainOtherPowers.updateAI ();
	}

	public override void setAimWeaponState (bool state)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainOtherPowers.inputSetAimPowerState (state);
	}

	public override void setShootWeaponState (bool state)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainOtherPowers.inputHoldOrReleaseShootPower (state);
	}

	public override void setHoldShootWeaponState (bool state)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainOtherPowers.inputHoldShootPower ();
	}

	public override void resetBehaviorStates ()
	{
		if (!behaviorEnabled) {
			return;
		}

	
	}
}
