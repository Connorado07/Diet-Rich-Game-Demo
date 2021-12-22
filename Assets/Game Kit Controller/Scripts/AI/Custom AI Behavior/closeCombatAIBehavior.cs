﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeCombatAIBehavior : AIBehaviorInfo
{
	public AICloseCombatSystemBrain mainAICloseCombatSystemBrain;

	public override void updateAI ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAICloseCombatSystemBrain.updateAI ();
	}

	public override void updateAIBehaviorState ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAICloseCombatSystemBrain.updateMainCloseCombatBehavior ();
	}

	public override void updateAIAttackState (bool canUseAttack)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAICloseCombatSystemBrain.updateMainCloseCombatAttack (canUseAttack);
	}

	public override void updateInsideRangeDistance (bool state)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAICloseCombatSystemBrain.updateInsideMinDistance (state);
	}

	public override void resetBehaviorStates ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAICloseCombatSystemBrain.resetBehaviorStates ();
	}
}
