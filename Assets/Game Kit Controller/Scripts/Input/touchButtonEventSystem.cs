using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System;

[System.Serializable]
//System to use touch buttons to check if the button is being pressing or released
public class touchButtonEventSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[Header ("Main Settings")]
	[Space]

	public bool eventEnabled;
	public UnityEvent eventOnPressDown;
	public UnityEvent eventOnPressUp;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool buttonPressed;

	//if you press the button
	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventEnabled) {
			eventOnPressDown.Invoke ();

			buttonPressed = true;
		}
	}

	//if you release the button
	public void OnPointerUp (PointerEventData eventData)
	{
		if (eventEnabled) {
			eventOnPressUp.Invoke ();

			buttonPressed = false;
		}
	}

	//if the button is disabled, reset the button
	void OnDisable ()
	{
		if (eventEnabled && buttonPressed) {
			eventOnPressUp.Invoke ();
		}
	}
}