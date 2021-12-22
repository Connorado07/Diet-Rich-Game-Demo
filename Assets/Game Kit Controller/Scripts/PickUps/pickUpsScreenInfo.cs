using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class pickUpsScreenInfo : MonoBehaviour
{
	public bool pickUpScreenInfoEnabled;

	public float durationTimerPerText;
	public float verticalDistance;
	public float horizontalOffset;

	public bool useIconsEnabled;

	public float iconHeight;
	public float verticalIconOffset;
	public float horizontalIconOffset;
	//	public float verticalIconOffsetWithText;

	public bool usedByAI;


	public GameObject originalIcon;

	public Transform pickupsInfoParent;
	public GameObject originalText;
	public RectTransform originalTextRectTransform;
	public playerController mainPlayerController;

	public string textToAddFromEditor;


	List<pickupScreenInfo> textList = new List<pickupScreenInfo> ();

	float lastTexTime;
	Vector3 originalTextRectTransformPosition;

	float heightToRemove;

	bool elementsStored;

	void Start ()
	{
		originalTextRectTransformPosition = originalTextRectTransform.anchoredPosition;
	}

	//display in the screen the type of pick ups that the objects grabs, setting their names and amount grabbed, setting the text position and the time that
	//is visible
	void FixedUpdate ()
	{
		if (!pickUpScreenInfoEnabled) {
			return;
		}

		if (elementsStored) {
			//if there are text elements, then check the timer, and delete them
			if (textList.Count > 0) {
				if (Time.time > lastTexTime + durationTimerPerText) {
					heightToRemove = textList [0].infoHeight;

					if (textList.Count > 1) {
						if (textList [1].hasIcon && !textList [0].hasIcon) {
							heightToRemove = verticalIconOffset;
						}
					}

					Destroy (textList [0].pickupText.gameObject);

					if (textList [0].hasIcon) {
						Destroy (textList [0].pickupIcon.gameObject);
					}

					textList.RemoveAt (0);

					if (textList.Count == 0) {
						elementsStored = false;
					}

					setPositions ();

					lastTexTime = Time.time;
				}
			}
		}
	}

	//the player has grabbed a pick up, so display the info in the screen, instantiating a new text component
	public void recieveInfo (string info)
	{
		if (usedByAI || mainPlayerController.isPlayerMenuActive ()) {
			return;
		}

		if (pickUpScreenInfoEnabled) {
			GameObject newText = (GameObject)Instantiate (originalText, originalText.transform.position, Quaternion.identity);

			RectTransform newTextRectTransform = newText.GetComponent<RectTransform> ();

			newTextRectTransform.gameObject.SetActive (true);
			newTextRectTransform.SetParent (pickupsInfoParent);
			newTextRectTransform.localScale = Vector3.one;

			if (info.Length > 12) {
				float extraPositionX = info.Length - 12;
				newTextRectTransform.sizeDelta = new Vector2 (newTextRectTransform.sizeDelta.x + extraPositionX * 20, newTextRectTransform.sizeDelta.y);
			}

			newTextRectTransform.anchoredPosition = originalTextRectTransformPosition.y * Vector2.up
			- Vector2.right * (newTextRectTransform.sizeDelta.x / 2) + Vector2.right * horizontalOffset;

			float infoHeight = 0;
			float verticalOffset = 0;

			if (textList.Count > 0) {
				verticalOffset = textList [textList.Count - 1].verticalOffset;

				if (textList [textList.Count - 1].hasIcon) {
					verticalOffset += verticalIconOffset;
				} else {
					verticalOffset += verticalDistance;
				}
			}

			infoHeight = verticalDistance;

			Vector2 extraPosition = Vector2.up * (verticalOffset);

			newTextRectTransform.anchoredPosition += extraPosition;

			newText.GetComponent<Text> ().text = info;

			pickupScreenInfo newPickupScreenInfo = new pickupScreenInfo ();
			newPickupScreenInfo.pickupText = newTextRectTransform;

			newPickupScreenInfo.verticalOffset = verticalOffset;
			newPickupScreenInfo.infoHeight = infoHeight;

			textList.Add (newPickupScreenInfo);

			elementsStored = true;
		
			lastTexTime = Time.time;
		}
	}

	public void recieveInfo (string info, Texture icon)
	{
		if (usedByAI || mainPlayerController.isPlayerMenuActive ()) {
			return;
		}

		if (pickUpScreenInfoEnabled) {

			if (!useIconsEnabled) {
				recieveInfo (info);
				return;
			}

			GameObject newText = (GameObject)Instantiate (originalText, originalText.transform.position, Quaternion.identity);
			RectTransform newTextRectTransform = newText.GetComponent<RectTransform> ();
			newTextRectTransform.gameObject.SetActive (true);
			newTextRectTransform.SetParent (pickupsInfoParent);
			newTextRectTransform.localScale = Vector3.one;

			if (info.Length > 12) {
				float extraPositionX = info.Length - 12;
				newTextRectTransform.sizeDelta = new Vector2 (newTextRectTransform.sizeDelta.x + extraPositionX * 20, newTextRectTransform.sizeDelta.y);
			}

			newTextRectTransform.anchoredPosition = originalTextRectTransformPosition.y * Vector2.up
			- Vector2.right * (newTextRectTransform.sizeDelta.x / 2) + Vector2.right * horizontalOffset - Vector2.right * (iconHeight + horizontalIconOffset);

			float infoHeight = 0;
			float verticalOffset = 0;

			if (textList.Count > 0) {
				verticalOffset = textList [textList.Count - 1].verticalOffset;

				infoHeight = verticalIconOffset;

				verticalOffset += infoHeight;
			} 

			infoHeight = verticalIconOffset;

			Vector2 extraPosition = Vector2.up * (verticalOffset);

			newTextRectTransform.anchoredPosition += extraPosition;

			newText.GetComponent<Text> ().text = info;

			GameObject newIcon = (GameObject)Instantiate (originalIcon, originalIcon.transform.position, Quaternion.identity);
			RectTransform newIconRectTransform = newIcon.GetComponent<RectTransform> ();
			newIconRectTransform.gameObject.SetActive (true);
			newIconRectTransform.SetParent (pickupsInfoParent);
			newIconRectTransform.localScale = Vector3.one;

			newIconRectTransform.anchoredPosition += new Vector2 (horizontalIconOffset, 0);

			newIconRectTransform.anchoredPosition += extraPosition;
		
			if (icon != null) {
				newIcon.GetComponent<RawImage> ().texture = icon;
			}

			pickupScreenInfo newPickupScreenInfo = new pickupScreenInfo ();
			newPickupScreenInfo.pickupText = newTextRectTransform;
			newPickupScreenInfo.pickupIcon = newIconRectTransform;
			newPickupScreenInfo.hasIcon = true;

			newPickupScreenInfo.verticalOffset = verticalOffset;
			newPickupScreenInfo.infoHeight = infoHeight;

			textList.Add (newPickupScreenInfo);

			elementsStored = true;

			lastTexTime = Time.time;
		}
	}

	void setPositions ()
	{
		for (int i = 0; i < textList.Count; i++) {
//			print (heightToRemove + " " + textList [i].verticalOffset + " " + textList [i].infoHeight);

			textList [i].verticalOffset -= heightToRemove;

			Vector2 extraPosition = heightToRemove * Vector2.up;

			if (textList [i].hasIcon) {
				textList [i].pickupIcon.anchoredPosition -= extraPosition;
			}

			textList [i].pickupText.anchoredPosition -= extraPosition;
		}
	}

	public void setPickUpScreenInfoEnabled (bool state)
	{
		pickUpScreenInfoEnabled = state;
	}

	public void setPickUpScreenInfoEnabledFromEditor (bool state)
	{
		pickUpScreenInfoEnabled = state;

		GKC_Utils.updateComponent (this);
	}

	public void setUsedByAIState (bool state)
	{
		usedByAI = state;
	}

	public void addTextFromEditor ()
	{
		recieveInfo (textToAddFromEditor);
	}

	public void addTextAndIconFromEditor ()
	{
		recieveInfo (textToAddFromEditor, null);
	}

	[System.Serializable]
	public class pickupScreenInfo
	{
		public RectTransform pickupText;
		public bool hasIcon;
		public RectTransform pickupIcon;
		public float verticalOffset;
		public float infoHeight;
	}
}
