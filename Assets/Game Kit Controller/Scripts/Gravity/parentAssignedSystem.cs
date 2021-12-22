using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parentAssignedSystem : MonoBehaviour
{
	public GameObject parentGameObject;

	public void assignParent (GameObject newParent)
	{
		parentGameObject = newParent;
	}

	public GameObject getAssignedParent ()
	{
		if (parentGameObject == null) {
			parentGameObject = gameObject;
		}

		return parentGameObject;
	}

	public Transform getAssignedParentTransform ()
	{
		if (parentGameObject != null) {
			return parentGameObject.transform;
		}

		return null;
	}
}
