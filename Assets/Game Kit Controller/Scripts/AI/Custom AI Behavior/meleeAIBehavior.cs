using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAIBehavior : AIBehaviorInfo
{
	public AIMeleeCombatSystemBrain mainAIMeleeCombatSystemBrain;

	public override void updateAI ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIMeleeCombatSystemBrain.updateAI ();
	}

	public override void updateAIBehaviorState ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIMeleeCombatSystemBrain.updateMainMeleeBehavior ();
	}

	public override void updateAIAttackState (bool canUseAttack)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIMeleeCombatSystemBrain.updateMainMeleeAttack (canUseAttack);
	}

	public override void updateInsideRangeDistance (bool state)
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIMeleeCombatSystemBrain.updateInsideMinDistance (state);
	}

	public override void resetBehaviorStates ()
	{
		if (!behaviorEnabled) {
			return;
		}

		mainAIMeleeCombatSystemBrain.resetBehaviorStates ();
	}
}
