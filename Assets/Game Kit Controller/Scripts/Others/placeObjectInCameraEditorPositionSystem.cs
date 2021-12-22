using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeObjectInCameraEditorPositionSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public Vector3 positionOffset;

	public float rotationAmount = 90;

	public LayerMask layerToPlaceElements;

	[Space]
	[Header ("Object Settings")]
	[Space]

	public GameObject objectToSelect;

	public List<Transform> objectsToMoveList = new List<Transform> ();

	RaycastHit hit;

	public void moveObjects ()
	{
		Camera currentCameraEditor = GKC_Utils.getCameraEditor ();

		if (currentCameraEditor != null) {
			Vector3 editorCameraPosition = currentCameraEditor.transform.position;
			Vector3 editorCameraForward = currentCameraEditor.transform.forward;

			RaycastHit hit;

			if (Physics.Raycast (editorCameraPosition, editorCameraForward, out hit, Mathf.Infinity, layerToPlaceElements)) {

				Vector3 positionToMove = hit.point + Vector3.right * positionOffset.x + Vector3.up * positionOffset.y + Vector3.forward * positionOffset.z;

				for (int i = 0; i < objectsToMoveList.Count; i++) { 
					objectsToMoveList [i].position = positionToMove;
				}
			}
		}

		updateDirtyScene ();
	}

	public void rotateObject (int direction)
	{
		for (int i = 0; i < objectsToMoveList.Count; i++) { 
			objectsToMoveList [i].Rotate (0, direction * rotationAmount, 0);
		}

		updateDirtyScene ();
	}

	public void resetObjectRotation ()
	{
		for (int i = 0; i < objectsToMoveList.Count; i++) { 
			objectsToMoveList [i].rotation = Quaternion.identity;
		}

		updateDirtyScene ();
	}

	public void selectObject ()
	{
		GKC_Utils.setActiveGameObjectInEditor (objectToSelect);
	}

	void updateDirtyScene ()
	{
		GKC_Utils.updateDirtyScene ("Move Object on the scene", gameObject);
	}
}
