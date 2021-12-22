using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class eventObjectFoundOnRaycastSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool checkObjectsEnabled = true;

	public Transform raycastTransform;
	public float rayDistanceToCheckObjectFound;
	public LayerMask layerToCheckObjectFound;

	[Space]
	[Header ("Event Settings")]
	[Space]

	public bool useEventToCallObjecObjectDetected;
	public UnityEvent eventToCallOnObjectDetected;

	RaycastHit hit;

	public void checkObjectWithRaycast ()
	{
		if (checkObjectsEnabled && raycastTransform != null) {
			if (Physics.Raycast (raycastTransform.position, raycastTransform.forward, out hit, rayDistanceToCheckObjectFound, layerToCheckObjectFound)) {
				eventObjectFoundOnCaptureSystem currentEventObjectFoundOnCaptureSystem = hit.collider.gameObject.GetComponent<eventObjectFoundOnCaptureSystem> ();

				if (currentEventObjectFoundOnCaptureSystem != null) {
					currentEventObjectFoundOnCaptureSystem.callEventOnCapture ();
				}

				if (useEventToCallObjecObjectDetected) {
					eventToCallOnObjectDetected.Invoke ();
				}
			}
		}
	}

	public void setRaycastTransform (Transform newObject)
	{
		raycastTransform = newObject;
	}
}
