using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class grabPhysicalObjectMeleeAttackSystem : weaponObjectInfo
{
	[Header ("Main Settings")]
	[Space]

	public string weaponName;

	public bool useRandomAttackIndex;

	public bool onlyAttackIfNoPreviousAttackInProcess;
	public float minDelayBetweenAttacks = 0.5f;

	public bool resetIndexIfNotAttackAfterDelay;

	public float delayToResetIndexAfterAttack;

	public string weaponInfoName;

	public bool objectUsesStaminaOnAttacks = true;

	public bool hideWeaponMeshWhenNotUsed;

	public bool disableMeleeObjectCollider = true;

	[Space]
	[Header ("Block Settings")]
	[Space]

	public bool canUseBlock;
	public string blockActionName;
	public float blockDamageProtectionAmount = 1;
	public bool useMaxBlockRangeAngle;
	public float maxBlockRangeAngle = 360;
	public int blockID;
	public float reducedBlockDamageProtectionAmount = 0.1f;

	[Space]
	[Header ("Shield Settings")]
	[Space]

	public bool canUseShieldBlock;
	public string shieldBlockActionName;
	public int shieldBlockID;

	[Space]
	[Header ("Throw/Return Settings")]
	[Space]

	public bool canThrowObject;
	public bool canReturnObject;
	public bool canReUseObjectIfNotReturnActive = true;

	public float throwSpeed;
	public float returnSpeed;

	public float returnSplineSpeed;
	public float resetObjectRotationSpeed;

	public float delayToThrowObject;
	public float delayToReturnObject;

	public float maxTimeOnAirIfNoSurfaceFound;

	public bool throwObjectWithRotation = true;
	public bool returnObjectWithRotation = true;

	public bool useSplineForReturn = true;

	public bool canReturnWeaponIfNoSurfaceFound;

	public bool rotateWeaponToSurfaceLocatedWhenCloseEnough;

	public float attachToSurfaceAdjustSpeed = 5;

	public bool useCutOnThrowObject;

	public bool useCutOnReturnObject;

	public bool returnWeaponIfObjectDetected;

	[Space]
	[Header ("Match Target Settings")]
	[Space]

	public bool useMatchPositionSystemOnAllAttacks;
	public float matchPositionOffsetOnAllAttacks = 1.6f;

	[Space]
	[Header ("Teleport Settings")]
	[Space]

	public bool teleportPlayerOnThrowEnabled = true;

	public bool throwWeaponQuicklyAndTeleportIfSurfaceFound;
	public float extraSpeedOnThrowWeaponQuicklyAndTeleport = 6;

	[Space]
	[Header ("Trail Renderer Settings")]
	[Space]

	public float trailSpeed = 10;
	public bool useTrailOnThrowReturn;

	[Space]
	[Header ("Throw/Return Action Settings")]
	[Space]

	public bool useThrowActionName;
	public string throwActionName;

	public bool useStartReturnActionName;
	public string startReturnActionName;

	public bool useEndReturnActionName;
	public string endReturnActionName;

	[Space]
	[Header ("Throw/Return Physics Settings")]
	[Space]

	public float forceToApplyToSurfaceFound;
	public float forceExtraToApplyOnVehiclesFound;

	public float capsuleCastRadius = 1;
	public float capsuleCastDistance = 2;

	[Space]
	[Header ("Throw/Return Damage Settings")]
	[Space]

	public float damageOnSurfaceDetectedOnThrow;

	public bool applyDamageOnSurfaceDetectedOnReturn;
	public float damageOnSurfaceDetectedOnReturn;

	public bool applyDamageOnObjectReturnPath = true;
	public float damageOnObjectReturnPath = 20;

	[Space]
	[Header ("Throw/Return Events Settings")]
	[Space]

	public bool useEventsOnThrowReturn;
	public UnityEvent eventOnThrowStart;
	public UnityEvent eventOnThrowEnd;
	public UnityEvent eventOnReturnStart;
	public UnityEvent eventOnReturnEnd;

	[Space]
	[Header ("Keep/Draw Weapon Events Settings")]
	[Space]

	public bool useEventsOnKeepOrDrawMeleeWeapon;

	public UnityEvent eventsOnKeepMeleeWeapon;
	public UnityEvent eventsOnDrawMeleeWeapon;

	[Space]
	[Header ("Cutting Mode Settings")]
	[Space]

	public bool canActivateCuttingMode;

	public Vector3 cutOverlapBoxSize;
	public Transform cutPositionTransform;
	public Transform cutDirectionTransform;

	public Transform planeDefiner1;
	public Transform planeDefiner2;
	public Transform planeDefiner3;

	[Space]
	[Header ("Attack List Settings")]
	[Space]

	public bool attacksEnabled = true;
	public bool useAnimationPercentageDuration;
	public bool useAnimationPercentageOver100;

	public bool useGeneralDamageTypeID;

	public int generalDamageTypeID = -1;

	public List<attackInfo> attackInfoList = new List<attackInfo> ();

	[Space]
	[Header ("Damage Detection Settings")]
	[Space]

	public List<Transform> raycastCheckTransfromList = new List<Transform> ();

	[Space]
	[Header ("Damage Type And Reaction Settings")]
	[Space]

	public List<damageTypeAndReactionInfo> damageTypeAndReactionInfoList = new List<damageTypeAndReactionInfo> ();

	[Space]
	[Header ("Melee Weapon Mesh Settings")]
	[Space]

	public GameObject weaponMesh;

	public bool useCustomWeaponMeshToInstantiate;

	public GameObject customWeaponMeshToInstantiate;

	[Space]
	[Header ("Remote Events Settings")]
	[Space]

	public bool useRemoteEventOnSurfacesDetected;
	public List<remoteEventOnSurfaceDetectedInfo> remoteEventNameListOnSurfaceDetected = new List<remoteEventOnSurfaceDetectedInfo> ();

	[Space]
	[Header ("Enable/Disable Abilities on Weapon Settings")]
	[Space]

	public bool useAbilitiesListToEnableOnWeapon;
	public List<string> abilitiesListToEnableOnWeapon = new List<string> ();

	[Space]

	public bool useAbilitiesListToDisableOnWeapon;
	public List<string> abilitiesListToDisableOnWeapon = new List<string> ();

	[Space]
	[Header ("Debug")]
	[Space]

	public bool keepGrabbedObjectState;
	public bool objectThrown;

	public bool previousStrafeMode;

	public bool secondaryAbilityActive;

	[Space]
	[Header ("Components")]
	[Space]

	public hitCombat mainHitCombat;

	public Collider mainObjectCollider;

	public Transform objectRotationPoint;
	public Transform raycastCheckTransfrom;
	public Transform objectRotationPointParent;

	public Transform raycastCheckTransformParent;

	public TrailRenderer mainTraileRenderer;

	public Transform attachToCharactersReferenceTransform;

	public Transform mainDamagePositionTransform;

	public Transform referencePositionToKeepObjectMesh;

	public grabPhysicalObjectSystem mainGrabPhysicalObjectSystem;


	[HideInInspector] public bool useCustomReferencePositionToKeepObjectMesh;

	[HideInInspector] public Transform customReferencePositionToKeepObjectMesh;

	Coroutine trailCoroutine;


	public void setMainObjectColliderEnabledState (bool state)
	{
		mainObjectCollider.enabled = state;
	}

	public int getAttackListCount ()
	{
		return attackInfoList.Count;
	}

	public hitCombat getMainHitCombat ()
	{
		return mainHitCombat;
	}

	public void setKeepOrCarryGrabbebObjectState (bool state)
	{
		keepGrabbedObjectState = state;
	}

	public bool iskeepGrabbedObjectStateActive ()
	{
		return keepGrabbedObjectState;
	}

	public void setObjectThrownState (bool state)
	{
		objectThrown = state;
	}

	public bool isObjectThrown ()
	{
		return objectThrown;
	}

	public void checkEventOnThrow (bool state)
	{
		if (useEventsOnThrowReturn) {
			if (state) {
				eventOnThrowStart.Invoke ();
			} else {
				eventOnThrowEnd.Invoke ();
			}
		}

		if (useTrailOnThrowReturn) {
			setTrailActiveState (state);
		}
	}

	public void checkEventOnReturn (bool state)
	{
		if (useEventsOnThrowReturn) {
			if (state) {
				eventOnReturnStart.Invoke ();
			} else {
				eventOnReturnEnd.Invoke ();
			}
		}

		if (useTrailOnThrowReturn) {
			setTrailActiveState (state);
		}
	}

	public void checkEventWhenKeepingOrDrawingMeleeWeapon (bool state)
	{
		if (useEventsOnKeepOrDrawMeleeWeapon) {
			if (state) {
				eventsOnKeepMeleeWeapon.Invoke ();
			} else {
				eventsOnDrawMeleeWeapon.Invoke ();
			}
		}
	}

	public void checkDisableTrail ()
	{
		if (useTrailOnThrowReturn) {
			setTrailActiveState (false);
		}
	}

	public void setTrailActiveState (bool state)
	{
		stopDisableTrailCoroutine ();

		if (state) {
			mainTraileRenderer.enabled = true;
			mainTraileRenderer.time = 1;
		} else {
			if (enabled) {
				mainTraileRenderer.enabled = false;
			} else {
				trailCoroutine = StartCoroutine (disableTrailCoroutine ());
			}
		}
	}

	void stopDisableTrailCoroutine ()
	{
		if (trailCoroutine != null) {
			StopCoroutine (trailCoroutine);
		}
	}

	IEnumerator disableTrailCoroutine ()
	{
		bool targetReached = false;

		float t = 0;

		while (!targetReached) {
			t += Time.deltaTime / trailSpeed; 
			mainTraileRenderer.time -= t;

			if (mainTraileRenderer.time <= 0) {
				targetReached = true;
			}

			yield return null;
		}

		mainTraileRenderer.enabled = false;
	}

	public void setPreviousStrafeModeState (bool state)
	{
		previousStrafeMode = state;
	}

	public bool wasStrafeModeActivePreviously ()
	{
		return previousStrafeMode;
	}

	public grabPhysicalObjectSystem getGrabPhysicalObjectMeleeAttackSystem ()
	{
		return mainGrabPhysicalObjectSystem;
	}

	public override bool isMeleeWeapon ()
	{
		return true;
	}

	public override string getWeaponName ()
	{
		return weaponName;
	}

	public void enableOrDisableRemoteEventsOnSurfacesDetected (bool state)
	{
		useRemoteEventOnSurfacesDetected = state;
	}

	public void enableOrDisableRemoteEventsOnSurfacesDetectedElement (bool state, string remoteEventName)
	{
		for (int i = 0; i < remoteEventNameListOnSurfaceDetected.Count; i++) {
			if (remoteEventNameListOnSurfaceDetected [i].remoteEventName.Equals (remoteEventName)) {
				remoteEventNameListOnSurfaceDetected [i].remoteEventActive = state;

				return;
			}
		}
	}

	public void enableRemoteEventsOnSurfacesDetectedElement (string remoteEventName)
	{
		enableOrDisableRemoteEventsOnSurfacesDetectedElement (true, remoteEventName);
	}

	public void disableRemoteEventsOnSurfacesDetectedElement (string remoteEventName)
	{
		enableOrDisableRemoteEventsOnSurfacesDetectedElement (false, remoteEventName);
	}

	public bool isRemoteEventIncluded (string remoteEventName)
	{
		for (int i = 0; i < remoteEventNameListOnSurfaceDetected.Count; i++) {
			if (remoteEventNameListOnSurfaceDetected [i].remoteEventActive &&
			    remoteEventNameListOnSurfaceDetected [i].remoteEventName.Equals (remoteEventName)) {

				return true;
			}
		}

		return false;
	}

	public void enableOrDisableRemoteEventsOnAttackList (bool state)
	{
		for (int i = 0; i < attackInfoList.Count; i++) {
			attackInfoList [i].useRemoteEvent = state;
		}
	}

	public void setSecondaryAbilityActiveState (bool state)
	{
		secondaryAbilityActive = state;
	}

	public bool isSecondaryAbilityActive ()
	{
		return secondaryAbilityActive;
	}

	public void setNewGeneralDamageTypeID (int newValue)
	{
		generalDamageTypeID = newValue;
	}

	public bool setDamageTypeAndReactionInfo (string newBuffObjectName)
	{
		int damageIndex = damageTypeAndReactionInfoList.FindIndex (s => s.buffObjectName.Equals (newBuffObjectName));

		if (damageIndex > -1) {
			for (int i = 0; i < damageTypeAndReactionInfoList.Count; i++) {
				if (damageIndex != i && damageTypeAndReactionInfoList [i].effectCurrentlyActive) {
					damageTypeAndReactionInfoList [i].effectCurrentlyActive = false;

					if (damageTypeAndReactionInfoList [i].useEventToDeactivateDamageType) {
						damageTypeAndReactionInfoList [i].eventToDeactivateDamageType.Invoke ();
					}
				}
			}

			damageTypeAndReactionInfo currentDamageTypeAndReactionInfo = damageTypeAndReactionInfoList [damageIndex];

			currentDamageTypeAndReactionInfo.effectCurrentlyActive = true;

			setNewGeneralDamageTypeID (currentDamageTypeAndReactionInfo.damageTypeID);

			if (currentDamageTypeAndReactionInfo.useEventToActivateDamageType) {
				currentDamageTypeAndReactionInfo.eventToActivateDamageType.Invoke ();
			}

			if (currentDamageTypeAndReactionInfo.useDamageTypeDuration) {
				stopDamageTypeAndReactionInfoCoroutine ();

				damageTypeInfoCoroutine = StartCoroutine (damageTypeAndReactionInfoCoroutine (currentDamageTypeAndReactionInfo));
			}

			return true;
		}

		return false;
	}



	Coroutine damageTypeInfoCoroutine;

	void stopDamageTypeAndReactionInfoCoroutine ()
	{
		if (damageTypeInfoCoroutine != null) {
			StopCoroutine (damageTypeInfoCoroutine);
		}
	}

	IEnumerator damageTypeAndReactionInfoCoroutine (damageTypeAndReactionInfo currentDamageTypeAndReactionInfo)
	{
		yield return new WaitForSeconds (currentDamageTypeAndReactionInfo.damageTypeDuration);

		currentDamageTypeAndReactionInfo.effectCurrentlyActive = false;

		if (currentDamageTypeAndReactionInfo.useEventToDeactivateDamageType) {
			currentDamageTypeAndReactionInfo.eventToDeactivateDamageType.Invoke ();
		}
	}

	[System.Serializable]
	public class attackInfo
	{
		[Header ("Main Settings")]
		[Space]

		public string Name;
		public float attackDamage;
		public string attackType;

		public float minDelayBeforeNextAttack;

		public int damageReactionID = -1;

		public int damageTypeID = -1;

		public bool playerOnGroundToActivateAttack = true;

		public bool pauseSoundIfAttackOnAir;

		[Space]
		[Header ("Damage Trigger Settings")]
		[Space]

		public List<damageTriggerActiveInfo> damageTriggerActiveInfoList = new List<damageTriggerActiveInfo> ();

		[Space]

		public bool useSingleSlashDamageInfo;

		public float delayToActivateSingleSlashDamageTrigger;
		public float delayToDeactivateSingleSlashDamageTrigger;

		public bool ignoreDetectedObjectsOnList;

		[Space]
		[Header ("Action System Settings")]
		[Space]
	
		public bool useCustomAction;
		public string customActionName;

		public float attackDuration;

		public float attackAnimationSpeed = 1;

		[Space]
		[Header ("Stamina Settings")]
		[Space]

		public float staminaUsedOnAttack;
		public float customRefillStaminaDelayAfterUse;

		[Space]
		[Header ("Remote Events Settings")]
		[Space]

		public bool useRemoteEvent;
		public List<string> remoteEventNameList = new List<string> ();

		[Space]
		[Header ("Cut Mode Settings")]
		[Space]

		public bool useCutModeOnAttack;

		[Space]
		[Header ("Push Character Settings")]
		[Space]

		public bool pushCharactersOnDamage;
		public string messageNameToSend = "pushCharacter";
		public float pushForceAmount = 4;
		public bool useProbabilityToPushCharacters;
		[Range (0, 1)]public float probability = 1;
		public bool useExtraPushDirection;
		public Vector3 extraPushDirection;
		public float extraPushForce;

		public bool ignoreMeleeWeaponAttackDirection;

		public bool useIgnoreTagsToPush;
		public List<string> tagsToIgnorePush = new List<string> ();

		[Space]
		[Header ("Match Attack Position Settings")]
		[Space]

		public bool useMatchPositionSystem = true;
		public float matchPositionOffset = 1.6f;
	}

	[System.Serializable]
	public class damageTriggerActiveInfo
	{
		[Header ("Damage Delay Settings")]
		[Space]

		public float delayToActiveTrigger;
		public bool activateDamageTrigger = true;
		[HideInInspector] public bool delayTriggered;

		[Space]
		[Header ("Damage Trigger Settings")]
		[Space]

		public bool setNewTriggerScale;
		public Vector3 newTriggerScale;
		public bool setOriginalScale = true;

		public bool checkSurfacesWithCapsuleRaycast;
		public float checkSurfaceCapsuleRaycastRadius;

		public bool changeDamageTriggerToLimb;
		public HumanBodyBones limbToPlaceTrigger;

		[Space]
		[Header ("Events Settings")]
		[Space]

		public bool useEventOnAttack;
		public UnityEvent eventOnAtatck;

		[HideInInspector] public float calculatedPercentageAttackDuration;

		[Space]
		[Header ("Range Attack Settings")]
		[Space]

		public bool useRangeAttackID;
		public int rangeAttackID;
		public bool disableRangeAttackID;
		public int rangeAttackIDToDisable;
	}

	[System.Serializable]
	public class damageTypeAndReactionInfo
	{
		public string Name;

		public string buffObjectName;

		public bool effectCurrentlyActive;

		public int damageTypeID = -1;

		public bool useDamageTypeDuration;
		public float damageTypeDuration;

		public bool useEventToActivateDamageType;
		public UnityEvent eventToActivateDamageType;

		public bool useEventToDeactivateDamageType;
		public UnityEvent eventToDeactivateDamageType;
	}

	[System.Serializable]
	public class remoteEventOnSurfaceDetectedInfo
	{
		public string remoteEventName;
		public bool remoteEventActive;
	}
	
}
