using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class throwGrenadeSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool throwGrenadeEnabled = true;

	public string grenadeInventoryObjectName = "Grenade";

	[Space]
	[Header ("Events Settings")]
	[Space]

	public UnityEvent eventOnStartThrowGrenade;

	public UnityEvent eventOnConfirmThrowGrenade;

	[Space]
	[Header ("Components")]
	[Space]

	public GameObject grenadeInfoPanel;

	public Text grenadeAmountText;

	public inventoryManager mainInventoryManager;


	bool canConfirmThrowGrenade;

	int currentGrenadeAmount;


	void Start ()
	{
		checkGrenadesInfo ();
	}

	public void checkGrenadesInfo ()
	{
		if (throwGrenadeEnabled) {
			updateGrenadeAmountText ();
		} else {
			if (grenadeInfoPanel.activeSelf != false) {
				grenadeInfoPanel.SetActive (false);
			}
		}
	}

	public void inputStartThrowGrenade ()
	{
		if (!throwGrenadeEnabled) {
			return;
		}

		canConfirmThrowGrenade = false;

		currentGrenadeAmount = mainInventoryManager.getInventoryObjectAmountByName (grenadeInventoryObjectName);

		if (currentGrenadeAmount > 0) {

			canConfirmThrowGrenade = true;

			eventOnStartThrowGrenade.Invoke ();
		}
	}


	public void inputConfirmThrowGrenade ()
	{
		if (!throwGrenadeEnabled) {
			return;
		}

		if (canConfirmThrowGrenade) {
			eventOnConfirmThrowGrenade.Invoke ();

			updateGrenadeAmountText ();
		}
	}

	public void updateGrenadeAmountText ()
	{
		if (!throwGrenadeEnabled) {
			return;
		}

		currentGrenadeAmount = mainInventoryManager.getInventoryObjectAmountByName (grenadeInventoryObjectName);

		if (grenadeAmountText != null) {
			if (currentGrenadeAmount < 0) {
				currentGrenadeAmount = 0;
			}

			grenadeAmountText.text = currentGrenadeAmount.ToString ();
		}
	}
}
