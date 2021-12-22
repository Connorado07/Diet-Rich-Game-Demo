using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class showMessageOnHUDSystem : MonoBehaviour
{
	public List<messageInfo> messageInfoList = new List<messageInfo> ();

	public void showMessagePanel (string messageName)
	{
		for (int i = 0; i < messageInfoList.Count; i++) {
			if (messageInfoList [i].Name.Equals (messageName)) {

				if (!messageInfoList [i].showingMessage || !messageInfoList [i].dontActivateMessageIfShowing) {
					showObjectMessage (i);
				}
			}
		}
	}

	public void showObjectMessage (int messageIndex)
	{
		if (messageInfoList [messageIndex].messageCoroutine != null) {
			StopCoroutine (messageInfoList [messageIndex].messageCoroutine);
		}

		messageInfoList [messageIndex].messageCoroutine = StartCoroutine (showObjectMessageCoroutine (messageIndex));
	}

	IEnumerator showObjectMessageCoroutine (int messageIndex)
	{
		messageInfoList [messageIndex].messageText.text = messageInfoList [messageIndex].messageContent;

		messageInfoList [messageIndex].showingMessage = true;
		messageInfoList [messageIndex].eventOnMessage.Invoke ();

		messageInfoList [messageIndex].messagePanel.SetActive (true);

		yield return new WaitForSeconds (messageInfoList [messageIndex].messageDuration);

		messageInfoList [messageIndex].messagePanel.SetActive (false);

		messageInfoList [messageIndex].showingMessage = false;
	}

	[System.Serializable]
	public class messageInfo
	{
		public string Name;

		[TextArea (10, 11)] public string messageContent;

		public GameObject messagePanel;

		public Text messageText;

		public float messageDuration;

		public Coroutine messageCoroutine;

		public UnityEvent eventOnMessage;

		public bool showingMessage;

		public bool dontActivateMessageIfShowing;
	}
}
