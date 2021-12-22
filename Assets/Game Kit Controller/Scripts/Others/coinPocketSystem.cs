using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class coinPocketSystem : MonoBehaviour
{
	public int currentCointAmount;

	public bool pickCoinsEnabled = true;

	public UnityEvent eventOnDropAllCoins;

	public eventParameters.eventToCallWithInteger eventToSetAmountOfCoins;

	public int getCoinAmount ()
	{
		return currentCointAmount;
	}

	public void addCoinAmount (int newAmount)
	{
		currentCointAmount += newAmount;
	}

	public void dropAllCoins ()
	{
		if (currentCointAmount > 0) {
			eventToSetAmountOfCoins.Invoke (currentCointAmount);

			eventOnDropAllCoins.Invoke ();

			currentCointAmount = 0;
		}
	}

	public bool canPickCoins ()
	{
		return pickCoinsEnabled;
	}

	public void setPickCoinsEnabledState (bool state)
	{
		pickCoinsEnabled = state;
	}
}
