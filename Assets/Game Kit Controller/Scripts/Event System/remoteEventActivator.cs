using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remoteEventActivator : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool useRemoveEventInfoList;
	public List<removeEventInfo> removeEventInfoList = new List<removeEventInfo> ();

	public string remoteEventToCall;

	public bool useAmount;
	public float amountValue;

	public bool useBool;
	public bool boolValue;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public bool useGameObject;
	public GameObject gameObjectToUse;

	public bool useTransform;
	public Transform transformToUse;

	[Space]
	[Header ("Set Object Manually Settings")]
	[Space]

	public bool assignObjectManually;
	public GameObject objectToAssign;

	public bool searchPlayerOnSceneIfNotAssigned;

	GameObject currentObjectToCall;

	removeEventInfo currentEventInfo;

	public void callRemoteEvent (GameObject objectToCall)
	{
		currentObjectToCall = objectToCall;

		callEvent ();
	}

	public void setObjectToCall (GameObject objectToCall)
	{
		currentObjectToCall = objectToCall;
	}

	public void callEvent ()
	{
		if (assignObjectManually) {
			if (objectToAssign == null) {
				if (searchPlayerOnSceneIfNotAssigned) {
					findPlayerOnScene ();
				}
			}

			if (objectToAssign == null) {
				print ("WARNING: no object has been assigned manually on remote event activator");

				return;
			}

			currentObjectToCall = objectToAssign;
		}

		remoteEventSystem currentRemoteEventSystem = currentObjectToCall.GetComponent<remoteEventSystem> ();

		if (currentRemoteEventSystem != null) {

			if (useRemoveEventInfoList) {
				for (int i = 0; i < removeEventInfoList.Count; i++) {
					currentEventInfo = removeEventInfoList [i];

					if (currentEventInfo.useAmount) {
						currentRemoteEventSystem.callRemoteEventWithAmount (remoteEventToCall, currentEventInfo.amountValue);
					} else if (currentEventInfo.useBool) {
						currentRemoteEventSystem.callRemoteEventWithBool (remoteEventToCall, currentEventInfo.boolValue);
					} else if (currentEventInfo.useGameObject) {
						currentRemoteEventSystem.callRemoteEventWithGameObject (remoteEventToCall, currentEventInfo.gameObjectToUse);
					} else if (currentEventInfo.useTransform) {
						currentRemoteEventSystem.callRemoteEventWithTransform (remoteEventToCall, currentEventInfo.transformToUse);
					} else {
						currentRemoteEventSystem.callRemoteEvent (currentEventInfo.remoteEventToCall);
					}
				}
			} else {
				if (useAmount) {
					currentRemoteEventSystem.callRemoteEventWithAmount (remoteEventToCall, amountValue);
				} else if (useBool) {
					currentRemoteEventSystem.callRemoteEventWithBool (remoteEventToCall, boolValue);
				} else if (useGameObject) {
					currentRemoteEventSystem.callRemoteEventWithGameObject (remoteEventToCall, gameObjectToUse);
				} else if (useTransform) {
					currentRemoteEventSystem.callRemoteEventWithTransform (remoteEventToCall, transformToUse);
				} else {
					currentRemoteEventSystem.callRemoteEvent (remoteEventToCall);
				}
			}
		}
	}

	public void findPlayerOnScene ()
	{
		if (searchPlayerOnSceneIfNotAssigned) {
			objectToAssign = GKC_Utils.findMainPlayerOnScene ();
		}
	}

	[System.Serializable]
	public class removeEventInfo
	{
		public string Name;

		public string remoteEventToCall;

		public bool useAmount;
		public float amountValue;

		public bool useBool;
		public bool boolValue;

		public bool useGameObject;
		public GameObject gameObjectToUse;

		public bool useTransform;
		public Transform transformToUse;
	}
}
