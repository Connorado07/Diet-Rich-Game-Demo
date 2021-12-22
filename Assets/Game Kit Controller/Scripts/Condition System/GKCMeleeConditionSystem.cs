using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKCMeleeConditionSystem : GKCConditionInfo
{
	[Header ("Custom Settings")]
	[Space]

	public bool checkIfCarryinWeapon;
	public string weaponCarriedName;

	public bool checkIfWeaponOnHand;

	public bool checkIfWeaponSecondaryAbilityActive;

	public override void checkIfConditionComplete ()
	{
		if (!checkIfPlayerAssigned ()) {
			return;
		}

		bool conditionResult = false;

		playerComponentsManager mainPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

		if (mainPlayerComponentsManager != null) {
			grabbedObjectMeleeAttackSystem mainGrabbedObjectMeleeAttackSystem = mainPlayerComponentsManager.getGrabbedObjectMeleeAttackSystem ();
		
			if (mainGrabbedObjectMeleeAttackSystem != null) {
				bool currentConditionState = true;

				if (checkIfCarryinWeapon) {
					if (!mainGrabbedObjectMeleeAttackSystem.getCurrentMeleeWeaponName ().Equals (weaponCarriedName)) {
						currentConditionState = false;
					}
				}

				if (checkIfWeaponOnHand) {
					if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
						currentConditionState = false;
					}
				}

				if (checkIfWeaponSecondaryAbilityActive) {
					if (!mainGrabbedObjectMeleeAttackSystem.isSecondaryAbilityActiveOnCurrentWeapon ()) {
						currentConditionState = false;
					}
				}

				conditionResult = currentConditionState;
			}
		}

		setConditionResult (conditionResult);
	}
}
