using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class setCanvasParentSystem : MonoBehaviour
{
	public RectTransform panelToChange;

	public RectTransform newPanelParent;

	public bool setNewPanelScale;
	public Vector3 newPanelScale;

	public bool useEventOnParentChanged;

	public UnityEvent eventOnParentChanged;

	public void activateParentChange ()
	{
		panelToChange.SetParent (newPanelParent);

		panelToChange.localPosition = Vector3.zero;

		panelToChange.localRotation = Quaternion.identity;

		if (setNewPanelScale) {
			panelToChange.localScale = newPanelScale;
		} else {
			panelToChange.localScale = Vector3.one;
		}

		if (useEventOnParentChanged) {
			eventOnParentChanged.Invoke ();
		}
	}
}
