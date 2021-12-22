using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class getObjectFromInventorySystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public string inventoryObjectName;

	public bool useInfiniteObjects;

	[Space]
	[Header ("Events Settings")]
	[Space]

	public UnityEvent eventOnObjectFoundOnInventory;
	public UnityEvent eventOnObjectNotFoundOnInventory;

	public UnityEvent eventOnAmountAvailable;
	public UnityEvent eventOnAmountNotAvailable;

	public inventoryManager mainInventoryManager;

	public void checkIfObjectFoundOnInventory ()
	{
		if (mainInventoryManager == null) {
			return;
		}

		int remainAmount = mainInventoryManager.getInventoryObjectAmountByName (inventoryObjectName);

		if (useInfiniteObjects) {
			remainAmount = 1;
		}

		if (remainAmount > 0) {
			eventOnObjectFoundOnInventory.Invoke ();
		} else {
			eventOnObjectNotFoundOnInventory.Invoke ();
		}
	}

	public void useInventoryObject (int amountToUse)
	{
		if (mainInventoryManager == null) {
			return;
		}

		int remainAmount = mainInventoryManager.getInventoryObjectAmountByName (inventoryObjectName);

		if (useInfiniteObjects) {
			remainAmount = 1;
		}

		if (remainAmount >= amountToUse) {
			mainInventoryManager.removeObjectAmountFromInventoryByName (inventoryObjectName, amountToUse);

			eventOnAmountAvailable.Invoke ();
		} else {
			eventOnAmountNotAvailable.Invoke ();
		}
	}

	public void setCurrentPlayer (GameObject newPlayer)
	{
		if (newPlayer != null) {
			playerComponentsManager currentPlayerComponentsManager = newPlayer.GetComponent<playerComponentsManager> ();

			if (currentPlayerComponentsManager != null) {
				mainInventoryManager = currentPlayerComponentsManager.getInventoryManager ();
			}
		}
	}

	public void setNewInventoryObjectName (string newName)
	{
		inventoryObjectName = newName;
	}
}
