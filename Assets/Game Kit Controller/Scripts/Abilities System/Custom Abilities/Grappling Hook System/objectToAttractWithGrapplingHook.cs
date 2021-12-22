using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class objectToAttractWithGrapplingHook : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool attractObjectEnabled = true;
	public bool attractingObjectActive;

	public bool enableGravityOnDeactivateAttract = true;

	public bool useReducedVelocityOnDisableAttract;
	public float maxReducedSpeed;
	public float reducedSpeedDuration;
	public float newSpeedAfterReducedDurationMultiplier = 1;

	[Space]
	[Header ("Rigidbody Elements")]
	[Space]

	public Rigidbody mainRigidbody;

	public bool useRigidbodyList;
	public List<Rigidbody> rigidbodyList = new List<Rigidbody> ();

	[Space]
	[Header ("Custom Settings")]
	[Space]

	public float customMinDistanceToStopAttractObject = 5;

	public bool useCustomForceAttractionValues;
	public bool customAddUpForceForAttraction;
	public float customUpForceForAttraction;
	public float customAddUpForceForAttractionDuration;

	[Space]
	[Header ("Event Settings")]
	[Space]

	public bool useEventsOnAttractState;
	public UnityEvent eventOnActivateAttract;
	public UnityEvent eventOnDeactivateAttract;

	public bool useEventAfterReduceSpeed;
	public UnityEvent eventAfterReduceSpeed;

	Coroutine reduceSpeedCoroutine;

	bool attractionHookRemovedByDistance;

	bool reducedSpeedInProcess;

	public void setAttractionHookRemovedByDistanceState (bool state)
	{
		attractionHookRemovedByDistance = state;
	}

	public bool setAttractObjectState (bool state)
	{
		if (attractObjectEnabled) {
			attractingObjectActive = state;

			if (enableGravityOnDeactivateAttract) {
				if (useRigidbodyList) {
					for (int i = 0; i < rigidbodyList.Count; i++) {
						if (rigidbodyList [i]) {
							rigidbodyList [i].useGravity = !state;
						}
					}
				} else {
					mainRigidbody.useGravity = !state;
				}
			}

			if (useEventsOnAttractState) {
				if (attractingObjectActive) {
					eventOnActivateAttract.Invoke ();
				} else {
					eventOnDeactivateAttract.Invoke ();
				}
			}

			if (!attractingObjectActive) {
				checkToReduceSpeed ();
			}

			return attractingObjectActive;
		} else {
			return false;
		}
	}

	public Rigidbody getRigidbodyToAttract ()
	{
		return mainRigidbody;
	}

	public void checkToReduceSpeed ()
	{
		if (useReducedVelocityOnDisableAttract) {
			stopCheckToReduceSpeed ();

			reduceSpeedCoroutine = StartCoroutine (checkToReduceSpeedCoroutine ());
		}
	}

	public void stopCheckToReduceSpeed ()
	{
		if (reduceSpeedCoroutine != null) {
			StopCoroutine (reduceSpeedCoroutine);
		}
	}

	Vector3 speedDirection;
	Vector3 previousAngularVelocity;
	float previousSpeed;

	IEnumerator checkToReduceSpeedCoroutine ()
	{
		if (!attractionHookRemovedByDistance) {
			yield return null;
		} else {
			reducedSpeedInProcess = true;

			previousSpeed = mainRigidbody.velocity.magnitude;

			speedDirection = mainRigidbody.velocity.normalized;

			previousAngularVelocity = mainRigidbody.angularVelocity;

			float t = 0;

			bool targetReached = false;

			while (!targetReached) {
				t += Time.deltaTime;

				if (t >= reducedSpeedDuration) {
					targetReached = true;
				}
				
				if (useRigidbodyList) {
					for (int i = 0; i < rigidbodyList.Count; i++) {
						if (rigidbodyList [i]) {
							rigidbodyList [i].velocity = speedDirection * maxReducedSpeed;

							rigidbodyList [i].angularVelocity = previousAngularVelocity * maxReducedSpeed;
						}
					}
				} else {
					mainRigidbody.velocity = speedDirection * maxReducedSpeed;

					mainRigidbody.angularVelocity = previousAngularVelocity * maxReducedSpeed;
				}

				yield return null;
			}

			resumeSpeed ();
		}

		if (useEventAfterReduceSpeed) {
			eventAfterReduceSpeed.Invoke ();
		}

		attractionHookRemovedByDistance = false;

		reducedSpeedInProcess = false;
	}

	public void stopGrapplingHookAndResumeValuesWithoutForce ()
	{
		speedDirection = Vector3.zero;
		previousAngularVelocity = Vector3.zero;
		previousSpeed = 0;

		stopGrapplingHookAndResumeValues ();
	}

	public void stopGrapplingHookAndResumeValues ()
	{
		if (!reducedSpeedInProcess) {
			return;
		}

		stopCheckToReduceSpeed ();

		resumeSpeed ();

		if (useEventAfterReduceSpeed) {
			eventAfterReduceSpeed.Invoke ();
		}

		attractionHookRemovedByDistance = false;

		reducedSpeedInProcess = false;
	}

	void resumeSpeed ()
	{
		if (useRigidbodyList) {
			for (int i = 0; i < rigidbodyList.Count; i++) {
				if (rigidbodyList [i]) {
					rigidbodyList [i].velocity = speedDirection * (previousSpeed * newSpeedAfterReducedDurationMultiplier);
				}
			}
		} else {
			mainRigidbody.velocity = speedDirection * (previousSpeed * newSpeedAfterReducedDurationMultiplier);
			mainRigidbody.angularVelocity = previousAngularVelocity * newSpeedAfterReducedDurationMultiplier;
		}

		speedDirection = Vector3.zero;
		previousAngularVelocity = Vector3.zero;
		previousSpeed = 0;
	}

	public void searchRigidbodyElements ()
	{
		useRigidbodyList = true;

		rigidbodyList.Clear ();

		mainRigidbody = null;

		Component[] childrens = transform.GetComponentsInChildren (typeof(Rigidbody));
		foreach (Rigidbody child in childrens) {
			if (child.transform != transform) {
				Collider currentCollider = child.GetComponent<Collider> ();

				if (currentCollider && !currentCollider.isTrigger) {
					if (!mainRigidbody) {
						mainRigidbody = child;
					}

					rigidbodyList.Add (child);
				}
			}
		}

		updateComponent ();
	}

	public void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}
}
