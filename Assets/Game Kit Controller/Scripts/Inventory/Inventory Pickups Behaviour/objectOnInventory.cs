using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectOnInventory : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public inventoryObject mainInventoryObject;

	public bool useOnlyAmountNeeded;

	public bool closeInventoryOnObjectUsed;

	[Space]
	[Header ("Remote Events Settings")]
	[Space]

	public bool useRemoteEvent;

	[Space]

	public List<string> remoteEventList = new List<string> ();

	[Space]
	[Header ("Enable Abilities on Use Inventory Object Settings")]
	[Space]

	public bool useAbilitiesListToEnableOnUseInventoryObject;

	[Space]

	public List<string> abilitiesListToEnableOnUseInventoryObject = new List<string> ();

	[Space]
	[Header ("Activate Abilities on Use Inventory Object Settings")]
	[Space]

	public bool activateAbilityOnUseInventoryObject;

	[Space]

	public string abilityNameToActiveOnUseInventoryObject;
	public bool abilityIsTemporallyActivated;

	public bool checkIfAbilityIsNotActiveOrOnCoolDown;

	public virtual void activateUseObjectActionOnInventory (GameObject currentPlayer, int amountToUse)
	{
		
	}

	public virtual void activateCombineObjectActionOnInventory (GameObject currentPlayer, inventoryInfo inventoryInfoToUse)
	{

	}

	public virtual void carryPhysicalObjectFromInventory (GameObject currentPlayer)
	{

	}

	public virtual void eventOnPickObject (GameObject currentPlayer)
	{

	}

	public virtual void eventOnDropObject (GameObject currentPlayer)
	{

	}

	public virtual void checkRemoteEvents (GameObject currentPlayer)
	{
		if (useRemoteEvent) {
			playerComponentsManager currentPlayerComponetsManager = currentPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponetsManager != null) {
				remoteEventSystem currentRemoteEventSystem = currentPlayerComponetsManager.getRemoteEventSystem ();

				if (currentRemoteEventSystem != null) {
					for (int i = 0; i < remoteEventList.Count; i++) {

						currentRemoteEventSystem.callRemoteEvent (remoteEventList [i]);
					}
				}
			}
		}
	}

	public virtual bool setObjectEquippedStateOnInventory (GameObject currentPlayer, bool state)
	{

		return false;
	}

	//	public virtual bool isObjectEquipped ()
	//	{
	//
	//		return false;
	//	}
	//
	//	public virtual void updateObjectState ()
	//	{
	//
	//	}

	public virtual void checkIfEnableAbilitiesOnUseInventoryObject (GameObject currentPlayer)
	{
		if (useAbilitiesListToEnableOnUseInventoryObject && currentPlayer != null) {
			GKC_Utils.enableOrDisableAbilityGroupByName (currentPlayer.transform, false, abilitiesListToEnableOnUseInventoryObject);
		}
	}

	public virtual void checkIfActivateAbilitiesOnUseInventoryObject (GameObject currentPlayer)
	{
		if (activateAbilityOnUseInventoryObject && currentPlayer != null) {
			GKC_Utils.activateAbilityByName (currentPlayer.transform, abilityNameToActiveOnUseInventoryObject, abilityIsTemporallyActivated);
		}
	}

	public virtual bool checkIfAbilitiesOnUseOrCooldown (GameObject currentPlayer)
	{
		if (activateAbilityOnUseInventoryObject && currentPlayer != null) {
			return GKC_Utils.checkIfAbilitiesOnUseOrCooldown (currentPlayer.transform, abilityNameToActiveOnUseInventoryObject);
		}

		return false;
	}
}
