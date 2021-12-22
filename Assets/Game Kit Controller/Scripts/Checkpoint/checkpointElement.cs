using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointElement : MonoBehaviour
{
	public int checkpointID;

	public bool overwriteThisCheckpoint;

	public bool useCustomSaveTransform;
	public Transform customSaveTransform;

	public bool useCustomCameraTransform;
	public Transform customCameraTransform;
	public Transform customCameraPivotTransform;

	public List<string> tagToSave = new List<string> ();
	public bool saveInEveryTriggerEnter;

	public bool checkpointAlreadyFound;
	public checkpointSystem checkpointManager;
	public Collider mainCollider;

	void Awake ()
	{
		StartCoroutine (activateTriggers ());
	}

	IEnumerator activateTriggers ()
	{
		if (mainCollider) {
			mainCollider.enabled = false;
			yield return new WaitForSeconds (1);
			mainCollider.enabled = true;
		}
	}

	public void setCheckPointManager (checkpointSystem manager)
	{
		checkpointManager = manager;

		updateComponent ();
	}

	public void OnTriggerEnter (Collider col)
	{
		if ((!checkpointAlreadyFound || saveInEveryTriggerEnter) && tagToSave.Contains (col.tag)) {
			checkpointAlreadyFound = true;

			playerComponentsManager currentPlayerComponentsManager = col.gameObject.GetComponent<playerComponentsManager> (); 

			if (currentPlayerComponentsManager) {
				saveGameSystem currentSaveGameSystem = currentPlayerComponentsManager.getSaveGameSystem ();
			
				if (useCustomSaveTransform) {
					currentSaveGameSystem.saveGameCheckpoint (customSaveTransform, checkpointID, checkpointManager.checkpointSceneID, overwriteThisCheckpoint, false);
				} else {
					currentSaveGameSystem.saveGameCheckpoint (null, checkpointID, checkpointManager.checkpointSceneID, overwriteThisCheckpoint, false);
				}

				checkpointManager.setCurrentCheckpointElement (transform);
			}
		}
	}

	public void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}
}
