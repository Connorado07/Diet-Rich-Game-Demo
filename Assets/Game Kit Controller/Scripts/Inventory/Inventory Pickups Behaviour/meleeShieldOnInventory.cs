using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeShieldOnInventory : objectOnInventory
{
	[Header ("Custom Settings")]
	[Space]

	public meleeShieldPickup mainMeleeShieldPickup;

	//	string weaponName;

	public override void activateUseObjectActionOnInventory (GameObject currentPlayer, int amountToUse)
	{
		playerComponentsManager mainPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

		if (mainPlayerComponentsManager != null) {
			meleeWeaponsGrabbedManager mainMeleeWeaponsGrabbedManager = mainPlayerComponentsManager.getMeleeWeaponsGrabbedManager ();

			if (mainMeleeWeaponsGrabbedManager != null) {

				mainMeleeWeaponsGrabbedManager.toggleDrawOrSheatheShield (mainMeleeShieldPickup.shieldName);
			}
		}
	}

	public override void eventOnPickObject (GameObject currentPlayer)
	{
		//		weaponName = mainMeleeWeaponPickup.weaponName;


	}

	public override void eventOnDropObject (GameObject currentPlayer)
	{
		//		weaponName = mainMeleeWeaponPickup.weaponName;


	}

	public override bool setObjectEquippedStateOnInventory (GameObject currentPlayer, bool state)
	{
		//		weaponName = mainMeleeWeaponPickup.weaponName;
		playerComponentsManager mainPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

		if (mainPlayerComponentsManager != null) {
			meleeWeaponsGrabbedManager mainMeleeWeaponsGrabbedManager = mainPlayerComponentsManager.getMeleeWeaponsGrabbedManager ();

			if (mainMeleeWeaponsGrabbedManager != null) {

				if (state) {
					return mainMeleeWeaponsGrabbedManager.equipShield (mainMeleeShieldPickup.shieldName);
				} else {
					return mainMeleeWeaponsGrabbedManager.unequipeShield (mainMeleeShieldPickup.shieldName);
				}
			}
		}

		return false;
	}
}