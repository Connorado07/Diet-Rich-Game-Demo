using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class customCharacterControllerBase : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool characterControllerEnabled = true;

	public float animatorForwardInputLerpSpeed = 0.1f;
	public float animatorTurnInputLerpSpeed = 0.1f;

	public float forwardAmountMultiplier = 1;

	public bool updateForwardAmountInputValueFromPlayerController;

	public bool updateTurnAmountInputValueFromPlayerController;

	public bool updateUsingInputValueFromPlayerController;

	[Space]
	[Header ("Collider Settings")]
	[Space]

	public bool setCapsuleColliderValues;
	public int capsuleColliderDirection = 0;
	public Vector3 capsuleColliderCenter;
	public float capsuleColliderRadius;
	public float capsuleColliderHeight;

	public float placeToShootOffset;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool characterControllerActive;

	public float horizontalInput;
	public float verticalInput;

	public bool playerUsingInput;

	public Vector3 movementInput;

	public float forwardAmount;
	public float turnAmount;

	public bool onGround;

	[Space]
	[Header ("Animator Components")]
	[Space]

	public Animator mainAnimator;
	public RuntimeAnimatorController originalAnimatorController;

	public Avatar originalAvatar;

	public bool useRootMotion = true;

	public Vector3 charactetPositionOffset;

	public Vector3 characterRotationOffset;

	[Space]
	[Header ("Camera Settings")]
	[Space]

	public bool setNewCameraStates;

	public string newCameraStateThirdPerson;
	public string newCameraStateFirstPerson;

	[Space]
	[Header ("Other Components")]
	[Space]

	public List<GameObject> characterMeshesList = new List<GameObject> ();
	public GameObject characterGameObject;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public int customActionCategoryID;
	public float characterRadius;
	public int combatTypeID;
	public Transform parentForCombatDamageTriggers;
	public string customRagdollInfoName;
	public string playerModeName = "Combat";

	public float healthBarOffset;

	[Space]
	[Header ("AI Visibility")]
	[Space]

	public bool hiddenFromAIAttacks;

	[Space]
	[Header ("AI Settings")]
	[Space]

	public bool setAIValues;
	public float maxRandomTimeBetweenAttacks;
	public float minRandomTimeBetweenAttacks;
	public float newMinDistanceToEnemyUsingCloseCombat;
	public float newMinDistanceToCloseCombat;

	public float raycastPositionOffset = 1;

	[Space]
	[Header ("Events Settings")]
	[Space]

	public bool useEventsOnStateChange;

	[Space]

	public UnityEvent eventOnCharacterControllerActivated;
	public UnityEvent eventOnCharacterControllerDeactivated;

	public bool useEventToSendCharacterObjectOnStart;
	public eventParameters.eventToCallWithGameObject eventToSendCharacterObjectOnStart;
	public eventParameters.eventToCallWithGameObject eventToSendCharacterObjectOnEnd;

	public UnityEvent eventOnInstantiateCustomCharacterObject;

	public virtual void updateCharacterControllerState ()
	{

	}

	public virtual void updateCharacterControllerAnimator ()
	{

	}

	public virtual void updateMovementInputValues (Vector3 newValues)
	{
		movementInput = newValues;
	}

	public virtual void updateHorizontalVerticalInputValues (Vector2 newValues)
	{
		horizontalInput = newValues.x;
		verticalInput = newValues.y;
	}

	public virtual void activateJumpAnimatorState ()
	{

	}

	public virtual void updateForwardAmountInputValue (float newValue)
	{
		forwardAmount = newValue * forwardAmountMultiplier;
	}

	public virtual void updateTurnAmountInputValue (float newValue)
	{
		turnAmount = newValue;
	}

	public virtual void updateOnGroundValue (bool state)
	{
		onGround = state;
	}

	public virtual void updatePlayerUsingInputValue (bool state)
	{
		playerUsingInput = state;
	}

	public virtual void setForwardAmountMultiplierValue (float newValue)
	{
		forwardAmountMultiplier = newValue;
	}

	public virtual void resetAnimatorState ()
	{

	}

	public virtual void setCharacterControllerActiveState (bool state)
	{
		if (!characterControllerEnabled) {
			return;
		}

		characterControllerActive = state;

		checkEventsOnStateChange (characterControllerActive);

		for (int i = 0; i < characterMeshesList.Count; i++) {
			if (characterMeshesList [i] != null) {
				if (characterMeshesList [i].activeSelf != state) {
					characterMeshesList [i].SetActive (state);
				}
			}
		}

//		if (characterGameObject != null) {
//			if (characterGameObject.activeSelf != state) {
//				characterGameObject.SetActive (state);
//			}
//		}
	}

	public void checkEventsOnStateChange (bool state)
	{
		if (useEventsOnStateChange) {
			if (state) {
				eventOnCharacterControllerActivated.Invoke ();
			} else {
				eventOnCharacterControllerDeactivated.Invoke ();
			}
		}

		if (useEventToSendCharacterObjectOnStart) {
			if (state) {
				eventToSendCharacterObjectOnStart.Invoke (characterGameObject);
			} else {
				eventToSendCharacterObjectOnEnd.Invoke (characterGameObject);
			}
		}
	}

	public void updateAnimatorFloatValue (int animatorIDValue, float floatValue)
	{
		mainAnimator.SetFloat (animatorIDValue, floatValue);
	}

	public void updateAnimatorIntegerValue (int animatorIDValue, int intValue)
	{
		mainAnimator.SetInteger (animatorIDValue, intValue);
	}

	public void updateAnimatorBoolValue (int animatorIDValue, bool boolValue)
	{
		mainAnimator.SetBool (animatorIDValue, boolValue);
	}

	public void updateAnimatorFloatValueLerping (int animatorIDValue, float floatValue, float  animatorLerpSpeed, float currentFixedUpdateDeltaTime)
	{
		mainAnimator.SetFloat (animatorIDValue, floatValue, animatorLerpSpeed, currentFixedUpdateDeltaTime);
	}
}
