using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class grabbedObjectMeleeAttackSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool grabbedObjectMeleeAttackEnabled = true;
	public bool grabbedObjectMeleeAttackActive;

	public bool useAttackTypes;

	public float generalAttackDamageMultiplier = 1;

	public List<string> meleeAttackTypes = new List<string> ();

	public List<int> meleeAttackTypesAmount = new List<int> ();

	public bool playerOnGroundToActivateAttack = true;

	public bool disableGrabObjectsInputWhenCarryingMeleeWeapon;

	[Space]
	[Header ("Damage Detection Settings")]
	[Space]

	public bool useCustomLayerToDetectSurfaces;
	public LayerMask customLayerToDetectSurfaces;

	public bool useCustomIgnoreTags;
	public List<string> customTagsToIgnoreList = new List<string> ();

	public bool checkSurfacesWithCapsuleRaycastEnabled = true;

	[Space]
	[Header ("Weapon Info List Settings")]
	[Space]

	public List<grabbedWeaponInfo> grabbedWeaponInfoList = new List<grabbedWeaponInfo> ();

	[Space]
	[Header ("Surface Info List Settings")]
	[Space]

	public bool checkSurfaceInfoEnabled = true;

	public string surfaceInfoOnMeleeAttackNameForNotFound = "Regular";
	public string surfaceInfoOnMeleeAttackNameForSwingOnAir = "Swing On Air";
	public List<surfaceInfoOnMeleeAttack> surfaceInfoOnMeleeAttackList = new List<surfaceInfoOnMeleeAttack> ();

	[Space]
	[Header ("Throw Object Settings")]
	[Space]

	public bool throwObjectEnabled = true;
	public bool returnObjectEnabled = true;

	public bool playerOnGroundToActivateThrow;
	public ForceMode throwObjectsForceMode;
	public LayerMask throwObjectsLayerToCheck;
	public float throwObjectRotationSpeed;

	public bool useSplineToReturnObject;
	public BezierSpline splineToReturnObject;

	public bool disableDropObjectWhenThrownOrReturning;

	public string movingObjectsTag = "moving";

	public float generalDamageOnSurfaceDetectedOnThrow = 1;

	public bool applyDamageOnSurfaceDetectedOnReturnEnabled = true;

	public bool applyDamageOnObjectReturnPathEnabled = true;

	public float generalDamageMultiplierOnObjectReturn = 1;

	public float generalDamageMultiplierOnReturnPath = 1;

	[Space]
	[Header ("Follow Object On Throw Melee Weapon Settings")]
	[Space]

	public bool checkIfObjectToFollowOnThrowMeleeWeapon;
	public float followObjectOnThrowMeleeWeaponSpeed;
	public float extraWeaponOnAirTimeIfObjectToFollowDetected = 6;

	[Space]
	[Header ("Throw Object Events Settings")]
	[Space]

	public bool useEventsOnThrowReturnObject;
	public UnityEvent eventOnThrowObject;
	public UnityEvent eventOnReturnObject;

	[Space]
	[Header ("Teleport Settings")]
	[Space]

	public bool teleportPlayerOnThrowEnabled = true;
	public bool grabMeleeWeaponOnTeleportPositionReached = true;
	public float teleportSpeed = 10;
	public float cameraFovOnTeleport = 70;
	public float cameraFovOnTeleportSpeed = 5;
	public float minDistanceToStopTeleport = 1;
	public bool teleportInstantlyToPosition;

	public bool useSmoothCameraFollowStateOnTeleport = true;
	public float smoothCameraFollowDuration = 3;

	[Space]

	public UnityEvent eventOnStartTeleport;
	public UnityEvent eventOnEndTeleport;

	[Space]
	[Header ("Stamina Settings")]
	[Space]

	public bool useStaminaOnAttackEnabled = true;
	public string attackStaminaStateName = "Melee Attack With Grabbed Object";

	public bool useStaminaOnThrowObjectEnabled = true;
	public string throwObjectStaminaState = "Throw Grabbed Object";
	public float staminaToUseOnThrowObject = 10;
	public float customRefillStaminaDelayAfterThrow;

	public bool useStaminaOnReturnObjectEnabled = true;
	public string returnObjectStaminaState = "Return Grabbed Object";
	public float staminaToUseOnReturnObject = 10;
	public float customRefillStaminaDelayAfterReturn;

	public float generalStaminaUseMultiplier = 1;

	[Space]
	[Header ("Cutting Mode Settings")]
	[Space]

	public bool cuttingModeEnabled = true;

	public UnityEvent eventOnCuttingModeStart;
	public UnityEvent eventOnCuttingModeEnd;

	[Space]
	[Header ("Block Mode Settings")]
	[Space]

	public bool blockModeEnabled = true;
	public float generalBlockProtectionMultiplier = 1;

	public string cancelBlockReactionStateName = "Disable Has Exit Time State";

	[Space]
	[Header ("Shield Settings")]
	[Space]

	public bool shieldCanBeUsedWithoutMeleeWeapon;

	public Transform rightHandMountPoint;
	public Transform leftHandMountPoint;
	public Transform shieldRightHandMountPoint;
	public Transform shieldLeftHandMountPoint;

	public Transform shieldBackMountPoint;

	[Space]

	public UnityEvent eventToActivateMeleeModeWhenUsingShieldWithoutMeleeWeapon;

	[Space]
	[Header ("Match Target Settings")]
	[Space]

	public bool useMatchTargetSystemOnAttack;
	public bool ignoreAttackSettingToMatchTarget;
	public matchPlayerToTargetSystem mainMatchPlayerToTargetSystem;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public string mainMeleeCombatAxesInputName = "Grab Objects";
	public string mainMeleeCombatBlockInputName = "Block Attack";

	[Space]
	[Header ("Debug")]
	[Space]

	public bool showDebugPrint;

	public bool carryingObject;

	public bool attackInProcess;

	public hitCombat currentHitCombat;

	public int currentAttackIndex;

	public bool blockActive;

	public bool blockActivePreviously;

	public bool objectThrown;

	public bool objectThrownTravellingToTarget;

	public bool continueObjecThrowActivated;

	public bool returningThrownObject;

	public bool cuttingModeActive;

	public bool attackInputPausedForStamina;

	public bool throwObjectInputPausedForStamina;

	public bool objectToFollowFound;
	public Transform objectToFollow;

	public bool damageTriggerInProcess;

	public bool allEventsTriggered;

	public bool meleeAttackInputPaused;

	public bool reducedBlockDamageProtectionActive;

	public bool canCancelBlockToStartAttackActive;

	public bool blockInputPaused;

	public bool teleportInProcess;

	[Space]
	[Header ("Shield Debug")]
	[Space]

	public bool shieldActive;

	public bool carryingShield;

	public string currentShieldName;

	public GameObject currentShieldGameObject;

	public Transform currentShieldHandMountPointTransformReference;
	public Transform currentShieldBackMountPointTransformReference;


	[Space]
	[Header ("Event Settings")]
	[Space]

	public bool useEventsOnAttack;
	public UnityEvent eventOnAttackStart;
	public UnityEvent eventOnAttackEnd;

	public bool useEventsOnBlockDamage;
	public UnityEvent eventOnBlockActivate;
	public UnityEvent eventOnBlockDeactivate;

	public bool useEventsOnGrabDropObject;
	public UnityEvent eventOnGrabObject;
	public UnityEvent eventOnDropObject;

	public UnityEvent eventOnDropObjectWhenIsThrown;

	public bool useEventsOnAttackCantBeBlocked;
	public UnityEvent eventOnAttackCantBeBlocked;

	public bool hideInventoryQuickAccessSlotsWhenCarryingMeleeWeapon;
	public UnityEvent eventOnHideInventoryQuickAccessSlots;
	public UnityEvent eventOnShowInventoryQuickAccessSlots;

	[Space]
	[Header ("Gizmo Settings")]
	[Space]

	public bool showGizmo;
	public Color sphereColor = Color.green;
	public Color cubeColor = Color.blue;

	[Space]
	[Header ("Components")]
	[Space]

	public Transform currentGrabbedObjectTransform;
	public remoteEventSystem mainRemoteEventSystem;

	public GameObject playerControllerGameObject;
	public playerController mainPlayerController;

	public grabObjects mainGrabObjects;
	public health mainHealth;
	public Transform mainCameraTransform;

	public staminaSystem mainStaminaSystem;

	public Transform handPositionReference;

	public AudioSource mainAudioSource;

	public sliceSystem mainSliceSystem;

	public Animator mainAnimator;

	public findObjectivesSystem mainFindObjectivesSystem;

	public meleeWeaponsGrabbedManager mainMeleeWeaponsGrabbedManager;

	public playerInputManager playerInput;

	public playerTeleportSystem mainPlayerTeleportSystem;

	Transform objectRotationPointParent;
	Transform objectRotationPoint;

	grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem;

	int attackListCount;

	Coroutine attackCoroutine;

	float lastTimeAttackActive;

	float lastTimeAttackComplete;

	grabPhysicalObjectMeleeAttackSystem.attackInfo currentAttackInfo;

	Coroutine damageTriggerCoroutine;

	grabPhysicalObjectSystem currentGrabPhysicalObjectSystem;

	Coroutine returnCoroutine;

	Coroutine throwCoroutine;

	Rigidbody currentObjectRigidbody;

	grabbedWeaponInfo currentGrabbedWeaponInfo;

	GameObject surfaceDetecetedOnObjectThrown;
	bool surfaceDetected;

	bool surfaceNotFound;

	float capsuleCastRadius;
	float capsuleCastDistance;

	Vector3 currentRayOriginPosition;
	Vector3 currentRayTargetPosition;

	float distanceToTarget;
	Vector3 rayDirection;

	Vector3 point1;
	Vector3 point2;

	RaycastHit[] hits;

	Transform raycastCheckTransfrom;

	bool throwObjectWithRotation;

	bool returnObjectWithRotation;

	bool useSplineForReturn;

	float lastTimeObjectThrown;

	float lastTimeObjectReturn;

	Vector3 originalHitCombatColliderSize;

	Vector3 originalHitCombatColliderPosition;

	BoxCollider currentHitCombatBoxCollider;

	bool hitCombatParentChanged;

	bool isAttachedToSurface;

	bool surfaceDetectedIsDead;

	bool previousStrafeMode;

	bool currentAttackCanBeBlocked;

	Coroutine pauseBlockInputCoroutine;
	Coroutine disableHasExitTimeCoroutine;

	Coroutine teleportCoroutine;

	float lastTimeDamageTriggerActivated;

	bool attackActivatedOnAir;

	bool thorwWeaponQuicklyAndTeleportIfSurfaceFound;

	RaycastHit hitToQuicklyTeleport;

	bool surfaceToQuicklyTeleportLocated;

	float timeToReachSurfaceOnQuickTeleport;

	Vector3 teleportPosition;
	float teleportDistanceOffset;


	public Transform getRightHandMountPoint ()
	{
		return rightHandMountPoint;
	}

	public Transform getLeftHandMountPoint ()
	{
		return leftHandMountPoint;
	}

	public void checkGrabbedMeleeWeaponLocalPositionRotationValues ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabbedWeaponInfo.useCustomGrabbedWeaponReferencePosition) {
			currentGrabbedObjectTransform.localRotation = currentGrabbedWeaponInfo.customGrabbedWeaponReferencePosition.localRotation;
			currentGrabbedObjectTransform.localPosition = currentGrabbedWeaponInfo.customGrabbedWeaponReferencePosition.localPosition;
		}
	}

	public void checkKeepMeleeWeaponLocalPositionRotationValues ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabbedWeaponInfo.useCustomReferencePositionToKeepObject) {
			currentGrabbedObjectTransform.localRotation = currentGrabbedWeaponInfo.customReferencePositionToKeepObject.localRotation;
			currentGrabbedObjectTransform.localPosition = currentGrabbedWeaponInfo.customReferencePositionToKeepObject.localPosition;
		}
	}

	public Transform getCustomGrabbedWeaponReferencePosition ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabbedWeaponInfo.useCustomGrabbedWeaponReferencePosition) {
			return currentGrabbedWeaponInfo.customGrabbedWeaponReferencePosition;
		}

		return null;
	}

	public Transform getCustomReferencePositionToKeepObject ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabbedWeaponInfo.useCustomReferencePositionToKeepObject) {
			return currentGrabbedWeaponInfo.customReferencePositionToKeepObject;
		}

		return null;
	}

	public void setNewGrabPhysicalObjectSystem (grabPhysicalObjectSystem newGrabPhysicalObjectSystem)
	{
		currentGrabPhysicalObjectSystem = newGrabPhysicalObjectSystem;
	}

	public void setNewGrabPhysicalObjectMeleeAttackSystem (grabPhysicalObjectMeleeAttackSystem newGrabPhysicalObjectMeleeAttackSystem)
	{
		if (!grabbedObjectMeleeAttackEnabled) {
			return;
		}

		currentGrabPhysicalObjectMeleeAttackSystem = newGrabPhysicalObjectMeleeAttackSystem;

		if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
			mainPlayerController.setPlayerUsingMeleeWeaponsState (true);

			currentGrabbedObjectTransform = currentGrabPhysicalObjectMeleeAttackSystem.transform;

			currentObjectRigidbody = currentGrabbedObjectTransform.GetComponent<Rigidbody> ();

			objectRotationPoint = currentGrabPhysicalObjectMeleeAttackSystem.objectRotationPoint;

			objectRotationPointParent = currentGrabPhysicalObjectMeleeAttackSystem.objectRotationPointParent;

			grabbedObjectMeleeAttackActive = true;

			currentHitCombat = currentGrabPhysicalObjectMeleeAttackSystem.getMainHitCombat ();

			if (useCustomLayerToDetectSurfaces) {
				currentHitCombat.setCustomLayerMask (customLayerToDetectSurfaces);
			}

			if (useCustomIgnoreTags) {
				currentHitCombat.setCustomTagsToIgnore (customTagsToIgnoreList);
			} else {
				currentHitCombat.setCustomTagsToIgnore (null);
			}

			currentHitCombat.setCustomDamageCanBeBlockedState (true);

			currentHitCombatBoxCollider = currentHitCombat.getMainCollider ().GetComponent<BoxCollider> ();

			originalHitCombatColliderSize = currentHitCombatBoxCollider.size;

			originalHitCombatColliderPosition = currentHitCombat.transform.localPosition;

			currentHitCombat.getOwner (playerControllerGameObject);

			currentHitCombat.setMainColliderEnabledState (true);
		
			attackListCount = currentGrabPhysicalObjectMeleeAttackSystem.getAttackListCount ();

			carryingObject = true;

			if (currentGrabPhysicalObjectMeleeAttackSystem.disableMeleeObjectCollider) {
				setGrabbedObjectClonnedColliderEnabledState (false);
			}

			checkGrabbedWeaponInfoStateAtStart (currentGrabPhysicalObjectMeleeAttackSystem.weaponInfoName, true);

			if (showDebugPrint) {
				print (currentGrabbedWeaponInfo.useEventsOnDamageDetected);
			}

			currentHitCombat.setSendMessageOnDamageDetectedState (currentGrabbedWeaponInfo.useEventsOnDamageDetected);

			if (currentGrabbedWeaponInfo.useEventsOnDamageDetected) {
				currentHitCombat.setCustomObjectToSendMessage (gameObject);
			}

			currentGrabPhysicalObjectMeleeAttackSystem.useCustomReferencePositionToKeepObjectMesh = currentGrabbedWeaponInfo.useCustomReferencePositionToKeepObjectMesh;

			if (currentGrabPhysicalObjectMeleeAttackSystem.useCustomReferencePositionToKeepObjectMesh) {
				currentGrabPhysicalObjectMeleeAttackSystem.customReferencePositionToKeepObjectMesh = currentGrabbedWeaponInfo.customReferencePositionToKeepObjectMesh;
			}
				
			checkEventOnGrabDropObject (true);

			raycastCheckTransfrom = currentGrabPhysicalObjectMeleeAttackSystem.raycastCheckTransfrom;

			mainMeleeWeaponsGrabbedManager.checkWeaponToStore (currentGrabPhysicalObjectMeleeAttackSystem.weaponName, currentGrabbedObjectTransform.gameObject);
		

			if (currentGrabbedWeaponInfo.isEmptyWeaponToUseOnlyShield) {
				mainGrabObjects.setGrabObjectsInputPausedState (true);
			} else {
				mainGrabObjects.setGrabObjectsInputPausedState (false);
			}


			if (currentGrabPhysicalObjectMeleeAttackSystem.isObjectThrown ()) {
				if (showDebugPrint) {
					print ("return weapon thrown");
				}

				objectThrown = true;

				surfaceNotFound = true;

				currentGrabbedObjectTransform.position = currentGrabPhysicalObjectSystem.getLastPositionBeforeGrabbed ();
				currentGrabbedObjectTransform.rotation = currentGrabPhysicalObjectSystem.getLastRotationBeforeGrabbed ();

				inputThrowOrReturnObject ();
			} else {
				checkGrabbedMeleeWeaponLocalPositionRotationValues ();
			}

			currentGrabPhysicalObjectSystem.setLastParentAssigned (null);

			meleeAttackTypesAmount.Clear ();


			for (int i = 0; i < meleeAttackTypes.Count; i++) {

				int meleeAttackAmount = 0;

				for (int j = 0; j < currentGrabPhysicalObjectMeleeAttackSystem.attackInfoList.Count; j++) {
					if (currentGrabPhysicalObjectMeleeAttackSystem.attackInfoList [j].attackType.Equals (meleeAttackTypes [i])) {
						meleeAttackAmount++;
					}
				}

				meleeAttackTypesAmount.Add (meleeAttackAmount);
			}

			checkIfCarryingShieldActive ();

			if (currentGrabPhysicalObjectMeleeAttackSystem.useAbilitiesListToEnableOnWeapon) {
				GKC_Utils.enableOrDisableAbilityGroupByName (playerControllerGameObject.transform, true, currentGrabPhysicalObjectMeleeAttackSystem.abilitiesListToEnableOnWeapon);
			}

			if (disableGrabObjectsInputWhenCarryingMeleeWeapon) {
				mainGrabObjects.setGrabObjectsInputDisabledState (true);
			}
		} else {
			removeGrabPhysicalObjectMeleeAttackSystem ();
		}
	}

	public List<int> getMeleeAttackTypesAmount ()
	{
		return meleeAttackTypesAmount;
	}

	bool removeWeaponsFromManager = true;

	public void setRemoveWeaponsFromManagerState (bool state)
	{
		removeWeaponsFromManager = state;
	}

	public void removeGrabPhysicalObjectMeleeAttackSystem ()
	{
		if (!carryingObject) {
			return;
		}

		mainPlayerController.setPlayerUsingMeleeWeaponsState (false);

		if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
			if (removeWeaponsFromManager) {
				mainMeleeWeaponsGrabbedManager.checkToDropWeaponFromList (currentGrabPhysicalObjectMeleeAttackSystem.weaponName);
			}

			if (objectThrown) {
				currentGrabPhysicalObjectMeleeAttackSystem.checkDisableTrail ();

				if (removeWeaponsFromManager) {
					currentGrabPhysicalObjectMeleeAttackSystem.setObjectThrownState (false);
				}
			}
		}

		stopActivateGrabbedObjectMeleeAttackCoroutine ();

		stopActivateDamageTriggerCoroutine ();

		resumeState ();

		if (currentHitCombat) {
			currentHitCombat.setMainColliderEnabledState (false);
		}

		drawOrSheatheShield (false);

		shieldActive = false;

		if (blockActive) {
			blockActivePreviously = false;

			disableBlockState ();
		}

		if (cuttingModeActive) {
			enableOrDisableCuttingMode (false);
		}

		if (meleeAttackInputPaused) {
			stopDisableMeleeAttackInputPausedStateWithDurationCoroutine ();

			mainGrabObjects.setGrabObjectsInputPausedState (false);

			meleeAttackInputPaused = false;
		}

		previousStrafeMode = false;

		if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
			previousStrafeMode = currentGrabPhysicalObjectMeleeAttackSystem.wasStrafeModeActivePreviously ();

			currentGrabPhysicalObjectMeleeAttackSystem.setPreviousStrafeModeState (false);

			if (currentGrabPhysicalObjectMeleeAttackSystem.useAbilitiesListToDisableOnWeapon) {
				GKC_Utils.enableOrDisableAbilityGroupByName (playerControllerGameObject.transform, false, currentGrabPhysicalObjectMeleeAttackSystem.abilitiesListToDisableOnWeapon);
			}
		}

		currentGrabPhysicalObjectMeleeAttackSystem = null;

		currentGrabPhysicalObjectSystem = null;

		grabbedObjectMeleeAttackActive = false;

		currentHitCombat = null;

		objectRotationPoint = null;

		objectRotationPointParent = null;

		carryingObject = false;

		blockActive = false;

		if (objectThrown) {
			stopThrowObjectCoroutine ();

			stopReturnObjectCoroutine ();

			eventOnDropObjectWhenIsThrown.Invoke ();
		}

		objectThrown = false;

		returningThrownObject = false;

		objectThrownTravellingToTarget = false;

		if (cuttingModeActive) {
			enableOrDisableCuttingMode (false);
		}

		cuttingModeActive = false;

		checkGrabbedWeaponInfoStateAtEnd ();

		checkEventOnGrabDropObject (false);

		updateShieldStateOnAnimator ();

		if (disableGrabObjectsInputWhenCarryingMeleeWeapon) {
			mainGrabObjects.setGrabObjectsInputDisabledState (false);
		}
	}

	public void drawOrKeepMeleeWeapon ()
	{
		if (mainMeleeWeaponsGrabbedManager != null) {
			if (mainMeleeWeaponsGrabbedManager.meleeWeaponsGrabbedManagerActive) {
				mainMeleeWeaponsGrabbedManager.inputDrawOrKeepMeleeWeapon ();
			}
		}
	}

	public void drawOrKeepMeleeWeaponWithoutCheckingInputActive ()
	{
		if (mainMeleeWeaponsGrabbedManager != null) {
			if (mainMeleeWeaponsGrabbedManager.meleeWeaponsGrabbedManagerActive) {
				mainMeleeWeaponsGrabbedManager.drawOrKeepMeleeWeaponWithoutCheckingInputActive ();
			}
		}
	}

	public void checkToKeepWeapon ()
	{
		if (mainMeleeWeaponsGrabbedManager != null) {
			mainMeleeWeaponsGrabbedManager.checkToKeepWeapon ();
		}
	}

	public void checkToKeepWeaponWithoutCheckingInputActive ()
	{
		if (mainMeleeWeaponsGrabbedManager != null) {
			mainMeleeWeaponsGrabbedManager.checkToKeepWeaponWithoutCheckingInputActive ();
		}
	}

	public void setAttackInputPausedForStaminaState (bool state)
	{
		attackInputPausedForStamina = state;
	}

	public void setThrowObjectInputPausedForStaminaState (bool state)
	{
		throwObjectInputPausedForStamina = state;
	}

	public void activateGrabbedObjectMeleeAttack (string attackType)
	{
		if (showDebugPrint) {
			print ("input activated");
		}

		if (!grabbedObjectMeleeAttackActive) {
			return;
		}

		if (meleeAttackInputPaused) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}

		if (showDebugPrint) {
			print ("1");
		}

		if (objectThrown) {
			return;
		}

		if (cuttingModeActive) {
			return;
		}

		attackActivatedOnAir = false;

		bool canActivateAttackOnAir = false;

		if (!mainPlayerController.isPlayerOnGround ()) {
			if (playerOnGroundToActivateAttack) {
				return;
			} else {
				canActivateAttackOnAir = true;

				attackActivatedOnAir = true;
			}
		}

		if (showDebugPrint) {
			print ("2");
		}

		if (attackInputPausedForStamina && generalStaminaUseMultiplier > 0) {
			if (showDebugPrint) {
				print ("Not enough stamina");
			}

			return;
		}

		if (!currentGrabPhysicalObjectMeleeAttackSystem.attacksEnabled) {
			return;
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.keepGrabbedObjectState) {
			return;
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.onlyAttackIfNoPreviousAttackInProcess) {
			if (attackInProcess) {
				return;
			}
		} 

		if (Time.time < lastTimeAttackActive + currentGrabPhysicalObjectMeleeAttackSystem.minDelayBetweenAttacks) {
			return;
		}

		if (!currentGrabPhysicalObjectMeleeAttackSystem.onlyAttackIfNoPreviousAttackInProcess) {
			if (currentAttackIndex > 0 && currentAttackInfo != null) {
//				print (currentAttackInfo.minDelayBeforeNextAttack);

				if (Time.time < lastTimeAttackActive + currentAttackInfo.minDelayBeforeNextAttack) {
					return;
				}
			}
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.resetIndexIfNotAttackAfterDelay && !attackInProcess) {
			if (Time.time > lastTimeAttackComplete + currentGrabPhysicalObjectMeleeAttackSystem.delayToResetIndexAfterAttack) {
//				print ("reset attack index");

				currentAttackIndex = 0;
			}
		}

		if (showDebugPrint) {
			print ("3");
		}

		if (useAttackTypes) {
			if (showDebugPrint) {
				print ("attack all conditions checked");
			}

			int numberOfAttacksSameType = 0;

			int numberOfAttacksAvailable = currentGrabPhysicalObjectMeleeAttackSystem.getAttackListCount ();

			for (int i = 0; i < numberOfAttacksAvailable; i++) {
				if (currentGrabPhysicalObjectMeleeAttackSystem.attackInfoList [i].attackType.Equals (attackType)) {
					numberOfAttacksSameType++;
				}
			}

			if (numberOfAttacksSameType == 1) {
				bool cancelAttack = false;

				if (attackInProcess) {
					cancelAttack = true;
				}

				if (Time.time < lastTimeAttackComplete + 0.4f) {
					cancelAttack = true;
				}

				if (cancelAttack) {
					if (showDebugPrint) {
						print ("just one attack type available and it is in process, avoiding to play it again");
					}

					return;
				}
			}
				
			bool attackFound = false;

			while (!attackFound) {
				currentAttackInfo = currentGrabPhysicalObjectMeleeAttackSystem.attackInfoList [currentAttackIndex];

//				print (currentAttackInfo.attackType + " " + attackType);

				if (currentAttackInfo.attackType.Equals (attackType)) {
					attackFound = true;

					setNextAttackIndex ();
				} else {
					setNextAttackIndex ();

					numberOfAttacksAvailable--;

					if (numberOfAttacksAvailable < 0) {
						return;
					}
				}
			}
		} else {
			currentAttackInfo = currentGrabPhysicalObjectMeleeAttackSystem.attackInfoList [currentAttackIndex];
		}

		if (canActivateAttackOnAir && currentAttackInfo.playerOnGroundToActivateAttack) {
			currentAttackIndex--;

			if (currentAttackIndex < 0) {
				currentAttackIndex = 0;
			}

			if (showDebugPrint) {
				print ("cancel attack on air");
			}

			return;
		}

		if (useStaminaOnAttackEnabled) {
			if (currentGrabPhysicalObjectMeleeAttackSystem.objectUsesStaminaOnAttacks) {
				mainStaminaSystem.activeStaminaStateWithCustomAmount (attackStaminaStateName, currentAttackInfo.staminaUsedOnAttack * generalStaminaUseMultiplier, currentAttackInfo.customRefillStaminaDelayAfterUse);	
			}
		}

		currentHitCombat.setNewHitDamage (currentAttackInfo.attackDamage * generalAttackDamageMultiplier);

		int attackID = currentAttackIndex - 1;

		if (attackID < 0) {
			attackID = currentGrabPhysicalObjectMeleeAttackSystem.getAttackListCount () - 1;
		}
		currentHitCombat.setTriggerId (attackID);

		currentAttackCanBeBlocked = true;

		if (currentGrabbedWeaponInfo.attacksCantBeBlocked) {
			bool attackCanBeBlocked = true;

			if (currentGrabbedWeaponInfo.attackIDCantBeBlockedList.Count > 0) {
				attackCanBeBlocked = !currentGrabbedWeaponInfo.attackIDCantBeBlockedList.Contains (attackID);
			}

			currentHitCombat.setCustomDamageCanBeBlockedState (attackCanBeBlocked);

			currentAttackCanBeBlocked = attackCanBeBlocked;
		}

		if (!currentAttackCanBeBlocked) {
			if (useEventsOnAttackCantBeBlocked) {
				eventOnAttackCantBeBlocked.Invoke ();
			}
		}

