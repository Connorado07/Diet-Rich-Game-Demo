using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectToFollowOnThrowMeleeWeapon : MonoBehaviour
{
	public Transform mainObjectToFollow;

	public Transform getMainObjectToFollow ()
	{
		if (mainObjectToFollow == null) {
			mainObjectToFollow = transform;
		}

		return mainObjectToFollow;
	}
}
