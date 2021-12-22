using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsAIBehavior : AIBehaviorInfo
{
	public AIFireWeaponsSystemBrain mainAIFireWeaponsSystemBrain;

	public override void updateAI ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.updateAI ();
	}

	public override void updateAIBehaviorState ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.updateMainFireWeaponsBehavior ();
	}

	public override void updateAIAttackState (bool canUseAttack)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.updateMainFireWeaponsAttack (canUseAttack);
	}

	public override void updateInsideRangeDistance (bool state)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.updateInsideMinDistance (state);
	}

	public override void resetBehaviorStates ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.resetBehaviorStates ();
	}

	//	public override void setDrawOrHolsterWeaponState (bool state)
	//	{
	//		if (!behaviorEnabled) {
	//			return;
	//		}
	//
	//		if (state) {
	//			mainPlayerWeaponsManager.drawCurrentWeaponWhenItIsReady (true);
	//		} else {
	//			mainPlayerWeaponsManager.drawOrKeepWeapon (false);
	//		}
	//	}
	//
	//	public override void setAimWeaponState (bool state)
	//	{
	//		if (!behaviorEnabled) {
	//			return;
	//		}
	//
	//		if (state) {
	//			mainPlayerWeaponsManager.aimCurrentWeaponWhenItIsReady (true);
	//		} else {
	//			mainPlayerWeaponsManager.stopAimCurrentWeaponWhenItIsReady (true);
	//		}
	//	}
	//
	//	public override void setShootWeaponState (bool state)
	//	{
	//		if (!behaviorEnabled) {
	//			return;
	//		}
	//
	//		mainPlayerWeaponsManager.shootWeapon (state);
	//	}

	public override void dropWeapon ()
	{
		if (!behaviorEnabled) {
			return;
		}

//		mainPlayerWeaponsManager.dropWeaponByBebugButton ();

		mainAIFireWeaponsSystemBrain.dropWeapon ();
	}

	public override void resetAttackState ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.resetAttackState ();
	}

	public override void stopAim ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.stopAim ();
	}

	public override void checkIfDrawWeaponsWhenResumingAI ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.checkIfDrawWeaponsWhenResumingAI ();
	}

	public override void disableOnSpottedState ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.disableOnSpottedState ();
	}

	public override void updateWeaponsAvailableState ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIFireWeaponsSystemBrain.updateWeaponsAvailableState ();
	}
}