//		print ("attack can be blocked " + currentAttackCanBeBlocked + "  " + currentAttackInfo.customActionName + " " + attackID);

		stopActivateGrabbedObjectMeleeAttackCoroutine ();

		attackCoroutine = StartCoroutine (activateGrabbedObjectMeleeAttackCoroutine ());

		stopActivateDamageTriggerCoroutine ();

		damageTriggerCoroutine = StartCoroutine (activateDamageTriggerCoroutine ());

		if (!useAttackTypes) {
			setNextAttackIndex ();
		}
	}

	public void setNextAttackIndex ()
	{
		if (currentGrabPhysicalObjectMeleeAttackSystem.useRandomAttackIndex) {
			currentAttackIndex = Random.Range (0, attackListCount - 1);
		} else {
			currentAttackIndex++;

			if (currentAttackIndex >= attackListCount) {
				currentAttackIndex = 0;
			}
		}
	}

	public void stopActivateGrabbedObjectMeleeAttackCoroutine ()
	{
		if (attackCoroutine != null) {
			StopCoroutine (attackCoroutine);
		}
	}

	IEnumerator activateGrabbedObjectMeleeAttackCoroutine ()
	{
		if (blockActive) {
			blockActivePreviously = true;

			disableBlockState ();
		}

		if (canCancelBlockToStartAttackActive) {
			if (showDebugPrint) {
				print ("cancel block");
			}

			mainAnimator.SetBool (cancelBlockReactionStateName, true);

			yield return new WaitForSeconds (0.3f);

			mainAnimator.SetBool (cancelBlockReactionStateName, false);

//			checkDisableHasExitTimeAnimator ();

			canCancelBlockToStartAttackActive = false;
		}

		if (useMatchTargetSystemOnAttack) {

			bool useMatchTargetAttack = false;

			float matchPositionOffset = 0;

			if (currentAttackInfo.useMatchPositionSystem || ignoreAttackSettingToMatchTarget) {
				useMatchTargetAttack = true;
				matchPositionOffset = currentAttackInfo.matchPositionOffset;
			}

			if (currentGrabPhysicalObjectMeleeAttackSystem.useMatchPositionSystemOnAllAttacks || ignoreAttackSettingToMatchTarget) {
				useMatchTargetAttack = true;

				if (currentAttackInfo.useMatchPositionSystem) {
					matchPositionOffset = currentAttackInfo.matchPositionOffset;
				} else {
					matchPositionOffset = currentGrabPhysicalObjectMeleeAttackSystem.matchPositionOffsetOnAllAttacks;
				}
			}

			if (useMatchTargetAttack) {
				mainMatchPlayerToTargetSystem.activateMatchPosition (matchPositionOffset);
			}
		}

		lastTimeSurfaceAudioPlayed = 0;

		lastSurfaceDetecetedIndex = -1;

		checkEventsOnAttack (true);

		lastTimeAttackActive = Time.time;

		attackInProcess = true;

		mainGrabObjects.setGrabObjectsInputPausedState (true);

		if (currentAttackInfo.useRemoteEvent) {
			for (int i = 0; i < currentAttackInfo.remoteEventNameList.Count; i++) {
				mainRemoteEventSystem.callRemoteEvent (currentAttackInfo.remoteEventNameList [i]);
			}
		}

		if (currentAttackInfo.useCustomAction) {
			mainPlayerController.activateCustomAction (currentAttackInfo.customActionName);

			if (showDebugPrint) {
				print ("attack activated :" + mainPlayerController.getCurrentActionName ());
			}
		}

		float attackWaitDuration = currentAttackInfo.attackDuration / currentAttackInfo.attackAnimationSpeed;
	
		yield return new WaitForSeconds (attackWaitDuration);

		if (damageTriggerInProcess) {
			if (showDebugPrint) {
				print ("damage in process, waiting to finish");
			}

			while (damageTriggerInProcess) {

				yield return null;
			}
		}

		yield return null;

		lastTimeAttackComplete = Time.time;

		resumeState ();

		if (blockActivePreviously) {
			if (!blockInputPaused) {
				blockActivePreviously = false;

				yield return new WaitForSeconds (0.3f);

				setBlockActiveState (true);
			}
		}

		if (showDebugPrint) {
			print ("end of attack");
		}
	}

	public void stopActivateDamageTriggerCoroutine ()
	{
		if (damageTriggerCoroutine != null) {
			StopCoroutine (damageTriggerCoroutine);
		}
	}

	public void resetActivateDamageTriggerCoroutine ()
	{
//		print ("start of reset");

		if (damageTriggerInProcess) {
//			print ("checking");
			if (Time.time > lastTimeDamageTriggerActivated + 0.05f) {
				stopActivateDamageTriggerCoroutine ();

				damageTriggerCoroutine = StartCoroutine (activateDamageTriggerCoroutine ());

//				print ("reset attack for delay of " + (Mathf.Abs (Time.time - (lastTimeDamageTriggerActivated + 0.05f))));
			}
		}
	}

	IEnumerator activateDamageTriggerCoroutine ()
	{
		if (showDebugPrint) {
			print ("activate attack");
		}

		damageTriggerInProcess = true;

		lastTimeDamageTriggerActivated = Time.time;

		currentHitCombat.setCurrentState (false);

		currentHitCombat.setIgnoreDetectedObjectsOnListState (false);

		if (currentGrabPhysicalObjectMeleeAttackSystem.useGeneralDamageTypeID) {
			currentHitCombat.setNewDamageTypeID (currentGrabPhysicalObjectMeleeAttackSystem.generalDamageTypeID);
		} else {
			currentHitCombat.setNewDamageTypeID (currentAttackInfo.damageTypeID);
		}

		int numberOfEventsTriggered = 0;

		float timer = Time.time;

		allEventsTriggered = false;

		bool useAnimationPercentageDuration = currentGrabPhysicalObjectMeleeAttackSystem.useAnimationPercentageDuration;
		bool useAnimationPercentageOver100 = currentGrabPhysicalObjectMeleeAttackSystem.useAnimationPercentageOver100;

		int damageTriggerActiveInfoListCount = currentAttackInfo.damageTriggerActiveInfoList.Count;

		bool canActivateCurrentEvent = false;

		float currentAttackDuration = currentAttackInfo.attackDuration / currentAttackInfo.attackAnimationSpeed;


		if (currentAttackInfo.useSingleSlashDamageInfo) {
			if (hitCombatParentChanged) {
				currentHitCombat.transform.SetParent (objectRotationPoint);
				currentHitCombat.transform.localPosition = originalHitCombatColliderPosition;
				currentHitCombat.transform.localRotation = Quaternion.identity;

				Transform raycastCheckTransformParent = currentGrabPhysicalObjectMeleeAttackSystem.raycastCheckTransformParent;
				raycastCheckTransformParent.SetParent (objectRotationPoint);
				raycastCheckTransformParent.localPosition = Vector3.zero;
				raycastCheckTransformParent.localRotation = Quaternion.identity;

				hitCombatParentChanged = false;
			}

			setHitCombatScale (originalHitCombatColliderSize);

			float delayToActivateSingleSlashDamageTrigger = currentAttackInfo.delayToActivateSingleSlashDamageTrigger;
			float delayToDeactivateSingleSlashDamageTrigger = currentAttackInfo.delayToDeactivateSingleSlashDamageTrigger;

			if (useAnimationPercentageDuration) {

				if (useAnimationPercentageOver100) {
					delayToActivateSingleSlashDamageTrigger /= 100;
					delayToDeactivateSingleSlashDamageTrigger /= 100;
				}

				delayToActivateSingleSlashDamageTrigger = currentAttackDuration * delayToActivateSingleSlashDamageTrigger;
				delayToDeactivateSingleSlashDamageTrigger = currentAttackDuration * delayToDeactivateSingleSlashDamageTrigger;
			}

			bool activateDamageTriggered = false;
			bool deactivateDamageTriggered = false;

			bool currentActivateDamageTriggerValue = false;

			float currentDelay = 0;

			while (!allEventsTriggered) {
				bool animatorIsPaused = mainAnimator.speed <= 0;

				if (animatorIsPaused) {
					timer += Time.deltaTime;
				}

				if (!activateDamageTriggered) {
					currentActivateDamageTriggerValue = true;
				} else {
					currentActivateDamageTriggerValue = false;
				}

				if (!activateDamageTriggered || !deactivateDamageTriggered) {

					canActivateCurrentEvent = false;

					if (useAnimationPercentageDuration) {
						if (!activateDamageTriggered) {
							currentDelay = delayToActivateSingleSlashDamageTrigger;
						} else {
							currentDelay = delayToDeactivateSingleSlashDamageTrigger;
						}
					} else {
						if (!activateDamageTriggered) {
							currentDelay = currentAttackInfo.delayToActivateSingleSlashDamageTrigger;
						} else {
							currentDelay = currentAttackInfo.delayToDeactivateSingleSlashDamageTrigger;
						}
					}

					if (Time.time > timer + currentDelay) {
						canActivateCurrentEvent = true;
					}
						
					if (canActivateCurrentEvent) {

						bool activateDamageTrigger = currentActivateDamageTriggerValue;

						currentHitCombat.setCurrentState (activateDamageTrigger);

						if (activateDamageTrigger && currentAttackInfo.ignoreDetectedObjectsOnList) {
							currentHitCombat.setIgnoreDetectedObjectsOnListState (true);
						}

						currentHitCombat.setNewDamageReactionID (currentAttackInfo.damageReactionID);

						numberOfEventsTriggered++;

						if (!activateDamageTriggered) {
							activateDamageTriggered = true;

//							print ("activate first delay");
						} else {
							deactivateDamageTriggered = true;

//							print ("activate second delay");
						}

						if ((activateDamageTriggered && deactivateDamageTriggered) || numberOfEventsTriggered == 2) {
							allEventsTriggered = true;
						}
								
						if (activateDamageTrigger) {
							checkSurfaceFoundOnAttack (false);
						}
					}
				}

				yield return null;
			}

//			print ("end of attack");
		} else {

			grabPhysicalObjectMeleeAttackSystem.damageTriggerActiveInfo currentdamageTriggerActiveInfo = null;

			for (int i = 0; i < damageTriggerActiveInfoListCount; i++) {
				currentdamageTriggerActiveInfo = currentAttackInfo.damageTriggerActiveInfoList [i];

				currentdamageTriggerActiveInfo.delayTriggered = false;

				if (useAnimationPercentageDuration) {
					float currentDelay = currentdamageTriggerActiveInfo.delayToActiveTrigger;

					if (currentDelay > 1) {
						print ("ERRRORORORROOROROR: DELAY IS HIGHER THAN 1 FIXXXXXXXXXXX----------------------------------------" +
						".............................................");
					}

					if (useAnimationPercentageOver100) {
						currentDelay /= 100;
					}

					currentdamageTriggerActiveInfo.calculatedPercentageAttackDuration = currentAttackDuration * currentDelay;
				}
			}

			detectedObjectsOnReturn.Clear ();

			while (!allEventsTriggered) {
				if (currentAttackInfo == null) {
					allEventsTriggered = true;

				} else {
					bool animatorIsPaused = mainAnimator.speed <= 0;

					if (animatorIsPaused) {
						timer += Time.deltaTime;
					}

					for (int i = 0; i < damageTriggerActiveInfoListCount; i++) {

						currentdamageTriggerActiveInfo = currentAttackInfo.damageTriggerActiveInfoList [i];

						if (!currentdamageTriggerActiveInfo.delayTriggered) {

							canActivateCurrentEvent = false;

							if (useAnimationPercentageDuration) {
								if (Time.time > timer + currentdamageTriggerActiveInfo.calculatedPercentageAttackDuration) {
									canActivateCurrentEvent = true;
								}
							} else {
								if (Time.time > timer + currentdamageTriggerActiveInfo.delayToActiveTrigger) {
									canActivateCurrentEvent = true;
								}
							}

							//print (currentdamageTriggerActiveInfo.delayToActiveTrigger + " " + i);

							if (canActivateCurrentEvent) {

								bool activateDamageTrigger = currentdamageTriggerActiveInfo.activateDamageTrigger;

								currentHitCombat.setCurrentState (activateDamageTrigger);

								if (activateDamageTrigger && currentAttackInfo.ignoreDetectedObjectsOnList) {
									currentHitCombat.setIgnoreDetectedObjectsOnListState (true);
								}

								currentHitCombat.setNewDamageReactionID (currentAttackInfo.damageReactionID);

								numberOfEventsTriggered++;

								currentdamageTriggerActiveInfo.delayTriggered = true;

								if (damageTriggerActiveInfoListCount == numberOfEventsTriggered) {
									allEventsTriggered = true;
								}

								if (currentdamageTriggerActiveInfo.setNewTriggerScale) {
									if (currentdamageTriggerActiveInfo.setOriginalScale) {
										setHitCombatScale (originalHitCombatColliderSize);
									} else {
										setHitCombatScale (currentdamageTriggerActiveInfo.newTriggerScale);
									}
								}

								if (currentdamageTriggerActiveInfo.changeDamageTriggerToLimb) {
									Transform newParent = getCharacterHumanBone (currentdamageTriggerActiveInfo.limbToPlaceTrigger);

									if (newParent != null) {
										currentHitCombat.transform.SetParent (newParent);
										currentHitCombat.transform.localPosition = Vector3.zero;
										currentHitCombat.transform.localRotation = Quaternion.identity;

										Transform raycastCheckTransformParent = currentGrabPhysicalObjectMeleeAttackSystem.raycastCheckTransformParent;
										raycastCheckTransformParent.SetParent (currentHitCombat.transform);
										raycastCheckTransformParent.localPosition = Vector3.zero;
										raycastCheckTransformParent.localRotation = Quaternion.identity;

										hitCombatParentChanged = true;
									}
								} else {
									if (hitCombatParentChanged) {
										currentHitCombat.transform.SetParent (objectRotationPoint);
										currentHitCombat.transform.localPosition = originalHitCombatColliderPosition;
										currentHitCombat.transform.localRotation = Quaternion.identity;

										Transform raycastCheckTransformParent = currentGrabPhysicalObjectMeleeAttackSystem.raycastCheckTransformParent;
										raycastCheckTransformParent.SetParent (objectRotationPoint);
										raycastCheckTransformParent.localPosition = Vector3.zero;
										raycastCheckTransformParent.localRotation = Quaternion.identity;

										hitCombatParentChanged = false;
									}
								}

								if (currentdamageTriggerActiveInfo.useEventOnAttack) {
									currentdamageTriggerActiveInfo.eventOnAtatck.Invoke ();
								}

								if (currentdamageTriggerActiveInfo.useRangeAttackID && currentGrabbedWeaponInfo.useRangeAttackID) {

									int rangeAttackID = currentdamageTriggerActiveInfo.rangeAttackID;

									for (int k = 0; k < currentGrabbedWeaponInfo.rangeAttackInfoList.Count; k++) {
										if (currentGrabbedWeaponInfo.rangeAttackInfoList [k].rangeAttackID == rangeAttackID) {
											currentGrabbedWeaponInfo.rangeAttackInfoList [k].eventOnRangeAttack.Invoke ();
										}
									}
								}

								if (currentdamageTriggerActiveInfo.disableRangeAttackID && currentGrabbedWeaponInfo.useRangeAttackID) {

									int rangeAttackID = currentdamageTriggerActiveInfo.rangeAttackIDToDisable;

									for (int k = 0; k < currentGrabbedWeaponInfo.rangeAttackInfoList.Count; k++) {
										if (currentGrabbedWeaponInfo.rangeAttackInfoList [k].rangeAttackID == rangeAttackID) {
											currentGrabbedWeaponInfo.rangeAttackInfoList [k].eventOnDisableRangeAttack.Invoke ();
										}
									}
								}

								if (activateDamageTrigger) {
									checkSurfaceFoundOnAttack (false);
								}

								detectedObjectsOnReturn.Clear ();
							} else {
								if (checkSurfacesWithCapsuleRaycastEnabled) {
									if (currentdamageTriggerActiveInfo.checkSurfacesWithCapsuleRaycast &&
									    currentdamageTriggerActiveInfo.activateDamageTrigger) {
										checkSurfacesDetectedRaycast (currentdamageTriggerActiveInfo.checkSurfaceCapsuleRaycastRadius);

										int hitsLenght = hits.Length;

										if (hitsLenght > 0) {
											for (int j = 0; j < hitsLenght; j++) {
												GameObject currentObject = hits [j].collider.gameObject;

												if (!detectedObjectsOnReturn.Contains (currentObject)) {
													currentHitCombat.checkTriggerInfo (hits [j].collider, true);
										
													detectedObjectsOnReturn.Add (currentObject);
												}
											}
										}
									}
								}
							}
						}
					}
				}

				yield return null;
			}

			yield return null;
		}

		damageTriggerInProcess = false;
	}

	public Transform getCharacterHumanBone (HumanBodyBones boneToFind)
	{
		return mainPlayerController.getCharacterHumanBone (boneToFind);
	}

	public Transform getCurrentGrabbedObjectTransform ()
	{
		return currentGrabbedObjectTransform;
	}

	public string getCurrentMeleeWeaponTypeName ()
	{
		if (currentGrabbedWeaponInfo != null) {
			return currentGrabbedWeaponInfo.Name;
		}

		return "";
	}

	public string getCurrentMeleeWeaponName ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabPhysicalObjectMeleeAttackSystem != null) {
			return currentGrabPhysicalObjectMeleeAttackSystem.weaponName;
		}

		return "";
	}

	public bool isSecondaryAbilityActiveOnCurrentWeapon ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabPhysicalObjectMeleeAttackSystem != null) {
			return currentGrabPhysicalObjectMeleeAttackSystem.isSecondaryAbilityActive ();
		}

		return false;
	}

	public void disableCurrentAttackInProcess ()
	{
		if (attackInProcess) {
			stopActivateGrabbedObjectMeleeAttackCoroutine ();

			stopActivateDamageTriggerCoroutine ();

			currentHitCombat.setCurrentState (false);

			currentHitCombat.setIgnoreDetectedObjectsOnListState (false);

			lastTimeAttackComplete = Time.time;

			resumeState ();
		
			blockActivePreviously = false;
		}

		if (objectThrown) {
			if (!continueObjecThrowActivated && !objectThrownTravellingToTarget) {
				cancelThrowObject ();
			}
		}
	}

	void cancelThrowObject ()
	{
		stopThrowObjectCoroutine ();

		objectThrown = false;

		currentGrabPhysicalObjectMeleeAttackSystem.setObjectThrownState (false);

		if (shieldActive) {
			setShieldParentState (true);
		}

		if (showDebugPrint) {
			print ("cancel throw object");
		}
	}


	//START OF BLOCK FUNCTIONS
	void checkDisableHasExitTimeAnimator ()
	{
		if (disableHasExitTimeCoroutine != null) {
			StopCoroutine (disableHasExitTimeCoroutine);
		}

		disableHasExitTimeCoroutine = StartCoroutine (checkDisablehasExitTimeAnimatorCoroutine ());
	}

	IEnumerator checkDisablehasExitTimeAnimatorCoroutine ()
	{
		mainAnimator.SetBool (cancelBlockReactionStateName, true);

		yield return new WaitForSeconds (0.2f);

		mainAnimator.SetBool (cancelBlockReactionStateName, false);
	}

	//CALLED ON DODGE/ROLL ACTION SYSTEM
	public void checkIfBlockInputIsCurrentlyInUse ()
	{
//		print ("check if block input is in use");
		if (playerInput.isKeyboardButtonPressed (mainMeleeCombatAxesInputName, mainMeleeCombatBlockInputName)) {
			if (showDebugPrint) {
				print ("block is being pressed");
			}
		} else {
			if (showDebugPrint) {
				print ("block is not being pressed, disabling block");
			}

			disableBlockStateInProcess ();
		}
	}

	public void disableBlockStateInProcess ()
	{
		if (blockActive) {
			blockActivePreviously = false;

			setBlockActiveState (false);
		}
	}

	public void disableBlockActiveState ()
	{
		inputDeactivateBlock ();
	}

	public void checkEventsOnBlockDamage (bool state)
	{
		if (useEventsOnBlockDamage) {
			if (state) {
				eventOnBlockActivate.Invoke ();
			} else {
				eventOnBlockDeactivate.Invoke ();	
			}
		}
	}

	IEnumerator disableMeleeAttackInputPausedStateWithDurationCoroutine (float pauseDuration)
	{
		meleeAttackInputPaused = true;

		mainGrabObjects.setGrabObjectsInputPausedState (true);

		yield return new WaitForSeconds (pauseDuration);

		mainGrabObjects.setGrabObjectsInputPausedState (false);

		meleeAttackInputPaused = false;
	}

	public void checkIfBlockActive ()
	{
		if (blockActive) {
			setBlockActiveState (true);
		}
	}

	public void setBlockActiveState (bool state)
	{
		if (state) {
			if (currentGrabPhysicalObjectMeleeAttackSystem.canUseBlock) {
				if (shieldActive && currentGrabbedWeaponInfo.weaponCanUseShield) {
					if (mainPlayerController.isStrafeModeActive ()) {
						if (currentGrabbedWeaponInfo.strafeMovementBlockShieldID > 0) {
							mainPlayerController.setCurrentStrafeIDValue (currentGrabbedWeaponInfo.strafeMovementBlockShieldID);
						}
					} else {
						if (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName != "") {
							mainPlayerController.activateCustomAction (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName);
						}
					}
				} else {
					mainPlayerController.activateCustomAction (currentGrabPhysicalObjectMeleeAttackSystem.blockActionName);

					mainPlayerController.setCurrentStrafeIDValue (currentGrabbedWeaponInfo.strafeIDUsed);
				}

				mainHealth.setBlockDamageActiveState (true);

				if (reducedBlockDamageProtectionActive) {
					mainHealth.setBlockDamageProtectionAmount (currentGrabPhysicalObjectMeleeAttackSystem.reducedBlockDamageProtectionAmount * generalBlockProtectionMultiplier);
				} else {
					mainHealth.setBlockDamageProtectionAmount (currentGrabPhysicalObjectMeleeAttackSystem.blockDamageProtectionAmount * generalBlockProtectionMultiplier);
				}

				mainHealth.setBlockDamageRangleAngleState (currentGrabPhysicalObjectMeleeAttackSystem.useMaxBlockRangeAngle,
					currentGrabPhysicalObjectMeleeAttackSystem.maxBlockRangeAngle);

				if (shieldActive) {
					if (mainPlayerController.isStrafeModeActive ()) {
						mainHealth.setHitReactionBlockIDValue (currentGrabbedWeaponInfo.shieldIDStrafeMovement);
					} else {
						mainHealth.setHitReactionBlockIDValue (currentGrabbedWeaponInfo.shieldIDFreeMovement);
					}
				} else {
					mainHealth.setHitReactionBlockIDValue (currentGrabPhysicalObjectMeleeAttackSystem.blockID);
				}

				blockActive = true;

				checkEventsOnBlockDamage (true);
			}
		} else {
			if (currentGrabPhysicalObjectMeleeAttackSystem.canUseBlock) {
				disableBlockState ();
			}
		}

		updateShieldStateOnAnimator ();
	}

	public void updateRegularBlockDamageProtectionValue ()
	{
		setBlockDamageProtectionValue (false);
	}

	public void updateReducedBlockDamageProtectionValue ()
	{
		setBlockDamageProtectionValue (true);
	}

	public void setBlockDamageProtectionValue (bool state)
	{
		reducedBlockDamageProtectionActive = state;

		if (blockActive) {
			if (!carryingObject) {
				return;
			}

			if (currentGrabPhysicalObjectMeleeAttackSystem.canUseBlock) {
				if (reducedBlockDamageProtectionActive) {
					mainHealth.setBlockDamageProtectionAmount (currentGrabPhysicalObjectMeleeAttackSystem.reducedBlockDamageProtectionAmount * generalBlockProtectionMultiplier);
				} else {
					mainHealth.setBlockDamageProtectionAmount (currentGrabPhysicalObjectMeleeAttackSystem.blockDamageProtectionAmount * generalBlockProtectionMultiplier);
				}
			}
		}
	}

	void disableBlockState ()
	{
		if (shieldActive && currentGrabbedWeaponInfo.weaponCanUseShield) {
			if (mainPlayerController.isStrafeModeActive ()) {
				if (currentGrabbedWeaponInfo.strafeMovementBlockShieldID > 0) {
					mainPlayerController.setCurrentStrafeIDValue (currentGrabbedWeaponInfo.strafeIDUsed);
				}
			} else {
				if (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName != "") {
					mainPlayerController.stopCustomAction (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName);
				}
			}
		} else {
			mainPlayerController.stopCustomAction (currentGrabPhysicalObjectMeleeAttackSystem.blockActionName);
		}

		mainHealth.setBlockDamageActiveState (false);

		blockActive = false;

		checkEventsOnBlockDamage (false);

		mainHealth.setHitReactionBlockIDValue (-1);
	}

	void updateBlockShieldState ()
	{
//		print ("idhsidhs " + shieldActive + "  " + blockActive + "  " + currentGrabbedWeaponInfo.weaponCanUseShield);
		if (shieldActive && blockActive && currentGrabbedWeaponInfo.weaponCanUseShield) {
			mainPlayerController.updateStrafeModeActiveState ();

//			print (mainPlayerController.isLookAlwaysInCameraDirectionActive ());

			if (mainPlayerController.isLookAlwaysInCameraDirectionActive ()) {
				if (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName != "") {
					mainPlayerController.stopCustomAction (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName);
				}

				if (currentGrabbedWeaponInfo.strafeMovementBlockShieldID > 0) {
					mainPlayerController.setCurrentStrafeIDValue (currentGrabbedWeaponInfo.strafeMovementBlockShieldID);
				}

				mainHealth.setHitReactionBlockIDValue (currentGrabbedWeaponInfo.shieldIDStrafeMovement);
			} else {
				if (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName != "") {
					mainPlayerController.activateCustomAction (currentGrabbedWeaponInfo.regularMovementBlockShieldActionName);
				}

				if (currentGrabbedWeaponInfo.strafeMovementBlockShieldID > 0) {
					mainPlayerController.setCurrentStrafeIDValue (currentGrabbedWeaponInfo.strafeIDUsed);
				}

				mainHealth.setHitReactionBlockIDValue (currentGrabbedWeaponInfo.shieldIDFreeMovement);
			}
		} 
	}

	public void disableBlockInputPausedStateWithDuration (float pauseDuration)
	{
		stopDisableBlockInputPausedStateWithDurationCoroutine ();

		pauseBlockInputCoroutine = StartCoroutine (disableBlockInputPausedStateWithDurationCoroutine (pauseDuration));
	}

	void stopDisableBlockInputPausedStateWithDurationCoroutine ()
	{
		if (pauseBlockInputCoroutine != null) {
			StopCoroutine (pauseBlockInputCoroutine);
		}
	}

	IEnumerator disableBlockInputPausedStateWithDurationCoroutine (float pauseDuration)
	{
		blockInputPaused = true;

		mainGrabObjects.setGrabObjectsInputPausedState (true);

		yield return new WaitForSeconds (pauseDuration);

		mainGrabObjects.setGrabObjectsInputPausedState (false);

		blockInputPaused = false;

		if (blockActivePreviously) {
			blockActivePreviously = false;

			yield return new WaitForSeconds (0.3f);

			setBlockActiveState (true);
		}
	}

	public void setCanCancelBlockToStartAttackActiveState (bool state)
	{
		canCancelBlockToStartAttackActive = state;
	}
	//END OF BLOCK FUNCTIONS


	//START SHIELD FUNCTIONS
	public void setShieldActiveState (bool state)
	{
		shieldActive = state;

//		print ("shield state " + shieldActive + " " + carryingObject);

		if (!carryingObject) {
			if (mainMeleeWeaponsGrabbedManager.currentMeleeWeaponSheathedOrCarried) {
				shieldActive = false;
			} else {
				if (shieldCanBeUsedWithoutMeleeWeapon) {
					if (showDebugPrint) {
						print ("not carrying melee weapon, using shield without weapon");
					}

					bool emptyMeleeWeaponForShieldFound = false;

					for (int i = 0; i < grabbedWeaponInfoList.Count; i++) {
						if (grabbedWeaponInfoList [i].isEmptyWeaponToUseOnlyShield) {
							if (!grabbedObjectMeleeAttackActive) {
								eventToActivateMeleeModeWhenUsingShieldWithoutMeleeWeapon.Invoke ();
							}

							mainMeleeWeaponsGrabbedManager.checkMeleeWeaponToUse (grabbedWeaponInfoList [i].Name, false);

							emptyMeleeWeaponForShieldFound = true;
						}
					}

					updateShieldStateOnAnimator ();

					if (emptyMeleeWeaponForShieldFound) {
						return;
					}
				} else {
					shieldActive = false;
				}
			}
		}

		if (currentGrabbedWeaponInfo != null && !currentGrabbedWeaponInfo.weaponCanUseShield) {
			shieldActive = false;
		}

		if (currentShieldGameObject != null) {
//			print (shieldActive);

			currentShieldGameObject.SetActive (shieldActive);

			if (shieldActive) {
				setShieldParentState (true);
			}
		}

		updateShieldStateOnAnimator ();
	}

	public string getEmptyWeaponToUseOnlyShield ()
	{
		for (int i = 0; i < grabbedWeaponInfoList.Count; i++) {
			if (grabbedWeaponInfoList [i].isEmptyWeaponToUseOnlyShield) {
				return grabbedWeaponInfoList [i].Name;
			}
		}

		return "";
	}

	public void setShieldActiveFieldValueDirectly (bool state)
	{
		shieldActive = state;

		updateShieldStateOnAnimator ();
	}

	void updateShieldStateOnAnimator ()
	{
		if (blockActive) {
			if (shieldActive) {
				mainPlayerController.setCurrentShieldActiveValue (2);
			} else {
				mainPlayerController.setCurrentShieldActiveValue (0);
			}
		} else {
			if (shieldActive) {
				mainPlayerController.setCurrentShieldActiveValue (1);
			} else {
				mainPlayerController.setCurrentShieldActiveValue (0);
			}
		}
	}

	public void drawOrSheatheShield (bool state)
	{
		if (carryingShield) {
			if (currentShieldGameObject != null) {

				currentShieldGameObject.SetActive (true);

				if (shieldActive) {
					setShieldParentState (state);
				}
					
				if (blockActive) {
					disableShieldBlockActive ();
				}
			}
		}
	}

	public void setShieldParentState (bool state)
	{
		if (state) {
			currentShieldGameObject.transform.SetParent (shieldLeftHandMountPoint);

			currentShieldGameObject.transform.localPosition = currentShieldHandMountPointTransformReference.localPosition;
			currentShieldGameObject.transform.localRotation = currentShieldHandMountPointTransformReference.localRotation;
		} else {
			currentShieldGameObject.transform.SetParent (shieldBackMountPoint);

			currentShieldGameObject.transform.localPosition = currentShieldBackMountPointTransformReference.localPosition;
			currentShieldGameObject.transform.localRotation = currentShieldBackMountPointTransformReference.localRotation;
		}
	}

	public void checkIfCarryingShieldActive ()
	{
		setShieldActiveState (carryingShield);
	}

	public void setShieldInfo (string newShieldName, GameObject newShieldGameObject, Transform newShieldHandMountPointTransformReference, 
	                           Transform newShieldBackMountPointTransformReference, bool state)
	{
		currentShieldName = newShieldName;

		currentShieldGameObject = newShieldGameObject;

		currentShieldHandMountPointTransformReference = newShieldHandMountPointTransformReference;

		currentShieldBackMountPointTransformReference = newShieldBackMountPointTransformReference;

		carryingShield = state;

		if (blockActive) {
			disableShieldBlockActive ();
		}

		if (!carryingShield) {
			if (carryingObject) {
				if (shieldCanBeUsedWithoutMeleeWeapon) {
					if (currentGrabbedWeaponInfo.isEmptyWeaponToUseOnlyShield) {
						mainMeleeWeaponsGrabbedManager.disableCurrentMeleeWeapon (currentGrabbedWeaponInfo.Name);

						print ("removing empty weapon used only when carrying just the shield");
					}
				}
			}
		}
	}

	void disableShieldBlockActive ()
	{
		setBlockActiveState (false);

		if (showDebugPrint) {
			print ("check disable shield block state");
		}
	}
	//END SHIELD FUNCTIONS


	void setHitCombatScale (Vector3 newScale)
	{
		if (currentHitCombatBoxCollider != null) {
			currentHitCombatBoxCollider.size = newScale;
		}
	}

	public void resumeState ()
	{
		attackInProcess = false;

		cutActivatedOnAttackChecked = false;

		damageTriggerInProcess = false;

		if (currentHitCombat != null) {
			currentHitCombat.setCurrentState (false);

			setHitCombatScale (originalHitCombatColliderSize);

			if (hitCombatParentChanged) {
				currentHitCombat.transform.SetParent (objectRotationPoint);
				currentHitCombat.transform.localPosition = originalHitCombatColliderPosition;
				currentHitCombat.transform.localRotation = Quaternion.identity;

				Transform raycastCheckTransformParent = currentGrabPhysicalObjectMeleeAttackSystem.raycastCheckTransformParent;
				raycastCheckTransformParent.SetParent (objectRotationPoint);
				raycastCheckTransformParent.localPosition = Vector3.zero;
				raycastCheckTransformParent.localRotation = Quaternion.identity;

				hitCombatParentChanged = false;
			}
		}

		checkEventsOnAttack (false);

		mainGrabObjects.setGrabObjectsInputPausedState (false);
	}

	public void checkEventsOnAttack (bool state)
	{
		if (useEventsOnAttack) {
			if (state) {
				eventOnAttackStart.Invoke ();
			} else {
				eventOnAttackEnd.Invoke ();
			}
		}
	}

	public void checkEventWhenKeepingOrDrawingMeleeWeapon (bool state)
	{
		if ((carryingObject || state) && currentGrabPhysicalObjectMeleeAttackSystem != null) {
			currentGrabPhysicalObjectMeleeAttackSystem.checkEventWhenKeepingOrDrawingMeleeWeapon (state);

			if (currentGrabbedWeaponInfo != null) {

				if (currentGrabbedWeaponInfo.useEventsOnKeepOrDrawMeleeWeapon) {
					if (state) {
						currentGrabbedWeaponInfo.eventsOnKeepMeleeWeapon.Invoke ();
					} else {
						currentGrabbedWeaponInfo.eventsOnDrawMeleeWeapon.Invoke ();
					}
				}
			}
		}
	}

	public void setRemoteEventOnCurrentMeleeWeapon (string remoteEventName)
	{
		if (carryingObject && currentGrabPhysicalObjectMeleeAttackSystem != null) {
			remoteEventSystem currentRemoteEventSystem = currentGrabPhysicalObjectMeleeAttackSystem.GetComponent<remoteEventSystem> ();

			if (currentRemoteEventSystem != null) {

				if (showDebugPrint) {
					print ("remote event on weapon activated");
				}
					
				currentRemoteEventSystem.callRemoteEvent (remoteEventName);
			}
		}
	}

	public void setRemoteEventsOnCurrentMeleeWeapon (List<string> remoteEventNameList, GameObject weaponObject)
	{
		if (weaponObject != null) {
			remoteEventSystem currentRemoteEventSystem = weaponObject.GetComponent<remoteEventSystem> ();

			if (currentRemoteEventSystem != null) {

				if (showDebugPrint) {
					print ("remote event on weapon activated");
				}

				for (int i = 0; i < remoteEventNameList.Count; i++) {
					currentRemoteEventSystem.callRemoteEvent (remoteEventNameList [i]);
				}
			}
		}
	}

	public bool setDamageTypeAndReactionInfo (string newBuffObjectName)
	{
		if (carryingObject && currentGrabPhysicalObjectMeleeAttackSystem != null) {
			return currentGrabPhysicalObjectMeleeAttackSystem.setDamageTypeAndReactionInfo (newBuffObjectName);
		}

		return false;
	}

	public void checkGrabbedWeaponInfoStateAtStart (string weaponInfoName, bool checkIfWeaponThrown)
	{
		currentGrabbedWeaponInfo = null;

		for (int i = 0; i < grabbedWeaponInfoList.Count; i++) {
			if (grabbedWeaponInfoList [i].Name.Equals (weaponInfoName)) {
				currentGrabbedWeaponInfo = grabbedWeaponInfoList [i];

//				print (weaponInfoName + " " + currentGrabbedWeaponInfo.useStrafeMode);

				bool setWeaponState = true;

				if (checkIfWeaponThrown && currentGrabPhysicalObjectMeleeAttackSystem.isObjectThrown ()) {
					setWeaponState = false;
				}

				if (setWeaponState) {
					if (removeWeaponsFromManager) {
						currentGrabPhysicalObjectMeleeAttackSystem.setPreviousStrafeModeState (mainPlayerController.isStrafeModeActive ());

						currentGrabbedWeaponInfo.previousStrafeMode = mainPlayerController.isStrafeModeActive ();
					} else {
						currentGrabPhysicalObjectMeleeAttackSystem.setPreviousStrafeModeState (false);

						currentGrabbedWeaponInfo.previousStrafeMode = false;

						mainPlayerController.activateOrDeactivateStrafeMode (false);

						setStrafeModeOnAISystemState (false);
					}

					if (currentGrabbedWeaponInfo.useStrafeMode) {
						mainPlayerController.activateOrDeactivateStrafeMode (true);

						setStrafeModeOnAISystemState (true);
					} else {
						if (!checkIfWeaponThrown) {
							if (showDebugPrint) {
								print ("checking strafe mode when returning thrown weapon");
							}

							currentGrabPhysicalObjectMeleeAttackSystem.setPreviousStrafeModeState (false);

							currentGrabbedWeaponInfo.previousStrafeMode = false;

							mainPlayerController.activateOrDeactivateStrafeMode (false);
						}
					}

					if (currentGrabbedWeaponInfo.toggleStrafeModeIfRunningActive) {
						mainPlayerController.setDisableStrafeModeExternallyIfIncreaseWalkSpeedActiveState (true);
					}

					mainPlayerController.setCurrentStrafeIDValue (currentGrabbedWeaponInfo.strafeIDUsed);

					if (currentGrabbedWeaponInfo.setNewIdleID) {
						mainPlayerController.setCurrentIdleIDValue (currentGrabbedWeaponInfo.idleIDUsed);
					}
				
					if (currentGrabbedWeaponInfo.setNewCrouchID) {
						mainPlayerController.setCurrentCrouchIDValue (currentGrabbedWeaponInfo.crouchIDUsed);
					}

					if (currentGrabbedWeaponInfo.setNewMovementID) {
						mainPlayerController.setPlayerStatusIDValue (currentGrabbedWeaponInfo.movementIDUsed);
					}

					if (currentGrabbedWeaponInfo.setSprintEnabledStateWithWeapon) {
						if (!currentGrabbedWeaponInfo.sprintEnabledStateWithWeapon && mainPlayerController.isPlayerRunning ()) {
							mainPlayerController.stopSprint ();
						}

						mainPlayerController.setSprintEnabledState (currentGrabbedWeaponInfo.sprintEnabledStateWithWeapon);
					}

					if (currentGrabbedWeaponInfo.useEventsOnGrabDropObject) {
						currentGrabbedWeaponInfo.eventOnGrabObject.Invoke ();
					}

					if (currentGrabbedWeaponInfo.useRemoteEventsOnGrabObject) {
						setRemoteEventsOnCurrentMeleeWeapon (currentGrabbedWeaponInfo.remoteEventOnGrabObject, currentGrabPhysicalObjectMeleeAttackSystem.gameObject);
					}
				}
			}
		}
	}

	public void checkGrabbedWeaponInfoStateAtEnd ()
	{
		if (currentGrabbedWeaponInfo != null) {
			if (currentGrabbedWeaponInfo.setPreviousStrafeModeOnDropObject) {
				mainPlayerController.activateOrDeactivateStrafeMode (previousStrafeMode);

				setStrafeModeOnAISystemState (previousStrafeMode);
			}

			mainPlayerController.setCurrentStrafeIDValue (0);

			if (currentGrabbedWeaponInfo.setNewIdleID) {
				mainPlayerController.setCurrentIdleIDValue (0);
			}

			if (currentGrabbedWeaponInfo.setNewCrouchID) {
				mainPlayerController.setCurrentCrouchIDValue (0);
			}

			if (currentGrabbedWeaponInfo.setNewMovementID) {
				mainPlayerController.resetPlayerStatusID ();
			}

			if (currentGrabbedWeaponInfo.setSprintEnabledStateWithWeapon) {
				mainPlayerController.setOriginalSprintEnabledValue ();
			}

			if (currentGrabbedWeaponInfo.useEventsOnGrabDropObject) {
				currentGrabbedWeaponInfo.eventOnDropObject.Invoke ();
			}

			if (currentGrabbedWeaponInfo.toggleStrafeModeIfRunningActive) {
				mainPlayerController.setDisableStrafeModeExternallyIfIncreaseWalkSpeedActiveState (false);
			}

			if (currentGrabbedWeaponInfo.useRemoteEventsOnDropObject) {
				if (currentGrabbedObjectTransform != null) {
					setRemoteEventsOnCurrentMeleeWeapon (currentGrabbedWeaponInfo.remoteEventOnDropObject, currentGrabbedObjectTransform.gameObject);
				}
			}
		}

		currentGrabbedWeaponInfo.previousStrafeMode = false;

		currentGrabbedWeaponInfo = null;
	}

	public void checkLookAtTargetActiveState ()
	{
		if (carryingObject && currentGrabbedWeaponInfo != null) {
			bool lookingAtTarget = mainPlayerController.isPlayerLookingAtTarget ();

			bool lockedCameraActive = mainPlayerController.isLockedCameraStateActive ();

			bool canSetStrafeMode = false;

			if (lockedCameraActive) {
				if (mainPlayerController.istargetToLookLocated ()) {
					canSetStrafeMode = true;
				} else {
					if (lookingAtTarget) {
						canSetStrafeMode = true;

						lookingAtTarget = false;
					}
				}
			} else {
				canSetStrafeMode = true;
			}

			if (lookingAtTarget) {
				if (canSetStrafeMode) {
					if (currentGrabbedWeaponInfo.activateStrafeModeOnLockOnTargetActive) {
						mainPlayerController.activateOrDeactivateStrafeMode (true);

						setStrafeModeOnAISystemState (true);
					}
				}
			} else {
				if (canSetStrafeMode) {
					if (currentGrabbedWeaponInfo.activateStrafeModeOnLockOnTargetActive) {
						mainPlayerController.activateOrDeactivateStrafeMode (false);

						setStrafeModeOnAISystemState (false);
					}
				}
			}

			updateBlockShieldState ();
		}
	}

	public void checkLookAtTargetDeactivateState ()
	{
		if (carryingObject && currentGrabbedWeaponInfo != null) {
			if (currentGrabbedWeaponInfo.activateStrafeModeOnLockOnTargetActive) {
				mainPlayerController.activateOrDeactivateStrafeMode (false);

				setStrafeModeOnAISystemState (false);
			}

			updateBlockShieldState ();
		}
	}

	public void setStrafeModeOnAISystemState (bool state)
	{
		if (mainFindObjectivesSystem != null) {
			mainFindObjectivesSystem.setStrafeModeActive (state);
		}
	}

	public void setStrafeModeState (bool state, int strafeID)
	{
		mainPlayerController.activateOrDeactivateStrafeMode (state);

		mainPlayerController.setCurrentStrafeIDValue (strafeID);
	}

	public void setCurrentCrouchIDValue (int crouchID)
	{
		mainPlayerController.setCurrentCrouchIDValue (crouchID);
	}

	public bool isStrafeModeActive ()
	{
		return mainPlayerController.isStrafeModeActive ();
	}

	public int getCurrentStrafeID ()
	{
		return mainPlayerController.getCurrentStrafeIDValue ();
	}

	public int getCurrentCrouchID ()
	{
		return mainPlayerController.getCurrentCrouchIDValue ();
	}

	public void throwObject ()
	{
		if (!throwObjectEnabled) {
			return;
		}

		if (objectThrown) {
			return;
		}

		if (returningThrownObject) {
			return;
		}

		if (cuttingModeActive) {
			return;
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.canThrowObject) {
			stopThrowObjectCoroutine ();

			throwCoroutine = StartCoroutine (throwObjectCoroutine ());
		}
	}

	void stopThrowObjectCoroutine ()
	{
		if (throwCoroutine != null) {
			StopCoroutine (throwCoroutine);
		}
	}

	IEnumerator throwObjectCoroutine ()
	{
		objectThrown = true;

		currentGrabPhysicalObjectMeleeAttackSystem.setObjectThrownState (true);

		if (disableDropObjectWhenThrownOrReturning) {
			mainGrabObjects.setGrabObjectsInputPausedState (true);
		}

		if (useStaminaOnThrowObjectEnabled) {
			mainStaminaSystem.activeStaminaStateWithCustomAmount (throwObjectStaminaState, staminaToUseOnThrowObject * generalStaminaUseMultiplier, customRefillStaminaDelayAfterThrow);				
		}

		surfaceDetected = false;

		surfaceNotFound = false;

		setGrabbedObjectClonnedColliderEnabledState (false);

		if (currentGrabPhysicalObjectMeleeAttackSystem.useThrowActionName) {
			mainPlayerController.activateCustomAction (currentGrabPhysicalObjectMeleeAttackSystem.throwActionName);
		}

		RaycastHit hit;

		objectToFollowFound = false;

		objectToFollow = null;

		if (checkIfObjectToFollowOnThrowMeleeWeapon) {
			if (mainPlayerController.isPlayerLookingAtTarget ()) {
				objectToFollow = mainPlayerController.getCurrentTargetToLook ();

				if (objectToFollow != null) {
					objectToFollowFound = true;
				}
			} 

			if (!objectToFollowFound) {
				if (Physics.Raycast (mainCameraTransform.position, mainCameraTransform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, throwObjectsLayerToCheck)) {

					objectToFollowOnThrowMeleeWeapon currentObjectToFollow = hit.transform.GetComponent<objectToFollowOnThrowMeleeWeapon> ();

					if (currentObjectToFollow != null) {
						objectToFollow = currentObjectToFollow.getMainObjectToFollow ();

						objectToFollowFound = true;
					}
				}
			}
		}

		thorwWeaponQuicklyAndTeleportIfSurfaceFound = currentGrabPhysicalObjectMeleeAttackSystem.throwWeaponQuicklyAndTeleportIfSurfaceFound;

		if (thorwWeaponQuicklyAndTeleportIfSurfaceFound) {

			surfaceToQuicklyTeleportLocated = false;

			bool surfaceDetectedToQuickTeleport = false;

			if (Physics.Raycast (mainCameraTransform.position, mainCameraTransform.TransformDirection (Vector3.forward), out hitToQuicklyTeleport, Mathf.Infinity, throwObjectsLayerToCheck)) {
				if (hitToQuicklyTeleport.collider.gameObject != playerControllerGameObject) {
					surfaceDetectedToQuickTeleport = true;
				} else {
					if (Physics.Raycast (hitToQuicklyTeleport.point, mainCameraTransform.TransformDirection (Vector3.forward), out hitToQuicklyTeleport, Mathf.Infinity, throwObjectsLayerToCheck)) {
						surfaceDetectedToQuickTeleport = true;
					}
				}
			}

			if (surfaceDetectedToQuickTeleport) {
				surfaceToQuicklyTeleportLocated = true;

				float dist = GKC_Utils.distance (hitToQuicklyTeleport.point, playerControllerGameObject.transform.position);

				timeToReachSurfaceOnQuickTeleport = dist /
				(currentObjectRigidbody.mass *
				currentGrabPhysicalObjectMeleeAttackSystem.throwSpeed *
				currentGrabPhysicalObjectMeleeAttackSystem.extraSpeedOnThrowWeaponQuicklyAndTeleport);

				timeToReachSurfaceOnQuickTeleport *= 0.93f;

				meleeAttackSurfaceInfo currentMeleeAttackSurfaceInfo = hitToQuicklyTeleport.collider.gameObject.GetComponent<meleeAttackSurfaceInfo> ();

				if (currentMeleeAttackSurfaceInfo != null) {
					if (currentMeleeAttackSurfaceInfo.disableInstaTeleportOnThisSurface) {
						thorwWeaponQuicklyAndTeleportIfSurfaceFound = false;

						surfaceToQuicklyTeleportLocated = false;
					}
				}
			}
		}

		if (shieldActive) {
			setShieldParentState (false);
		}

		continueObjecThrowActivated = false;

		teleportPosition = Vector3.zero;

		teleportDistanceOffset = 0;

		yield return null;

		if (currentGrabPhysicalObjectMeleeAttackSystem.delayToThrowObject > 0) {
			continueThrowObject ();
		}
	}

	public void continueThrowObject ()
	{
		stopThrowObjectCoroutine ();

		if (!grabbedObjectMeleeAttackActive) {
			objectThrown = false;

			return;
		}

		throwCoroutine = StartCoroutine (continueThrowObjectCoroutine ());
	}

	IEnumerator continueThrowObjectCoroutine ()
	{
		objectThrownTravellingToTarget = true;

		continueObjecThrowActivated = true;

		if (currentGrabPhysicalObjectMeleeAttackSystem.delayToThrowObject > 0) {
			yield return new WaitForSeconds (currentGrabPhysicalObjectMeleeAttackSystem.delayToThrowObject);
		}

		currentGrabPhysicalObjectMeleeAttackSystem.checkEventOnThrow (true);

		//Disable the strafe mode
		if (currentGrabbedWeaponInfo.setPreviousStrafeModeOnDropObject) {
			mainPlayerController.activateOrDeactivateStrafeMode (currentGrabPhysicalObjectMeleeAttackSystem.wasStrafeModeActivePreviously ());
		}

		if (currentGrabbedWeaponInfo.toggleStrafeModeIfRunningActive) {
			mainPlayerController.setDisableStrafeModeExternallyIfIncreaseWalkSpeedActiveState (false);
		}

		mainPlayerController.setCurrentStrafeIDValue (0);

		if (currentGrabbedWeaponInfo.useEventsOnGrabDropObject) {
			currentGrabbedWeaponInfo.eventOnDropObject.Invoke ();
		}


		checkEventOnThrowOrReturnObject (true);

		//Enable physics and throw the object
		currentGrabbedObjectTransform.SetParent (null);

		currentObjectRigidbody.useGravity = false;
		currentObjectRigidbody.isKinematic = false;

		Vector3 throwObjectDirection = mainCameraTransform.forward;

		RaycastHit hit;

		if (Physics.Raycast (mainCameraTransform.position, mainCameraTransform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, throwObjectsLayerToCheck)) {
			Vector3 heading = hit.point - currentObjectRigidbody.transform.position;

			float distance = heading.magnitude;
			throwObjectDirection = heading / distance;
			throwObjectDirection.Normalize ();
		}

		Vector3 forceDirection = throwObjectDirection * (currentObjectRigidbody.mass * currentGrabPhysicalObjectMeleeAttackSystem.throwSpeed);

		if (surfaceToQuicklyTeleportLocated) {
			forceDirection *= currentGrabPhysicalObjectMeleeAttackSystem.extraSpeedOnThrowWeaponQuicklyAndTeleport;
		}

		currentObjectRigidbody.AddForce (forceDirection, throwObjectsForceMode);

		// Check the state of the throw object
		bool targetReached = false;

		lastTimeObjectThrown = Time.time;

		surfaceDetecetedOnObjectThrown = null;

		capsuleCastRadius = currentGrabPhysicalObjectMeleeAttackSystem.capsuleCastRadius;

		capsuleCastDistance = currentGrabPhysicalObjectMeleeAttackSystem.capsuleCastDistance;

		throwObjectWithRotation = currentGrabPhysicalObjectMeleeAttackSystem.throwObjectWithRotation;

		returnObjectWithRotation = currentGrabPhysicalObjectMeleeAttackSystem.returnObjectWithRotation;

		useSplineForReturn = currentGrabPhysicalObjectMeleeAttackSystem.useSplineForReturn;

		Quaternion targetRotation = Quaternion.LookRotation (throwObjectDirection, objectRotationPoint.up);


		float timeOnAir = currentGrabPhysicalObjectMeleeAttackSystem.maxTimeOnAirIfNoSurfaceFound;

		if (checkIfObjectToFollowOnThrowMeleeWeapon && objectToFollowFound) {
			timeOnAir += extraWeaponOnAirTimeIfObjectToFollowDetected;
		}

		var waitTime = new WaitForFixedUpdate ();

		while (!targetReached) {
			yield return waitTime;

			if (throwObjectWithRotation) {

				bool rotateObjectActive = true;

				if (currentGrabPhysicalObjectMeleeAttackSystem.rotateWeaponToSurfaceLocatedWhenCloseEnough) {
					Vector3 raycastDirection = throwObjectDirection;

					if (objectToFollowFound) {
						raycastDirection = objectToFollow.position - objectRotationPoint.position;
						raycastDirection = raycastDirection / raycastDirection.magnitude;
					}

					if (Physics.Raycast (objectRotationPoint.position, raycastDirection, out hit, 5, throwObjectsLayerToCheck)) {
						Vector3 surfaceDirection = hit.point - objectRotationPoint.position;
						surfaceDirection = surfaceDirection / surfaceDirection.magnitude;

						Quaternion newTargetRotation = Quaternion.LookRotation (surfaceDirection, objectRotationPoint.up);

						objectRotationPoint.rotation = Quaternion.Lerp (objectRotationPoint.rotation, newTargetRotation, 25 * Time.deltaTime);

						rotateObjectActive = false;
					}
				} 

				if (rotateObjectActive) {
					objectRotationPoint.Rotate (Vector3.right * (throwObjectRotationSpeed * Time.deltaTime));
				}
			} else {
				objectRotationPoint.rotation = Quaternion.Lerp (objectRotationPoint.rotation, targetRotation, 10 * Time.deltaTime);
			}

			checkSurfacesDetectedRaycast (capsuleCastRadius);

			if (hits.Length > 0) {
				if (showDebugPrint) {
					print (hits.Length + " " + hits [0].collider.gameObject.name);
				}

				surfaceDetecetedOnObjectThrown = hits [0].collider.gameObject;

				if (surfaceDetecetedOnObjectThrown == playerControllerGameObject) {

					if (showDebugPrint) {
						print ("player found, selecting another collider -------------------------------------------------");
					}

					if (hits.Length > 1) {
						surfaceDetecetedOnObjectThrown = hits [1].collider.gameObject;

						targetReached = true;

					} else {
						surfaceDetecetedOnObjectThrown = null;
					}
				} else {
					targetReached = true;
				}
			}

			if (!targetReached) {
				if (thorwWeaponQuicklyAndTeleportIfSurfaceFound) {
					if (surfaceToQuicklyTeleportLocated) {
						float currentObjectDistance = GKC_Utils.distance (hitToQuicklyTeleport.point, currentGrabbedObjectTransform.position);

						if (currentObjectDistance < 2 || Time.time > timeToReachSurfaceOnQuickTeleport + lastTimeObjectThrown) {
							currentObjectRigidbody.velocity = Vector3.zero;
							currentObjectRigidbody.isKinematic = true;

							surfaceDetecetedOnObjectThrown = hitToQuicklyTeleport.collider.gameObject;

							targetReached = true;
						}
					}
				}

				if (Physics.Raycast (objectRotationPoint.position, throwObjectDirection, out hit, 1.5f, throwObjectsLayerToCheck)) {
					if (hit.collider.gameObject != playerControllerGameObject) {

						if (showDebugPrint) {
							print ("player found, selecting another collider -------------------------------------------------");
						}

						surfaceDetecetedOnObjectThrown = hit.collider.gameObject;

						targetReached = true;
					}
				}
			}

			if (objectToFollowFound && objectToFollow != null) {
				Vector3 nextObjectPosition = objectToFollow.position;

				Vector3 currentObjectPosition = currentGrabbedObjectTransform.position;

				Vector3 currentSpeed = (nextObjectPosition - currentObjectPosition);

				currentSpeed.Normalize ();

				currentSpeed *= followObjectOnThrowMeleeWeaponSpeed;
			
				currentObjectRigidbody.velocity = currentSpeed;
			}

			if (Time.time > timeOnAir + lastTimeObjectThrown) {
				targetReached = true;

				surfaceNotFound = true;
			}

			if (targetReached && surfaceDetecetedOnObjectThrown != null) {
				if (currentGrabPhysicalObjectMeleeAttackSystem.useCutOnThrowObject) {
					Vector3 cutOverlapBoxSize = currentGrabPhysicalObjectMeleeAttackSystem.cutOverlapBoxSize;
					Transform cutPositionTransform = currentGrabPhysicalObjectMeleeAttackSystem.cutPositionTransform;
					Transform cutDirectionTransform = currentGrabPhysicalObjectMeleeAttackSystem.cutDirectionTransform;

					Transform planeDefiner1 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner1;
					Transform planeDefiner2 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner2;
					Transform planeDefiner3 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner3;

					mainSliceSystem.setCustomCutTransformValues (cutOverlapBoxSize, cutPositionTransform, cutDirectionTransform,
						planeDefiner1, planeDefiner2, planeDefiner3);

					mainSliceSystem.activateCut ();

					if (mainSliceSystem.anyObjectDetectedOnLastSlice ()) {
						surfaceDetecetedOnObjectThrown = null;
						targetReached = false;

						surfaceNotFound = true;
					}
				}
			}
		}

		isAttachedToSurface = false;

		surfaceDetectedIsDead = false;

		bool isAttachedToCharacter = false;

		currentGrabPhysicalObjectSystem.setLastParentAssigned (null);

		if (targetReached) {
			if (surfaceDetecetedOnObjectThrown != null) {
				if (currentGrabPhysicalObjectMeleeAttackSystem.damageOnSurfaceDetectedOnThrow > 0) {
					currentHitCombat.setNewHitDamage (currentGrabPhysicalObjectMeleeAttackSystem.damageOnSurfaceDetectedOnThrow * generalDamageOnSurfaceDetectedOnThrow);
				}

				forceDirection = surfaceDetecetedOnObjectThrown.transform.position - currentGrabbedObjectTransform.position;
				forceDirection = forceDirection / forceDirection.magnitude;

				currentHitCombat.setCustomForceDirection (forceDirection);
				currentHitCombat.setCustomForceAmount (currentGrabPhysicalObjectMeleeAttackSystem.forceToApplyToSurfaceFound);
				currentHitCombat.setCustomForceToVehiclesMultiplier (currentGrabPhysicalObjectMeleeAttackSystem.forceExtraToApplyOnVehiclesFound);

				checkSurfacesDetectedRaycast (capsuleCastRadius + 0.05f);

				for (int i = 0; i < hits.Length; i++) {
					GameObject currentObject = hits [i].collider.gameObject;
					
					if (currentObject != surfaceDetecetedOnObjectThrown) {
						currentHitCombat.activateDamage (currentObject);
					}
				}

				currentHitCombat.setCurrentState (false);

				currentHitCombat.checkObjectDetected (surfaceDetecetedOnObjectThrown, false);

				currentHitCombat.setCustomForceDirection (Vector3.zero);
				currentHitCombat.setCustomForceAmount (0);
				currentHitCombat.setCustomForceToVehiclesMultiplier (0);

				surfaceDetected = true;

				if (applyDamage.objectCanBeDamaged (surfaceDetecetedOnObjectThrown)) {
					if (!applyDamage.checkIfDead (surfaceDetecetedOnObjectThrown)) {
						isAttachedToCharacter = applyDamage.attachObjectToSurfaceFound (surfaceDetecetedOnObjectThrown.transform, currentGrabbedObjectTransform, 
							currentGrabPhysicalObjectMeleeAttackSystem.mainDamagePositionTransform.position, false);

						if (showDebugPrint) {
							print ("target not dead");
						}

						isAttachedToSurface = true;
					} else {
						surfaceDetectedIsDead = true;
						if (showDebugPrint) {
							print ("target killed");
						}
					}
				} else {
					if (showDebugPrint) {
						print ("target dead");
					}

					isAttachedToSurface = true;
				}
			}
		}

		if (showDebugPrint) {
			print ("isAttachedToSurface " + isAttachedToSurface + " surface detected is dead " + surfaceDetectedIsDead);
		}

		if (surfaceDetecetedOnObjectThrown != null) {
			currentObjectRigidbody.useGravity = !isAttachedToSurface;
			currentObjectRigidbody.isKinematic = isAttachedToSurface;

			currentObjectRigidbody.freezeRotation = isAttachedToSurface;

			if (showDebugPrint) {
				print (" kinematic " + isAttachedToSurface);
			}

			Rigidbody detectedObjectRigidbody = surfaceDetecetedOnObjectThrown.GetComponent<Rigidbody> ();

			if (!surfaceDetectedIsDead && !isAttachedToCharacter && (surfaceDetecetedOnObjectThrown.CompareTag (movingObjectsTag) || detectedObjectRigidbody != null)) {
				applyDamage.checkParentToAssign (currentGrabbedObjectTransform, surfaceDetecetedOnObjectThrown.transform);
			}

			checkSurfaceOnThrowWeapon (surfaceDetecetedOnObjectThrown);

			currentGrabPhysicalObjectSystem.setLastParentAssigned (currentGrabbedObjectTransform.parent);
		} else {
			currentObjectRigidbody.useGravity = true;
			currentObjectRigidbody.freezeRotation = false;
		}

		if (!isAttachedToSurface) {
			currentGrabPhysicalObjectMeleeAttackSystem.setMainObjectColliderEnabledState (true);
		}

		Transform originalParent = currentGrabbedObjectTransform.parent;

		Vector3 originalPosition = objectRotationPointParent.localPosition;

		objectRotationPoint.SetParent (null);

		currentGrabbedObjectTransform.SetParent (objectRotationPoint);

		currentGrabbedObjectTransform.localEulerAngles = objectRotationPointParent.localEulerAngles * -1;

		if (objectRotationPointParent.localEulerAngles == Vector3.zero) {
			currentGrabbedObjectTransform.localPosition = originalPosition * (-1);
		} else {
			originalPosition = new Vector3 (0, 0, originalPosition.y);

			currentGrabbedObjectTransform.localPosition = originalPosition * (-1);
		}

		currentGrabbedObjectTransform.SetParent (originalParent);

		objectRotationPoint.SetParent (objectRotationPointParent);

		//If the object can't return after checking surfaces due to not being configured like that, then drop the object in its current state
		if (!currentGrabPhysicalObjectMeleeAttackSystem.canReturnObject) {
			if (showDebugPrint) {
				print ("object can't return, setting next state");
			}

			Transform grabbedObjectTransform = currentGrabbedObjectTransform;
			Transform currentParent = currentGrabbedObjectTransform.parent;

			Rigidbody currentGrabbedObjectRigidbody = currentObjectRigidbody;

			bool canReUseObjectIfNotReturnActive = currentGrabPhysicalObjectMeleeAttackSystem.canReUseObjectIfNotReturnActive;

			Collider grabbedObjectMainTrigger = currentGrabPhysicalObjectSystem.grabObjectTrigger;

			mainGrabObjects.dropObject ();

			if (isAttachedToSurface) {
				grabbedObjectTransform.SetParent (currentParent);
				currentGrabbedObjectRigidbody.useGravity = false;
				currentGrabbedObjectRigidbody.isKinematic = true;

				if (showDebugPrint) {
					print ("is fixed on surface");
				}
			}

			if (!canReUseObjectIfNotReturnActive) {
				if (showDebugPrint) {
					print ("disabling grabbing properties");
				}

				grabbedObjectMainTrigger.enabled = false;

				mainGrabObjects.removeCurrentPhysicalObjectToGrabFound (grabbedObjectTransform.gameObject);
			}
		}

		currentGrabPhysicalObjectMeleeAttackSystem.checkEventOnThrow (false);

		if (isAttachedToCharacter) {
			targetReached = false;

			float t = 0;

			float positionDifference = 0;
			float angleDifference = 0;

			float movementTimer = 0;

			float attachToSurfaceAdjustSpeed = currentGrabPhysicalObjectMeleeAttackSystem.attachToSurfaceAdjustSpeed;

			Vector3 targetPosition = Vector3.zero;
			targetRotation = Quaternion.identity;

			if (currentGrabPhysicalObjectMeleeAttackSystem.attachToCharactersReferenceTransform) {
				targetPosition = currentGrabPhysicalObjectMeleeAttackSystem.attachToCharactersReferenceTransform.localPosition;
				targetRotation = currentGrabPhysicalObjectMeleeAttackSystem.attachToCharactersReferenceTransform.localRotation;
			}

			while (!targetReached) {
				t += Time.deltaTime / attachToSurfaceAdjustSpeed; 

				currentGrabbedObjectTransform.localPosition = Vector3.Lerp (currentGrabbedObjectTransform.localPosition, targetPosition, t);
			
				currentGrabbedObjectTransform.localRotation = Quaternion.Lerp (currentGrabbedObjectTransform.localRotation, targetRotation, t);

				positionDifference = GKC_Utils.distance (currentGrabbedObjectTransform.localPosition, targetPosition);

				angleDifference = Quaternion.Angle (currentGrabbedObjectTransform.localRotation, targetRotation);

				movementTimer += Time.deltaTime;
			
				if ((positionDifference < 0.01f && angleDifference < 0.02f) || movementTimer > 3) {
					targetReached = true;
				}

				yield return null;
			}
		}

		objectThrownTravellingToTarget = false;

		bool autoReturnActivated = false;

		if (surfaceDetected) {
			if (currentGrabPhysicalObjectMeleeAttackSystem.returnWeaponIfObjectDetected) {
				inputThrowOrReturnObject ();

				autoReturnActivated = true;
			}
		}

		if (!autoReturnActivated) {
			if (thorwWeaponQuicklyAndTeleportIfSurfaceFound) {
				if (surfaceToQuicklyTeleportLocated) {
					bool disableInstaTeleportOnThisSurface = false;

					if (surfaceDetecetedOnObjectThrown != null) {
						
					}

					if (!disableInstaTeleportOnThisSurface) {
						inputActivateTeleport ();
					}
				}
			}
		}
	}

	void checkSurfacesDetectedRaycast (float capsuleRadius)
	{
		currentRayOriginPosition = raycastCheckTransfrom.position;
		currentRayTargetPosition = raycastCheckTransfrom.position + raycastCheckTransfrom.forward * capsuleCastDistance;

		distanceToTarget = GKC_Utils.distance (currentRayOriginPosition, currentRayTargetPosition);
		rayDirection = currentRayOriginPosition - currentRayTargetPosition;
		rayDirection = rayDirection / rayDirection.magnitude;

		Debug.DrawLine (currentRayTargetPosition, (rayDirection * distanceToTarget) + currentRayTargetPosition, Color.red, 2);

		point1 = currentRayOriginPosition - rayDirection * capsuleCastRadius;
		point2 = currentRayTargetPosition + rayDirection * capsuleCastRadius;

		hits = Physics.CapsuleCastAll (point1, point2, capsuleRadius, rayDirection, 0, currentHitCombat.layerMask);
	}

	public void returnObject ()
	{
		if (!returnObjectEnabled) {
			return;
		}

		if (!objectThrown) {
			return;
		}

		if (returningThrownObject) {
			return;
		}

		if (cuttingModeActive) {
			return;
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.canReturnObject) {
			bool canActivateReturn = false;

			if (surfaceDetected || surfaceNotFound) {
				canActivateReturn = true;
			}

			if (currentGrabPhysicalObjectMeleeAttackSystem.canReturnWeaponIfNoSurfaceFound && surfaceDetecetedOnObjectThrown == null) {
				if (Time.time > 0.6f + lastTimeObjectThrown) {

					stopThrowObjectCoroutine ();

					canActivateReturn = true;
				}
			}

			if (canActivateReturn) {
				stopReturnObjectCoroutine ();

				returnCoroutine = StartCoroutine (returnObjectCoroutine ());
			}
		}
	}

	void stopReturnObjectCoroutine ()
	{
		if (returnCoroutine != null) {
			StopCoroutine (returnCoroutine);
		}
	}

	IEnumerator returnObjectCoroutine ()
	{
		surfaceDetected = false;

		surfaceNotFound = false;

		if (useStaminaOnReturnObjectEnabled) {
			mainStaminaSystem.activeStaminaStateWithCustomAmount (returnObjectStaminaState, staminaToUseOnReturnObject * generalStaminaUseMultiplier, customRefillStaminaDelayAfterReturn);				
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.useStartReturnActionName) {
			mainPlayerController.activateCustomAction (currentGrabPhysicalObjectMeleeAttackSystem.startReturnActionName);
		}

		yield return new WaitForSeconds (currentGrabPhysicalObjectMeleeAttackSystem.delayToReturnObject);

		currentGrabPhysicalObjectMeleeAttackSystem.setMainObjectColliderEnabledState (false);

		currentObjectRigidbody.useGravity = false;
		currentObjectRigidbody.isKinematic = true;

		currentHitCombat.setCurrentExtraDamageValue (0);

		if (applyDamageOnSurfaceDetectedOnReturnEnabled && isAttachedToSurface && currentGrabPhysicalObjectMeleeAttackSystem.applyDamageOnSurfaceDetectedOnReturn) {
			if (surfaceDetecetedOnObjectThrown != null) {
				if (currentHitCombat.checkObjectDetection (surfaceDetecetedOnObjectThrown, false)) {
					currentHitCombat.setNewHitDamage (currentGrabPhysicalObjectMeleeAttackSystem.damageOnSurfaceDetectedOnReturn * generalDamageMultiplierOnObjectReturn);

					currentHitCombat.activateDamage (surfaceDetecetedOnObjectThrown);
				}
			}
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.applyDamageOnObjectReturnPath) {
			currentHitCombat.setNewHitDamage (currentGrabPhysicalObjectMeleeAttackSystem.damageOnObjectReturnPath * generalDamageMultiplierOnReturnPath);
		
			detectedObjectsOnReturn.Clear ();
		}

		returningThrownObject = true;

		currentGrabPhysicalObjectMeleeAttackSystem.checkEventOnReturn (true);

		Transform currentHandForObject = currentGrabPhysicalObjectSystem.getCurrentObjectParent ();

		Transform referencePositionThirdPerson = currentGrabPhysicalObjectSystem.getReferencePositionThirdPerson ();

		if (currentGrabbedWeaponInfo.useCustomGrabbedWeaponReferencePosition) {
			referencePositionThirdPerson = currentGrabbedWeaponInfo.customGrabbedWeaponReferencePosition;
		}

		handPositionReference.localRotation = referencePositionThirdPerson.localRotation;
		handPositionReference.localPosition = referencePositionThirdPerson.localPosition;

		float dist = GKC_Utils.distance (handPositionReference.position, currentGrabbedObjectTransform.position);

		float duration = dist / currentGrabPhysicalObjectMeleeAttackSystem.returnSpeed;

		if (useSplineForReturn) {
			duration = dist / currentGrabPhysicalObjectMeleeAttackSystem.returnSplineSpeed;
		}

		float t = 0;

		bool targetReached = false;

		float angleDifference = 0;

		float positionDifference = 0;

		float movementTimer = 0;

		bool objectCloseEnough = false;

		bool activateCatchObjectAction = false;

		bool resetObjectRotationPointRotation = false;

		float progress = 0;

		if (useSplineToReturnObject) {
			splineToReturnObject.transform.LookAt (currentGrabbedObjectTransform.position);

			splineToReturnObject.transform.localEulerAngles = new Vector3 (0, splineToReturnObject.transform.localEulerAngles.y, 0);

			splineToReturnObject.setInitialSplinePoint (currentGrabbedObjectTransform.position);
		}

		Vector3 targetPosition = handPositionReference.position;

		Quaternion targetRotation = handPositionReference.rotation;

		while (!targetReached) {
			t += Time.deltaTime / duration; 

			if (resetObjectRotationPointRotation) {
				objectRotationPoint.localRotation = Quaternion.Lerp (objectRotationPoint.localRotation, Quaternion.identity, t);
			} else {
				if (returnObjectWithRotation) {
					objectRotationPoint.Rotate (Vector3.right * (throwObjectRotationSpeed * Time.deltaTime));
				} else {
					Vector3 rotationPointTargetDirection = objectRotationPoint.position - playerControllerGameObject.transform.position;
					rotationPointTargetDirection = rotationPointTargetDirection / rotationPointTargetDirection.magnitude;

					Quaternion rotationPointTargetRotation = Quaternion.LookRotation (rotationPointTargetDirection, objectRotationPoint.up);

					objectRotationPoint.rotation = Quaternion.Lerp (objectRotationPoint.rotation, rotationPointTargetRotation, t);
				}
			}

			if (useSplineToReturnObject && !objectCloseEnough && useSplineForReturn) {
				progress += Time.deltaTime / currentGrabPhysicalObjectMeleeAttackSystem.returnSplineSpeed;

				Vector3 position = splineToReturnObject.GetPoint (progress);
				currentGrabbedObjectTransform.position = position;
			} else {
				targetPosition = handPositionReference.position;

				targetRotation = handPositionReference.rotation;

				currentGrabbedObjectTransform.position = Vector3.Lerp (currentGrabbedObjectTransform.position, targetPosition, t);
				currentGrabbedObjectTransform.rotation = Quaternion.Lerp (currentGrabbedObjectTransform.rotation, targetRotation, t);
			}

			angleDifference = Quaternion.Angle (currentGrabbedObjectTransform.rotation, targetRotation);

			positionDifference = GKC_Utils.distance (currentGrabbedObjectTransform.position, targetPosition);

			if (positionDifference < 4 || progress > 0.60f) {
				resetObjectRotationPointRotation = true;
			}

			if (positionDifference < 2.5 || progress > 0.70f) {
				if (!objectCloseEnough) {

					dist = GKC_Utils.distance (handPositionReference.position, currentGrabbedObjectTransform.position);

					duration = dist / currentGrabPhysicalObjectMeleeAttackSystem.resetObjectRotationSpeed;

					objectCloseEnough = true;
				}
			}

			if (positionDifference < 0.1f || progress > 0.95f) {
				if (!activateCatchObjectAction) {

					checkGrabbedWeaponInfoStateAtStart (currentGrabPhysicalObjectMeleeAttackSystem.weaponInfoName, false);

					if (currentGrabPhysicalObjectMeleeAttackSystem.useStartReturnActionName) {
						mainPlayerController.stopCustomAction (currentGrabPhysicalObjectMeleeAttackSystem.startReturnActionName);
					}

					if (currentGrabPhysicalObjectMeleeAttackSystem.useEndReturnActionName) {
						mainPlayerController.activateCustomAction (currentGrabPhysicalObjectMeleeAttackSystem.endReturnActionName);
					}

					activateCatchObjectAction = true;
				}
			}

			movementTimer += Time.deltaTime;

			if ((positionDifference < 0.01f && angleDifference < 0.2f) || movementTimer > (duration + 2)) {
				targetReached = true;
			}

			if (applyDamageOnObjectReturnPathEnabled && currentGrabPhysicalObjectMeleeAttackSystem.applyDamageOnObjectReturnPath) {
				checkSurfacesDetectedRaycast (capsuleCastRadius);

				if (hits.Length > 0) {
					for (int i = 0; i < hits.Length; i++) {
						GameObject currentObject = hits [i].collider.gameObject;

						if (!detectedObjectsOnReturn.Contains (currentObject)) {
							if (currentHitCombat.checkObjectDetection (currentObject, false)) {
							
								currentHitCombat.activateDamage (currentObject);

								detectedObjectsOnReturn.Add (currentObject);
							}
						}
					}
				}
			}

			if (currentGrabPhysicalObjectMeleeAttackSystem.useCutOnReturnObject) {
				Vector3 cutOverlapBoxSize = currentGrabPhysicalObjectMeleeAttackSystem.cutOverlapBoxSize;
				Transform cutPositionTransform = currentGrabPhysicalObjectMeleeAttackSystem.cutPositionTransform;
				Transform cutDirectionTransform = currentGrabPhysicalObjectMeleeAttackSystem.cutDirectionTransform;

				Transform planeDefiner1 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner1;
				Transform planeDefiner2 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner2;
				Transform planeDefiner3 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner3;

				mainSliceSystem.setCustomCutTransformValues (cutOverlapBoxSize, cutPositionTransform, cutDirectionTransform,
					planeDefiner1, planeDefiner2, planeDefiner3);

				mainSliceSystem.activateCut ();
			}

			yield return null;
		}

		currentGrabbedObjectTransform.SetParent (currentHandForObject);

		currentGrabbedObjectTransform.localRotation = referencePositionThirdPerson.localRotation;
		currentGrabbedObjectTransform.localPosition = referencePositionThirdPerson.localPosition;

		objectRotationPoint.localRotation = Quaternion.identity;

		returningThrownObject = false;

		currentGrabPhysicalObjectMeleeAttackSystem.checkEventOnReturn (false);

		currentGrabPhysicalObjectMeleeAttackSystem.setObjectThrownState (false);

		objectThrown = false;

		if (!currentGrabPhysicalObjectMeleeAttackSystem.disableMeleeObjectCollider) {
			setGrabbedObjectClonnedColliderEnabledState (true);
		}

		mainGrabObjects.setGrabObjectsInputPausedState (false);

		checkEventOnThrowOrReturnObject (false);

		objectThrownTravellingToTarget = false;

		lastTimeObjectReturn = Time.time;

		currentHitCombatBoxCollider = null;

		if (shieldActive) {
			setShieldParentState (true);
		}
	}

	public void setGrabbedObjectClonnedColliderEnabledState (bool state)
	{
		mainGrabObjects.setGrabbedObjectClonnedColliderEnabledState (state);
	}

	List<GameObject> detectedObjectsOnReturn = new List<GameObject> ();

	public void checkEventOnThrowOrReturnObject (bool state)
	{
		if (useEventsOnThrowReturnObject) {
			if (state) {
				eventOnThrowObject.Invoke ();
			} else {
				eventOnReturnObject.Invoke ();
			}
		}
	}

	public void setDamageDetectedOnTriggerById (int attackID)
	{
		if (!carryingObject) {
			return;
		}

		if (currentGrabbedWeaponInfo.useEventsOnDamageDetected) {
			for (int i = 0; i < currentGrabbedWeaponInfo.eventOnDamageInfoList.Count; i++) {
				eventOnDamageInfo currentEventOnDamageInfo = currentGrabbedWeaponInfo.eventOnDamageInfoList [i];

				if (currentEventOnDamageInfo.damageInfoID == attackID) {
					currentEventOnDamageInfo.eventOnDamageDetected.Invoke ();

					if (currentEventOnDamageInfo.useRemoteEvent) {
						bool useRemoteEvents = false;
						GameObject objectDetected =	currentHitCombat.getLastSurfaceDetected ();

						if (objectDetected != null) {

							if (showDebugPrint) {
								print ("object detected to set remote event");
							}

							if (currentGrabbedWeaponInfo.checkObjectsToUseRemoteEventsOnDamage) {
								if ((1 << objectDetected.layer & currentGrabbedWeaponInfo.layerToUseRemoteEventsOnDamage.value) == 1 << objectDetected.layer) {
									useRemoteEvents = true;
								}
							} else {
								useRemoteEvents = true;
							}

							if (useRemoteEvents) {
								remoteEventSystem currentRemoteEventSystem = objectDetected.GetComponent<remoteEventSystem> ();

								if (currentRemoteEventSystem != null) {
									if (showDebugPrint) {
										print (currentRemoteEventSystem.name);
									}

									for (int j = 0; j < currentEventOnDamageInfo.remoteEventNameList.Count; j++) {
										currentRemoteEventSystem.callRemoteEvent (currentEventOnDamageInfo.remoteEventNameList [j]);
									}
								}
							}
						}
					}

					checkSurfaceFoundOnAttack (true);

					return;
				}
			}
		}
	}

	public void setNoDamageDetectedOnTriggerById (GameObject objectDetected)
	{
		checkSurfaceFoundOnAttack (true);
	}

	public void setPressDownState ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabPhysicalObjectMeleeAttackSystem != null) {
			if (currentGrabbedWeaponInfo.useEventsOnPressDownState) {
				currentGrabbedWeaponInfo.eventOnPressDownState.Invoke ();
			}
		}
	}

	public void setPressUpState ()
	{
		if (currentGrabbedWeaponInfo != null && currentGrabPhysicalObjectMeleeAttackSystem != null) {
			if (currentGrabbedWeaponInfo.useEventsOnPressUpState) {
				currentGrabbedWeaponInfo.eventOnPressUpState.Invoke ();
			}
		}
	}


	//Input functions
	public void inputSetPressDownState ()
	{
		if (showDebugPrint) {
			print ("input activated");
		}

		if (!grabbedObjectMeleeAttackActive) {
			return;
		}

		if (meleeAttackInputPaused) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}

		if (showDebugPrint) {
			print ("1");
		}

		if (objectThrown) {
			return;
		}

		if (cuttingModeActive) {
			return;
		}

		setPressDownState ();
	}

	public void inputSetPressUpState ()
	{
		if (showDebugPrint) {
			print ("input activated");
		}

		if (!grabbedObjectMeleeAttackActive) {
			return;
		}

		if (meleeAttackInputPaused) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}

		if (showDebugPrint) {
			print ("1");
		}

		if (objectThrown) {
			return;
		}

		if (cuttingModeActive) {
			return;
		}

		setPressUpState ();
	}


	public void inputActivateBlock ()
	{
		if (!blockModeEnabled) {
			return;
		}

		if (blockInputPaused) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}

		if (!grabbedObjectMeleeAttackActive) {
			blockActivePreviously = false;

			return;
		}

		if (objectThrown) {
			return;
		}

		if (cuttingModeActive) {
			return;
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.keepGrabbedObjectState) {
			return;
		}

		if (attackInProcess) {
			blockActivePreviously = true;

			return;
		}

		if (blockActive) {
			return;
		}

		if (!mainPlayerController.isPlayerOnGround ()) {
			return;
		}

		setBlockActiveState (true);
	}

	public void inputDeactivateBlock ()
	{
		if (!grabbedObjectMeleeAttackActive) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}
			
		if (attackInProcess) {

			blockActivePreviously = false;

			return;
		}

		if (objectThrown) {
			return;
		}

		if (!blockActive) {
			return;
		}

		if (cuttingModeActive) {
			return;
		}

		setBlockActiveState (false);
	}

	public void inputThrowOrReturnObject ()
	{
		if (!grabbedObjectMeleeAttackActive) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}

		if (blockActive) {
			return;
		}

		if (throwObjectInputPausedForStamina && generalStaminaUseMultiplier > 0) {
			if (showDebugPrint) {
				print ("not enough stamina");
			}

			return;
		}
			
		if (attackInProcess) {
			return;
		}

		if (!objectThrown) {
			if (!mainPlayerController.isPlayerOnGround () && playerOnGroundToActivateThrow) {
				return;
			}

			throwObject ();
		} else {
			returnObject ();
		}
	}

	public void inputEnableOrDisableCuttingMode ()
	{
		if (!grabbedObjectMeleeAttackActive) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}

		if (objectThrown) {
			return;
		}

		if (blockActive) {
			return;
		}

		if (attackInProcess) {
			return;
		}

		if (currentGrabPhysicalObjectMeleeAttackSystem.canActivateCuttingMode) {
			enableOrDisableCuttingMode (!cuttingModeActive);
		}
	}

	public void inputActivateTeleport ()
	{
		if (!grabbedObjectMeleeAttackActive) {
			return;
		}

		if (!canUseWeaponsInput ()) {
			return;
		}

		if (!objectThrown) {
			return;
		}

		if (blockActive) {
			return;
		}

		if (attackInProcess) {
			return;
		}

		if (!teleportPlayerOnThrowEnabled) {
			return;
		}

		activateTeleport ();
	}

	public void activateTeleport ()
	{
		stopActivateTeleportCoroutine ();

		if (surfaceDetecetedOnObjectThrown != null) {
			meleeAttackSurfaceInfo currentMeleeAttackSurfaceInfo = surfaceDetecetedOnObjectThrown.GetComponent<meleeAttackSurfaceInfo> ();

			if (currentMeleeAttackSurfaceInfo != null) {
				if (currentMeleeAttackSurfaceInfo.useOffsetTransformOnWeaponThrow) {
					teleportPosition = currentMeleeAttackSurfaceInfo.offsetTransformOnWeaponThrow.position;
				}

				if (currentMeleeAttackSurfaceInfo.useOffsetDistanceOnWeaponThrow) {
					teleportDistanceOffset = currentMeleeAttackSurfaceInfo.offsetDistanceOnWeaponThrow;
				}
			}
		}

		if (teleportPosition == Vector3.zero) {
			teleportPosition = currentGrabbedObjectTransform.position;
		}

		mainPlayerTeleportSystem.teleportPlayer (playerControllerGameObject.transform, teleportPosition, false, true, 
			cameraFovOnTeleport, cameraFovOnTeleportSpeed, teleportSpeed, true, teleportInstantlyToPosition, true,
			useSmoothCameraFollowStateOnTeleport, smoothCameraFollowDuration, teleportDistanceOffset);

		teleportCoroutine = StartCoroutine (activateTeleportCoroutine ());

		teleportDistanceOffset = 0;
	}

	public void stopActivateTeleportCoroutine ()
	{
		if (teleportCoroutine != null) {
			StopCoroutine (teleportCoroutine);
		}

		teleportInProcess = false;
	}

	IEnumerator activateTeleportCoroutine ()
	{
		eventOnStartTeleport.Invoke ();

		teleportInProcess = true;

		bool targetReached = false;

		float positionDifference = 0;

		float lastTimeDistanceChecked = 0;

		Vector3 targetPosition = teleportPosition;

		teleportPosition = Vector3.zero;

		if (targetPosition == Vector3.zero) {
			targetPosition = currentGrabbedObjectTransform.position;
		}

		while (!targetReached) {

			if (!mainPlayerTeleportSystem.isTeleportInProcess ()) {
				targetReached = true;
			}

			positionDifference = GKC_Utils.distance (targetPosition, playerControllerGameObject.transform.position);

//			print (positionDifference);

			if (lastTimeDistanceChecked == 0) {
				if (positionDifference < minDistanceToStopTeleport + 1) {
					lastTimeDistanceChecked = Time.time;
				}
			} else {
//				print ("checking distance time ");
				if (Time.time > lastTimeDistanceChecked + 0.5f) {
					print ("too much time without moving");
					targetReached = true;
				}
			}

			if (positionDifference < minDistanceToStopTeleport) {
				targetReached = true;
			}

			if (targetReached) {
				mainPlayerTeleportSystem.resumeIfTeleportActive ();
			}

			yield return null;
		}

		teleportInProcess = false;

		if (grabMeleeWeaponOnTeleportPositionReached) {
			inputThrowOrReturnObject ();
		}

		eventOnEndTeleport.Invoke ();
	}

	public void enableOrDisableCuttingMode (bool state)
	{
		if (!cuttingModeEnabled) {
			return;
		}

		cuttingModeActive = state;

		if (cuttingModeActive) {
			eventOnCuttingModeStart.Invoke ();
		} else {
			eventOnCuttingModeEnd.Invoke ();
		}
			
		if (shieldActive) {
			setShieldParentState (!cuttingModeActive);
		}
	}

	public void checkObjectsDetectedOnCuttingMode ()
	{
		if (cuttingModeActive) {
			List<GameObject> hitsGameObjectList = new List<GameObject> ();

			hitsGameObjectList = mainSliceSystem.getLastCollidersListDetected ();

			if (hitsGameObjectList.Count > 0) {
				for (int i = 0; i < hitsGameObjectList.Count; i++) {
					string surfaceName = surfaceInfoOnMeleeAttackNameForSwingOnAir;

					RaycastHit hit = new RaycastHit ();

					Vector3 raycastPosition = playerControllerGameObject.transform.position + playerControllerGameObject.transform.up;

					Vector3 raycastDirection = hitsGameObjectList [i].transform.position - raycastPosition;

					raycastDirection = raycastDirection / raycastDirection.magnitude;

					float currentRaycastDistance = GKC_Utils.distance (raycastPosition, hitsGameObjectList [i].transform.position);

					currentRaycastDistance += 1;

					Physics.Raycast (raycastPosition, raycastDirection, out hit, currentRaycastDistance, currentHitCombat.layerMask);

					meleeAttackSurfaceInfo currentMeleeAttackSurfaceInfo = hitsGameObjectList [i].GetComponent<meleeAttackSurfaceInfo> ();

					bool sendSurfaceLocatedToCheck = true;

					if (currentMeleeAttackSurfaceInfo != null) {
						if (!currentMeleeAttackSurfaceInfo.isSurfaceEnabled ()) {
							sendSurfaceLocatedToCheck = false;
						} else {
							surfaceName = currentMeleeAttackSurfaceInfo.getSurfaceName ();

							currentMeleeAttackSurfaceInfo.checkEventOnSurfaceDetected ();
						}
					} else {
						sendSurfaceLocatedToCheck = false;
					}
		
					if (sendSurfaceLocatedToCheck) {
						checkSurfaceFoundOnAttackToProcess (surfaceName, true, hit.point, hit.normal, false, false);
					}
				}
			} else {
				checkSurfaceFoundOnAttack (false);
			}
		}
	}

	public void checkEventOnGrabDropObject (bool state)
	{
		checKEventsHideShowInventoryQuickAccessSlots (state);

		if (useEventsOnGrabDropObject) {
			if (state) {
				eventOnGrabObject.Invoke ();
			} else {
				eventOnDropObject.Invoke ();
			}
		}
	}

	public void checkEventsIfObjectGrabbed ()
	{
		if (carryingObject) {
			checkEventOnGrabDropObject (true);
		}
	}

	public void checKEventsHideShowInventoryQuickAccessSlots (bool state)
	{
		if (hideInventoryQuickAccessSlotsWhenCarryingMeleeWeapon) {
			if (state) {
				eventOnHideInventoryQuickAccessSlots.Invoke ();
			} else {
				eventOnShowInventoryQuickAccessSlots.Invoke ();
			}
		}
	}

	public void setThrowObjectEnabledState (bool state)
	{
		if (showDebugPrint) {
			print (state);
		}

		throwObjectEnabled = state;
	}

	public void setReturnObjectEnabledState (bool state)
	{
		returnObjectEnabled = state;
	}

	public void setApplyDamageOnSurfaceDetectedOnReturnEnabledState (bool state)
	{
		applyDamageOnSurfaceDetectedOnReturnEnabled = state;
	}

	public void setApplyDamageOnObjectReturnPathEnabledState (bool state)
	{
		applyDamageOnObjectReturnPathEnabled = state;
	}

	public void setGeneralDamageMultiplierOnObjectReturnValue (float newValue)
	{
		generalDamageMultiplierOnObjectReturn = newValue;
	}

	public void setGeneralDamageMultiplierOnReturnPathValue (float newValue)
	{
		generalDamageMultiplierOnReturnPath = newValue;
	}

	public void setGeneralDamageOnSurfaceDetectedOnThrowValue (float newValue)
	{
		generalDamageOnSurfaceDetectedOnThrow = newValue;
	}

	public void setCuttingModeEnabledState (bool state)
	{
		cuttingModeEnabled = state;
	}

	public void setBlockModeEnabledState (bool state)
	{
		blockModeEnabled = state;
	}

	public void setGeneralBlockProtectionMultiplierValue (float newValue)
	{
		generalBlockProtectionMultiplier = newValue;
	}

	public void setUseStaminaOnAttackEnabledState (bool state)
	{
		useStaminaOnAttackEnabled = state;
	}

	public void setGeneralStaminaUseMultiplierValue (float newValue)
	{
		generalStaminaUseMultiplier = newValue;
	}

	public void setGeneralAttackDamageMultiplierValue (float newValue)
	{
		generalAttackDamageMultiplier = newValue;
	}

	public bool isObjectThrownTravellingToTarget ()
	{
		return objectThrownTravellingToTarget;
	}

	public bool isAttackInProcess ()
	{
		return attackInProcess;
	}

	public bool isBlockActive ()
	{
		return blockActive;
	}

	public bool canUseWeaponsInput ()
	{
		if (!playerIsBusy ()) {
			return true;
		}

		return false;
	}

	public bool playerIsBusy ()
	{
		if (!mainPlayerController.isUsingDevice () && !mainPlayerController.isUsingSubMenu () && !mainPlayerController.isPlayerMenuActive ()) {
			return false;
		}

		return true;
	}

	public float getLastTimeObjectReturn ()
	{
		return lastTimeObjectReturn;
	}

	public float getLastTimeObjectThrown ()
	{
		return lastTimeObjectThrown;
	}

	public bool isCurrentWeaponThrown ()
	{
		return objectThrown;
	}

	public void setMeleeAttackInputPausedState (bool state)
	{
		meleeAttackInputPaused = state;
	}

	Coroutine pauseMeleeAttackInputCoroutine;

	public void disableMeleeAttackInputPausedStateWithDuration (float pauseDuration)
	{
		stopDisableMeleeAttackInputPausedStateWithDurationCoroutine ();

		pauseMeleeAttackInputCoroutine = StartCoroutine (disableMeleeAttackInputPausedStateWithDurationCoroutine (pauseDuration));
	}

	void stopDisableMeleeAttackInputPausedStateWithDurationCoroutine ()
	{
		if (pauseMeleeAttackInputCoroutine != null) {
			StopCoroutine (pauseMeleeAttackInputCoroutine);
		}
	}

	public bool isCarryingObject ()
	{
		return carryingObject;
	}

	bool cutActivatedOnAttackChecked;

	public void checkSurfaceFoundOnAttack (bool surfaceLocated)
	{
		if (!checkSurfaceInfoEnabled) {
			return;
		}

		if (objectThrown) {
			return;
		}

		cutActivatedOnAttackChecked = false;
			
		string surfaceName = surfaceInfoOnMeleeAttackNameForSwingOnAir;

		Vector3 attackPosition = Vector3.zero;
		Vector3 attackNormal = Vector3.zero;

		RaycastHit hit = new RaycastHit ();

		if (surfaceLocated) {
			GameObject lastSurfaceDetected = currentHitCombat.getLastSurfaceDetected ();

			currentHitCombat.setLastSurfaceDetected (null);

			GameObject surfaceFound = null;
		
			if (lastSurfaceDetected != null) {
				Collider lastSurfaceDetectedCollider = lastSurfaceDetected.GetComponent<Collider> ();

				Vector3 raycastPositionTarget = lastSurfaceDetectedCollider.ClosestPointOnBounds (currentGrabPhysicalObjectMeleeAttackSystem.mainDamagePositionTransform.position);
		
				List<Transform> raycastCheckTransfromList = currentGrabPhysicalObjectMeleeAttackSystem.raycastCheckTransfromList;

				for (int i = 0; i < raycastCheckTransfromList.Count; i++) {
					if (!surfaceFound) {			
						Vector3 raycastPosition = raycastCheckTransfromList [i].position;
			
						Vector3 raycastDirection = raycastPositionTarget - raycastPosition;
			
						raycastDirection = raycastDirection / raycastDirection.magnitude;
			
						float currentRaycastDistance = GKC_Utils.distance (raycastPosition, raycastPositionTarget);

						currentRaycastDistance += 0.5f;

						Debug.DrawLine (raycastPosition, raycastPositionTarget, Color.black, 6);

						Debug.DrawLine (raycastPosition, raycastPosition + raycastDirection, Color.red, 4);

						Debug.DrawLine (raycastPosition + raycastDirection, 
							raycastPosition + (raycastDirection * 0.5f), Color.white, 4);

						Debug.DrawLine (raycastPositionTarget, raycastPositionTarget - (raycastDirection * 0.5f), Color.yellow, 4);

						if (Physics.Raycast (raycastPosition, raycastDirection, out hit, currentRaycastDistance, currentHitCombat.layerMask)) {
							if (showDebugPrint) {
								print ("detected " + lastSurfaceDetected.name + " and raycast " + hit.collider.gameObject.name);
							}

							if (hit.collider.gameObject != playerControllerGameObject) {
								surfaceFound = hit.collider.gameObject;
							}
						} else {
							if (showDebugPrint) {
								print ("detected " + lastSurfaceDetected.name + " no raycast found");
							}

							if (raycastPositionTarget != Vector3.zero) {
								attackPosition = raycastPositionTarget;

								attackNormal = lastSurfaceDetectedCollider.transform.position - attackPosition;

								attackNormal = attackNormal / attackNormal.magnitude;
							}
						}
					}
				}
		
//				print (currentAttackInfo.useCutModeOnAttack);

				if (currentAttackInfo.useCutModeOnAttack && !cutActivatedOnAttackChecked) {
					Vector3 cutOverlapBoxSize = currentGrabPhysicalObjectMeleeAttackSystem.cutOverlapBoxSize;
					Transform cutPositionTransform = currentGrabPhysicalObjectMeleeAttackSystem.cutPositionTransform;
					Transform cutDirectionTransform = currentGrabPhysicalObjectMeleeAttackSystem.cutDirectionTransform;

					Transform planeDefiner1 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner1;
					Transform planeDefiner2 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner2;
					Transform planeDefiner3 = currentGrabPhysicalObjectMeleeAttackSystem.planeDefiner3;

					mainSliceSystem.setCustomCutTransformValues (cutOverlapBoxSize, cutPositionTransform, cutDirectionTransform,
						planeDefiner1, planeDefiner2, planeDefiner3);

//					print ("CHECK CUT");

					mainSliceSystem.activateCut ();

					cutActivatedOnAttackChecked = true;
				}

				if (currentAttackInfo.pushCharactersOnDamage && currentAttackInfo.messageNameToSend != "") {

					bool activatePushCharacter = true;

					if (currentAttackInfo.useProbabilityToPushCharacters) {
						float pushProbability = Random.Range (1, 100);

						pushProbability /= 100;

//						print (lastSurfaceDetected.name + " " + currentAttackInfo.pushCharactersOnDamage + " " + pushProbability);

						if (currentAttackInfo.probability < pushProbability) {
							activatePushCharacter = false;
						}
					}

					if (currentAttackInfo.useIgnoreTagsToPush) {
						if (currentAttackInfo.tagsToIgnorePush.Contains (lastSurfaceDetected.tag)) {
							activatePushCharacter = false;
						}
					}

					if (activatePushCharacter) {
						Vector3 pushCharacterDirection = Vector3.zero;

						if (currentAttackInfo.ignoreMeleeWeaponAttackDirection) {
							pushCharacterDirection = playerControllerGameObject.transform.forward * currentAttackInfo.pushForceAmount;
						} else {
							pushCharacterDirection = currentGrabPhysicalObjectMeleeAttackSystem.mainDamagePositionTransform.forward * currentAttackInfo.pushForceAmount;
						}

						if (currentAttackInfo.useExtraPushDirection) {
							Transform playerTransform = playerControllerGameObject.transform;

							pushCharacterDirection += playerTransform.right * currentAttackInfo.extraPushDirection.x;
							pushCharacterDirection += playerTransform.up * currentAttackInfo.extraPushDirection.y;
							pushCharacterDirection += playerTransform.forward * currentAttackInfo.extraPushDirection.z;

							pushCharacterDirection *= currentAttackInfo.extraPushForce;
						}

						lastSurfaceDetected.SendMessage (currentAttackInfo.messageNameToSend, pushCharacterDirection, SendMessageOptions.DontRequireReceiver);
					}
				}
			}

			if (surfaceFound != null) {
				if (showDebugPrint) {
					print ("SURFACE FOUND " + surfaceFound.name);
				}
					
				attackPosition = hit.point;
				attackNormal = hit.normal;
			} else {
				if (showDebugPrint) {
					print ("SURFACE NOT FOUND BY RAYCAST!!!!!!!!!!");
				}
			}

			if (lastSurfaceDetected != null) {
				meleeAttackSurfaceInfo currentMeleeAttackSurfaceInfo = lastSurfaceDetected.GetComponent<meleeAttackSurfaceInfo> ();

				if (currentMeleeAttackSurfaceInfo != null) {
					if (!currentMeleeAttackSurfaceInfo.isSurfaceEnabled ()) {
						surfaceLocated = false;
					} else {

						surfaceName = currentMeleeAttackSurfaceInfo.getSurfaceName ();

						if (currentMeleeAttackSurfaceInfo.useRemoteEventOnWeapon) {
							remoteEventSystem currentRemoteEventSystem = currentGrabPhysicalObjectMeleeAttackSystem.GetComponent<remoteEventSystem> ();

							if (currentRemoteEventSystem != null) {

								if (showDebugPrint) {
									print ("remote event on weapon detected");
								}

								for (int j = 0; j < currentMeleeAttackSurfaceInfo.remoteEventOnWeaponNameList.Count; j++) {
									currentRemoteEventSystem.callRemoteEvent (currentMeleeAttackSurfaceInfo.remoteEventOnWeaponNameList [j]);
								}
							}
						}

						if (currentGrabPhysicalObjectMeleeAttackSystem.useRemoteEventOnSurfacesDetected) {
							if (currentMeleeAttackSurfaceInfo.useRemoteEvent) {
								remoteEventSystem currentRemoteEventSystem = lastSurfaceDetected.GetComponent<remoteEventSystem> ();

								if (currentRemoteEventSystem != null) {

									if (showDebugPrint) {
										print ("remote event on object detected");
									}

									for (int j = 0; j < currentMeleeAttackSurfaceInfo.remoteEventNameList.Count; j++) {
										string currentRemoteEventName = currentMeleeAttackSurfaceInfo.remoteEventNameList [j];

										if (currentGrabPhysicalObjectMeleeAttackSystem.isRemoteEventIncluded (currentRemoteEventName)) {
											currentRemoteEventSystem.callRemoteEvent (currentRemoteEventName);
										}
									}
								}
							}
						}

						currentMeleeAttackSurfaceInfo.checkEventOnSurfaceDetected ();
					}
				} else {
					GameObject currentCharacter = applyDamage.getCharacterOrVehicle (lastSurfaceDetected);

					if (currentCharacter != null) {
						currentMeleeAttackSurfaceInfo = currentCharacter.GetComponent<meleeAttackSurfaceInfo> ();

						if (currentMeleeAttackSurfaceInfo != null) {
							if (!currentMeleeAttackSurfaceInfo.isSurfaceEnabled ()) {
								surfaceLocated = false;
							} else {
								surfaceName = currentMeleeAttackSurfaceInfo.getSurfaceName ();

								currentMeleeAttackSurfaceInfo.checkEventOnSurfaceDetected ();
							}
						}
					} else {
						surfaceLocated = false;
					}

					if (!surfaceLocated) {
						return;
					}
				}
			} else {
				if (showDebugPrint) {
					print ("SURFACE NOT FOUND BY TRIGGER!!!!!!!!!!");
				}
			}
		}

		bool ignoreBounceEvent = false;
		if (!currentAttackCanBeBlocked) {
			ignoreBounceEvent = true;
		}

		bool ignoreSoundOnSurface = false;

		if (attackActivatedOnAir && currentAttackInfo.pauseSoundIfAttackOnAir) {
			ignoreSoundOnSurface = true;
		}

		checkSurfaceFoundOnAttackToProcess (surfaceName, surfaceLocated, attackPosition, attackNormal, ignoreBounceEvent, ignoreSoundOnSurface);
	}

	public void checkSurfaceOnThrowWeapon (GameObject objectToCheck)
	{
		bool surfaceLocated = true;

		string surfaceName = surfaceInfoOnMeleeAttackNameForSwingOnAir;

		Vector3 attackPosition = Vector3.zero;
		Vector3 attackNormal = Vector3.zero;

		RaycastHit hit = new RaycastHit ();

		GameObject surfaceFound = null;

		Transform raycastTransform = objectRotationPoint;

		Vector3 raycastPosition = raycastTransform.position;

		Vector3 raycastDirection = objectToCheck.transform.position - raycastPosition;

		raycastDirection = raycastDirection / raycastDirection.magnitude;

		float currentRaycastDistance = GKC_Utils.distance (raycastPosition, objectToCheck.transform.position);

		currentRaycastDistance += 1;

		if (Physics.Raycast (raycastPosition, raycastDirection, out hit, currentRaycastDistance, currentHitCombat.layerMask)) {
			surfaceFound = hit.collider.gameObject;
		}

		if (surfaceFound != null) {
			attackPosition = hit.point;
			attackNormal = hit.normal;

			meleeAttackSurfaceInfo currentMeleeAttackSurfaceInfo = surfaceFound.GetComponent<meleeAttackSurfaceInfo> ();

			if (currentMeleeAttackSurfaceInfo != null) {
				if (!currentMeleeAttackSurfaceInfo.isSurfaceEnabled ()) {
					surfaceLocated = false;
				}

				surfaceName = currentMeleeAttackSurfaceInfo.getSurfaceName ();

				currentMeleeAttackSurfaceInfo.checkEventOnSurfaceDetected ();
			} else {
				GameObject currentCharacter = applyDamage.getCharacterOrVehicle (surfaceFound);

				if (currentCharacter != null) {
					currentMeleeAttackSurfaceInfo = currentCharacter.GetComponent<meleeAttackSurfaceInfo> ();

					if (currentMeleeAttackSurfaceInfo != null) {
						if (!currentMeleeAttackSurfaceInfo.isSurfaceEnabled ()) {
							surfaceLocated = false;
						}

						surfaceName = currentMeleeAttackSurfaceInfo.getSurfaceName ();

						currentMeleeAttackSurfaceInfo.checkEventOnSurfaceDetected ();
					}
				} else {
					surfaceLocated = false;
				}

				if (!surfaceLocated) {
					return;
				}
			}
		} 

		checkSurfaceFoundOnAttackToProcess (surfaceName, true, attackPosition, attackNormal, true, false);
	}

	float lastTimeSurfaceAudioPlayed;
	int lastSurfaceDetecetedIndex = -1;

	public void checkSurfaceFoundOnAttackToProcess (string surfaceName, bool surfaceLocated, Vector3 attackPosition, Vector3 attackNormal, 
	                                                bool ignoreBounceEvent, bool ignoreSoundOnSurface)
	{
		for (int i = 0; i < surfaceInfoOnMeleeAttackList.Count; i++) {
			surfaceInfoOnMeleeAttack currentSurfaceInfo = surfaceInfoOnMeleeAttackList [i];

			if (surfaceName.Equals (currentSurfaceInfo.surfaceName)) {

				int soundIndex = 0;

				if (currentSurfaceInfo.useSoundsListOnOrder) {
					currentSurfaceInfo.currentSoundIndex++;

					if (currentSurfaceInfo.currentSoundIndex >= currentSurfaceInfo.soundsList.Count) {
						currentSurfaceInfo.currentSoundIndex = 0;
					}

					soundIndex = currentSurfaceInfo.currentSoundIndex;
				} else {
					soundIndex = Random.Range (0, currentSurfaceInfo.soundsList.Count);
				}

				bool soundCanBePlayed = false;

				if (Time.time > lastTimeSurfaceAudioPlayed + 0.5f) {
					soundCanBePlayed = true;
				}

				if (lastSurfaceDetecetedIndex == -1 || lastSurfaceDetecetedIndex != i) {
					soundCanBePlayed = true;
				}

				if (soundCanBePlayed && !meleeAttackSoundPaused) {
					if (!ignoreSoundOnSurface) {
						mainAudioSource.PlayOneShot (currentSurfaceInfo.soundsList [soundIndex]);
					}

					lastTimeSurfaceAudioPlayed = Time.time;

					lastSurfaceDetecetedIndex = i;
				}

				if (surfaceLocated) {
					if (!ignoreBounceEvent) {
						if (currentSurfaceInfo.surfaceActivatesBounceOnCharacter) {
							currentSurfaceInfo.eventOnBounceCharacter.Invoke ();
						}
					}

					if (currentSurfaceInfo.useParticlesOnSurface && attackPosition != Vector3.zero) {
						GameObject newParticles = (GameObject)Instantiate (currentSurfaceInfo.particlesOnSurface, Vector3.zero, Quaternion.identity);
						newParticles.transform.position = attackPosition;
						newParticles.transform.LookAt (attackPosition + attackNormal * 3);
					}
				}

//				print ("surface type detected " + surfaceName);

				return;
			}
		}
	}

	bool meleeAttackSoundPaused;

	public void setMeleeAttackSoundPausedState (bool state)
	{
		meleeAttackSoundPaused = state;
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
		if (showGizmo && Application.isPlaying && objectThrown) {
			GKC_Utils.drawCapsuleGizmo (point1, point2, capsuleCastRadius, sphereColor, cubeColor, currentRayTargetPosition, rayDirection, distanceToTarget);
		}
	}

	[System.Serializable]
	public class grabbedWeaponInfo
	{
		public string Name;

		[Space]
		[Header ("Movements Settings")]
		[Space]

		public bool setNewIdleID;
		public int idleIDUsed;

		public bool useStrafeMode;
		public int strafeIDUsed;
		public bool setPreviousStrafeModeOnDropObject;
		public bool previousStrafeMode;

		public bool activateStrafeModeOnLockOnTargetActive;
		public bool deactivateStrafeModeOnLockOnTargetDeactivate;

		[Space]

		public bool toggleStrafeModeIfRunningActive;

		public bool setSprintEnabledStateWithWeapon;
		public bool sprintEnabledStateWithWeapon;

		[Space]

		public bool setNewCrouchID;
		public int crouchIDUsed;

		[Space]

		public bool setNewMovementID;
		public int movementIDUsed;

		[Space]
		[Header ("Shield Settings")]
		[Space]

		public bool weaponCanUseShield;

		public int shieldID;

		public string regularMovementBlockShieldActionName;

		public int strafeMovementBlockShieldID;

		public int shieldIDFreeMovement;
		public int shieldIDStrafeMovement;

		public bool isEmptyWeaponToUseOnlyShield;

		[Space]
		[Header ("Events Settings")]
		[Space]

		public bool useEventsOnGrabDropObject;
		public UnityEvent eventOnGrabObject;
		public UnityEvent eventOnDropObject;

		[Space]
		[Header ("Keep/Draw Weapon Events Settings")]
		[Space]

		public bool useEventsOnKeepOrDrawMeleeWeapon;

		public UnityEvent eventsOnKeepMeleeWeapon;
		public UnityEvent eventsOnDrawMeleeWeapon;

		[Space]
		[Header ("Remote Events Settings")]
		[Space]

		public bool useRemoteEventsOnGrabObject;
		public List<string> remoteEventOnGrabObject = new List<string> ();

		[Space]

		public bool useRemoteEventsOnDropObject;
		public List<string> remoteEventOnDropObject = new List<string> ();

		[Space]
		[Header ("Attacks Unable To Block Settings")]
		[Space]

		public bool attacksCantBeBlocked;

		public List<int> attackIDCantBeBlockedList = new List<int> ();

		[Space]
		[Header ("Damage Detected Settings")]
		[Space]

		public bool useEventsOnDamageDetected;

		public bool checkObjectsToUseRemoteEventsOnDamage;
		public LayerMask layerToUseRemoteEventsOnDamage;

		public List<eventOnDamageInfo> eventOnDamageInfoList = new List<eventOnDamageInfo> ();

		[Space]
		[Header ("Range Attack Settings")]
		[Space]

		public bool useRangeAttackID;
		public List<rangeAttackInfo> rangeAttackInfoList = new List<rangeAttackInfo> ();

		[Space]
		[Header ("Press Up/Down Events Settings")]
		[Space]

		public bool useEventsOnPressDownState;
		public UnityEvent eventOnPressDownState;

		public bool useEventsOnPressUpState;
		public UnityEvent eventOnPressUpState;


		[Space]
		[Header ("Custom Position Reference Settings")]
		[Space]

		public bool useCustomGrabbedWeaponReferencePosition;
		public Transform customGrabbedWeaponReferencePosition;

		[Space]

		public bool useCustomReferencePositionToKeepObjectMesh;
		public Transform customReferencePositionToKeepObjectMesh;

		[Space]

		public bool useCustomReferencePositionToKeepObject;
		public Transform customReferencePositionToKeepObject;
	}

	[System.Serializable]
	public class eventOnDamageInfo
	{
		public int damageInfoID;
		public UnityEvent eventOnDamageDetected;

		public bool useRemoteEvent;
		public List<string> remoteEventNameList;
	}

	[System.Serializable]
	public class surfaceInfoOnMeleeAttack
	{
		public string surfaceName;

		[Space]
		[Header ("Sounds And Particles Settings")]
		[Space]

		public bool useSoundsListOnOrder;
		public int currentSoundIndex;
		public List<AudioClip> soundsList;

		public bool useParticlesOnSurface;
		public GameObject particlesOnSurface;

		[Space]
		[Header ("Events Settings")]
		[Space]

		public bool surfaceActivatesBounceOnCharacter;
		public UnityEvent eventOnBounceCharacter;
		public bool stopAttackOnBounce;
	}

	[System.Serializable]
	public class rangeAttackInfo
	{
		public int rangeAttackID;
		public UnityEvent eventOnRangeAttack;
		public UnityEvent eventOnDisableRangeAttack;
	}
}
