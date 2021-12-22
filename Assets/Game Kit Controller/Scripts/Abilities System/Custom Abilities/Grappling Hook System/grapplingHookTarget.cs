using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingHookTarget : MonoBehaviour
{
	public bool grapplingHookTargetEnabled = true;

	public List<string> tagsToCheck = new List<string> ();
	public LayerMask layermaskToCheck;

	public bool showGizmo;
	public Color gizmoLabelColor = Color.green;
	public Color gizmoColor = Color.white;
	public float gizmoRadius = 0.3f;

	void OnTriggerEnter (Collider col)
	{
		checkTriggerInfo (col, true);
	}

	void OnTriggerExit (Collider col)
	{
		checkTriggerInfo (col, false);
	}

	public void checkTriggerInfo (Collider col, bool isEnter)
	{
		if (!grapplingHookTargetEnabled) {
			return;
		}

		if ((1 << col.gameObject.layer & layermaskToCheck.value) == 1 << col.gameObject.layer) {

			if (isEnter) {

				if (tagsToCheck.Contains (col.tag)) {

					GameObject currentPlayer = col.gameObject;

					playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

					if (currentPlayerComponentsManager != null) {

						grapplingHookTargetsSystem currentGrapplingHookTargetsSystem = currentPlayerComponentsManager.getGrapplingHookTargetsSystem ();

						if (currentGrapplingHookTargetsSystem != null) {
							currentGrapplingHookTargetsSystem.addNewGrapplingHookTarget (transform);
						}
					}
				}
			} else {
				if (tagsToCheck.Contains (col.tag)) {
					GameObject currentPlayer = col.gameObject;

					playerComponentsManager currentPlayerComponentsManager = currentPlayer.GetComponent<playerComponentsManager> ();

					if (currentPlayerComponentsManager != null) {

						grapplingHookTargetsSystem currentGrapplingHookTargetsSystem = currentPlayerComponentsManager.getGrapplingHookTargetsSystem ();

						if (currentGrapplingHookTargetsSystem != null) {
							currentGrapplingHookTargetsSystem.removeNewGrapplingHookTarget (transform);
						}
					}
				}
			}
		}
	}

	void OnDrawGizmos ()
	{
		if (!showGizmo) {
			return;
		}

		if (GKC_Utils.isCurrentSelectionActiveGameObject (gameObject)) {
			DrawGizmos ();
		}
	}

	void OnDrawGizmosSelected ()
	{
		DrawGizmos ();
	}

	void DrawGizmos ()
	{
		if (showGizmo) {
			Gizmos.color = gizmoColor;

			Gizmos.DrawSphere (transform.position, gizmoRadius);
		}
	}
}