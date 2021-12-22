using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideBodyPartOnCharacterSystem : MonoBehaviour
{
	public bool useTimeBullet = true;
	public float timeBulletDuration = 3;
	public float timeScale = 0.2f;

	public Transform currentBodyPartToHide;

	public void hideBodyPart ()
	{
		if (useTimeBullet) {
			GKC_Utils.activateTimeBulletXSeconds (timeBulletDuration, timeScale);
		}

		setBodyPartScale ();
	}

	public void hideBodyPart (Transform newBodyPart)
	{
		currentBodyPartToHide = newBodyPart;

		hideBodyPart ();
	}

	public void hideBodyPartWithoutBulletTimeCheck ()
	{
		setBodyPartScale ();
	}

	public void hideBodyPartWithoutBulletTimeCheck (Transform newBodyPart)
	{
		currentBodyPartToHide = newBodyPart;

		setBodyPartScale ();
	}

	public void setBodyPartScale ()
	{
		if (currentBodyPartToHide != null) {
			currentBodyPartToHide.localScale = Vector3.zero;
		}
	}
}
