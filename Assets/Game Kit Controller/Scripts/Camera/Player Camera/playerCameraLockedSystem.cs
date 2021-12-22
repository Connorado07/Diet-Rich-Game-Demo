using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class playerCameraLockedSystem : MonoBehaviour
{
//	public bool cameraCanBeUsed;
//	public Camera mainCamera;
//	public Transform mainCameraTransform;
//	public Transform pivotCameraTransform;
//
//
//	public Transform playerCameraTransform;
//
//	public bool useCustomThirdPersonAimActive;
//	string customDefaultThirdPersonAimRightStateName;
//	string customDefaultThirdPersonAimLeftStateName;
//
//	public float firstPersonVerticalRotationSpeed = 5;
//	public float firstPersonHorizontalRotationSpeed = 5;
//	public float thirdPersonVerticalRotationSpeed = 5;
//	public float thirdPersonHorizontalRotationSpeed = 5;
//
//	public float currentVerticalRotationSpeed;
//	public float currentHorizontalRotationSpeed;
//
//	float originalFirstPersonVerticalRotationSpeed;
//	float originalFirstPersonHorizontalRotationSpeed;
//	float originalThirdPersonVerticalRotationSpeed;
//	float originalThirdPersonHorizontalRotationSpeed;
//
//	public float smoothBetweenState;
//	public float maxCheckDist = 0.1f;
//	public float movementLerpSpeed = 5;
//	public float zoomSpeed = 120;
//	public float fovChangeSpeed;
//
//	public List<cameraStateInfo> playerCameraStates = new List<cameraStateInfo> ();
//
//	public bool onGround;
//	public bool aimingInThirdPerson;
//	public bool crouching;
//	public bool firstPersonActive;
//	public bool usingZoomOn;
//	public bool usingZoomOff;
//	public bool cameraCanRotate = true;
//
//	public GameObject playerControllerGameObject;
//	public typeOfCamera cameraType;
//
//	public bool cameraCurrentlyLocked;
//
//	public bool changeCameraViewEnabled = true;
//	bool originalChangeCameraViewEnabled;
//
//	bool pausePlayerCameraViewChange;
//
//	public enum typeOfCamera
//	{
//		free,
//		locked
//	}
//
//	public Vector2 lookAngle;
//	public cameraStateInfo currentState;
//	public cameraStateInfo lerpState;
//
//
//	public float lastTimeInputUsedWhenLookInPlayerDirectionActive;
//
//	public bool adjustCameraToPreviousCharacterDirectionActive;
//	public bool useSmoothCameraTransitionBetweenCharacters;
//	public float smoothCameraTransitionBetweenCharactersSpeed;
//
//	public bool setNewCharacterAlwaysInThirdPerson;
//	public bool setNewCharacterAlwaysInFirstPerson;
//	public bool setNewCharacterAlwaysInPreviousCharacterView;
//
//	GameObject previousCharacterControl;
//	Vector3 previousCharacterControlCameraPosition;
//	Quaternion previousCharacterControlCameraRotation;
//	bool previousCharacterControlInFirstPersonView;
//
//	public float horizontalInput;
//	public float verticalInput;
//
//	public bool playerNavMeshEnabled;
//	bool touchPlatform;
//	Touch currentTouch;
//	Vector2 playerLookAngle;
//
//	//Look at target variables
//	public Transform lookAtTargetTransform;
//	public bool lookAtTargetEnabled;
//	public bool canActivateLookAtTargetEnabled = true;
//	public Transform targetToLook;
//	public bool lookingAtTarget;
//
//	public float lookAtTargetSpeed;
//	public float lookCloserAtTargetSpeed = 3;
//	public float lookAtTargetSpeed2_5dView = 5;
//	public float lookAtTargetSpeedOthersLockedViews = 5;
//
//	public float maxDistanceToFindTarget;
//	public bool useLookTargetIcon;
//	public GameObject lookAtTargetIcon;
//	public RectTransform lookAtTargetIconRectTransform;
//	public GameObject lookAtTargetRegularIconGameObject;
//	public GameObject lookAtTargetIconWhenNotAiming;
//
//	bool originalUseLookTargetIconValue;
//
//	public List<string> tagToLookList = new List<string> ();
//	public LayerMask layerToLook;
//	public bool lookOnlyIfTargetOnScreen;
//	public bool checkObstaclesToTarget;
//	public bool getClosestToCameraCenter;
//	public bool useMaxDistanceToCameraCenter;
//	public float maxDistanceToCameraCenter;
//	public bool useTimeToStopAimAssist;
//	public float timeToStopAimAssist;
//
//	public bool useTimeToStopAimAssistLockedCamera;
//	public float timeToStopAimAssistLockedCamera;
//
//	public bool lookingAtPoint;
//	public Vector3 pointTargetPosition;
//	float originalLookAtTargetSpeed;
//	float originalMaxDistanceToFindTarget;
//
//	public bool searchPointToLookComponents;
//	public LayerMask pointToLookComponentsLayer;
//
//	public bool lookAtBodyPartsOnCharacters;
//	public List<string> bodyPartsToLook = new List<string> ();
//	Vector3 lookTargetPosition;
//
//	bool originalLookAtBodyPartsOnCharactersValue;
//
//	public List<Camera> cameraList = new List<Camera> ();
//
//	bool searchingToTargetOnLockedCamera;
//
//	public bool canActiveLookAtTargetOnLockedCamera;
//	bool activeLookAtTargetOnLockedCamera;
//
//	public bool canChangeTargetToLookWithCameraAxis;
//	public float minimumCameraDragToChangeTargetToLook = 1;
//	public float waitTimeToNextChangeTargetToLook = 0.5f;
//	public bool useOnlyHorizontalCameraDragValue;
//
//	//Aim assist variables
//	float lastTimeAimAsisst;
//	bool usingAutoCameraRotation;
//	bool lookintAtTargetByInput;
//
//	Vector3 targetPosition;
//	Vector3 screenPoint;
//	Transform placeToShoot;
//	bool targetOnScreen;
//
//	//Custom editor variables
//	public bool showSettings;
//	public bool showLookTargetSettings;
//	public bool showElements;
//
//	//2.5d variables
//	public bool using2_5ViewActive;
//
//	Vector2 moveCameraLimitsX2_5d;
//	Vector2 moveCameraLimitsY2_5d;
//
//	bool moveInXAxisOn2_5d;
//	Vector3 originalLockedCameraPivotPosition;
//
//	public bool clampAimDirections;
//	public int numberOfAimDirections = 8;
//	public float minDistanceToCenterClampAim = 1.2f;
//
//	bool useCircleCameraLimit2_5d;
//	float circleCameraLimit2_5d;
//	float originalCircleCameraLimit2_5d;
//
//	//Top down variables
//	public bool useTopDownView;
//	Vector2 moveCameraLimitsXTopDown;
//	Vector2 moveCameraLimitsYTopDown;
//
//	bool useCircleCameraLimitTopDown;
//	float circleCameraLimitTopDown;
//	float originalCircleCameraLimitTopDown;
//
//	public bool setLookDirection;
//	Vector3 currentCameraMovementPosition;
//	public Vector3 targetToFollowForward;
//
//	public bool regularMovementOnBulletTime = true;
//
//	bool fieldOfViewChanging;
//	float AccelerometerUpdateInterval = 0.01f;
//	float LowPassKernelWidthInSeconds = 0.001f;
//	float lastTimeMoved;
//	bool adjustPivotAngle;
//
//	bool horizontalCameraLimitActiveOnGround = true;
//	bool horizontalCameraLimitActiveOnAir = true;
//
//	GameObject currentDetectedSurface;
//	GameObject currentTargetDetected;
//	Transform currentTargetToAim;
//
//	bool isMoving;
//	RaycastHit hit;
//	Vector3 lowPassValue = Vector3.zero;
//	Vector2 acelerationAxis;
//
//	Matrix4x4 calibrationMatrix;
//
//	public playerInputManager playerInput;
//	public playerController playerControllerManager;
//
//	public playerWeaponsManager weaponsManager;
//	public playerNavMeshSystem playerNavMeshManager;
//
//	public Collider mainCollider;
//
//	public Transform hipsTransform;
//
//	public Transform targetToFollow;
//
//	bool rotatingLockedCameraFixedRotationAmountActive;
//
//	Coroutine rotateLockedCameraFixedAmountCoroutine;
//
//	//AI variables
//	Vector3 navMeshCurrentLookPos;
//
//	//Locked camera variables
//
//	bool lockedCameraChanged;
//	bool lockedCameraCanFollow;
//
//	bool followFixedCameraPosition;
//
//	Transform currentLockedCameraAxisTransform;
//	Transform previousLockedCameraAxisTransform;
//	lockedCameraSystem.cameraAxis currentLockedCameraAxisInfo;
//	lockedCameraSystem.cameraAxis previousLockedCameraAxisInfo;
//	Coroutine lockedCameraCoroutine;
//	Vector3 lockedCameraFollowPlayerPositionVelocity = Vector3.zero;
//	bool lockedCameraMoving;
//
//	bool inputNotPressed;
//
//	Vector2 currentLockedLoonAngle;
//	Quaternion currentLockedCameraRotation;
//	Quaternion currentLockedPivotRotation;
//	bool usingLockedZoomOn;
//	float lastTimeLockedSpringRotation;
//	float lastTimeLockedSpringMovement;
//	Vector3 currentLockedCameraMovementPosition;
//	Vector3 currentLockedMoveCameraPosition;
//	Vector2 currentLockedLimitLookAngle;
//
//	public Transform lockedCameraElementsParent;
//	public Transform lockedMainCameraTransform;
//	public Transform lockedCameraAxis;
//	public Transform lockedCameraPosition;
//	public Transform lockedCameraPivot;
//
//	public Transform lookCameraParent;
//	public Transform lookCameraPivot;
//	public Transform lookCameraDirection;
//
//	public Transform clampAimDirectionTransform;
//
//	public Transform lookDirectionTransform;
//
//	public Transform auxLockedCameraAxis;
//
//	public RectTransform currentLockedCameraCursor;
//	public Vector2 currentLockedCameraCursorSize;
//
//	public bool useLayerToSearchTargets;
//
//	float horizontaLimit;
//	float verticalLimit;
//
//	float newVerticalPosition;
//	float newVerticalPositionVelocity;
//
//	float newHorizontalPosition;
//	float newHorizontalPositionVelocity;
//
//	//Locked Camera Limits Variables
//	public bool useCameraLimit;
//	public Vector3 currentCameraLimitPosition;
//
//	public bool useWidthLimit;
//	public float widthLimitRight;
//	public float widthLimitLeft;
//	public bool useHeightLimit;
//	public float heightLimitUpper;
//	public float heightLimitLower;
//
//	public bool useDepthLimit;
//	public float depthLimitFront;
//	public float depthLimitBackward;
//
//	public setTransparentSurfaces setTransparentSurfacesManager;
//
//
//	Quaternion currentPivotRotation;
//
//	Quaternion currentCameraRotation;
//
//	float currentCameraUpRotation;
//
//	public cameraRotationType cameraRotationMode;
//
//	public enum cameraRotationType
//	{
//		vertical,
//		horizontal,
//		free
//	}
//
//	float currentDeltaTime;
//	float currentUpdateDeltaTime;
//	float currentFixedUpdateDeltaTime;
//	float currentLateUpdateDeltaTime;
//
//	Vector2 axisValues;
//
//	bool usingPlayerNavMeshPreviously;
//
//	Vector3 cameraInitialPosition;
//	Vector3 offsetInitialPosition;
//
//	Coroutine lookAtTargetEnabledCoroutine;
//	Coroutine fixPlayerZPositionCoroutine;
//	Coroutine cameraFovStartAndEndCoroutine;
//	Coroutine zoomStateDurationCoroutine;
//
//	List<Transform> targetsListToLookTransform = new List<Transform> ();
//	int currentTargetToLookIndex;
//
//	bool driving;
//
//	public bool useEventOnMovingLockedCamera;
//	public UnityEvent eventOnStartMovingLockedCamera;
//	public UnityEvent eventOnKeepMovingLockedCamera;
//	public UnityEvent eventOnStopMovingLockedCamera;
//	public bool useEventOnFreeCamereToo;
//
//	bool movingCameraState;
//	bool movingCameraPrevioslyState;
//
//	//Camera Bound variables
//	Vector3 horizontalOffsetOnSide;
//	Vector3 horizontalOffsetOnFaceSideSpeed;
//
//	Vector3 horizontalOffsetOnSideOnMoving;
//	Vector3 horizontalOffsetOnFaceSideOnMovingSpeed;
//
//	Vector3 verticalOffsetOnMove;
//	Vector3 verticalOffsetOnMoveSpeed;
//
//	public FocusArea focusArea;
//
//	public Vector3 focusTargetPosition;
//
//	bool playerAiming;
//	bool playerAimingPreviously;
//	float lastTimeAiming;
//
//	float originalMaxDistanceToCameraCenterValue;
//	bool originalUsemaxDistanceToCameraCenterValue;
//
//	public bool resetCameraRotationAfterTime;
//	public float timeToResetCameraRotation;
//	public float resetCameraRotationSpeed;
//	float lastTimeCameraRotated;
//	bool resetingCameraActive;
//
//	public bool setExtraCameraRotationEnabled = true;
//	public float extraCameraRotationAmount = 180;
//	public float extraCameraRotationSpeed = 3;
//	public bool useCameraForwardDirection = true;
//
//	Coroutine extraRotationCoroutine;
//
//	Coroutine adjustLockedCameraAxisPositionOnGravityChangeCoroutine;
//
//	bool usingSetTransparentSurfacesPreviously;
//
//	bool rotatingLockedCameraToRight;
//	bool rotatingLockedCameraToLeft;
//
//	float lockedCameraRotationDirection;
//	bool selectingTargetToLookWithMouseActive;
//
//	float currentAxisValuesMagnitude;
//	float lastTimeChangeCurrentTargetToLook;
//	float currentLockedCameraAngle;
//
//	public UnityEvent setThirdPersonInEditorEvent;
//	public UnityEvent setFirstPersonInEditorEvent;
//
//	public bool useEventOnThirdFirstPersonViewChange;
//	public UnityEvent setThirdPersonEvent;
//	public UnityEvent setFirstPersonEvent;
//
//	public GameObject lockedCameraSystemPrefab;
//	public GameObject lockedCameraLimitSystemPrefab;
//
//	public List<lockedCameraPrefabsTypes> lockedCameraPrefabsTypesList = new List<lockedCameraPrefabsTypes> ();
//
//	string currentCameraShakeStateName;
//
//
//	public Vector2 mainCanvasSizeDelta;
//	public Vector2 halfMainCanvasSizeDelta;
//	public bool usingScreenSpaceCamera;
//
//	RectTransform iconRectTransform;
//	Vector3 iconPositionViewPoint;
//	Vector2 iconPosition2d;
//
//	Quaternion pivotRotation;
//
//	int closestWaypointIndex;
//
//	Transform closestWaypoint;
//
//	public float extraCameraCollisionDistance;
//
//	public bool cameraCollisionAlwaysActive = true;
//
//	bool previouslyInFirstPerson;
//
//	public bool cameraRotationInputEnabled = true;
//	public bool cameraActionsInputEnabled = true;
//
//	public bool updateReticleActiveState;
//	public bool externalReticleCurrentlyActive;
//	public bool cameraReticleCurrentlyActive;
//	public bool useCameraReticleThirdPerson;
//	public bool useCameraReticleFirstPerson;
//	public GameObject cameraReticleGameObject;
//	public GameObject mainCameraReticleGameObject;
//
//	Vector2 currentYLimits;
//
//	public bool activateStrafeModeIfNoTargetsToLookFound;
//	bool strafeModeActivateFromNoTargetsFoundActive;
//
//	public LayerMask layerToCheckPossibleTargetsBelowCursor;
//
//	bool useMouseInputToRotateCameraHorizontally;
//
//	Vector3 vector3Zero = Vector3.zero;
//	Quaternion quaternionIdentity = Quaternion.identity;
//
//
//
//	void Awake ()
//	{ 
//
//		touchPlatform = touchJoystick.checkTouchPlatform ();
//
//		if (lockedCameraElementsParent) {
//			lockedCameraElementsParent.SetParent (null);
//			lockedCameraElementsParent.position = vector3Zero;
//
//			lockedCameraElementsParent.rotation = quaternionIdentity;
//		}
//
//		if (lookAtTargetIcon) {
//			lookAtTargetIconRectTransform = lookAtTargetIcon.GetComponent<RectTransform> ();
//		}
//
//	}
//
//	void Update ()
//	{
//		if (!isCameraTypeFree ()) {
//			//if current camera mode is locked, set its values according to every fixed camera trigger configuration
//			checkCameraPosition ();
//
//			playerCameraTransform.position = targetToFollow.position;
//
//			if (lockedCameraChanged) {
//				if (currentLockedCameraAxisInfo.axis != previousLockedCameraAxisTransform) {
//					if (!playerControllerManager.isPlayerMoving (0.6f) && !playerControllerManager.isPlayerUsingInput ()) {
//						inputNotPressed = true;
//					}
//
//					if ((inputNotPressed && (playerControllerManager.isPlayerUsingInput () || !playerControllerManager.isPlayerMoving (0))) ||
//					    !currentLockedCameraAxisInfo.changeLockedCameraDirectionOnInputReleased) {
//
//						setCurrentAxisTransformValues (lockedCameraAxis);
//
//						previousLockedCameraAxisTransform = currentLockedCameraAxisInfo.axis;
//
//						lockedCameraChanged = false;
//						lockedCameraCanFollow = true;
//
//						inputNotPressed = false;
//					}
//				} else {
//					lockedCameraChanged = false;
//				}
//			}
//
//			//look at player position
//			if (!lockedCameraMoving && currentLockedCameraAxisInfo.lookAtPlayerPosition) {
//
//				calculateLockedCameraLookAtPlayerPosition ();
//
//				lockedCameraPosition.localRotation = Quaternion.Slerp (lockedCameraPosition.localRotation, 
//					Quaternion.Euler (new Vector3 (currentLockedLimitLookAngle.x, 0, 0)), currentUpdateDeltaTime * currentLockedCameraAxisInfo.lookAtPlayerPositionSpeed);
//
//				lockedCameraAxis.localRotation = Quaternion.Slerp (lockedCameraAxis.localRotation, 
//					Quaternion.Euler (new Vector3 (0, currentLockedLimitLookAngle.y, 0)),	currentUpdateDeltaTime * currentLockedCameraAxisInfo.lookAtPlayerPositionSpeed);
//
//				if (lockedCameraCanFollow) {
//					setCurrentAxisTransformValues (lockedCameraAxis);
//				}
//			}
//
//			//rotate camera with mouse using a range
//			if (!lockedCameraMoving && currentLockedCameraAxisInfo.cameraCanRotate && cameraCanBeUsed && !playerAiming) {
//				Vector2 currentAxisValues = playerInput.getPlayerMouseAxis ();
//				float horizontalMouse = currentAxisValues.x;
//				float verticalMouse = currentAxisValues.y;
//
//				currentLockedLoonAngle.x += horizontalMouse * currentHorizontalRotationSpeed;
//				currentLockedLoonAngle.y -= verticalMouse * currentVerticalRotationSpeed;
//
//				currentLockedLoonAngle.x = Mathf.Clamp (currentLockedLoonAngle.x, currentLockedCameraAxisInfo.rangeAngleY.x, currentLockedCameraAxisInfo.rangeAngleY.y);
//
//				currentLockedLoonAngle.y = Mathf.Clamp (currentLockedLoonAngle.y, currentLockedCameraAxisInfo.rangeAngleX.x, currentLockedCameraAxisInfo.rangeAngleX.y);
//
//				if (currentLockedCameraAxisInfo.smoothRotation) {
//
//					currentLockedCameraRotation = Quaternion.Euler (currentLockedCameraAxisInfo.originalCameraRotationX + currentLockedLoonAngle.y, 0, 0);
//
//					currentLockedPivotRotation = Quaternion.Euler (0, currentLockedCameraAxisInfo.originalPivotRotationY + currentLockedLoonAngle.x, 0);
//
//					if (currentLockedCameraAxisInfo.useSpringRotation) {
//						if (horizontalMouse != 0 || verticalMouse != 0) {
//							lastTimeLockedSpringRotation = Time.time;
//
//						}
//						if (Time.time > lastTimeLockedSpringRotation + currentLockedCameraAxisInfo.springRotationDelay) {
//							currentLockedCameraRotation = Quaternion.Euler (currentLockedCameraAxisInfo.originalCameraRotationX, 0, 0);
//							currentLockedPivotRotation = Quaternion.Euler (0, currentLockedCameraAxisInfo.originalPivotRotationY, 0);
//							currentLockedLoonAngle = Vector2.zero;
//						}
//					}
//
//					lockedCameraPosition.localRotation = Quaternion.Slerp (lockedCameraPosition.localRotation, currentLockedCameraRotation, 
//						currentUpdateDeltaTime * currentLockedCameraAxisInfo.rotationSpeed);
//
//					lockedCameraAxis.localRotation = Quaternion.Slerp (lockedCameraAxis.localRotation, currentLockedPivotRotation, 
//						currentUpdateDeltaTime * currentLockedCameraAxisInfo.rotationSpeed);
//
//				} else {
//					lockedCameraPosition.localRotation = Quaternion.Euler (currentLockedCameraAxisInfo.originalCameraRotationX + currentLockedLoonAngle.y, 0, 0);
//
//					lockedCameraAxis.localRotation = Quaternion.Euler (0, currentLockedCameraAxisInfo.originalPivotRotationY + currentLockedLoonAngle.x, 0);
//				}
//			}
//
//			//move camera up, down, right and left
//			if (!lockedCameraMoving && currentLockedCameraAxisInfo.canMoveCamera && cameraCanBeUsed && !playerAiming) {
//				Vector2 currentAxisValues = playerInput.getPlayerMouseAxis ();
//				float horizontalMouse = currentAxisValues.x;
//				float verticalMouse = currentAxisValues.y;
//
//				currentLockedMoveCameraPosition.x += horizontalMouse * currentLockedCameraAxisInfo.moveCameraSpeed;
//				currentLockedMoveCameraPosition.y += verticalMouse * currentLockedCameraAxisInfo.moveCameraSpeed;
//
//				currentLockedMoveCameraPosition.x = Mathf.Clamp (currentLockedMoveCameraPosition.x, currentLockedCameraAxisInfo.moveCameraLimitsX.x, currentLockedCameraAxisInfo.moveCameraLimitsX.y);
//
//				currentLockedMoveCameraPosition.y = Mathf.Clamp (currentLockedMoveCameraPosition.y, currentLockedCameraAxisInfo.moveCameraLimitsY.x, currentLockedCameraAxisInfo.moveCameraLimitsY.y);
//
//				Vector3 moveInput = currentLockedMoveCameraPosition.x * Vector3.right +	currentLockedMoveCameraPosition.y * Vector3.up;	
//
//				if (currentLockedCameraAxisInfo.smoothCameraMovement) {
//					currentLockedCameraMovementPosition = currentLockedCameraAxisInfo.originalCameraAxisLocalPosition + moveInput;
//
//					if (currentLockedCameraAxisInfo.useSpringMovement) {
//						if (horizontalMouse != 0 || verticalMouse != 0) {
//							lastTimeLockedSpringMovement = Time.time;
//
//						}
//						if (Time.time > lastTimeLockedSpringMovement + currentLockedCameraAxisInfo.springMovementDelay) {
//							currentLockedCameraMovementPosition = currentLockedCameraAxisInfo.originalCameraAxisLocalPosition;
//							currentLockedMoveCameraPosition = vector3Zero;
//						}
//					}
//
//					lockedCameraAxis.localPosition = Vector3.MoveTowards (lockedCameraAxis.localPosition, currentLockedCameraMovementPosition, 
//						currentUpdateDeltaTime * currentLockedCameraAxisInfo.smoothCameraSpeed);
//				} else {
//					lockedCameraAxis.localPosition = currentLockedCameraAxisInfo.originalCameraAxisLocalPosition + moveInput;
//				}
//			}
//
//			if (useEventOnMovingLockedCamera) {
//				checkEventOnMoveCamera (playerInput.getPlayerMouseAxis ());
//			}
//
//			if (!lockedCameraMoving && cameraCanBeUsed && currentLockedCameraAxisInfo.canRotateCameraHorizontally) {
//
//				if (!currentLockedCameraAxisInfo.useFixedRotationAmount) {
//					float currentRotationValue = 0;
//
//					if (currentLockedCameraAxisInfo.useMouseInputToRotateCameraHorizontally || useMouseInputToRotateCameraHorizontally) {
//						if (!isPlayerAiming ()) {
//							currentRotationValue = currentUpdateDeltaTime * currentLockedCameraAxisInfo.horizontalCameraRotationSpeed * playerInput.getPlayerMouseAxis ().x;
//						}
//					} else {
//						if (rotatingLockedCameraToLeft || rotatingLockedCameraToRight) {
//							currentRotationValue = currentUpdateDeltaTime * currentLockedCameraAxisInfo.horizontalCameraRotationSpeed * lockedCameraRotationDirection;
//						}
//					}
//
//					lockedMainCameraTransform.Rotate (0, currentRotationValue, 0);
//					lockedCameraPivot.Rotate (0, currentRotationValue, 0);
//				}
//			}
//
//			if (!lockedCameraMoving && currentLockedCameraAxisInfo.useDistanceToTransformToMoveCameraCloserOrFarther) {
//				Vector3 originalCameraPosition = currentLockedCameraAxisInfo.originalCameraLocalPosition;
//
//				float distanceToPlayer = GKC_Utils.distance (currentLockedCameraAxisInfo.transformToCalculateDistance.position, targetToFollow.position);
//
//				distanceToPlayer *= currentLockedCameraAxisInfo.cameraDistanceMultiplier;
//
//				if (currentLockedCameraAxisInfo.useClampCameraBackwardDirection) {
//					distanceToPlayer = Mathf.Clamp (distanceToPlayer, 0, currentLockedCameraAxisInfo.clampCameraBackwardDirection);
//				}
//
//				Vector3 newPosition = (Vector3.forward * distanceToPlayer);
//
//				if (currentLockedCameraAxisInfo.useDistanceDirectlyProportional) {
//					newPosition = -newPosition;
//				}
//
//				if (currentLockedCameraAxisInfo.useClampCameraForwardDirection) {
//					if (newPosition.z > 0) {
//						newPosition = vector3Zero;
//					}
//				}
//
//				lockedCameraPosition.localPosition = Vector3.Lerp (lockedCameraPosition.localPosition, originalCameraPosition + newPosition, currentUpdateDeltaTime * currentLockedCameraAxisInfo.moveCameraCloserOrFartherSpeed);
//			}
//
//			if (!lockedCameraMoving && currentLockedCameraAxisInfo.useZoomByMovingCamera && currentLockedCameraAxisInfo.canUseZoom) {
//				Vector3 originalCameraPosition = currentLockedCameraAxisInfo.originalCameraLocalPosition;
//
//				lockedCameraZoomMovingCameraValue = Mathf.Clamp (lockedCameraZoomMovingCameraValue, currentLockedCameraAxisInfo.zoomCameraOffsets.x, currentLockedCameraAxisInfo.zoomCameraOffsets.y); 
//
//				Vector3 newPosition = (currentLockedCameraAxisInfo.originalCameraForward * lockedCameraZoomMovingCameraValue);
//
//				if (currentLockedCameraAxisInfo.zoomByMovingCameraDirectlyProportional) {
//					newPosition = -newPosition;
//				}
//
//				lockedCameraPosition.localPosition = 
//					Vector3.Lerp (lockedCameraPosition.localPosition, 
//					originalCameraPosition + newPosition, 
//					currentUpdateDeltaTime * currentLockedCameraAxisInfo.zoomByMovingCameraSpeed);
//			}
//
//			//aim weapon
//			if (playerAiming) {
//				if (currentLockedCameraCursor != null) {
//					float horizontalMouse = 0;
//					float verticalMouse = 0;
//
//					if (cameraCanBeUsed && !lookingAtFixedTarget) {
//						Vector2 currentAxisValues = playerInput.getPlayerMouseAxis ();
//
//						horizontalMouse = currentAxisValues.x;
//						verticalMouse = currentAxisValues.y;
//					}
//
//					//if the player is on 2.5d view, set the cursor position on screen where the player will aim
//					if (using2_5ViewActive) {
//						if (Time.time < lastTimeAiming + 0.01f) {
//							return;
//						}
//
//						if (!setLookDirection) {
//							moveCameraLimitsX2_5d = currentLockedCameraAxisInfo.moveCameraLimitsX2_5d;
//							moveCameraLimitsY2_5d = currentLockedCameraAxisInfo.moveCameraLimitsY2_5d;
//
//							useCircleCameraLimit2_5d = currentLockedCameraAxisInfo.useCircleCameraLimit2_5d;
//							circleCameraLimit2_5d = currentLockedCameraAxisInfo.circleCameraLimit2_5d;
//							originalCircleCameraLimit2_5d = circleCameraLimit2_5d;
//
//							if (targetToLook) {
//								Vector3 worldPosition = targetToLook.position;
//
//								if (moveInXAxisOn2_5d) {
//									lookCameraDirection.position = new Vector3 (worldPosition.x, worldPosition.y, lookCameraDirection.position.z);
//								} else {
//									lookCameraDirection.position = new Vector3 (lookCameraDirection.position.x, worldPosition.y, worldPosition.z);
//								}
//							} else {
//
//								//Check the rotation of the player in his local Y axis to check the closest direction to look
//								bool lookingAtRight = false;
//								float lookingDirectionAngle = 0;
//
//								if (moveInXAxisOn2_5d) {
//									lookingDirectionAngle = Vector3.Dot (targetToFollow.forward, lookCameraPivot.right); 
//								} else {
//									lookingDirectionAngle = Vector3.Dot (targetToFollow.forward, lookCameraPivot.forward); 
//								}
//
//								if (lookingDirectionAngle > 0) {
//									lookingAtRight = true;
//								}
//
//								//The player will look in the right direction of the screen
//								if (lookingAtRight) {
//									if (moveInXAxisOn2_5d) {
//										lookCameraDirection.localPosition = new Vector3 (moveCameraLimitsX2_5d.y, 0, 0);
//									} else {
//										lookCameraDirection.localPosition = new Vector3 (0, 0, moveCameraLimitsX2_5d.y);
//									}
//									currentCameraMovementPosition.x = moveCameraLimitsX2_5d.y;
//								} 
//
//								//else the player will look in the left direction
//								else {
//									if (moveInXAxisOn2_5d) {
//										lookCameraDirection.localPosition = new Vector3 (moveCameraLimitsX2_5d.x, 0, 0);
//									} else {
//										lookCameraDirection.localPosition = new Vector3 (0, 0, moveCameraLimitsX2_5d.x);
//									}
//									currentCameraMovementPosition.x = moveCameraLimitsX2_5d.x;
//								}
//
//								if (moveInXAxisOn2_5d) {
//									lookCameraDirection.localPosition = new Vector3 (currentCameraMovementPosition.x, 0, lookCameraDirection.localPosition.z);
//								} else {
//									lookCameraDirection.localPosition = new Vector3 (lookCameraDirection.localPosition.x, 0, currentCameraMovementPosition.x);
//								}
//							}
//
//							setLookDirection = true;
//
//							pivotCameraTransform.localRotation = quaternionIdentity;
//							playerCameraTransform.rotation = targetToFollow.rotation;
//
//							//set the transform position and rotation to follow the lookCameraDirection direction only in the local Y axis, to get the correct direction to look to right or left
//							lookDirectionTransform.localRotation = Quaternion.Euler (getLookDirectionTransformRotationValue (playerCameraTransform.forward));
//
//							lookAngle = Vector2.zero;
//
//							clampAimDirectionTransform.localPosition = lookCameraDirection.localPosition;
//						}
//
//
//						//if the camera is following a target, set that direction to the camera to aim directly at that object
//						if (targetToLook) {
//							Vector3 worldPosition = targetToLook.position;
//							if (moveInXAxisOn2_5d) {
//								lookCameraDirection.position = new Vector3 (worldPosition.x, worldPosition.y, lookCameraDirection.position.z);
//							} else {
//								lookCameraDirection.position = new Vector3 (lookCameraDirection.position.x, worldPosition.y, worldPosition.z);
//							}
//						} else {
//							//else, the player is aiming freely on the screen
//							if (moveInXAxisOn2_5d) {
//								currentCameraMovementPosition = horizontalMouse * currentLockedCameraAxisInfo.moveCameraCursorSpeed * Vector3.right;
//							} else {
//								currentCameraMovementPosition = horizontalMouse * currentLockedCameraAxisInfo.moveCameraCursorSpeed * Vector3.forward;
//							}
//
//							if (currentLockedCameraAxisInfo.reverseMovementDirection) {
//								currentCameraMovementPosition = -currentCameraMovementPosition;
//							}
//
//							currentCameraMovementPosition += verticalMouse * currentLockedCameraAxisInfo.moveCameraCursorSpeed * Vector3.up;
//
//							lookCameraDirection.Translate (lookCameraDirection.InverseTransformDirection (currentCameraMovementPosition));
//
//							//clamp aim direction in 8, 4 or 2 directions
//							if (clampAimDirections) {
//								bool lookingAtRight = false;
//								float lookingDirectionAngle = 0;
//
//								if (moveInXAxisOn2_5d) {
//									lookingDirectionAngle = Vector3.Dot (lookDirectionTransform.forward, lookCameraPivot.right); 
//								} else {
//									lookingDirectionAngle = Vector3.Dot (lookDirectionTransform.forward, lookCameraPivot.forward); 
//								}
//
//								if (lookingDirectionAngle > 0) {
//									lookingAtRight = true;
//								}
//
//								float targetHorizontalValue = 0;
//
//								Vector3 currentDirectionToLook = lookCameraDirection.position - lookDirectionTransform.position;
//
//								if (lookingAtRight) {
//									if (moveInXAxisOn2_5d) {
//										targetHorizontalValue = Vector3.SignedAngle (currentDirectionToLook, lookCameraPivot.right, lookCameraPivot.forward);
//									} else {
//										targetHorizontalValue = -Vector3.SignedAngle (currentDirectionToLook, lookCameraPivot.forward, lookCameraPivot.right);
//									}
//								} else {
//									if (moveInXAxisOn2_5d) {
//										targetHorizontalValue = Vector3.SignedAngle (currentDirectionToLook, -lookCameraPivot.right, -lookCameraPivot.forward);
//									} else {
//										targetHorizontalValue = -Vector3.SignedAngle (currentDirectionToLook, -lookCameraPivot.forward, -lookCameraPivot.right);
//									}
//								}
//
//								Vector2 new2DPosition = Vector2.zero;
//
//								float distanceToCenter = GKC_Utils.distance (lookCameraDirection.localPosition, vector3Zero);
//
//								//print (lookingAtRight + " " + targetHorizontalValue + " " + distanceToCenter);
//
//								if (numberOfAimDirections == 8) {
//									if (targetHorizontalValue > 0) {
//										if (targetHorizontalValue < 30 || distanceToCenter < minDistanceToCenterClampAim) {
//											if (lookingAtRight) {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.y);
//											} else {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.x);
//											}
//										} else if (targetHorizontalValue > 30 && targetHorizontalValue < 60) {
//											if (lookingAtRight) {
//												new2DPosition = new Vector2 (moveCameraLimitsY2_5d.x, moveCameraLimitsX2_5d.y);
//											} else {
//												new2DPosition = new Vector2 (moveCameraLimitsY2_5d.x, moveCameraLimitsX2_5d.x);
//											}
//										} else {
//											new2DPosition = new Vector2 (moveCameraLimitsY2_5d.x, 0);
//										}
//									} else {
//										if (targetHorizontalValue > -30 || distanceToCenter < minDistanceToCenterClampAim) {
//											if (lookingAtRight) {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.y);
//											} else {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.x);
//											}
//										} else if (targetHorizontalValue < -30 && targetHorizontalValue > -60) {
//											if (lookingAtRight) {
//												new2DPosition = new Vector2 (moveCameraLimitsY2_5d.y, moveCameraLimitsX2_5d.y);
//											} else {
//												new2DPosition = new Vector2 (moveCameraLimitsY2_5d.y, moveCameraLimitsX2_5d.x);
//											}
//										} else {
//											new2DPosition = new Vector2 (moveCameraLimitsY2_5d.y, 0);
//										}
//									}
//								} else if (numberOfAimDirections == 4) {
//									if (targetHorizontalValue > 0) {
//										if (targetHorizontalValue < 45 || distanceToCenter < minDistanceToCenterClampAim) {
//											if (lookingAtRight) {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.y);
//											} else {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.x);
//											}
//										} else {
//											new2DPosition = new Vector2 (moveCameraLimitsY2_5d.x, 0);
//										}
//									} else {
//										if (targetHorizontalValue > -45 || distanceToCenter < minDistanceToCenterClampAim) {
//											if (lookingAtRight) {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.y);
//											} else {
//												new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.x);
//											}
//										} else {
//											new2DPosition = new Vector2 (moveCameraLimitsY2_5d.y, 0);
//										}
//									}
//								} else if (numberOfAimDirections == 2) {
//									if (lookingAtRight) {
//										new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.y);
//									} else {
//										new2DPosition = new Vector2 (0, moveCameraLimitsX2_5d.x);
//									}
//								}
//
//								if (moveInXAxisOn2_5d) {
//									clampAimDirectionTransform.localPosition = new Vector3 (new2DPosition.y, new2DPosition.x, lookCameraDirection.localPosition.z);
//								} else {
//									clampAimDirectionTransform.localPosition = new Vector3 (lookCameraDirection.localPosition.x, new2DPosition.x, new2DPosition.y);
//								}
//							}
//						}
//
//						//clamp the aim position to the limits of the current camera
//						Vector3 newCameraPosition = lookCameraDirection.localPosition;
//
//						float posX = 0;
//						float posY = 0;
//						float posZ = 0;
//
//						if (useCircleCameraLimit2_5d) {
//							if (currentLockedCameraAxisInfo.scaleCircleCameraLimitWithDistanceToCamera2_5d) {
//								float distanceOfCamera = lockedCameraZoomMovingCameraValue * 0.5f;
//
//								circleCameraLimit2_5d = originalCircleCameraLimit2_5d + distanceOfCamera;
//
//								if (currentLockedCameraAxisInfo.circleCameraLimitScaleClamp2_5d != Vector2.zero) {
//									circleCameraLimit2_5d = Mathf.Clamp (circleCameraLimit2_5d, 
//										currentLockedCameraAxisInfo.circleCameraLimitScaleClamp2_5d.x, currentLockedCameraAxisInfo.circleCameraLimitScaleClamp2_5d.y);
//								}
//
//							}
//
//							Vector3 newCirclePosition = Vector3.ClampMagnitude (newCameraPosition, circleCameraLimit2_5d);
//
//							posY = newCirclePosition.y;
//							posX = newCirclePosition.x;
//							posZ = newCirclePosition.z;
//						} else {
//							posY = Mathf.Clamp (newCameraPosition.y, moveCameraLimitsY2_5d.x, moveCameraLimitsY2_5d.y);
//
//							if (moveInXAxisOn2_5d) {
//								posX = Mathf.Clamp (newCameraPosition.x, moveCameraLimitsX2_5d.x, moveCameraLimitsX2_5d.y);
//							} else {
//								posZ = Mathf.Clamp (newCameraPosition.z, moveCameraLimitsX2_5d.x, moveCameraLimitsX2_5d.y);
//							}
//						}
//
//						if (moveInXAxisOn2_5d) {
//							lookCameraDirection.localPosition = new Vector3 (posX, posY, newCameraPosition.z);
//						} else {
//							lookCameraDirection.localPosition = new Vector3 (newCameraPosition.x, posY, posZ);
//						}
//
//						if (clampAimDirections && targetToLook == null) {
//							pointTargetPosition = clampAimDirectionTransform.position;
//						} else {
//							pointTargetPosition = lookCameraDirection.position;
//						}
//
//						//set the position to the UI icon showing the position where teh player aims
//						if (usingScreenSpaceCamera) {
//							currentLockedCameraCursor.anchoredPosition = getIconPosition (pointTargetPosition);
//						} else {
//							screenPoint = mainCamera.WorldToScreenPoint (pointTargetPosition);
//							currentLockedCameraCursor.transform.position = screenPoint;
//						}
//
//
//						//set the transform position and rotation to follow the lookCameraDirection direction only in the local Y axis, to get the correct direction to look to right or left
//						Vector3 newDirectionToLook = lookCameraDirection.position - lookDirectionTransform.position;
//
//						Vector3 newLookDirectionTransformRotation = getLookDirectionTransformRotationValue (newDirectionToLook);
//
//						lookDirectionTransform.localRotation = Quaternion.Lerp (lookDirectionTransform.localRotation, Quaternion.Euler (newLookDirectionTransformRotation), Time.deltaTime * 5);
//					} 
//
//					//else, the player is on top down view or in point and click mode, so check the cursor position to aim
//					else {
//						//the current view is top down
//						if (useTopDownView) {
//							//							if (Time.time < lastTimeAiming + 0.01f) {
//							//								return;
//							//							}
//
//							if (currentLockedCameraAxisInfo.useCustomPivotHeightOffset) {
//								lookCameraPivot.localPosition = new Vector3 (0, currentLockedCameraAxisInfo.customPivotOffset, 0);
//							}
//
//							lookCameraParent.localRotation = lockedCameraAxis.localRotation;
//
//							if (!setLookDirection) {
//								moveCameraLimitsXTopDown = currentLockedCameraAxisInfo.moveCameraLimitsXTopDown;
//								moveCameraLimitsYTopDown = currentLockedCameraAxisInfo.moveCameraLimitsYTopDown;
//
//								useCircleCameraLimitTopDown = currentLockedCameraAxisInfo.useCircleCameraLimitTopDown;
//								circleCameraLimitTopDown = currentLockedCameraAxisInfo.circleCameraLimitTopDown;
//								originalCircleCameraLimitTopDown = circleCameraLimitTopDown;
//
//								//								if (targetToLook) {
//								//									print (targetToLook.name);
//								//								}
//
//								playerCameraTransform.rotation = targetToFollow.rotation;
//
//								if (targetToLook) {
//									Vector3 worldPosition = mainCamera.ScreenToWorldPoint (currentLockedCameraCursor.position);
//									lookCameraDirection.position = new Vector3 (worldPosition.x, lookCameraDirection.position.y, worldPosition.z);
//								} else {
//									if (currentLockedCameraAxisInfo.use8DiagonalAim) {
//										float currentPlayerRotationY = targetToFollow.eulerAngles.y + currentLockedCameraAxisInfo.extraTopDownYRotation;
//										if (currentPlayerRotationY > 180) {
//											currentPlayerRotationY -= 360;
//										}
//
//										float currentPlayerRotationYABS = Mathf.Abs (currentPlayerRotationY);
//
//										//check the current forward direction in Y axis to aim to the closes direction in an angle diviced in 8 directions, so every angles is 360/8=45
//										if (currentPlayerRotationYABS < 45) {
//											if (currentPlayerRotationYABS > 22.5f) {
//												if (currentPlayerRotationY > 0) {
//													currentCameraMovementPosition.x = moveCameraLimitsXTopDown.y;
//												} else {
//													currentCameraMovementPosition.x = moveCameraLimitsXTopDown.x;
//												}
//												currentCameraMovementPosition.y = moveCameraLimitsYTopDown.y;
//											} else {
//												currentCameraMovementPosition.x = 0;
//												currentCameraMovementPosition.y = moveCameraLimitsYTopDown.y;
//											}
//										} else if (currentPlayerRotationYABS > 45 && currentPlayerRotationYABS < 135) {
//											if (currentPlayerRotationY > 0) {
//												currentCameraMovementPosition.x = moveCameraLimitsXTopDown.y;
//											} else {
//												currentCameraMovementPosition.x = moveCameraLimitsXTopDown.x;
//											}
//											currentCameraMovementPosition.y = 0;
//										} else if (currentPlayerRotationYABS > 135) {
//											if (currentPlayerRotationYABS < 157.5f) {
//												if (currentPlayerRotationY > 0) {
//													currentCameraMovementPosition.x = moveCameraLimitsXTopDown.y;
//												} else {
//													currentCameraMovementPosition.x = moveCameraLimitsXTopDown.x;
//												}
//												currentCameraMovementPosition.y = moveCameraLimitsYTopDown.x;
//											} else {
//												currentCameraMovementPosition.x = 0;
//												currentCameraMovementPosition.y = moveCameraLimitsYTopDown.x;
//											}
//										}
//
//										lookCameraDirection.localPosition = new Vector3 (currentCameraMovementPosition.x / 2, lookCameraDirection.localPosition.y, currentCameraMovementPosition.y / 2);
//									} else {
//										currentCameraMovementPosition = targetToFollow.position + targetToFollowForward * 4;
//										lookCameraDirection.position = new Vector3 (currentCameraMovementPosition.x, lookCameraDirection.position.y, currentCameraMovementPosition.z);
//									}
//								}
//
//								setLookDirection = true;
//							}
//
//							if (targetToLook) {
//								Vector3 worldPosition = targetToLook.position;
//								lookCameraDirection.position = new Vector3 (worldPosition.x, lookCameraDirection.position.y, worldPosition.z);
//							} else {
//								currentCameraMovementPosition = 
//									horizontalMouse * currentLockedCameraAxisInfo.moveCameraCursorSpeed * Vector3.right +
//								verticalMouse * currentLockedCameraAxisInfo.moveCameraCursorSpeed * Vector3.forward;	
//
//								lookCameraDirection.Translate (currentCameraMovementPosition);
//							}
//
//							Vector3 newCameraPosition = lookCameraDirection.localPosition;
//
//							float posX, posZ;
//
//							if (useCircleCameraLimitTopDown) {
//								if (currentLockedCameraAxisInfo.scaleCircleCameraLimitWithDistanceToCamera) {
//									float distanceOfCamera = lockedCameraZoomMovingCameraValue * 0.5f;
//
//									circleCameraLimitTopDown = originalCircleCameraLimitTopDown + distanceOfCamera;
//
//									if (currentLockedCameraAxisInfo.circleCameraLimitScaleClamp != Vector2.zero) {
//										circleCameraLimitTopDown = Mathf.Clamp (circleCameraLimitTopDown, 
//											currentLockedCameraAxisInfo.circleCameraLimitScaleClamp.x, currentLockedCameraAxisInfo.circleCameraLimitScaleClamp.y);
//									}
//								}
//
//								Vector3 newCirclePosition = Vector3.ClampMagnitude (newCameraPosition, circleCameraLimitTopDown);
//								//								Vector3 newCirclePosition = GKC_Utils.ClampMagnitude (newCameraPosition, circleCameraLimitTopDown, 1);
//
//								posX = newCirclePosition.x;
//								posZ = newCirclePosition.z;
//
//							} else {
//								posX = Mathf.Clamp (newCameraPosition.x, moveCameraLimitsXTopDown.x, moveCameraLimitsXTopDown.y);
//								posZ = Mathf.Clamp (newCameraPosition.z, moveCameraLimitsYTopDown.x, moveCameraLimitsYTopDown.y);
//							}
//
//							lookCameraDirection.localPosition = new Vector3 (posX, newCameraPosition.y, posZ);
//
//							pointTargetPosition = lookCameraDirection.position;
//
//							if (usingScreenSpaceCamera) {
//								currentLockedCameraCursor.anchoredPosition = getIconPosition (pointTargetPosition);
//							} else {
//								screenPoint = mainCamera.WorldToScreenPoint (pointTargetPosition);
//								currentLockedCameraCursor.transform.position = screenPoint;
//							}
//
//							if (currentLockedCameraAxisInfo.checkPossibleTargetsBelowCursor) {
//								//check objects below the current camera cursor on the screen to check possible targets to aim, getting their proper place to shoot position
//								Ray newRay = mainCamera.ScreenPointToRay (currentLockedCameraCursor.position);
//
//								if (Physics.Raycast (newRay, out hit, Mathf.Infinity, layerToCheckPossibleTargetsBelowCursor)) {
//									currentDetectedSurface = hit.collider.gameObject;
//								} else {
//									currentDetectedSurface = null;
//								}
//
//								if (currentTargetDetected != currentDetectedSurface) {
//									currentTargetDetected = currentDetectedSurface;
//
//									if (currentTargetDetected) {
//										currentTargetToAim = applyDamage.getPlaceToShoot (currentTargetDetected);
//										//										if (currentTargetToAim) {
//										//											print (currentTargetToAim.name);
//										//										}
//									} else {
//										currentTargetToAim = null;
//									}
//								}
//
//								if (currentTargetToAim) {
//									pointTargetPosition = currentTargetToAim.position;
//								}
//							}
//						} 
//
//						//the player is on point and click camera type
//						else {
//							if (playerNavMeshEnabled) {
//								if (cameraCanBeUsed) {
//									int touchCount = Input.touchCount;
//									if (!touchPlatform) {
//										touchCount++;
//									}
//									for (int i = 0; i < touchCount; i++) {
//										if (!touchPlatform) {
//											currentTouch = touchJoystick.convertMouseIntoFinger ();
//										} else {
//											currentTouch = Input.GetTouch (i);
//										}
//
//										if (touchPlatform) {
//											if (currentTouch.phase == TouchPhase.Began) {
//												currentLockedCameraCursor.position = currentTouch.position;
//											}
//										} else {
//											currentLockedCameraCursor.position = currentTouch.position;
//										}
//									}
//								}
//							} else {
//								if (!setLookDirection) {
//									setLookDirection = true;
//									playerCameraTransform.rotation = targetToFollow.rotation;
//
//									Vector3 positionToFollow = targetToFollow.position;
//
//									if (!targetToLook) {
//										bool surfaceFound = false;
//										bool surfaceFoundOnScreen = false;
//										Vector2 screenPos = Vector2.zero;
//
//										if (Physics.Raycast (positionToFollow + targetToFollow.up, targetToFollow.forward, out hit, Mathf.Infinity, layerToLook)) {
//											surfaceFound = true;
//
//											screenPos = mainCamera.WorldToScreenPoint (hit.point);
//											if (screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height) {
//												surfaceFoundOnScreen = true;
//											}
//										}
//
//										if (!surfaceFound || !surfaceFoundOnScreen) {
//											screenPos = mainCamera.WorldToScreenPoint (positionToFollow + targetToFollow.forward * 3);
//											Debug.DrawLine (positionToFollow, positionToFollow + targetToFollow.forward * 3, Color.white, 5);
//										}
//
//										if (currentLockedCameraCursor) {
//											currentLockedCameraCursor.position = screenPos;
//										}
//
//										lookAngle = Vector2.zero;
//
//										pivotCameraTransform.localRotation = quaternionIdentity;
//										playerCameraTransform.rotation = targetToFollow.rotation;
//									}
//								}
//
//								currentLockedCameraCursor.Translate (new Vector3 (horizontalMouse, verticalMouse, 0) * currentLockedCameraAxisInfo.moveCameraCursorSpeed);
//							}
//
//							Vector3 newCameraPosition = currentLockedCameraCursor.position;
//							newCameraPosition.x = Mathf.Clamp (newCameraPosition.x, currentLockedCameraCursorSize.x, horizontaLimit);
//							newCameraPosition.y = Mathf.Clamp (newCameraPosition.y, currentLockedCameraCursorSize.y, verticalLimit);
//							currentLockedCameraCursor.position = new Vector3 (newCameraPosition.x, newCameraPosition.y, 0);
//
//							Ray newRay = mainCamera.ScreenPointToRay (currentLockedCameraCursor.position);
//							if (Physics.Raycast (newRay, out hit, Mathf.Infinity, layerToLook)) {
//								pointTargetPosition = hit.point;
//							} else {
//								print ("look at screen point in work position");
//							}
//						}
//					}
//				}
//			} else {
//				if (setLookDirection) {
//					if (using2_5ViewActive) {
//						lookCameraDirection.localPosition = vector3Zero;
//
//						playerCameraTransform.rotation = targetToFollow.rotation;
//					}
//
//					if (useTopDownView) {
//						if (currentLockedCameraAxisInfo.showCameraCursorWhenNotAiming) {
//							settingShowCameraCursorWhenNotAimingBackToActive = true;
//
//							activateShowCameraCursorWhenNotAimingState ();
//
//							settingShowCameraCursorWhenNotAimingBackToActive = false;
//						} else {
//							lookCameraDirection.localPosition = new Vector3 (0, lookCameraDirection.localPosition.y, 0);
//						}
//					}
//				}
//
//				currentCameraMovementPosition = vector3Zero;
//				setLookDirection = false;
//
//				if (!lockedCameraMoving) {
//					if (using2_5ViewActive) {
//						playerCameraTransform.rotation = targetToFollow.rotation;
//					}
//
//					if (useTopDownView) {
//						if (currentLockedCameraAxisInfo.showCameraCursorWhenNotAiming && currentLockedCameraCursor) {
//
//							Vector2 currentAxisValues = playerInput.getPlayerMouseAxis ();
//							float horizontalMouse = currentAxisValues.x;
//							float verticalMouse = currentAxisValues.y;
//
//							lookCameraParent.localRotation = lockedCameraAxis.localRotation;
//
//							if (targetToLook) {
//								Vector3 worldPosition = targetToLook.position;
//								lookCameraDirection.position = new Vector3 (worldPosition.x, lookCameraDirection.position.y, worldPosition.z);
//							} else {
//								currentCameraMovementPosition = 
//									horizontalMouse * currentLockedCameraAxisInfo.moveCameraCursorSpeed * Vector3.right +
//								verticalMouse * currentLockedCameraAxisInfo.moveCameraCursorSpeed * Vector3.forward;	
//
//								lookCameraDirection.Translate (currentCameraMovementPosition);
//							}
//
//							Vector3 newCameraPosition = lookCameraDirection.localPosition;
//							float posX, posZ;
//							posX = Mathf.Clamp (newCameraPosition.x, moveCameraLimitsXTopDown.x, moveCameraLimitsXTopDown.y);
//							posZ = Mathf.Clamp (newCameraPosition.z, moveCameraLimitsYTopDown.x, moveCameraLimitsYTopDown.y);
//							lookCameraDirection.localPosition = new Vector3 (posX, newCameraPosition.y, posZ);
//
//							pointTargetPosition = lookCameraDirection.position;
//
//							if (usingScreenSpaceCamera) {
//								currentLockedCameraCursor.anchoredPosition = getIconPosition (pointTargetPosition);
//							} else {
//								screenPoint = mainCamera.WorldToScreenPoint (pointTargetPosition);
//								currentLockedCameraCursor.transform.position = screenPoint;
//							}
//
//							if (currentLockedCameraAxisInfo.checkPossibleTargetsBelowCursor) {
//								//check objects below the current camera cursor on the screen to check possible targets to aim, getting their proper place to shoot position
//								Ray newRay = mainCamera.ScreenPointToRay (currentLockedCameraCursor.position);
//
//								if (Physics.Raycast (newRay, out hit, Mathf.Infinity, layerToCheckPossibleTargetsBelowCursor)) {
//									currentDetectedSurface = hit.collider.gameObject;
//								} else {
//									currentDetectedSurface = null;
//								}
//
//								if (currentTargetDetected != currentDetectedSurface) {
//									currentTargetDetected = currentDetectedSurface;
//
//									if (currentTargetDetected) {
//										currentTargetToAim = applyDamage.getPlaceToShoot (currentTargetDetected);
//										//										if (currentTargetToAim) {
//										//											print (currentTargetToAim.name);
//										//										}
//									} else {
//										currentTargetToAim = null;
//									}
//								}
//
//								if (currentTargetToAim) {
//									pointTargetPosition = currentTargetToAim.position;
//								}
//							}
//						} else {
//							if (playerControllerManager.isPlayerUsingMeleeWeapons ()) {
//								if (playerControllerManager.isStrafeModeActive ()) {
//									bool setPlayerCameraRotation = true;
//
//									if (!currentLockedCameraAxisInfo.showCameraCursorWhenNotAiming) {
//										if (lookingAtTarget && targetToLook != null) {
//											useMouseInputToRotateCameraHorizontally = false;
//
//											Vector3 worldPosition = targetToLook.position;
//											lookCameraDirection.position = new Vector3 (worldPosition.x, lookCameraDirection.position.y, worldPosition.z);
//
//											setPlayerCameraRotation = false;
//										} else {
//											useMouseInputToRotateCameraHorizontally = true;
//										}
//									}
//
//									if (setPlayerCameraRotation) {
//										playerCameraTransform.rotation = lockedMainCameraTransform.rotation;
//									}
//								} else {
//									playerCameraTransform.rotation = targetToFollow.rotation;
//								}
//							} else {
//								playerCameraTransform.rotation = targetToFollow.rotation;
//							}
//						}
//					}
//				}
//
//				targetToFollowForward = targetToFollow.forward;
//			}
//		}
//	}
//
//	public Vector3 getLookDirectionTransformRotationValue (Vector3 forwardDirection)
//	{
//		float lookDirectionTransformRotationAngle = 0;
//		if (moveInXAxisOn2_5d) {
//			lookDirectionTransformRotationAngle = Vector3.Dot (forwardDirection, lookCameraDirection.right); 
//		} else {
//			lookDirectionTransformRotationAngle = Vector3.Dot (forwardDirection, lookCameraDirection.forward); 
//		}
//
//		Vector3 lookDirectionTransformRotation = vector3Zero;
//		if (lookDirectionTransformRotationAngle < 0) {
//			if (moveInXAxisOn2_5d) {
//				lookDirectionTransformRotation = Vector3.up * (-90);
//			} else {
//				lookDirectionTransformRotation = Vector3.up * (-180);
//			}
//		} else {
//			if (moveInXAxisOn2_5d) {
//				lookDirectionTransformRotation = Vector3.up * (90);
//			}
//		}
//
//		return lookDirectionTransformRotation;
//	}
//
//	public Transform getCurrentLookDirection2_5d ()
//	{
//		return lookCameraDirection;
//	}
//
//	public Transform getCurrentLookDirectionTopDown ()
//	{
//		return lookCameraDirection;
//	}
//
//	public void checkUpdateReticleActiveState (bool state)
//	{
//		if (updateReticleActiveState) {
//			externalReticleCurrentlyActive = state;
//
//			cameraReticleCurrentlyActive = !externalReticleCurrentlyActive;
//
//			updateReticleActive ();
//		}
//	}
//
//	public void updateReticleActive ()
//	{
//		if (updateReticleActiveState) {
//			if (isCameraTypeFree ()) {
//				if (firstPersonActive) {
//					if (useCameraReticleFirstPerson) {
//						cameraReticleGameObject.SetActive (cameraReticleCurrentlyActive);
//					} else {
//						cameraReticleGameObject.SetActive (false);
//					}
//				} else {
//					if (useCameraReticleThirdPerson) {
//						cameraReticleGameObject.SetActive (cameraReticleCurrentlyActive);
//					} else {
//						cameraReticleGameObject.SetActive (false);
//					}
//				}
//			} else {
//				cameraReticleGameObject.SetActive (false);
//			}
//		}
//	}
//
//	public void enableOrDisableMainCameraReticle (bool state)
//	{
//		mainCameraReticleGameObject.SetActive (state);
//	}
//
//	public void setCurrentLockedCameraCursor (RectTransform currentCursor)
//	{
//		if (currentCursor != null) {
//			currentLockedCameraCursor = currentCursor;
//			currentLockedCameraCursorSize = currentLockedCameraCursor.sizeDelta;
//
//			Vector2 currentResolution = GKC_Utils.getScreenResolution ();
//
//			horizontaLimit = currentResolution.x - currentLockedCameraCursorSize.x;
//			verticalLimit = currentResolution.y - currentLockedCameraCursorSize.y;
//		} else {
//			if (currentLockedCameraCursor) {
//				currentLockedCameraCursor.anchoredPosition = Vector2.zero;
//				currentLockedCameraCursor = null;
//			}
//		}
//	}
//
//	void LateUpdate ()
//	{
//		if (!isCameraTypeFree ()) {
//			if (driving) {
//				followPlayerPositionOnLockedCamera (currentLateUpdateDeltaTime);
//			}
//
//			if (followFixedCameraPosition) {
//				lockedCameraAxis.position = currentLockedCameraAxisTransform.position;
//			}
//		}
//	}
//
//	bool previouslyAimingThirdPerson;
//	bool currentlyAimingThirdPerson;
//
//	bool resetRotationLookAngleYActive;
//
//	public void sethorizontalCameraLimitActiveOnAirState (bool state)
//	{
//		horizontalCameraLimitActiveOnAir = state;
//	}
//
//	void FixedUpdate ()
//	{
//		currentFixedUpdateDeltaTime = getCurrentDeltaTime ();
//
//		if (!isCameraTypeFree ()) {
//			if (!driving) {
//				followPlayerPositionOnLockedCamera (currentFixedUpdateDeltaTime);
//			}
//		}
//	}
//
//	public Vector3 projectOnSegment (Vector3 v1, Vector3 v2, Vector3 playerPosition)
//	{
//		Vector3 v1ToPos = playerPosition - v1;
//		Vector3 segDirection = (v2 - v1).normalized;
//
//		float DistanceFromV1 = Vector3.Dot (segDirection, v1ToPos);
//
//		if (DistanceFromV1 < 0.0f) {
//			return v1;
//		} else if (DistanceFromV1 * DistanceFromV1 > (v2 - v1).sqrMagnitude) {
//			return v2;
//		} else {
//			Vector3 fromV1 = segDirection * DistanceFromV1;
//
//			return v1 + fromV1;
//		}
//	}
//
//	public void followPlayerPositionOnLockedCamera (float deltaTimeToUse)
//	{
//		//follow player position
//		if (currentLockedCameraAxisInfo.followPlayerPosition) {
//			Vector3 targetPosition = vector3Zero;
//
//			if (currentLockedCameraAxisInfo.useWaypoints) {
//
//				Vector3 currentPosition = targetToFollow.position;
//
//				int numberOfWaypoints = currentLockedCameraAxisInfo.waypointList.Count;
//
//				closestWaypointIndex = -1;
//				float currentDistance = 0.0f;
//
//				for (int i = 0; i < currentLockedCameraAxisInfo.waypointList.Count; i++) {
//					float sqrDistance = (currentLockedCameraAxisInfo.waypointList [i].position - currentPosition).sqrMagnitude;
//
//					if (currentDistance == 0.0f || sqrDistance < currentDistance) {
//						currentDistance = sqrDistance;
//						closestWaypointIndex = i;
//					}
//				}
//
//				if (closestWaypointIndex > -1) {
//					closestWaypoint = currentLockedCameraAxisInfo.waypointList [closestWaypointIndex];
//
//					if (closestWaypointIndex == 0) {
//						Vector3 position1 = currentLockedCameraAxisInfo.waypointList [0].position;
//						Vector3 position2 = currentLockedCameraAxisInfo.waypointList [1].position;
//
//						targetPosition = projectOnSegment (position1, position2, currentPosition);
//					} else if (closestWaypointIndex == numberOfWaypoints - 1) {
//						Vector3 position1 = currentLockedCameraAxisInfo.waypointList [numberOfWaypoints - 1].position;
//						Vector3 position2 = currentLockedCameraAxisInfo.waypointList [numberOfWaypoints - 2].position;
//
//						targetPosition = projectOnSegment (position1, position2, currentPosition);
//					} else {
//						Vector3 previousWaypointPosition = currentLockedCameraAxisInfo.waypointList [closestWaypointIndex - 1].position;
//						Vector3 nextWaypointPosition = currentLockedCameraAxisInfo.waypointList [closestWaypointIndex + 1].position;
//
//						Vector3 currentWaypointPosition = closestWaypoint.position;
//
//						Vector3 LeftSeg = projectOnSegment (previousWaypointPosition, currentWaypointPosition, currentPosition);
//
//						Vector3 RightSeg = projectOnSegment (nextWaypointPosition, currentWaypointPosition, currentPosition);
//
//						Debug.DrawLine (currentPosition, LeftSeg, Color.red);
//						Debug.DrawLine (currentPosition, RightSeg, Color.blue);
//
//						if ((currentPosition - LeftSeg).sqrMagnitude <= (currentPosition - RightSeg).sqrMagnitude) {
//							targetPosition = LeftSeg;
//						} else {
//							targetPosition = RightSeg;
//						}
//					}
//				} else {
//					targetPosition = currentPosition;
//
//					print ("WARNING: Closest position of the player to the waypoint list not found, make sure it is properly configured");
//				}
//
//			} else {
//				//check if the current camera has a limit in the width, height or depth axis
//				if (useCameraLimit) {
//					lookCameraParent.position = targetToFollow.position;
//				} else {
//					lookCameraParent.localPosition = vector3Zero;
//				}
//
//				targetPosition = getPositionToLimit (true);
//			}
//
//			//set locked pivot position smoothly
//			if (currentLockedCameraAxisInfo.followPlayerPositionSmoothly) {
//				if (currentLockedCameraAxisInfo.useLerpToFollowPlayerPosition) {
//					lockedCameraPivot.position = Vector3.MoveTowards (lockedCameraPivot.position, targetPosition, deltaTimeToUse * currentLockedCameraAxisInfo.followPlayerPositionSpeed);
//				} else {
//					if (currentLockedCameraAxisInfo.useSeparatedVerticalHorizontalSpeed) {
//						newVerticalPosition = Mathf.SmoothDamp (newVerticalPosition, targetPosition.y, ref newVerticalPositionVelocity, currentLockedCameraAxisInfo.verticalFollowPlayerPositionSpeed);
//
//						if (moveInXAxisOn2_5d) {
//							newHorizontalPosition = 
//								Mathf.SmoothDamp (newHorizontalPosition, targetPosition.x, ref newHorizontalPositionVelocity, currentLockedCameraAxisInfo.horizontalFollowPlayerPositionSpeed);
//
//							lockedCameraPivot.position = new Vector3 (newHorizontalPosition, newVerticalPosition, lockedCameraPivot.position.z);
//						} else {
//							newHorizontalPosition = 
//								Mathf.SmoothDamp (newHorizontalPosition, targetPosition.z, ref newHorizontalPositionVelocity, currentLockedCameraAxisInfo.horizontalFollowPlayerPositionSpeed);
//
//							lockedCameraPivot.position = new Vector3 (lockedCameraPivot.position.x, newVerticalPosition, newHorizontalPosition);
//						}
//					} else {
//						lockedCameraPivot.position = Vector3.SmoothDamp (lockedCameraPivot.position, 
//							targetPosition, ref lockedCameraFollowPlayerPositionVelocity, currentLockedCameraAxisInfo.followPlayerPositionSpeed);
//					}
//				}
//
//				lockedMainCameraTransform.position = lockedCameraPivot.position;
//			} else {
//				lockedCameraPivot.position = targetPosition;
//				lockedMainCameraTransform.position = targetPosition;
//			}
//		}
//	}
//
//	public float getHorizontalInput ()
//	{
//		return horizontalInput;
//	}
//
//	public float getVerticalInput ()
//	{
//		return verticalInput;
//	}
//
//	public bool useMultipleTargetFov;
//	public float multipleTargetsMinFov = 20;
//	public float multipleTargetsMaxFov = 70;
//
//	public float multipleTargetsFovSpeed = 40;
//
//	float currentMultipleTargetsFov;
//
//	public bool useMultipleTargetsYPosition;
//	public float multipleTargetsYPositionSpeed;
//	public float multipleTargetsMaxHeight = 30;
//
//	public bool followingMultipleTargets;
//
//	public Vector3 extraPositionOffset;
//
//	public List<Transform> multipleTargetsToFollowList = new List<Transform> ();
//
//	float multipleTargetsCurrentYPosition;
//
//	public void setextraPositionOffsetValue (Vector3 newOffset)
//	{
//		extraPositionOffset = newOffset;
//	}
//
//	public Vector3 getPositionToLimit (bool calculateOnRunTime)
//	{
//		Vector3 newPosition = playerCameraTransform.position;
//
//		if (followingMultipleTargets) {
//			if (multipleTargetsToFollowList.Count == 1) {
//				newPosition = multipleTargetsToFollowList [0].position;
//			} else if (multipleTargetsToFollowList.Count >= 2) {
//				var bounds = new Bounds (multipleTargetsToFollowList [0].position, vector3Zero);
//
//				for (int i = 0; i < multipleTargetsToFollowList.Count; i++) {
//					bounds.Encapsulate (multipleTargetsToFollowList [i].position);
//				}
//
//				newPosition = bounds.center;
//
//				if (useTopDownView) {
//					currentMultipleTargetsFov = bounds.size.x;
//				}
//
//				if (using2_5ViewActive) {
//					if (moveInXAxisOn2_5d) {
//						currentMultipleTargetsFov = bounds.size.x;
//					} else {
//						currentMultipleTargetsFov = bounds.size.z;
//					}
//				}
//			}
//
//			if (useMultipleTargetFov) {
//				mainCamera.fieldOfView = Mathf.Lerp (multipleTargetsMinFov, multipleTargetsMaxFov, currentMultipleTargetsFov / multipleTargetsFovSpeed);
//			}
//
//			if (using2_5ViewActive) {
//				if (moveInXAxisOn2_5d) {
//					multipleTargetsCurrentYPosition = newPosition.z;
//					newPosition.z = Mathf.Lerp (multipleTargetsCurrentYPosition, multipleTargetsCurrentYPosition + multipleTargetsMaxHeight, currentMultipleTargetsFov / multipleTargetsYPositionSpeed);
//				} else {
//					multipleTargetsCurrentYPosition = newPosition.x;
//					newPosition.x = Mathf.Lerp (multipleTargetsCurrentYPosition, multipleTargetsCurrentYPosition + multipleTargetsMaxHeight, currentMultipleTargetsFov / multipleTargetsYPositionSpeed);
//				}
//			}
//
//			if (useTopDownView) {
//				multipleTargetsCurrentYPosition = newPosition.y;
//				newPosition.y = Mathf.Lerp (multipleTargetsCurrentYPosition, multipleTargetsCurrentYPosition + multipleTargetsMaxHeight, currentMultipleTargetsFov / multipleTargetsYPositionSpeed);
//			}
//		} else {
//			newPosition += extraPositionOffset;
//		}
//
//		//if the calculation of the position is made on update, check if the camera has an offset, that can be applied only when the player moves or also, when he moves
//		if (calculateOnRunTime) {
//
//			if (currentLockedCameraAxisInfo.useBoundToFollowPlayer) {
//				newPosition = calculateBoundPosition (true);
//			}
//
//			//if the player is on 2.5d view
//			if (using2_5ViewActive) {
//				if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSide || currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSideOnMoving) {
//					//check if the player is using the input
//					bool playerIsUsingInput = playerControllerManager.isPlayerMovingHorizontal (currentLockedCameraAxisInfo.inputToleranceOnFaceSide);
//					bool playerIsMoving = playerControllerManager.isPlayerMovingHorizontal (currentLockedCameraAxisInfo.inputToleranceOnFaceSideOnMoving);
//
//					bool lookToRight = false;
//
//					//Check the rotation of the player in his local Y axis to check the closest direction to look
//					float currentPlayerRotationY = 0;
//
//					Vector3 newOffset = vector3Zero;
//					Vector3 newOffsetOnMoving = vector3Zero;
//
//					//check the axis where he is moving, on XY or YZ
//					if (moveInXAxisOn2_5d) {
//						currentPlayerRotationY = Vector3.SignedAngle (targetToFollow.forward, lockedCameraPivot.right, playerCameraTransform.up);
//					} else {
//						currentPlayerRotationY = Vector3.SignedAngle (targetToFollow.forward, lockedCameraPivot.forward, playerCameraTransform.up);
//					}
//
//					//check if the player is moving to the left or to the right
//					if (moveInXAxisOn2_5d) {
//						if (Math.Abs (currentPlayerRotationY) < 90) {
//							lookToRight = true;
//						}
//					} else {
//						if (Math.Abs (currentPlayerRotationY) <= 90) {
//							lookToRight = true;
//						}
//					}
//
//					//add the offset to left and right according to the direction where the player is moving
//					if (moveInXAxisOn2_5d) {
//						if (playerIsMoving) {
//							if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSideOnMoving) {
//								if (lookToRight) {
//									newOffsetOnMoving = Vector3.right * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideOnMoving;
//								} else {
//									newOffsetOnMoving = -Vector3.right * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideOnMoving;
//								}
//							}
//						} 
//
//						if (!playerIsUsingInput) {
//							if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSide) {
//								if (lookToRight) {
//									newOffset = Vector3.right * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSide;
//								} else {
//									newOffset = -Vector3.right * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSide;
//								}
//							}
//						}
//					} else {
//						if (playerIsMoving) {
//							if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSideOnMoving) {
//								if (lookToRight) {
//									newOffsetOnMoving = Vector3.forward * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideOnMoving;
//								} else {
//									newOffsetOnMoving = -Vector3.forward * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideOnMoving;
//								}
//							}
//						} 
//
//						if (!playerIsUsingInput) {
//							if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSide) {
//								if (lookToRight) {
//									newOffset = Vector3.forward * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSide;
//								} else {
//									newOffset = -Vector3.forward * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSide;
//								}
//							}
//						}
//					}
//
//					//add this offset to the current camera position
//					horizontalOffsetOnSide = Vector3.SmoothDamp (horizontalOffsetOnSide, newOffset, ref horizontalOffsetOnFaceSideSpeed, currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideSpeed);
//
//					horizontalOffsetOnSideOnMoving = Vector3.SmoothDamp (horizontalOffsetOnSideOnMoving, newOffsetOnMoving,
//						ref horizontalOffsetOnFaceSideOnMovingSpeed, currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideOnMovingSpeed);
//
//					newPosition += horizontalOffsetOnSide + horizontalOffsetOnSideOnMoving;
//				}
//
//				//check to add vertical offset to the camera according to vertical input on 2.5d
//				if (currentLockedCameraAxisInfo.use2_5dVerticalOffsetOnMove) {
//					float newVerticalInput = playerControllerManager.getRawAxisValues ().y;
//
//					Vector3 newOffsetOnMoving = vector3Zero;
//
//					if (newVerticalInput > 0) {
//						newOffsetOnMoving = Vector3.up * currentLockedCameraAxisInfo.verticalTopOffsetOnMove;
//					} else if (newVerticalInput < 0) {
//						newOffsetOnMoving = -Vector3.up * currentLockedCameraAxisInfo.verticalBottomOffsetOnMove;
//					}
//
//					verticalOffsetOnMove = Vector3.SmoothDamp (verticalOffsetOnMove, newOffsetOnMoving, ref verticalOffsetOnMoveSpeed, currentLockedCameraAxisInfo.verticalOffsetOnMoveSpeed);
//
//					newPosition += verticalOffsetOnMove;
//				}
//			}
//
//			//the player is on a top down view or similar, like isometric
//			if (useTopDownView) {
//				if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSide || currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSideOnMoving) {
//					//check if the player is using the input
//					bool playerIsUsingInput = playerControllerManager.isPlayerMoving (currentLockedCameraAxisInfo.inputToleranceOnFaceSide);
//					bool playerIsMoving = playerControllerManager.isPlayerMoving (currentLockedCameraAxisInfo.inputToleranceOnFaceSideOnMoving);
//
//					Vector3 newOffset = vector3Zero;
//					Vector3 newOffsetOnMoving = vector3Zero;
//
//					//add the offset to the camera, setting the direction of this offset as the forward direction of the player
//					if (playerIsMoving) {
//						if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSideOnMoving) {
//							newOffsetOnMoving = targetToFollow.forward * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideOnMoving;
//						}
//					} 
//
//					if (!playerIsUsingInput) {
//						if (currentLockedCameraAxisInfo.useHorizontalOffsetOnFaceSide) {
//							newOffset = targetToFollow.forward * currentLockedCameraAxisInfo.horizontalOffsetOnFaceSide;
//						}
//					}
//
//					//add the offset to the camera position
//					horizontalOffsetOnSide = Vector3.SmoothDamp (horizontalOffsetOnSide, newOffset, ref horizontalOffsetOnFaceSideSpeed, currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideSpeed);
//
//					horizontalOffsetOnSideOnMoving = Vector3.SmoothDamp (horizontalOffsetOnSideOnMoving, newOffsetOnMoving, 
//						ref horizontalOffsetOnFaceSideOnMovingSpeed, currentLockedCameraAxisInfo.horizontalOffsetOnFaceSideOnMovingSpeed);
//
//					newPosition += horizontalOffsetOnSide + horizontalOffsetOnSideOnMoving;
//				}
//			}
//		} else {
//			calculateBoundPosition (false);
//		}
//
//		if (useCameraLimit) {
//			if (useHeightLimit) {
//				newPosition.y = Mathf.Clamp (newPosition.y, currentCameraLimitPosition.y - heightLimitLower - lockedCameraAxis.localPosition.y,
//					currentCameraLimitPosition.y + heightLimitUpper - lockedCameraAxis.localPosition.y);
//			}
//
//			if (currentLockedCameraAxisInfo.moveInXAxisOn2_5d) {
//				if (useWidthLimit) {
//					newPosition.x = Mathf.Clamp (newPosition.x, currentCameraLimitPosition.x - widthLimitLeft - lockedCameraAxis.localPosition.x, 
//						currentCameraLimitPosition.x + widthLimitRight - lockedCameraAxis.localPosition.x);
//				}
//
//				if (useDepthLimit) {
//					newPosition.z = Mathf.Clamp (newPosition.z, currentCameraLimitPosition.z - depthLimitBackward, currentCameraLimitPosition.z + depthLimitFront);
//				}
//			} else {
//				if (useWidthLimit) {
//					newPosition.z = Mathf.Clamp (newPosition.z, currentCameraLimitPosition.z - widthLimitLeft, currentCameraLimitPosition.z + widthLimitRight);
//				}
//
//				if (useDepthLimit) {
//					newPosition.x = Mathf.Clamp (newPosition.x, currentCameraLimitPosition.x - depthLimitBackward, currentCameraLimitPosition.x + depthLimitFront);
//				}
//			}
//		}
//
//		return newPosition;
//	}
//
//	public Vector3 calculateBoundPosition (bool calculateOnRunTime)
//	{
//		if (!calculateOnRunTime) {
//			focusArea = new FocusArea (mainCollider.bounds, currentLockedCameraAxisInfo.heightBoundTop,
//				currentLockedCameraAxisInfo.widthBoundRight, currentLockedCameraAxisInfo.widthBoundLeft,
//				currentLockedCameraAxisInfo.depthBoundFront, currentLockedCameraAxisInfo.depthBoundBackward);
//		}
//
//		focusArea.Update (mainCollider.bounds);
//
//		focusTargetPosition = focusArea.centre +
//		Vector3.right * currentLockedCameraAxisInfo.boundOffset.x +
//		Vector3.up * currentLockedCameraAxisInfo.boundOffset.y +
//		Vector3.forward * currentLockedCameraAxisInfo.boundOffset.z;
//
//		return focusTargetPosition;
//	}
//
//	bool collisionSurfaceFound;
//	float distanceToCamPositionOffset;
//	Vector3 directionFromPivotToCamera;
//	float collisionDistance;
//
//	Vector3 pivotCameraPosition;
//
//	Vector3 mainCameraPosition;
//	Vector3 mainCameraNewPosition;
//	Vector3 targetCameraPosition;
//	Vector3 cameraNewPosition;
//
//	Vector3 currentCamPositionOffset;
//
//	public void setCameraToFreeOrLocked (typeOfCamera state, lockedCameraSystem.cameraAxis lockedCameraInfo)
//	{
//		if (state == typeOfCamera.free) {
//			if (cameraType != state) {
//				if (rotatingLockedCameraFixedRotationAmountActive) {
//					stopRotateLockedCameraFixedRotationAmount ();
//				}
//
//				if (driving) {
//					cameraType = state;
//
//					cameraCurrentlyLocked = false;
//
//					previousLockedCameraAxisTransform = null;
//					playerControllerManager.setLockedCameraState (false, false, false);
//
//					if (usingPlayerNavMeshPreviously) {
//						playerNavMeshManager.setPlayerNavMeshEnabledState (false);
//					}
//
//					usingPlayerNavMeshPreviously = false;
//
//					//check if the player is driving and set the locked camera properly
//					vehicleHUDManager currentVehicleHUDManager = playerControllerManager.getCurrentVehicle ().GetComponent<vehicleHUDManager> ();
//
//					if (currentVehicleHUDManager != null) {
//						currentVehicleHUDManager.setPlayerCameraParentAndPosition (mainCamera.transform, this);
//					}
//
//					if (currentVehicleHUDManager.getCurrentDriver () == playerControllerGameObject) {
//						setCameraState (previousFreeCameraStateName);
//					} else {
//						setCameraState (defaultVehiclePassengerStateName);
//					}
//				} else {
//					currentLockedCameraAxisInfo = lockedCameraInfo;
//
//					string auxPreviousFreeCameraStateName = previousFreeCameraStateName;
//
//					print ("previous free camera state " + previousFreeCameraStateName);
//
//					if (!previousFreeCameraStateName.Equals ("")) {
//
//						setCameraState (previousFreeCameraStateName);
//						resetCurrentCameraStateAtOnce ();
//
//						configureCameraAndPivotPositionAtOnce ();
//
//						previousFreeCameraStateName = "";
//					}
//
//					playerCameraTransform.eulerAngles = new Vector3 (playerCameraTransform.eulerAngles.x, lockedCameraInfo.axis.eulerAngles.y, playerCameraTransform.eulerAngles.z);
//					pivotCameraTransform.eulerAngles = new Vector3 (lockedCameraInfo.axis.localEulerAngles.x, pivotCameraTransform.eulerAngles.y, pivotCameraTransform.eulerAngles.z);
//					mainCamera.transform.SetParent (mainCameraTransform);
//
//					if (currentLockedCameraAxisInfo.smoothCameraTransition) {
//						lockedCameraMovement (false);
//
//						if (previousLockedCameraAxisInfo.useDifferentCameraFov || usingLockedZoomOn) {
//							setMainCameraFov (currentState.initialFovValue, zoomSpeed);
//						}
//					} else {
//						stopLockedCameraMovementCoroutine ();
//
//						cameraType = state;
//
//						cameraCurrentlyLocked = false;
//
//						mainCamera.transform.localPosition = vector3Zero;
//						mainCamera.transform.localRotation = quaternionIdentity;
//
//						if (previousLockedCameraAxisInfo.useDifferentCameraFov || usingLockedZoomOn) {
//							mainCamera.fieldOfView = currentState.initialFovValue;
//						}
//					}
//
//					lookAngle = Vector2.zero;
//					previousLockedCameraAxisTransform = null;
//					playerControllerManager.setLockedCameraState (false, false, false);
//
//					if (usingPlayerNavMeshPreviously) {
//						playerNavMeshManager.setPlayerNavMeshEnabledState (false);
//					}
//					usingPlayerNavMeshPreviously = false;
//
//					if (weaponsManager.isAimingWeapons ()) {
//						weaponsManager.inputAimWeapon ();
//
//						if (!auxPreviousFreeCameraStateName.Equals ("")) {
//							setCameraState (auxPreviousFreeCameraStateName);
//						}
//					}
//
//					if (previouslyInFirstPerson) {
//						changeCameraToThirdOrFirstView ();
//					}
//				}
//
//				setLookAtTargetState (false, null);
//
//				//check the unity events on enter and exit
//				callLockedCameraEventOnEnter (lockedCameraInfo);
//
//				if (previousLockedCameraAxisInfo != null) {
//					callLockedCameraEventOnExit (previousLockedCameraAxisInfo);
//				}
//
//				if (previousLockedCameraAxisInfo.changeRootMotionActive) {
//					playerControllerManager.setOriginalUseRootMotionActiveState ();
//				}
//
//				previousLockedCameraAxisInfo = null;
//
//				useCameraLimit = false;
//
//				if (usingSetTransparentSurfacesPreviously) {
//					setTransparentSurfacesManager.setCheckSurfaceActiveState (false);
//				}
//
//				setTransparentSurfacesManager.setLockedCameraActiveState (false);
//
//				enableOrDisableMainCameraReticle (true);
//
//				lockedCameraZoomMovingCameraValue = 0;
//
//				playerControllerManager.setDeactivateRootMotionOnStrafeActiveOnLockedViewState (false);
//			}
//		} 
//
//		if (state == typeOfCamera.locked) {
//			//assign the new locked camera info
//
//			currentLockedCameraAxisInfo = lockedCameraInfo;
//
//			if (temporalCameraViewToLockedCameraActive) {
//				return;
//			}
//
//			currentLockedCameraAxisTransform = currentLockedCameraAxisInfo.axis;
//
//			followFixedCameraPosition = currentLockedCameraAxisInfo.followFixedCameraPosition;
//
//			bool newCameraFound = false;
//
//			if (previousLockedCameraAxisInfo == null || previousLockedCameraAxisInfo != currentLockedCameraAxisInfo) {
//				newCameraFound = true;
//			}
//
//			//			print ("New locked camera found: " + newCameraFound + " " + currentLockedCameraAxisInfo.name);
//
//			//if a new camera is found, adjust the position of the locked camera on the player
//			if (!newCameraFound) {
//				return;
//			}
//
//			if (rotatingLockedCameraFixedRotationAmountActive) {
//				stopRotateLockedCameraFixedRotationAmount ();
//			}
//
//			mainCamera.transform.SetParent (null);
//
//			//set the position and rotations of the new locked camera transform to the previous locked transform elements of the player
//			lockedCameraPosition.localPosition = currentLockedCameraAxisInfo.cameraPosition.localPosition;
//			lockedCameraPosition.localRotation = currentLockedCameraAxisInfo.cameraPosition.localRotation;
//
//			lockedCameraAxis.localPosition = currentLockedCameraAxisInfo.axis.localPosition;
//			lockedCameraAxis.localRotation = currentLockedCameraAxisInfo.axis.localRotation;
//
//			Vector3 targetToFollowPosition = targetToFollow.position;
//
//			//if the is a locked camera pivot, it means the camera follows the player position on this locked view
//			if (currentLockedCameraAxisInfo.lockedCameraPivot) {
//				if (activatingLockedCameraByInputActive && currentLockedCameraAxisInfo.followPlayerPosition) {
//					lockedCameraPivot.position = targetToFollowPosition;
//				} else {
//					lockedCameraPivot.rotation = currentLockedCameraAxisInfo.lockedCameraPivot.rotation;
//
//					//place the new camera found in the current position of the player to have a smoother transition between cameras that are following the player positition
//					if (currentLockedCameraAxisInfo.useZeroCameraTransition) {
//						lockedCameraPivot.position = targetToFollowPosition;
//
//						lockedCameraPivot.position = getPositionToLimit (false);
//					} else {
//						lockedCameraPivot.position = currentLockedCameraAxisInfo.lockedCameraPivot.position;
//					}
//				}
//			} else {
//				if (activatingLockedCameraByInputActive && currentLockedCameraAxisInfo.followPlayerPosition) {
//					lockedCameraPivot.position = targetToFollowPosition;
//				} else {
//					//else, the camera will stay in a fixed position
//
//					lockedCameraAxis.position = currentLockedCameraAxisInfo.axis.position;
//					lockedCameraAxis.rotation = currentLockedCameraAxisInfo.axis.rotation;
//
//					lockedCameraPosition.position = currentLockedCameraAxisInfo.cameraPosition.position;
//					lockedCameraPosition.rotation = currentLockedCameraAxisInfo.cameraPosition.rotation;
//				}
//			}
//
//			lookCameraParent.localPosition = vector3Zero;
//			lookCameraParent.localRotation = quaternionIdentity;
//
//			if (!cameraCurrentlyLocked) {
//				previousLockedCameraAxisTransform = getCameraTransform ();
//
//				previouslyInFirstPerson = isFirstPersonActive ();
//
//				if (previouslyInFirstPerson) {
//					changeCameraToThirdOrFirstView ();
//				}
//
//				headBobManager.stopAllHeadbobMovements ();
//
//				stopShakeCamera ();
//
//				cameraCurrentlyLocked = true;
//			}
//
//			if (previousLockedCameraAxisTransform != null) {
//				setCurrentAxisTransformValues (previousLockedCameraAxisTransform);
//			}
//
//			lockedCameraCanFollow = false;
//
//			//2.5d camera setting
//			if (currentLockedCameraAxisInfo.use2_5dView) {
//
//				lookCameraPivot.localPosition = currentLockedCameraAxisInfo.pivot2_5d.localPosition;
//				lookCameraPivot.localRotation = currentLockedCameraAxisInfo.pivot2_5d.localRotation;
//
//				lookCameraDirection.localPosition = currentLockedCameraAxisInfo.lookDirection2_5d.localPosition;
//				lookCameraDirection.localRotation = currentLockedCameraAxisInfo.lookDirection2_5d.localRotation;
//
//				playerControllerManager.set3dOr2_5dWorldType (false);
//				using2_5ViewActive = true;
//
//				originalLockedCameraPivotPosition = currentLockedCameraAxisInfo.originalLockedCameraPivotPosition;
//				moveInXAxisOn2_5d = currentLockedCameraAxisInfo.moveInXAxisOn2_5d;
//
//				if (currentLockedCameraAxisInfo.useDefaultZValue2_5d) {
//					movePlayerToDefaultHorizontalValue2_5d ();
//				}
//
//				clampAimDirections = currentLockedCameraAxisInfo.clampAimDirections;
//				numberOfAimDirections = (int)currentLockedCameraAxisInfo.numberOfAimDirections;
//			} else {
//				playerControllerManager.set3dOr2_5dWorldType (true);
//				using2_5ViewActive = false;
//			}
//
//			horizontalCameraLimitActiveOnGround = !using2_5ViewActive;
//
//			//point and click setting
//
//			if (currentLockedCameraAxisInfo.usePointAndClickSystem) {
//				if (!usingPlayerNavMeshPreviously) {
//					playerNavMeshManager.setPlayerNavMeshEnabledState (true);
//				}
//
//				usingPlayerNavMeshPreviously = true;
//			} else {
//				if (usingPlayerNavMeshPreviously) {
//					playerNavMeshManager.setPlayerNavMeshEnabledState (false);
//				}
//
//				usingPlayerNavMeshPreviously = false;
//			}
//
//			//top down setting
//			if (currentLockedCameraAxisInfo.useTopDownView) {
//
//				lookCameraPivot.localPosition = currentLockedCameraAxisInfo.topDownPivot.localPosition;
//				lookCameraPivot.localRotation = currentLockedCameraAxisInfo.topDownPivot.localRotation;
//
//				lookCameraDirection.localPosition = currentLockedCameraAxisInfo.topDownLookDirection.localPosition;
//				lookCameraDirection.localRotation = currentLockedCameraAxisInfo.topDownLookDirection.localRotation;
//
//				useTopDownView = true;
//			} else {
//				useTopDownView = false;
//			}
//
//			if (useTopDownView) {
//				if (currentLockedCameraAxisInfo.showCameraCursorWhenNotAiming) {
//					setLookDirection = true;
//				}
//			} else {
//				setLookAtTargetState (false, null);
//			}
//
//			useMouseInputToRotateCameraHorizontally = false;
//
//			previouslyOnFreeCamera = cameraType == typeOfCamera.free;
//
//			//			print ("previously on free camera: " + previouslyOnFreeCamera);
//
//			cameraType = state;
//
//			mainCamera.transform.SetParent (lockedCameraPosition);
//
//			if (currentLockedCameraAxisInfo.smoothCameraTransition) {
//				lockedCameraMovement (true);
//
//				if (currentLockedCameraAxisInfo.useDifferentCameraFov) {
//					setMainCameraFov (currentLockedCameraAxisInfo.fovValue, zoomSpeed);
//				}
//
//				if (previousLockedCameraAxisInfo != null) {
//					if (usingLockedZoomOn) {
//						setMainCameraFov (currentState.initialFovValue, zoomSpeed);
//					}
//				}
//			} else {
//				stopLockedCameraMovementCoroutine ();
//
//				mainCamera.transform.localPosition = vector3Zero;
//				mainCamera.transform.localRotation = quaternionIdentity;
//
//				lockedCameraChanged = true;
//
//				if (currentLockedCameraAxisInfo.useDifferentCameraFov) {
//					mainCamera.fieldOfView = currentLockedCameraAxisInfo.fovValue;
//				}
//
//				if (previousLockedCameraAxisInfo != null) {
//					if (usingLockedZoomOn) {
//						mainCamera.fieldOfView = currentState.initialFovValue;
//					}
//				}
//			}
//
//			playerControllerManager.setLockedCameraState (true, currentLockedCameraAxisInfo.useTankControls, currentLockedCameraAxisInfo.useRelativeMovementToLockedCamera);
//
//			if (previousLockedCameraAxisInfo != null) {
//				if (usingLockedZoomOn || previousLockedCameraAxisInfo.cameraCanRotate || previousLockedCameraAxisInfo.canMoveCamera) {
//
//					//Reset locked camera values on this player camera
//					currentLockedLoonAngle = Vector2.zero;
//					currentLockedCameraRotation = quaternionIdentity;
//					currentLockedPivotRotation = quaternionIdentity;
//					lastTimeLockedSpringRotation = 0;
//					lastTimeLockedSpringMovement = 0;
//					currentLockedCameraMovementPosition = vector3Zero;
//					currentLockedMoveCameraPosition = vector3Zero;
//
//					usingLockedZoomOn = false;
//				}
//
//				if (previousLockedCameraAxisInfo.changeRootMotionActive) {
//					playerControllerManager.setOriginalUseRootMotionActiveState ();
//				}
//			}
//
//			//Assign the previous locked camera and call the event on exit
//			if (previousLockedCameraAxisInfo != currentLockedCameraAxisInfo) {
//				previousLockedCameraAxisInfo = currentLockedCameraAxisInfo;
//
//				//check the unity events on exit
//				callLockedCameraEventOnExit (previousLockedCameraAxisInfo);
//			}
//
//			//check the unity events on enter
//			callLockedCameraEventOnEnter (currentLockedCameraAxisInfo);
//
//			//set the current vertical and horizontal position of the camera in case the following speed is separated into these two values, so the position is calculated starting at that point
//			if (currentLockedCameraAxisInfo.lockedCameraPivot) {
//				newVerticalPosition = lockedCameraPivot.position.y;
//
//				if (moveInXAxisOn2_5d) {
//					newHorizontalPosition = lockedCameraPivot.position.x;
//				} else {
//					newHorizontalPosition = lockedCameraPivot.position.z;
//				}
//			}
//
//			if (currentLockedCameraAxisInfo.lookAtPlayerPosition) {
//				calculateLockedCameraLookAtPlayerPosition ();
//
//				lockedCameraPosition.localRotation = Quaternion.Euler (new Vector3 (currentLockedLimitLookAngle.x, 0, 0));
//
//				lockedCameraAxis.localRotation = Quaternion.Euler (new Vector3 (0, currentLockedLimitLookAngle.y, 0));
//			}
//
//			horizontalOffsetOnSide = vector3Zero;
//
//			horizontalOffsetOnSideOnMoving = vector3Zero;
//
//			verticalOffsetOnMove = vector3Zero;
//
//			if (currentLockedCameraAxisInfo.useTransparetSurfaceSystem) {
//				setTransparentSurfacesManager.setCheckSurfaceActiveState (true);
//			} else {
//				setTransparentSurfacesManager.setCheckSurfaceActiveState (false);
//			}
//
//			setTransparentSurfacesManager.setLockedCameraActiveState (true);
//
//			usingSetTransparentSurfacesPreviously = currentLockedCameraAxisInfo.useTransparetSurfaceSystem;
//
//			if (previousFreeCameraStateName.Equals ("")) {
//
//				if (previouslyInFirstPerson) {
//					previousFreeCameraStateName = defaultFirstPersonStateName;
//				} else {
//					previousFreeCameraStateName = defaultThirdPersonStateName;
//				}
//
//				setCameraState (defaultLockedCameraStateName);
//				resetCurrentCameraStateAtOnce ();
//			}
//
//			if (currentLockedCameraAxisInfo.changeRootMotionActive) {
//				playerControllerManager.setUseRootMotionActiveState (currentLockedCameraAxisInfo.useRootMotionActive);
//			}
//
//			if (previouslyOnFreeCamera) {
//				if (!previouslyInFirstPerson && weaponsManager.isAimingWeapons ()) {
//					weaponsManager.inputAimWeapon ();
//				}
//			}
//
//			enableOrDisableMainCameraReticle (false);
//
//			lockedCameraZoomMovingCameraValue = 0;
//
//			if (currentLockedCameraAxisInfo.disablePreviousCameraLimitSystem) {
//				useCameraLimit = false;
//			}
//
//			playerControllerManager.setDeactivateRootMotionOnStrafeActiveOnLockedViewState (currentLockedCameraAxisInfo.deactivateRootMotionOnStrafeActiveOnLockedView);
//		}
//	}
//
//	bool previouslyOnFreeCamera;
//
//	public void setClampAimDirectionsState (bool state)
//	{
//		clampAimDirections = state;
//	}
//
//	public void calculateLockedCameraLookAtPlayerPosition ()
//	{
//		Vector3 cameraAxisPosition = lockedCameraPosition.position;
//		if (currentLockedCameraAxisInfo.usePositionOffset) {
//			cameraAxisPosition += lockedCameraAxis.transform.right * currentLockedCameraAxisInfo.positionOffset.x;
//			cameraAxisPosition += lockedCameraAxis.transform.up * currentLockedCameraAxisInfo.positionOffset.y;
//			cameraAxisPosition += lockedCameraAxis.transform.forward * currentLockedCameraAxisInfo.positionOffset.z;
//		}
//
//		Vector3 lookPos = targetToFollow.position - cameraAxisPosition;
//		Quaternion rotation = Quaternion.LookRotation (lookPos);
//		Vector3 rotatioEuler = rotation.eulerAngles;
//		float lockedCameraPivotY = lockedCameraPivot.localEulerAngles.y;
//		currentLockedLimitLookAngle.x = rotatioEuler.x;
//		currentLockedLimitLookAngle.y = rotatioEuler.y - lockedCameraPivotY;
//
//		if (currentLockedCameraAxisInfo.useRotationLimits) {
//			if (currentLockedLimitLookAngle.x > 180) {
//				currentLockedLimitLookAngle.x -= 360;				
//				currentLockedLimitLookAngle.x = Mathf.Clamp (currentLockedLimitLookAngle.x, currentLockedCameraAxisInfo.rotationLimitsX.x, 0);
//			} else {
//				currentLockedLimitLookAngle.x = Mathf.Clamp (currentLockedLimitLookAngle.x, currentLockedCameraAxisInfo.rotationLimitsX.x, currentLockedCameraAxisInfo.rotationLimitsX.y);
//			}
//
//			currentLockedLimitLookAngle.y = Mathf.Clamp (currentLockedLimitLookAngle.y, currentLockedCameraAxisInfo.rotationLimitsY.x, currentLockedCameraAxisInfo.rotationLimitsY.y);
//		} 
//	}
//
//	public void setCameraLimit (bool useCameraLimitValue, bool useWidthLimitValue, float newWidthLimitRight, float newWidthLimitLeft, bool useHeightLimitValue, float newHeightLimitUpper,
//	                            float newHeightLimitLower, Vector3 newCameraLimitPosition, bool depthLimitEnabled, float newDepthLimitFront, float newDepthLimitBackward)
//	{
//		useCameraLimit = useCameraLimitValue;
//
//		currentCameraLimitPosition = newCameraLimitPosition;
//
//		useWidthLimit = useWidthLimitValue;
//		widthLimitRight = newWidthLimitRight;
//		widthLimitLeft = newWidthLimitLeft;
//
//		useHeightLimit = useHeightLimitValue;
//		heightLimitUpper = newHeightLimitUpper;
//		heightLimitLower = newHeightLimitLower;
//
//		useDepthLimit = depthLimitEnabled;
//		depthLimitFront = newDepthLimitFront;
//		depthLimitBackward = newDepthLimitBackward;
//	}
//
//	public void setNewCameraForwardPosition (float newCameraForwardPosition)
//	{
//		mainCamera.transform.SetParent (null);
//
//		Vector3 originalCameraAxisLocalPosition = currentLockedCameraAxisInfo.originalCameraAxisLocalPosition;
//		if (moveInXAxisOn2_5d) {
//			lockedCameraAxis.localPosition = new Vector3 (lockedCameraAxis.localPosition.x, lockedCameraAxis.localPosition.y, newCameraForwardPosition);
//			currentLockedCameraAxisInfo.originalCameraAxisLocalPosition = new Vector3 (originalCameraAxisLocalPosition.x, originalCameraAxisLocalPosition.y, newCameraForwardPosition);
//		} else {
//			lockedCameraAxis.localPosition = new Vector3 (newCameraForwardPosition, lockedCameraAxis.localPosition.y, lockedCameraAxis.localPosition.z);
//			currentLockedCameraAxisInfo.originalCameraAxisLocalPosition = new Vector3 (newCameraForwardPosition, originalCameraAxisLocalPosition.y, originalCameraAxisLocalPosition.z);
//		}
//
//		mainCamera.transform.SetParent (lockedCameraPosition);
//
//		lockedCameraMovement (true);
//	}
//
//	public bool isTopdownViewEnabled ()
//	{
//		return useTopDownView;
//	}
//
//	public bool is2_5ViewActive ()
//	{
//		return using2_5ViewActive;
//	}
//
//	public void callLockedCameraEventOnEnter (lockedCameraSystem.cameraAxis cameraAxisToCheck)
//	{
//		if (cameraAxisToCheck.useUnityEvent && cameraAxisToCheck.useUnityEventOnEnter) {
//			if (cameraAxisToCheck.unityEventOnEnter.GetPersistentEventCount () > 0) {
//				cameraAxisToCheck.unityEventOnEnter.Invoke ();
//			}
//		}
//	}
//
//	public void callLockedCameraEventOnExit (lockedCameraSystem.cameraAxis cameraAxisToCheck)
//	{
//		if (cameraAxisToCheck.useUnityEvent && cameraAxisToCheck.useUnityEventOnExit) {
//			if (cameraAxisToCheck.unityEventOnExit.GetPersistentEventCount () > 0) {
//				cameraAxisToCheck.unityEventOnExit.Invoke ();
//			}
//		}
//	}
//
//	public bool isMoveInXAxisOn2_5d ()
//	{
//		return moveInXAxisOn2_5d;
//	}
//
//	public Vector3 getOriginalLockedCameraPivotPosition ()
//	{
//		return originalLockedCameraPivotPosition;
//	}
//
//	public void setManualAimStateOnLockedCamera (bool state)
//	{
//		if (!isCameraTypeFree ()) {
//			setManualAimState (state);
//		}
//	}
//
//	public void setManualAimState (bool state)
//	{
//		if (state) {
//			setCurrentLockedCameraCursor (weaponsManager.cursorRectTransform);
//			setMaxDistanceToCameraCenter (true, 100);
//
//			if (!isCameraTypeFree ()) {
//				setLookAtTargetOnLockedCameraState ();
//			}
//
//			setLookAtTargetState (true, null);
//		} else {
//			setCurrentLockedCameraCursor (null);
//			setMaxDistanceToCameraCenter (true, 100);
//			setLookAtTargetState (false, null);
//		}
//
//		playerControllerManager.enableOrDisableAiminig (state);		
//	}
//
//	//Adjust the player to a fixed axis position for the 2.5d view
//	public void movePlayerToDefaultHorizontalValue2_5d ()
//	{
//		Vector3 positionToFollow = targetToFollow.position;
//
//		if (moveInXAxisOn2_5d) {
//			float forwardDifference = Math.Abs (Math.Abs (positionToFollow.z) - Math.Abs (originalLockedCameraPivotPosition.z));
//
//			if (positionToFollow.z == originalLockedCameraPivotPosition.z || forwardDifference < 0.01f) {
//				return;
//			}
//		} else {
//			float rightDifference = Math.Abs (Math.Abs (positionToFollow.x) - Math.Abs (originalLockedCameraPivotPosition.x));
//
//			if (positionToFollow.x == originalLockedCameraPivotPosition.x || rightDifference < 0.01f) {
//				return;
//			}
//		}
//
//		if (fixPlayerZPositionCoroutine != null) {
//			StopCoroutine (fixPlayerZPositionCoroutine);
//		}
//
//		fixPlayerZPositionCoroutine = StartCoroutine (movePlayerToDefaultHorizontalValue2_5dCoroutine ());
//	}
//
//	IEnumerator movePlayerToDefaultHorizontalValue2_5dCoroutine ()
//	{
//		//		playerControllerManager.changeScriptState (false);
//
//		playerControllerManager.setCanMoveState (false);
//
//		playerControllerManager.resetPlayerControllerInput ();
//
//		playerControllerManager.resetOtherInputFields ();
//
//		Vector3 targetPosition = vector3Zero;
//
//		Vector3 currentPosition = targetToFollow.position;
//
//		if (moveInXAxisOn2_5d) {
//			targetPosition = new Vector3 (currentPosition.x, currentPosition.y, originalLockedCameraPivotPosition.z);
//		} else {
//			targetPosition = new Vector3 (originalLockedCameraPivotPosition.x, currentPosition.y, currentPosition.z);
//		}
//
//		float dist = GKC_Utils.distance (currentPosition, targetPosition);
//
//		float duration = dist / currentLockedCameraAxisInfo.adjustPlayerPositionToFixed2_5dPosition;
//
//		float t = 0;
//
//		float movementTimer = 0;
//
//		bool targetReached = false;
//
//		float positionDifference = 0;
//		float angleDifference = 0;
//
//		if (currentLockedCameraAxisInfo.rotatePlayerToward2dCameraOnTriggerEnter) {
//			Vector3 lockedCameraDirection = currentLockedCameraAxisInfo.axis.position - targetToFollow.position;
//
//			lockedCameraDirection = lockedCameraDirection / lockedCameraDirection.magnitude;
//
//			float rotationAngle = Vector3.SignedAngle (targetToFollow.forward, lockedCameraDirection, targetToFollow.up);
//
//			if (moveInXAxisOn2_5d) {
//				if (rotationAngle < 0) {
//					rotationAngle = -90;
//				} else {
//					rotationAngle = 90;
//				}
//			} else {
//				if (rotationAngle < 0) {
//					rotationAngle = 180;
//				} else {
//					rotationAngle = -180;
//				}
//			}
//
//			Vector3 targetRotationEuler = targetToFollow.up * rotationAngle;
//
//			Quaternion targetRotation = Quaternion.Euler (targetRotationEuler);
//
//			Quaternion currentRotation = targetToFollow.rotation;
//
//			while (!targetReached) {
//				t += getCurrentDeltaTime () / duration; 
//
//				targetToFollow.position = Vector3.Lerp (currentPosition, targetPosition, t);
//
//				targetToFollow.rotation = Quaternion.Lerp (currentRotation, targetRotation, t);
//
//				movementTimer += Time.deltaTime;
//
//				angleDifference = Quaternion.Angle (targetToFollow.rotation, targetRotation);
//
//				positionDifference = GKC_Utils.distance (targetToFollow.position, targetPosition);
//
//				if ((positionDifference < 0.01f && angleDifference < 0.2f) || movementTimer > (duration + 2)) {
//					targetReached = true;
//				}
//				yield return null;
//			}
//		} else {
//			while (!targetReached) {
//				t += getCurrentDeltaTime () / duration; 
//				targetToFollow.position = Vector3.Lerp (currentPosition, targetPosition, t);
//
//				movementTimer += Time.deltaTime;
//
//				positionDifference = GKC_Utils.distance (targetToFollow.position, targetPosition);
//
//				if (positionDifference < 0.01f || movementTimer > (duration + 1)) {
//					targetReached = true;
//				}
//				yield return null;
//			}
//		}
//
//		if (lockedCameraMoving) {
//			targetReached = false;
//
//			while (!targetReached) {
//				if (!lockedCameraMoving) {
//					targetReached = true;
//				}
//
//				yield return null;
//			}
//		} else {
//			yield return new WaitForSeconds (0.6f);
//		}
//
//		//		playerControllerManager.changeScriptState (true);
//
//		playerControllerManager.setCanMoveState (true);
//	}
//
//	public void stopLockedCameraMovementCoroutine ()
//	{
//		lockedCameraMoving = false;
//
//		if (lockedCameraCoroutine != null) {
//			StopCoroutine (lockedCameraCoroutine);
//		}
//	}
//
//	public void lockedCameraMovement (bool isBeingSetToLocked)
//	{
//		if (rotatingLockedCameraFixedRotationAmountActive) {
//			stopRotateLockedCameraFixedRotationAmount ();
//		}
//
//		stopLockedCameraMovementCoroutine ();
//
//		lockedCameraCoroutine = StartCoroutine (lockedCameraMovementCoroutine (isBeingSetToLocked));
//	}
//
//	//move the camera from its position in player camera to a fix position
//	IEnumerator lockedCameraMovementCoroutine (bool isBeingSetToLocked)
//	{
//		lockedCameraMoving = true;
//
//		float i = 0;
//		//store the current rotation of the camera
//		Quaternion currentQ = mainCamera.transform.localRotation;
//		//store the current position of the camera
//		Vector3 currentPos = mainCamera.transform.localPosition;
//
//		//translate position and rotation camera
//		while (i < 1) {
//			i += getCurrentDeltaTime () * currentLockedCameraAxisInfo.cameraTransitionSpeed;
//			mainCamera.transform.localRotation = Quaternion.Lerp (currentQ, quaternionIdentity, i);
//			mainCamera.transform.localPosition = Vector3.Lerp (currentPos, vector3Zero, i);
//			yield return null;
//		}
//
//		if (isBeingSetToLocked) {
//			lockedCameraChanged = true;
//		} else {
//			cameraType = typeOfCamera.free;
//			cameraCurrentlyLocked = false;
//		}
//
//		lockedCameraMoving = false;
//	}
//
//	public void setCurrentAxisTransformValues (Transform newValues)
//	{
//		lockedMainCameraTransform.position = newValues.position;
//		lockedMainCameraTransform.eulerAngles = new Vector3 (0, newValues.eulerAngles.y, 0);
//	}
//
//	public Transform getLockedCameraTransform ()
//	{
//		return lockedMainCameraTransform;
//	}
//
//	public void setLockedMainCameraTransformRotation (Vector3 normal)
//	{
//		if (!cameraCurrentlyLocked) {
//			return;
//		}
//
//		//print (normal);
//
//		Vector3 previousLockedCameraAxisPosition = vector3Zero;
//		Vector3 lockedCameraAxisLocalPosition = vector3Zero;
//		Quaternion previousLockedCameraPivotRotation = lockedCameraPivot.rotation;
//
//		if (moveInXAxisOn2_5d) {
//			Quaternion targetRotation = Quaternion.LookRotation (Vector3.forward, normal);
//			float rotationAmount = targetRotation.eulerAngles.z;
//
//			lockedCameraPivot.eulerAngles = new Vector3 (0, 0, rotationAmount);
//		} else {
//			Quaternion targetRotation = Quaternion.LookRotation (Vector3.right, normal);
//			float rotationAmount = targetRotation.eulerAngles.x;
//
//			lockedCameraPivot.eulerAngles = new Vector3 (rotationAmount, 0, 0);
//		}
//
//		float lookCameraParentTargetAngle = 0;
//		float lockedMainCameraTransformTargetAngle = 0;
//		if (normal != Vector3.up) {
//			float currentUpAngle = 0;
//			float currentRightAngle = 0;
//
//			if (moveInXAxisOn2_5d) {
//				currentUpAngle = Vector3.SignedAngle (normal, Vector3.up, Vector3.forward);
//				currentRightAngle = Vector3.SignedAngle (normal, Vector3.right, Vector3.forward);
//			} else {
//				currentUpAngle = Vector3.SignedAngle (normal, Vector3.up, Vector3.right);
//				currentRightAngle = Vector3.SignedAngle (normal, Vector3.right, Vector3.right);
//			}
//
//			//print (currentUpAngle + " " + currentRightAngle);
//
//			lockedMainCameraTransformTargetAngle = currentUpAngle;
//			lookCameraParentTargetAngle = currentUpAngle;
//
//			if (currentUpAngle < 0) {
//				lockedMainCameraTransformTargetAngle = -currentUpAngle;
//				lookCameraParentTargetAngle = -currentUpAngle;
//			}
//
//			if (currentUpAngle < -90) {
//				lockedMainCameraTransformTargetAngle = currentUpAngle + 90;
//			}
//
//			if (currentUpAngle > 175 && currentUpAngle < 185) {
//				lockedMainCameraTransformTargetAngle = 0;
//			}
//
//			if (currentUpAngle > 90 && currentUpAngle < 175) {
//				lockedMainCameraTransformTargetAngle = 90 - currentRightAngle;
//				lookCameraParentTargetAngle = 360 - currentUpAngle;
//			}
//
//			if (currentUpAngle > 85 && currentUpAngle < 95) {
//				lookCameraParentTargetAngle = -currentUpAngle;
//			}
//
//			if (currentUpAngle > 0 && currentUpAngle < 85) {
//				lockedMainCameraTransformTargetAngle = -currentUpAngle;
//				lookCameraParentTargetAngle = -currentUpAngle;
//			}
//
//		}
//
//		lockedMainCameraTransform.eulerAngles = new Vector3 (lockedMainCameraTransform.eulerAngles.x, lockedMainCameraTransform.eulerAngles.y, lockedMainCameraTransformTargetAngle);
//
//		lookCameraParent.localRotation = Quaternion.Euler (new Vector3 (0, 0, lookCameraParentTargetAngle));
//
//		auxLockedCameraAxis.localPosition = currentLockedCameraAxisInfo.originalCameraAxisLocalPosition;
//
//		previousLockedCameraAxisPosition = auxLockedCameraAxis.position;
//
//		lockedCameraPivot.rotation = previousLockedCameraPivotRotation;
//
//		auxLockedCameraAxis.position = previousLockedCameraAxisPosition;
//
//		lockedCameraAxisLocalPosition = auxLockedCameraAxis.localPosition;
//
//		if (moveInXAxisOn2_5d) {
//			setLockedCameraAxisPositionOnGravityChange (
//				new Vector3 (lockedCameraAxisLocalPosition.x, lockedCameraAxisLocalPosition.y, currentLockedCameraAxisInfo.originalCameraAxisLocalPosition.z));
//		} else {
//			setLockedCameraAxisPositionOnGravityChange (
//				new Vector3 (currentLockedCameraAxisInfo.originalCameraAxisLocalPosition.x, lockedCameraAxisLocalPosition.y, lockedCameraAxisLocalPosition.z));
//		}
//	}
//
//	public void setLockedCameraAxisPositionOnGravityChange (Vector3 newPosition)
//	{
//		if (adjustLockedCameraAxisPositionOnGravityChangeCoroutine != null) {
//			StopCoroutine (adjustLockedCameraAxisPositionOnGravityChangeCoroutine);
//		}
//		adjustLockedCameraAxisPositionOnGravityChangeCoroutine = StartCoroutine (setLockedCameraAxisPositionOnGravityChangeCoroutine (newPosition));
//	}
//
//	//move the camera from its position in player camera to a fix position
//	IEnumerator setLockedCameraAxisPositionOnGravityChangeCoroutine (Vector3 newPosition)
//	{
//		float i = 0;
//		Vector3 currentPos = lockedCameraAxis.localPosition;
//		while (i < 1) {
//			i += getCurrentDeltaTime () * currentLockedCameraAxisInfo.cameraTransitionSpeed;
//			lockedCameraAxis.localPosition = Vector3.Lerp (lockedCameraAxis.localPosition, newPosition, i);
//			yield return null;
//		}
//	}
//
//
//	public Transform getCurrentLockedCameraTransform ()
//	{
//		return lockedCameraPosition;
//	}
//
//	public Transform getCurrentLockedCameraAxis ()
//	{
//		return lockedCameraAxis;
//	}
//
//	public Transform getCameraTransform ()
//	{
//		return mainCameraTransform;
//	}
//
//	public Camera getMainCamera ()
//	{
//		return mainCamera;
//	}
//
//	public Transform getPivotCameraTransform ()
//	{
//		return pivotCameraTransform;
//	}
//
//	public void setLookAtBodyPartsOnCharactersState (bool state)
//	{
//		lookAtBodyPartsOnCharacters = state;
//	}
//
//	public void setOriginalLookAtBodyPartsOnCharactersState ()
//	{
//		setLookAtBodyPartsOnCharactersState (originalLookAtBodyPartsOnCharactersValue);
//	}
//
//	public void setUseLookTargetIconState (bool state)
//	{
//		useLookTargetIcon = state;
//	}
//
//	public void setOriginalUseLookTargetIconValue ()
//	{
//		setUseLookTargetIconState (originalUseLookTargetIconValue);
//	}
//
//	public bool lookingAtFixedTarget;
//
//	public bool isPlayerLookingAtTarget ()
//	{
//		return lookingAtTarget;
//	}
//
//	public bool istargetToLookLocated ()
//	{
//		return targetToLook != null;
//	}
//
//	public void setLookAtTargetOnLockedCameraState ()
//	{
//		searchingToTargetOnLockedCamera = true;
//
//		//		print ("locked");
//	}
//
//	public bool getClosestTargetToLookOnLockedCamera ()
//	{
//		bool targetFound = false;
//
//		currentTargetToLookIndex = 0;
//		targetsListToLookTransform.Clear ();
//		List<Collider> targetsListCollider = new List<Collider> ();
//
//		List<GameObject> targetist = new List<GameObject> ();
//		List<GameObject> fullTargetList = new List<GameObject> ();
//
//		if (useLayerToSearchTargets) {
//			targetsListCollider.AddRange (Physics.OverlapSphere (playerCameraTransform.position, maxDistanceToFindTarget, layerToLook));
//			for (int i = 0; i < targetsListCollider.Count; i++) {
//				fullTargetList.Add (targetsListCollider [i].gameObject);
//			}
//		} else {
//			for (int i = 0; i < tagToLookList.Count; i++) {
//				GameObject[] enemiesList = GameObject.FindGameObjectsWithTag (tagToLookList [i]);
//				targetist.AddRange (enemiesList);
//			}
//
//			for (int i = 0; i < targetist.Count; i++) {	
//				float distance = GKC_Utils.distance (targetist [i].transform.position, playerCameraTransform.position);
//				if (distance < maxDistanceToFindTarget) {
//					fullTargetList.Add (targetist [i]);
//				}
//			}
//		}
//
//		if (fullTargetList.Contains (playerControllerGameObject)) {
//			fullTargetList.Remove (playerControllerGameObject);
//		}
//
//		List<GameObject> pointToLookComponentList = new List<GameObject> ();
//
//		if (searchPointToLookComponents) {
//			targetsListCollider.Clear ();
//
//			targetsListCollider.AddRange (Physics.OverlapSphere (playerCameraTransform.position, maxDistanceToFindTarget, pointToLookComponentsLayer));
//
//			for (int i = 0; i < targetsListCollider.Count; i++) {
//				if (targetsListCollider [i].isTrigger) {
//					pointToLook currentPointToLook = targetsListCollider [i].GetComponent<pointToLook> ();
//					if (currentPointToLook) {
//						GameObject currenTargetToLook = currentPointToLook.getPointToLookTransform ().gameObject;
//
//						fullTargetList.Add (currenTargetToLook);
//
//						pointToLookComponentList.Add (currenTargetToLook);
//					}
//				}
//			}
//		}
//
//		float screenWidth = Screen.width;
//		float screenHeight = Screen.height;
//
//		for (int i = 0; i < fullTargetList.Count; i++) {
//			if (fullTargetList [i] != null) {
//				GameObject currentTarget = fullTargetList [i];
//				if (tagToLookList.Contains (currentTarget.tag) || pointToLookComponentList.Contains (currentTarget)) {
//
//					bool obstacleDetected = false;
//
//					//for every target in front of the camera, use a raycast, if it finds an obstacle between the target and the camera, the target is removed from the list
//					Vector3 originPosition = playerCameraTransform.position + playerCameraTransform.up * 0.5f;
//					Vector3 targetPosition = currentTarget.transform.position + currentTarget.transform.up * 0.5f;
//
//					Vector3 direction = targetPosition - originPosition;
//					direction = direction / direction.magnitude;
//
//					float distance = GKC_Utils.distance (targetPosition, originPosition);
//
//					Debug.DrawLine (originPosition, originPosition + direction * distance, Color.black, 4);
//
//					if (Physics.Raycast (originPosition, direction, out hit, distance, layerToLook)) {
//						Debug.DrawLine (targetPosition, hit.point, Color.cyan, 4);
//						if (hit.collider.gameObject != currentTarget) {
//							obstacleDetected = true;
//						}
//					}
//
//					bool objectVisible = false;
//					if (currentLockedCameraAxisInfo.lookOnlyIfTargetOnScreen) {
//
//						if (usingScreenSpaceCamera) {
//							screenPoint = mainCamera.WorldToViewportPoint (currentTarget.transform.position);
//							targetOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1; 
//						} else {
//							screenPoint = mainCamera.WorldToScreenPoint (currentTarget.transform.position);
//							targetOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < screenWidth && screenPoint.y > 0 && screenPoint.y < screenHeight;
//						}
//
//						//the target is visible in the screen
//						if (targetOnScreen) {
//							//check if the target is visible on the current locked camera
//							if (currentLockedCameraAxisInfo.lookOnlyIfTargetVisible) {
//								originPosition = mainCamera.transform.position;
//								targetPosition = currentTarget.transform.position + currentTarget.transform.up * 0.5f;
//
//								direction = targetPosition - originPosition;
//								direction = direction / direction.magnitude;
//
//								distance = GKC_Utils.distance (targetPosition, originPosition);
//								if (Physics.Raycast (originPosition, direction, out hit, distance, layerToLook)) {
//									Debug.DrawLine (targetPosition, hit.point, Color.cyan, 4);
//									if (hit.collider.gameObject == currentTarget) {
//										objectVisible = true;
//									}
//								}
//							} else {
//								objectVisible = true;
//							}
//						}
//					} else {
//						objectVisible = true;
//					}
//
//					if (objectVisible && !obstacleDetected) {
//						targetsListToLookTransform.Add (currentTarget.transform);
//					}
//				}
//			}
//		}
//
//		//finally, get the target closest to the player
//		float minDistance = Mathf.Infinity;
//
//		for (int i = 0; i < targetsListToLookTransform.Count; i++) {
//			float currentDistance = GKC_Utils.distance (targetsListToLookTransform [i].position, playerCameraTransform.position);
//			if (currentDistance < minDistance) {
//				minDistance = currentDistance;
//				setTargetToLook (targetsListToLookTransform [i]);
//
//				targetFound = true;
//			}
//		}
//
//		targetsListToLookTransform.Sort (delegate(Transform a, Transform b) {
//			return Vector3.Distance (playerCameraTransform.position, a.position)
//				.CompareTo (
//				Vector3.Distance (playerCameraTransform.position, b.position));
//		});
//
//		if (targetFound) {
//			//			bool bodyPartFound = false;
//			//			if (lookAtBodyPartsOnCharacters) {
//			//
//			//				Transform bodyPartToLook = targetToLook;
//			//
//			//				List<health.weakSpot> characterWeakSpotList = applyDamage.getCharacterWeakSpotList (targetToLook.gameObject);
//			//
//			//				if (characterWeakSpotList != null) {
//			//					minDistance = Mathf.Infinity;
//			//					for (int i = 0; i < characterWeakSpotList.Count; i++) {
//			//						if (bodyPartsToLook.Contains (characterWeakSpotList [i].name)) {
//			//
//			//							screenPoint = mainCamera.WorldToScreenPoint (characterWeakSpotList [i].spotTransform.position);
//			//							float currentDistance = Utils.distance (screenPoint, centerScreen);
//			//							if (currentDistance < minDistance) {
//			//								minDistance = currentDistance;
//			//								bodyPartToLook = characterWeakSpotList [i].spotTransform;
//			//							}
//			//						}
//			//					}
//			//
//			//					bodyPartFound = true;
//			//
//			//					targetToLook = bodyPartToLook;
//			//				}
//			//			} 
//			//
//			//			if (!bodyPartFound) {
//
//			checkTargetToLookShader (targetToLook);
//
//			placeToShoot = applyDamage.getPlaceToShoot (targetToLook.gameObject);
//
//			if (placeToShoot != null) {
//				setTargetToLook (placeToShoot);
//			}
//			//			}
//		}
//
//		return targetFound;
//	}
//
//	public void checkRemoveEventOnLockedOnStart (Transform objectToCheck)
//	{
//		if (useRemoteEventsOnLockOn) {
//			if (previousTargetToLook != objectToCheck) {
//				previousTargetToLook = objectToCheck;
//
//				remoteEventSystem currentRemoteEventSystem = objectToCheck.GetComponent<remoteEventSystem> ();
//
//				if (currentRemoteEventSystem) {
//					for (int i = 0; i < remoteEventOnLockOnStart.Count; i++) {
	//						currentRemoteEventSystem.callRemoteEvent (remoteEventOnLockOnStart [i]);
//					}
//				}
//			}
//		}
//	}
//
//	public void checkRemoveEventOnLockOnEnd ()
//	{
//		if (useRemoteEventsOnLockOn) {
//			if (previousTargetToLook != null) {
//				remoteEventSystem currentRemoteEventSystem = previousTargetToLook.GetComponent<remoteEventSystem> ();
//
//				if (currentRemoteEventSystem) {
//					for (int i = 0; i < remoteEventOnLockOnEnd.Count; i++) {
	//						currentRemoteEventSystem.callRemoteEvent (remoteEventOnLockOnEnd [i]);
//					}
//				}
//
//				previousTargetToLook = null;
//			}
//		}
//	}
//
//	public void checkEventOnLockedOnStart ()
//	{
//		if (useEventsOnLockOn) {
//			eventOnLockOnStart.Invoke ();
//		}
//	}
//
//	public void checkEventOnLockOnEnd ()
//	{
//		if (useEventsOnLockOn) {
//			eventOnLockOnEnd.Invoke ();
//		}
//	}
//
//	public void setTargetToLook (Transform newTarget)
//	{
//		//		if (newTarget != null) {
//		//			print ("assign new target " + newTarget.name);
//		//		} else {
//		//			print ("remove target");
//		//		}
//
//		targetToLook = newTarget;
//	}
//
//	public Transform getCurrentTargetToLook ()
//	{
//		return targetToLook;
//	}
//
//	public GameObject getPlayerControllerGameObject ()
//	{
//		return playerControllerGameObject;
//	}
//
//	public void setHipsTransform (Transform newHips)
//	{
//		hipsTransform = newHips;
//
//		updateComponent ();
//	}
//
//	public void setLookAtTargetEnabledState (bool state)
//	{
//		lookAtTargetEnabled = state;
//	}
//
//	public void setLookAtTargetEnabledStateDuration (bool currentState, float duration, bool nextState)
//	{
//		if (lookAtTargetEnabledCoroutine != null) {
//			StopCoroutine (lookAtTargetEnabledCoroutine);
//		}
//		lookAtTargetEnabledCoroutine = StartCoroutine (setLookAtTargetEnabledStateDurationCoroutine (currentState, duration, nextState));
//	}
//
//	IEnumerator setLookAtTargetEnabledStateDurationCoroutine (bool currentState, float duration, bool nextState)
//	{
//		lookAtTargetEnabled = currentState;
//		yield return new WaitForSeconds (duration);
//		lookAtTargetEnabled = true;
//
//		setLookAtTargetStateInput (false);
//
//		lookAtTargetEnabled = nextState;
//
//		if (lookAtTargetEnabled) {
//			setOriginallookAtTargetSpeedValue ();
//		}
//	}
//
//	public void setLookAtTargetSpeedValue (float newValue)
//	{
//		lookAtTargetSpeed = newValue;
//	}
//
//	public void setOriginallookAtTargetSpeedValue ()
//	{
//		lookAtTargetSpeed = originalLookAtTargetSpeed;
//	}
//
//	public void setMaxDistanceToFindTargetValue (float newValue)
//	{
//		maxDistanceToFindTarget = newValue;
//	}
//
//	public void setOriginalmaxDistanceToFindTargetValue ()
//	{
//		maxDistanceToFindTarget = originalMaxDistanceToFindTarget;
//	}
//
//	public void setMaxDistanceToCameraCenter (bool useMaxDistance, float maxDistance)
//	{
//		useMaxDistanceToCameraCenter = useMaxDistance;
//		maxDistanceToCameraCenter = maxDistance;
//	}
//
//	public void slerpCameraState (cameraStateInfo to, cameraStateInfo from, float lerpSpeed)
//	{
//		to.assignCameraStateValues (from, lerpSpeed);
//	}
//
//
//	public bool isCurrentCameraStateHeadTrackActive ()
//	{
//		return currentState.headTrackActive;
//	}
//
//	public void changeRotationSpeedValue (float newVerticalRotationValue, float newHorizontalRotationValue)
//	{
//		if (firstPersonActive) {
//			firstPersonVerticalRotationSpeed = newVerticalRotationValue;
//			firstPersonHorizontalRotationSpeed = newHorizontalRotationValue;
//		} else {
//			thirdPersonVerticalRotationSpeed = newVerticalRotationValue;
//			thirdPersonHorizontalRotationSpeed = newHorizontalRotationValue;
//		}
//	}
//
//	public void setOriginalRotationSpeed ()
//	{
//		firstPersonVerticalRotationSpeed = originalFirstPersonVerticalRotationSpeed;
//		firstPersonHorizontalRotationSpeed = originalFirstPersonHorizontalRotationSpeed;
//		thirdPersonVerticalRotationSpeed = originalThirdPersonVerticalRotationSpeed;
//		thirdPersonHorizontalRotationSpeed = originalThirdPersonHorizontalRotationSpeed;
//	}
//
//	public void setUseCustomThirdPersonAimActiveState (bool state, string newCustomDefaultThirdPersonAimRightStateName, string newCustomDefaultThirdPersonAimLeftStateName)
//	{
//		useCustomThirdPersonAimActive = state;
//
//		if (useCustomThirdPersonAimActive) {
//			customDefaultThirdPersonAimRightStateName = newCustomDefaultThirdPersonAimRightStateName;
//			customDefaultThirdPersonAimLeftStateName = newCustomDefaultThirdPersonAimLeftStateName;
//
//			if (customDefaultThirdPersonAimRightStateName.Equals ("")) {
//				customDefaultThirdPersonAimRightStateName = defaultThirdPersonAimRightStateName;
//			}
//
//			if (customDefaultThirdPersonAimLeftStateName.Equals ("")) {
//				customDefaultThirdPersonAimLeftStateName = defaultThirdPersonAimLeftStateName;
//			}
//		}
//	}
//
//	public void setCameraRotationInputEnabled (bool state)
//	{
//		cameraRotationInputEnabled = state;
//	}
//
//	public void enableCameraRotationInput ()
//	{
//		cameraRotationInputEnabled = true;
//	}
//
//	public void disableCameraRotationInput ()
//	{
//		cameraRotationInputEnabled = false;
//	}
//
//	public void setCameraActionsInputEnabledState (bool state)
//	{
//		cameraActionsInputEnabled = state;
//	}
//
//	public void enableCameraActionsInput ()
//	{
//		cameraActionsInputEnabled = true;
//	}
//
//	public void disableCameraActionsInput ()
//	{
//		cameraActionsInputEnabled = false;
//	}
//
//	public bool isCameraRotating ()
//	{
//		return isMoving;
//	}
//
//	public bool isPlayerAiming ()
//	{
//		return playerAiming;
//	}
//
//	public void setLastTimeMoved ()
//	{
//		lastTimeMoved = Time.time;
//	}
//
//	public float getLastTimeMoved ()
//	{
//		return lastTimeMoved;
//	}
//
//	public void setLastTimeCameraRotated ()
//	{
//		lastTimeCameraRotated = Time.time;
//	}
//		
//	public float getOriginalCameraFov ()
//	{
//		return currentState.initialFovValue;
//	}
//
//	public bool isFirstPersonActive ()
//	{
//		return firstPersonActive;
//	}
//
//	public bool isCameraTypeFree ()
//	{
//		return cameraType == typeOfCamera.free;
//	}
//
//	public void setLookAngleValue (Vector2 newValue)
//	{
//		lookAngle = newValue;
//	}
//
//	public void resetMainCameraTransformLocalPosition ()
//	{
//		mainCameraTransform.localPosition = lerpState.camPositionOffset;
//	}
//
//	public void resetPivotCameraTransformLocalPosition ()
//	{
//		pivotCameraTransform.localPosition = lerpState.pivotPositionOffset;
//	}
//
//	public void configureCameraAndPivotPositionAtOnce ()
//	{
//		pivotCameraTransform.localPosition = currentState.pivotPositionOffset;
//		mainCameraTransform.localPosition = currentState.camPositionOffset;
//	}
//
//	public void setCurrentCameraUpRotationValue (float newRotation)
//	{
//		currentCameraUpRotation = newRotation;
//	}
//
//	public void setForwardRotationValue (float value)
//	{
//		targetForwardRotationAngle = value;
//	}
//
//	public Vector2 getIconPosition (Vector3 worldObjectPosition)
//	{
//		iconPositionViewPoint = mainCamera.WorldToViewportPoint (worldObjectPosition);
//		iconPosition2d = new Vector2 ((iconPositionViewPoint.x * mainCanvasSizeDelta.x) - halfMainCanvasSizeDelta.x, (iconPositionViewPoint.y * mainCanvasSizeDelta.y) - halfMainCanvasSizeDelta.y);
//
//		return iconPosition2d;
//	}
//
//	public void setLockedCameraRotationActiveToLeft (bool holdingButton)
//	{
//		if (currentLockedCameraAxisInfo.useFixedRotationAmount) {
//			if (!rotatingLockedCameraFixedRotationAmountActive) {
//				stopRotateLockedCameraFixedRotationAmount ();
//
//				rotateLockedCameraFixedAmountCoroutine = StartCoroutine (rotateLockedCameraFixedRotationAmount (false));
//			}
//		} else {
//			rotatingLockedCameraToLeft = holdingButton;
//			lockedCameraRotationDirection = -1;
//
//			if (!rotatingLockedCameraToLeft && rotatingLockedCameraToRight) {
//				lockedCameraRotationDirection = 1;
//			}
//		}
//	}
//
//	public void setLockedCameraRotationActiveToRight (bool holdingButton)
//	{
//		if (currentLockedCameraAxisInfo.useFixedRotationAmount) {
//			if (!rotatingLockedCameraFixedRotationAmountActive) {
//				stopRotateLockedCameraFixedRotationAmount ();
//
//				rotateLockedCameraFixedAmountCoroutine = StartCoroutine (rotateLockedCameraFixedRotationAmount (true));
//			}
//		} else {
//			rotatingLockedCameraToRight = holdingButton;
//			lockedCameraRotationDirection = 1;
//
//			if (!rotatingLockedCameraToRight && rotatingLockedCameraToLeft) {
//				lockedCameraRotationDirection = -1;
//			}
//		}
//	}
//
//	public void stopRotateLockedCameraFixedRotationAmount ()
//	{
//		rotatingLockedCameraFixedRotationAmountActive = false;
//
//		if (rotateLockedCameraFixedAmountCoroutine != null) {
//			StopCoroutine (rotateLockedCameraFixedAmountCoroutine);
//		}
//	}
//
//	IEnumerator rotateLockedCameraFixedRotationAmount (bool rotateToRight)
//	{
//		rotatingLockedCameraFixedRotationAmountActive = true;
//
//		float rotationAmount = currentLockedCameraAxisInfo.fixedRotationAmountToLeft;
//
//		if (rotateToRight) {
//			rotationAmount = currentLockedCameraAxisInfo.fixedRotationAmountToRight;
//		}
//
//		Vector3 targetRotationEuler = lockedMainCameraTransform.up * rotationAmount;
//
//		targetRotationEuler += lockedMainCameraTransform.eulerAngles;
//
//		Quaternion targetRotation = Quaternion.Euler (targetRotationEuler);
//
//		bool targetReached = false;
//
//		float t = 0;
//
//		float movementTimer = 0;
//
//		float angleDifference = 0;
//
//		float duration = Math.Abs (Quaternion.Angle (lockedMainCameraTransform.rotation, targetRotation)) / currentLockedCameraAxisInfo.fixedRotationAmountSpeed;
//
//		while (!targetReached) {
//			t += getCurrentDeltaTime () / duration; 
//
//			lockedMainCameraTransform.rotation = Quaternion.Lerp (lockedMainCameraTransform.rotation, targetRotation, t);
//			lockedCameraPivot.rotation = Quaternion.Lerp (lockedCameraPivot.rotation, targetRotation, t);
//
//			movementTimer += Time.deltaTime;
//
//			angleDifference = Quaternion.Angle (lockedMainCameraTransform.rotation, targetRotation);
//
//			if (angleDifference < 0.2 || movementTimer > (duration + 2)) {
//				targetReached = true;
//			}
//
//			yield return null;
//		}
//
//		rotatingLockedCameraFixedRotationAmountActive = false;
//	}
//
//	public void setCanActivateLookAtTargetEnabledState (bool state)
//	{
//		canActivateLookAtTargetEnabled = state;
//	}
//
//	public void disableStrafeModeActivateFromNoTargetsFoundActive ()
//	{
//		strafeModeActivateFromNoTargetsFoundActive = false;
//	}
//
//	public bool changeCameraSideActive = true;
//
//	bool checkToKeepAfterAimingFromShooting;
//
//	public void setCheckToKeepAfterAimingFromShootingState (bool state)
//	{
//		checkToKeepAfterAimingFromShooting = state;
//	}
//
//	bool aimingFromShooting;
//
//	public void setAimingFromShootingState (bool state)
//	{
//		aimingFromShooting = state;
//	}
//
//	public bool isTemporalCameraViewToLockedCameraActive ()
//	{
//		return temporalCameraViewToLockedCameraActive;
//	}
//
//	bool temporalCameraViewToLockedCameraActive;
//
//	bool activatingLockedCameraByInputActive;
//
//	//CALL INPUT FUNCTIONS
//
//	public void inputRotateLockedCameraToRight (bool holdingButton)
//	{
//		if (!cameraActionsInputEnabled) {
//			return;
//		}
//
//		if (!isCameraTypeFree ()) {
//			setLockedCameraRotationActiveToRight (holdingButton);
//		}
//	}
//
//	public void inputRotateLockedCameraToLeft (bool holdingButton)
//	{
//		if (!cameraActionsInputEnabled) {
//			return;
//		}
//
//		if (!isCameraTypeFree ()) {
//			setLockedCameraRotationActiveToLeft (holdingButton);
//		}
//	}
//
//	float lockedCameraZoomMovingCameraValue;
//
//	public void inputMoveLockedCameraZoomOn ()
//	{
//		if (!cameraActionsInputEnabled) {
//			return;
//		}
//
//		inputMoveLockedCameraZoom (true);
//	}
//
//	public void inputMoveLockedCameraZoomOff ()
//	{
//		if (!cameraActionsInputEnabled) {
//			return;
//		}
//
//		inputMoveLockedCameraZoom (false);
//	}
//
//	public void inputMoveLockedCameraZoom (bool moveForward)
//	{
//		if (!cameraActionsInputEnabled) {
//			return;
//		}
//
//		if (isCameraTypeFree ()) {
//			return;
//		}
//
//		if (moveForward) {
//			lockedCameraZoomMovingCameraValue += currentLockedCameraAxisInfo.zoomByMovingCameraAmount;
//		} else {
//			lockedCameraZoomMovingCameraValue -= currentLockedCameraAxisInfo.zoomByMovingCameraAmount;
//		}
//	}
//	//END OF CALL INPUT FUNCTIONS
//
//
//	public void resetCameraRotation ()
//	{
//		if (isCameraTypeFree () && !firstPersonActive && cameraCanBeUsed) {
//			resetingCameraActive = true;
//		}
//	}
//		
//	public Vector2 getMainCanvasSizeDelta ()
//	{
//		return mainCanvasSizeDelta;
//	}
//
//	public bool isUsingScreenSpaceCamera ()
//	{
//		return usingScreenSpaceCamera;
//	}
//
//	public void setResetCameraRotationAfterTimeState (bool state)
//	{
//		resetCameraRotationAfterTime = state;
//	}
//
//	public void addNewLockedCameraSystemToLevel ()
//	{
//		GameObject newLockedCameraSystem = (GameObject)Instantiate (lockedCameraSystemPrefab, vector3Zero, quaternionIdentity);
//		newLockedCameraSystem.name = lockedCameraSystemPrefab.name;
//
//		GKC_Utils.setActiveGameObjectInEditor (newLockedCameraSystem);
//	}
//
//	public void addNewLockedCameraLimitSystemToLevel ()
//	{
//		GameObject newLockedCameraLimitSystem = (GameObject)Instantiate (lockedCameraLimitSystemPrefab, vector3Zero, quaternionIdentity);
//		newLockedCameraLimitSystem.name = lockedCameraLimitSystemPrefab.name;
//
//		GKC_Utils.setActiveGameObjectInEditor (newLockedCameraLimitSystem);
//	}
//
//	public void addNewLockedCameraPrefabTypeLevel (int cameraIndex)
//	{
//		if (cameraIndex <= lockedCameraPrefabsTypesList.Count && lockedCameraPrefabsTypesList [cameraIndex].lockedCameraPrefab != null) {
//			GameObject newLockedCameraSystem = (GameObject)Instantiate (lockedCameraPrefabsTypesList [cameraIndex].lockedCameraPrefab, vector3Zero, quaternionIdentity);
//			newLockedCameraSystem.name = lockedCameraPrefabsTypesList [cameraIndex].lockedCameraPrefab.name;
//
//			GKC_Utils.setActiveGameObjectInEditor (newLockedCameraSystem);
//		} else {
//			print ("WARNING: prefab of the selected camera doesn't exist or is not configured on this list, make sure a prefab is assigned");
//		}
//	}
//
//	public void updateCameraStateValuesOnEditor (int stateIndex)
//	{
//		cameraStateInfo newState = new cameraStateInfo (playerCameraStates [stateIndex]);
//
//		lerpState = newState;
//	}
//
//	public void updateComponent ()
//	{
//		GKC_Utils.updateComponent (this);
//	}
//
//	//draw the lines of the pivot camera in the editor
//	void OnDrawGizmos ()
//	{
//		if (!settings.showCameraGizmo) {
//			return;
//		}
//
//		if (GKC_Utils.isCurrentSelectionActiveGameObject (gameObject)) {
//			DrawGizmos ();
//		}
//	}
//
//	void OnDrawGizmosSelected ()
//	{
//		DrawGizmos ();
//	}
//
//	void DrawGizmos ()
//	{
//		if (settings.showCameraGizmo) {
//
//			if (closestWaypoint) {
//				Gizmos.color = Color.green;
//				Gizmos.DrawWireSphere (closestWaypoint.position, 1.1f);
//			}
//
//			if (useCameraLimit) {
//				Gizmos.color = Color.red;
//
//				Gizmos.DrawWireSphere (currentCameraLimitPosition, 1);
//			}
//
//			if (!isCameraTypeFree ()) {
//				if (currentLockedCameraAxisInfo != null) {
//					if (currentLockedCameraAxisInfo.lockedCameraPivot) {
//						Gizmos.color = Color.green;
//						Vector3 pivotPosition = lockedCameraPivot.position + lockedCameraPivot.up * lockedCameraAxis.localPosition.y;
//
//						Gizmos.DrawLine (lockedCameraAxis.position, pivotPosition);
//
//						Gizmos.DrawLine (pivotPosition, lockedCameraPivot.position);
//
//						GKC_Utils.drawGizmoArrow (lockedCameraPosition.position, lockedCameraPosition.forward * 1, Color.green, 0.5f, 10);
//
//						Gizmos.color = Color.yellow;
//						Gizmos.DrawLine (lookCameraParent.position, lookCameraPivot.position);
//						Gizmos.DrawLine (lookCameraPivot.position, lookCameraDirection.position);
//					}
//
//					if (currentLockedCameraAxisInfo.useBoundToFollowPlayer) {
//						//lockedCameraSystem.drawBoundGizmo (focusArea.centre, currentLockedCameraAxisInfo, new Color (1, 0, 0, .5f));
//
//						Gizmos.color = new Color (1, 0, 0, .5f);
//
//						float height = (currentLockedCameraAxisInfo.heightBoundTop);
//						float width = (currentLockedCameraAxisInfo.widthBoundRight + currentLockedCameraAxisInfo.widthBoundLeft);
//						float depth = (currentLockedCameraAxisInfo.depthBoundFront + currentLockedCameraAxisInfo.depthBoundBackward);
//
//						Gizmos.DrawCube (focusArea.centre, new Vector3 (width, height, depth));
//					}
//				}
//			}
//		}
//	}
//
//	[System.Serializable]
//	public struct FocusArea
//	{
//		public Vector3 centre;
//		public Vector3 velocity;
//		public float left, right;
//		public float top, bottom;
//		public float front, backward;
//
//		public FocusArea (Bounds targetBounds, float heightBoundTop, float widthBoundRight, float widthBoundLeft, float depthBoundFront, float depthBoundBackward)
//		{
//			left = targetBounds.center.x - widthBoundLeft;
//			right = targetBounds.center.x + widthBoundRight;
//			bottom = targetBounds.min.y;
//			top = targetBounds.min.y + heightBoundTop;
//
//			front = targetBounds.center.z + depthBoundFront;
//			backward = targetBounds.center.z - depthBoundBackward;
//
//			velocity = Vector3.zero;
//			centre = new Vector3 ((left + right) / 2, (top + bottom) / 2, (front + backward) / 2);
//		}
//
//		public void Update (Bounds targetBounds)
//		{
//			float shiftX = 0;
//			if (targetBounds.min.x < left) {
//				shiftX = targetBounds.min.x - left;
//			} else if (targetBounds.max.x > right) {
//				shiftX = targetBounds.max.x - right;
//			}
//			left += shiftX;
//			right += shiftX;
//
//			float shiftY = 0;
//			if (targetBounds.min.y < bottom) {
//				shiftY = targetBounds.min.y - bottom;
//			} else if (targetBounds.max.y > top) {
//				shiftY = targetBounds.max.y - top;
//			}
//			top += shiftY;
//			bottom += shiftY;
//
//			float shiftZ = 0;
//			if (targetBounds.min.z < backward) {
//				shiftZ = targetBounds.min.z - backward;
//			} else if (targetBounds.max.z > front) {
//				shiftZ = targetBounds.max.z - front;
//			}
//			front += shiftZ;
//			backward += shiftZ;
//
//			centre = new Vector3 ((left + right) / 2, (top + bottom) / 2, (front + backward) / 2);
//			velocity = new Vector3 (shiftX, shiftY, shiftZ);
//		}
//	}
//
//	[System.Serializable]
//	public class lockedCameraPrefabsTypes
//	{
//		public string Name;
//		public GameObject lockedCameraPrefab;
//	}
}