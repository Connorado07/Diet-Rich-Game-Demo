using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class simpleEventSystem : MonoBehaviour
{
	public bool useEventsOnActivateAndDisabled = true;

	public UnityEvent eventToCallOnActivate = new UnityEvent ();
	public UnityEvent eventToCallOnDisable = new UnityEvent ();

	public UnityEvent regularEventToCall;

	public bool useDelayToEvent;
	public float delayToEvent;

	public bool activated;
	public bool callOnStart;

	Coroutine eventCoroutine;

	void Start ()
	{
		if (callOnStart) {
			activateDevice ();
		}
	}

	public void activateDevice ()
	{
		if (useDelayToEvent) {
			if (eventCoroutine != null) {
				StopCoroutine (eventCoroutine);
			}

			eventCoroutine = StartCoroutine (activateDeviceCoroutine ());
		} else {
			callEvent ();
		}
	}

	IEnumerator activateDeviceCoroutine ()
	{
		yield return new WaitForSeconds (delayToEvent);

		callEvent ();
	}

	public void callEvent ()
	{
		if (useEventsOnActivateAndDisabled) {
			activated = !activated;

			if (activated) {
				eventToCallOnActivate.Invoke ();
			} else {
				eventToCallOnDisable.Invoke ();
			}
		} else {
			regularEventToCall.Invoke ();
		}
	}
}
