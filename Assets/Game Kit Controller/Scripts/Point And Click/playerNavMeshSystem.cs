using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class playerNavMeshSystem : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public bool playerNavMeshEnabled;

	public LayerMask layerToPress;

	public LayerMask layerForGround;

	public LayerMask layerForElements;

	public LayerMask layerToFindGround;

	public bool searchCorrectSurfaceOnHighAngle;
	public float maxSurfaceAngle = 80;

	public float lookAtTargetSpeed = 15;
	public float targetChangeTolerance = 1;

	public float minDistanceToFriend;
	public float minDistanceToEnemy;
	public float minDistanceToObjects;
	public float minDistanceToSurface;

	public float maxDistanceToRecalculatePath = 1;

	public bool smoothSpeedWhenCloseToTarget = true;

	public float updateNavMeshPositionRate = 0.5f;
	public float calculatePathRate = 0.5f;

	public bool useDoubleClickToMoveTowardDevices;
	public float timeBetweenClicks;

	public bool debugMoveEnabled = true;

	public bool useCheckHoverPointAndClickElements;

	[Space]
	[Header ("Other Settings")]
	[Space]

	public bool usePointAndClickPanel;
	public bool disablePointAndClickPanelOnSurfacePressed;
	public GameObject pointAndClickPanel;

	public bool useElementInfoText;
	public Text elementInfoText;

	public bool useElementNameText;
	public Text elementNameText;

	public bool useDeviceButtons;
	public GameObject startUseDeviceButton;
	public GameObject stopToUseDeviceButton;

	public Text startUseDeviceButtonText;
	public Text stopToUseDeviceButtonText;

	public Transform transformToFollow;
	public GameObject particlesInTransformToFollow;
	public bool useParticlesToTargetPosition = true;

	[Space]
	[Header ("Dynamic Obstacle Detection Settings")]
	[Space]

	public bool useDynamicObstacleDetection;
	public LayerMask dynamicObstacleLayer;
	public float dynamicAvoidSpeed = 4;
	public float dynamicRaycastDistanceForward = 3;
	public float dynamicRaycastDistanceDiagonal = 0.5f;
	public float dynamicRaycastDistanceSides = 1;
	public float dynamicRaycastDistanceAround = 4;

	[Space]
	[Header ("AI State (Debug)")]
	[Space]

	public Transform targetToReach;

	public GameObject currentElementToUse;

	public bool insideADeviceElement;

	public bool targetIsFriendly;
	public bool targetIsObject;
	public bool targetIsSurface;

	public bool navMeshPaused;

	public bool attacking;
	public bool following;
	public bool waiting;
	public bool hiding;

	public bool followingTarget;

	public bool checkingToResumeNavmesh;

	public bool onGround;

	public bool targetSelected;

	public bool playerIsUsingDevice;

	public bool usingPlayerNavmeshExternally;

	public float navSpeed = 1;

	public Vector3 currentMovementDirection;

	public Vector3 navMeshVelocity;

	public float currentMinDistance;

	public bool lookingPathAfterJump;

	public bool obstacleOnForward;
	public bool obstacleOnRight;
	public bool obstacleOnLeft;
	public bool obstacleOnRightForward;
	public bool obstacleOnLeftForward;

	[Space]
	[Header ("Gizmo Settings")]
	[Space]

	public bool showGizmo;
	public bool showPathRenderer = true;

	[Space]
	[Header ("Components")]
	[Space]
	public GameObject playerCameraGameObject;
	public Transform rayCastPosition;

	public menuPause pauseManager;
	public usingDevicesSystem devicesManager;
	public playerController playerControllerManager;
	public playerCamera playerCameraManager;
	public Camera mainCamera;
	public NavMeshAgent agent;
	public LineRenderer lineRenderer;

	Vector3 currentTargetPosition;
	Color c = Color.white;

	bool useCustomNavMeshSpeed;
	float customNavMeshSpeed;

	bool useCustomMinDistance;
	float customMinDistance;

	OffMeshLinkData _currLink;

	AINavMeshMoveInfo AIMoveInput = new AINavMeshMoveInfo ();

	Vector3 targetOffset;

	Vector3 lastTargetPosition;
	NavMeshPath currentNavMeshPath;
	List<Vector3> pathCorners = new List<Vector3> ();
	Vector3 lastPathTargetPosition;
	Vector3 currentDestinationPosition;
	float positionOffset = 0.1f;
	Coroutine waitToResumeCoroutine;
	float currentDistanceToTarget;
	float lastTimeCalculatePath;
	float lastTimeUpdateNavMeshPosition;
	Vector3 currentPosition;
	Vector3 targetCurrentPosition;

	bool touchPlatform;
	Touch currentTouch;

	RaycastHit hit;
	Ray vRay;

	GameObject currentObjectPressed;

	GameObject previousElementToUse;
	Vector3 positionToReach;

	readonly List<RaycastResult> captureRaycastResults = new List<RaycastResult> ();
	Vector2 currentTouchPosition;

	bool currentElementUseOnOffDeviceState;
	string currentElementDeviceActionText;

	float lastTimePressed;
	pointAndClickElement currentPointAndClickElement;
	bool useCustomElementMinDistance;
	float customElementMinDistance;

	bool playerIsUsingDeviceChecked;

	bool canUseTargetParticles = true;

	pointAndClickElement currentHoverPointAndClickElement;
	GameObject currentHoverGameObjectDetected;
	GameObject previosHoverGameObjectDetected;

	bool originalUseDynamicObstacleDetection;

	Vector3 currentUp;
	Vector3 currentForward;

	bool obstacleDetected;
	RaycastHit rayhit;

	Vector3 avoidDirection;

	Vector3 currentCheckObstalceDirection;

	bool moveToRight;

	int numberOfObstacleDirectionDetected = 0;

	float obstacleDistance;

	Vector3 newMoveDirection;

	Vector3 currentMoveDirection;

	void Start ()
	{
		if (lineRenderer == null && showPathRenderer) {
			lineRenderer = gameObject.AddComponent<LineRenderer> ();
			lineRenderer.material = new Material (Shader.Find ("Sprites/Default")) { color = c };
			lineRenderer.startWidth = 0.5f;
			lineRenderer.endWidth = 0.5f;
			lineRenderer.startColor = c;
			lineRenderer.endColor = c;
		}

		touchPlatform = touchJoystick.checkTouchPlatform ();
			
		if (transformToFollow == null) {
			GameObject transformToFollowGameObject = new GameObject ();
			transformToFollow = transformToFollowGameObject.transform;
			transformToFollow.name = "Transform To Follow (Nav Mesh)";
		}

		transformToFollow.gameObject.SetActive (false);

		originalUseDynamicObstacleDetection = useDynamicObstacleDetection;
	}

	void Update ()
	{
		if (!playerNavMeshEnabled || playerControllerManager.isGamePaused () || playerControllerManager.isPlayerMenuActive ()) {
			return;
		}

		if (!usingPlayerNavmeshExternally) {

			playerIsUsingDevice = playerControllerManager.isUsingDevice ();

			//check if the player is using a device, in that case, disable the point and click panel if it is active and the object that the player is using is configured to be disabled
			if (playerIsUsingDevice != playerIsUsingDeviceChecked) {
				playerIsUsingDeviceChecked = playerIsUsingDevice;

				if (currentPointAndClickElement != null && usePointAndClickPanel) {
					if (playerIsUsingDevice) {
						if (currentPointAndClickElement.disablePanelAfterUse) {
							pointAndClickPanel.SetActive (false);
						}
					} else {
						if (currentPointAndClickElement.activePanelAfterStopUse) {
							pointAndClickPanel.SetActive (true);
						}
					}
				}
			}

			if (useDoubleClickToMoveTowardDevices) {
				if (Time.time > lastTimePressed + timeBetweenClicks) {
					previousElementToUse = null;
					lastTimePressed = 0;
				}
			}

			int touchCount = Input.touchCount;
			if (!touchPlatform) {
				touchCount++;
			}

			for (int i = 0; i < touchCount; i++) {
				if (!touchPlatform) {
					currentTouch = touchJoystick.convertMouseIntoFinger ();
				} else {
					currentTouch = Input.GetTouch (i);
				}

				if (useCheckHoverPointAndClickElements && !playerIsUsingDevice) {
					vRay = mainCamera.ScreenPointToRay (currentTouch.position);

					if (Physics.Raycast (vRay, out hit, Mathf.Infinity, layerForElements)) {
						currentHoverGameObjectDetected = hit.collider.gameObject;

						if (currentHoverGameObjectDetected != previosHoverGameObjectDetected) {
							previosHoverGameObjectDetected = currentHoverGameObjectDetected;

							currentHoverPointAndClickElement = currentHoverGameObjectDetected.GetComponent<pointAndClickElement> ();

							if (currentHoverPointAndClickElement != null) {
								currentHoverPointAndClickElement.setHoveringPointAndClickElementState (true);
							}
						}
					} else {
						if (currentHoverGameObjectDetected != null) {
							currentHoverGameObjectDetected = null;
							previosHoverGameObjectDetected = null;

							if (currentHoverPointAndClickElement != null) {
								currentHoverPointAndClickElement.setHoveringPointAndClickElementState (false);

								currentHoverPointAndClickElement = null;
							}
						}
					}
				}
		
				if (currentTouch.phase == TouchPhase.Began) {
				
					currentTouchPosition = currentTouch.position;

					captureRaycastResults.Clear ();
					PointerEventData p = new PointerEventData (EventSystem.current);

					p.position = currentTouchPosition;
					p.clickCount = i;
					p.dragging = false;
					EventSystem.current.RaycastAll (p, captureRaycastResults);

					foreach (RaycastResult r in captureRaycastResults) {
						if (r.gameObject.transform.IsChildOf (pointAndClickPanel.transform)) {
							return;
						}
					}

					vRay = mainCamera.ScreenPointToRay (currentTouch.position);

					checkRaycastPosition (vRay);
				}
			}
		}

		if (!navMeshPaused) {
			if (onGround) {
				if (lookingPathAfterJump) {
					lookingPathAfterJump = false;
				}
			}

			if (checkingToResumeNavmesh) {
				if (onGround) {
					agent.isStopped = false;
					checkingToResumeNavmesh = false;

					return;
				} else {
					return;
				}
			}

			if (targetToReach != null && agent.enabled) {

				if (playerControllerManager.isPlayerAiming ()) {
					removeTarget ();

					return;
				}

				currentPosition = transform.position;
				targetCurrentPosition = targetToReach.position;

				currentDistanceToTarget = GKC_Utils.distance (targetCurrentPosition, currentPosition);

				if (useCustomElementMinDistance) {
					currentMinDistance = customElementMinDistance;
				} else {
					if (targetIsObject) {
						currentMinDistance = minDistanceToObjects;
					} else if (targetIsSurface) {
						currentMinDistance = minDistanceToSurface;
					} else {
						if (targetIsFriendly) {
							currentMinDistance = minDistanceToFriend;
						} else {
							currentMinDistance = minDistanceToEnemy;
						}
					}
				}

				if (useCustomMinDistance) {
					currentMinDistance = customMinDistance;
				}

				if (currentDistanceToTarget > currentMinDistance) {
					// update the progress if the character has made it to the previous target
					currentTargetPosition = targetCurrentPosition + targetToReach.up * positionOffset;

					// use the values to move the character
					if (smoothSpeedWhenCloseToTarget) {
						navSpeed = currentDistanceToTarget / 20;
						navSpeed = Mathf.Clamp (navSpeed, 0.1f, 1);
					} else {
						navSpeed = 1;
					}

					followingTarget = true;

					lootAtTarget (targetToReach);
				} else {
					// We still need to call the character's move function, but we send zeroed input as the move param.

					if (devicesManager.existInDeviceList (currentElementToUse)) {
						if (useDeviceButtons) {
							//print ("device: " + currentElementToUse.name);
							currentElementDeviceActionText = devicesManager.getCurrentDeviceActionText ();
							currentElementUseOnOffDeviceState = currentPointAndClickElement.useOnOffDeviceState;

							startUseDeviceButtonText.text = currentElementDeviceActionText;

							setStartAndStopUseDeviceButtonsState (true, false);

							if (usePointAndClickPanel) {
								pointAndClickPanel.SetActive (true);
							}
						}
					}


					moveNavMesh (Vector3.zero, false, false);
					removeTarget ();
				}

				if (followingTarget) {
					if ((!targetIsFriendly || targetIsObject)) {
						navSpeed = 1;
					}

					if (useCustomNavMeshSpeed) {
						navSpeed = customNavMeshSpeed;
					}

					if (!lookingPathAfterJump) {
						setAgentDestination (currentTargetPosition);
						currentDestinationPosition = currentTargetPosition;
					}

					if (!lookingPathAfterJump) {
						if (!updateCurrentNavMeshPath (currentDestinationPosition)) {
							bool getClosestPosition = false;

							if (currentNavMeshPath.status != NavMeshPathStatus.PathComplete) {
								getClosestPosition = true;
							}

							if (getClosestPosition) {
								//get closest position to target that can be reached
								//maybe a for bucle checking every position ofthe corner and get the latest reachable
								Vector3 positionToGet = currentDestinationPosition;
								if (pathCorners.Count > 1) {
									positionToGet = pathCorners [pathCorners.Count - 2];
								}
							}
						}

						if (currentNavMeshPath != null) {
							navMeshVelocity = agent.velocity;
							navMeshVelocity.Normalize ();

							if (currentNavMeshPath.status == NavMeshPathStatus.PathComplete) {
								//print ("Can reach" +agent.desiredVelocity);
								moveNavMesh (navMeshVelocity * navSpeed, false, false);
								c = Color.white;
							} else if (currentNavMeshPath.status == NavMeshPathStatus.PathPartial) {
								c = Color.yellow;
								if (GKC_Utils.distance (currentPosition, pathCorners [pathCorners.Count - 1]) > 2) {
									moveNavMesh (navMeshVelocity * navSpeed, false, false);
								} else {
									moveNavMesh (Vector3.zero, false, false);
								}
								//print ("Can get close");
							} else if (currentNavMeshPath.status == NavMeshPathStatus.PathInvalid) {
								c = Color.red;
								//print ("Can't reach");
							}

							if (agent.isOnOffMeshLink && !lookingPathAfterJump) {
								_currLink = agent.currentOffMeshLinkData;
								lookingPathAfterJump = true;
								moveNavMesh (navMeshVelocity * navSpeed, false, true);

								setAgentDestination (currentTargetPosition);
								currentDestinationPosition = currentTargetPosition;
				
								updateCurrentNavMeshPath (currentDestinationPosition);

								StartCoroutine (MoveAcrossNavMeshLink ());
							} 
						}
					} else {
						navMeshVelocity = _currLink.endPos - transform.position;
						navMeshVelocity.Normalize ();

						moveNavMesh (navMeshVelocity * navSpeed, false, false);
					}

					if (showPathRenderer) {
						lineRenderer.enabled = true;
						lineRenderer.startColor = c;
						lineRenderer.endColor = c;

						lineRenderer.positionCount = pathCorners.Count;

						for (int i = 0; i < pathCorners.Count; i++) {
							lineRenderer.SetPosition (i, pathCorners [i]);
						}
					}
				} else {
					if (showPathRenderer) {
						lineRenderer.enabled = false;
					}
				}
			} else {
				lootAtTarget (null);

				moveNavMesh (Vector3.zero, false, false);

				if (followingTarget) {
					if (agent.enabled) {
						agent.ResetPath ();
					}

					followingTarget = false;
				}

				if (showPathRenderer) {
					lineRenderer.enabled = false;
				}
			}

		} else {
			if (followingTarget) {
				if (agent.enabled) {
					agent.ResetPath ();
				}

				followingTarget = false;
			}

			if (showPathRenderer) {
				lineRenderer.enabled = false;
			}
		}
	}

	public void checkRaycastPositionWithVector3 (Vector3 raycastPosition)
	{
		vRay.origin = raycastPosition;
		vRay.direction = -Vector3.up;

		checkRaycastPosition (vRay);
	}

	public void checkRaycastPosition (Ray newRay)
	{
		if (Physics.Raycast (newRay, out hit, Mathf.Infinity, layerToPress) && !playerIsUsingDevice) {

			positionToReach = hit.point;

			currentObjectPressed = hit.collider.gameObject;

			//if the current surface pressed has rigidbody, use another raycast to find the proper surface below it
			if (currentObjectPressed.GetComponent<Rigidbody> () || applyDamage.isVehicle (currentObjectPressed)) {
				newRay.origin = hit.point + Vector3.up;
				newRay.direction = -Vector3.up;

				RaycastHit[] hits = Physics.RaycastAll (newRay, 100, layerToPress);
				System.Array.Sort (hits, (x, y) => x.distance.CompareTo (y.distance));

				bool surfaceFound = false;

				foreach (RaycastHit rh in hits) {
					if (!surfaceFound) {
						Rigidbody currenRigidbodyFound = applyDamage.applyForce (rh.collider.gameObject);

						if (currenRigidbodyFound == null && applyDamage.isVehicle (rh.collider.gameObject)) {
							positionToReach = rh.point + rh.normal * 0.1f;
							currentObjectPressed = rh.collider.gameObject;
							surfaceFound = true;
						}
					}
				}
			}

			//here check if the pressed surface is a navigable place or not, to use a raycast to find the closest position to move
			if ((1 << currentObjectPressed.layer & layerForGround.value) == 1 << currentObjectPressed.layer) {

				lastTimePressed = Time.time;
				previousElementToUse = null;
				useCustomElementMinDistance = false;

				if (playerControllerManager.isPlayerAiming ()) {
					print ("player is aiming, so he can't move");
					return;
				}

				if (playerIsUsingDevice) {
					print ("player is using a device, he can't move");
					return;
				}

				//print ("pressed surface");

				float surfaceAngle = Vector3.Angle (transform.up, hit.normal);

				if (searchCorrectSurfaceOnHighAngle) {
					if (surfaceAngle >= maxSurfaceAngle) {
						print ("search for correct position");
						if (Physics.Raycast (positionToReach * 0.4f, -Vector3.up, out hit, Mathf.Infinity, layerToFindGround)) {
							positionToReach = hit.point;
						}
					}
				} else {
					if (surfaceAngle >= maxSurfaceAngle) {
						print ("not valid surface");
						return;
					}
				}

				if (usePointAndClickPanel && disablePointAndClickPanelOnSurfacePressed) {
					pointAndClickPanel.SetActive (false);
				}

				setTargetType (false, false, true);

				if (!usingPlayerNavmeshExternally) {
					if (particlesInTransformToFollow != null && useParticlesToTargetPosition && canUseTargetParticles) {
						particlesInTransformToFollow.SetActive (true);
					}
				}
			} else if ((1 << currentObjectPressed.layer & layerForElements.value) == 1 << currentObjectPressed.layer) {
				print ("pressed element");

				bool canUseElement = checkPointAndClickElementPressed ();

				if (!canUseElement) {
					return;
				}
			}

			agent.enabled = true;

			currentNavMeshPath = new NavMeshPath ();
			pathCorners.Clear ();

			bool hasFoundPath = agent.CalculatePath (positionToReach, currentNavMeshPath);

			if (hasFoundPath) {
				targetSelected = true;
				transformToFollow.position = positionToReach;
				targetToReach = transformToFollow;
			} else {
				agent.enabled = false;
			}
		}
	}

	IEnumerator MoveAcrossNavMeshLink ()
	{
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		agent.updateRotation = false;

		Vector3 startPos = agent.transform.position;
		Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
		float duration = (endPos - startPos).magnitude / agent.velocity.magnitude;
		float t = 0.0f;
		float tStep = 1.0f / duration;

		while (t < 1.0f) {
			agent.destination = Vector3.Lerp (startPos, endPos, t);
			t += tStep * Time.deltaTime;
			yield return null;
		}

		agent.destination = endPos;
		agent.updateRotation = true;
		agent.CompleteOffMeshLink ();

		lookingPathAfterJump = false;
	}

	public bool checkPointAndClickElementPressed ()
	{
		//get the current point and click element info
		currentPointAndClickElement = currentObjectPressed.GetComponent<pointAndClickElement> ();

		if (currentPointAndClickElement != null) {
			//if the element is a device
			if (currentPointAndClickElement.isDevice ()) {
				//set the type of element found
				setTargetType (false, true, false);

				bool closeEnoughtToTarget = false;

				Transform positionForNavmesh = currentPointAndClickElement.getPositionForNavMesh (transform.position);

				if (positionForNavmesh == null) {
					print ("WARNING: navmesh position not configured in point and click element");

					return false;
				}

				Vector3 targetPosition = positionForNavmesh.position;

				float distanceToTarget = GKC_Utils.distance (new Vector3 (targetPosition.x, 0, targetPosition.z), new Vector3 (transform.position.x, 0, transform.position.z));

				//print (distanceToTarget);

				if (distanceToTarget > minDistanceToObjects) {
					//launch a raycast to get the move position to that device
					if (Physics.Raycast (targetPosition, -Vector3.up, out hit, Mathf.Infinity, layerToFindGround)) {
						positionToReach = hit.point;

						if (!usingPlayerNavmeshExternally) {
							if (particlesInTransformToFollow && useParticlesToTargetPosition && canUseTargetParticles) {
								particlesInTransformToFollow.SetActive (true);
							}
						}
					}
				} else {
					closeEnoughtToTarget = true;
				}

				//get the device object
				currentElementToUse = currentPointAndClickElement.getElementToUse ();

				if (previousElementToUse != currentElementToUse) {
					previousElementToUse = currentElementToUse;
					lastTimePressed = Time.time;
					//print ("new");
				}

				//set the info if the element contains any 
				if (currentPointAndClickElement.useElementTextInfo) {
					if (useElementInfoText) {
						elementInfoText.text = currentPointAndClickElement.getPointAndClickElementTextInfo ();
					}

					if (usePointAndClickPanel) {
						pointAndClickPanel.SetActive (true);
					}

					if (useElementNameText) {
						deviceStringAction currentDeviceStringAction = currentPointAndClickElement.getElementToUse ().GetComponentInChildren<deviceStringAction> ();

						if (currentDeviceStringAction != null) {
							elementNameText.text = currentDeviceStringAction.getDeviceName ();
						} else {
							if (currentPointAndClickElement.useCustomElementName) {
								elementNameText.text = currentPointAndClickElement.customElementName;
							} else {
								elementNameText.text = "";
							}
						}
					}
				} else {
					elementInfoText.text = "";
				}

				//if the player has reached the device, and the player press it again, enable the use devices buttons
				if (devicesManager.existInDeviceList (currentElementToUse)) {
					if (useDeviceButtons) {
						//print ("device: " + currentElementToUse.name);
						currentElementDeviceActionText = devicesManager.getCurrentDeviceActionText ();
						currentElementUseOnOffDeviceState = currentPointAndClickElement.useOnOffDeviceState;

						startUseDeviceButtonText.text = currentElementDeviceActionText;

						setStartAndStopUseDeviceButtonsState (true, false);

						if (usePointAndClickPanel) {
							pointAndClickPanel.SetActive (true);
						}

						return false;
					}
				} else {
					setStartAndStopUseDeviceButtonsState (false, false);
					//if the player is not close to a device, and he has to press two times in an object to use it, check the time between presses
					if (useDoubleClickToMoveTowardDevices) {
						if (Time.time == lastTimePressed) {
							//print ("primer pulsacion");
							return false;
						}

						if (previousElementToUse != currentElementToUse || Time.time > lastTimePressed + timeBetweenClicks) {
							//print ("two slow or different element");
							previousElementToUse = null;
							lastTimePressed = 0;

							return false;
						}
					}
				}

				useCustomElementMinDistance = currentPointAndClickElement.useCustomElementMinDistance;

				if (useCustomElementMinDistance) {
					customElementMinDistance = currentPointAndClickElement.customElementMinDistance;
				}

				if (!useCustomElementMinDistance && closeEnoughtToTarget) {
					return false;
				}
			} else if (currentPointAndClickElement.isFriend ()) {

				lastTimePressed = Time.time;
				previousElementToUse = null;
				useCustomElementMinDistance = false;

				setTargetType (true, false, false);
			} else if (currentPointAndClickElement.isEnemy ()) {

				lastTimePressed = Time.time;
				previousElementToUse = null;
				useCustomElementMinDistance = false;

				setTargetType (false, false, false);
			} 
		}

		return true;
	}

	//follow the enemy position, to rotate torwards his direction
	void lootAtTarget (Transform objective)
	{
		Debug.DrawRay (rayCastPosition.position, rayCastPosition.forward, Color.red);

		if (objective != null) {
			Vector3 targetDir = objective.position - rayCastPosition.position;
			Quaternion targetRotation = Quaternion.LookRotation (targetDir, transform.up);
			rayCastPosition.rotation = Quaternion.Slerp (rayCastPosition.rotation, targetRotation, lookAtTargetSpeed * Time.deltaTime);
		} else {
			Quaternion targetRotation = Quaternion.LookRotation (transform.forward, transform.up);
			rayCastPosition.rotation = Quaternion.Slerp (rayCastPosition.rotation, targetRotation, lookAtTargetSpeed * Time.deltaTime);
		}
	}

	public void setAgentDestination (Vector3 targetPosition)
	{
		if (GKC_Utils.distance (lastTargetPosition, targetPosition) > maxDistanceToRecalculatePath || Time.time > lastTimeUpdateNavMeshPosition + updateNavMeshPositionRate) {
			lastTargetPosition = targetPosition;
			agent.SetDestination (targetPosition);
			lastTimeUpdateNavMeshPosition = Time.time;
		}

		agent.transform.position = transform.position;
	}

	public bool updateCurrentNavMeshPath (Vector3 targetPosition)
	{
		bool hasFoundPath = true;

		if (GKC_Utils.distance (lastPathTargetPosition, targetPosition) > maxDistanceToRecalculatePath || pathCorners.Count == 0 || Time.time > calculatePathRate + lastTimeCalculatePath) {
			lastPathTargetPosition = targetPosition;

			setAgentDestination (lastPathTargetPosition);

			currentNavMeshPath = new NavMeshPath ();

			pathCorners.Clear ();

			hasFoundPath = agent.CalculatePath (lastPathTargetPosition, currentNavMeshPath);

			pathCorners.AddRange (currentNavMeshPath.corners);

			lastTimeCalculatePath = Time.time;
		}

		return hasFoundPath;
	}

	public void setOnGroundState (bool state)
	{
		onGround = state;
	}

	public Vector3 getCurrentMovementDirection ()
	{
		return currentMovementDirection;
	}

	public void moveNavMesh (Vector3 move, bool crouch, bool jump)
	{
		if (useDynamicObstacleDetection && move != Vector3.zero) {
			currentUp = transform.up;
			currentForward = transform.forward;

			if (navMeshPaused) {
				move = Vector3.zero;
			}

			if (move != Vector3.zero) {

				Vector3 currentRaycastPosition = transform.position + currentUp;

				if (!obstacleDetected) {
					if (Physics.Raycast (currentRaycastPosition, currentForward, out rayhit, dynamicRaycastDistanceForward, dynamicObstacleLayer)) {
						obstacleDetected = true;

						avoidDirection = move.normalized;

						currentCheckObstalceDirection = Quaternion.Euler (currentUp * 60) * currentForward;

						if (Physics.Raycast (currentRaycastPosition, currentCheckObstalceDirection, out rayhit, dynamicRaycastDistanceForward, dynamicObstacleLayer)) {

							moveToRight = false;
						}
					} else {
						currentCheckObstalceDirection = Quaternion.Euler (currentUp * 60) * currentForward;
						if (Physics.Raycast (currentRaycastPosition, currentCheckObstalceDirection, out rayhit, dynamicRaycastDistanceDiagonal, dynamicObstacleLayer)) {

							obstacleDetected = true;

							avoidDirection = move.normalized;

							moveToRight = false;
						} else {
							currentCheckObstalceDirection = Quaternion.Euler (-currentUp * 60) * currentForward;
							if (Physics.Raycast (currentRaycastPosition, currentCheckObstalceDirection, out rayhit, dynamicRaycastDistanceDiagonal, dynamicObstacleLayer)) {

								obstacleDetected = true;

								avoidDirection = move.normalized;
							} else {
								currentCheckObstalceDirection = Quaternion.Euler (currentUp * 90) * currentForward;
								if (Physics.Raycast (currentRaycastPosition, currentCheckObstalceDirection, out rayhit, dynamicRaycastDistanceSides, dynamicObstacleLayer)) {

									obstacleDetected = true;

									avoidDirection = move.normalized;

									moveToRight = false;
								} else {
									currentCheckObstalceDirection = Quaternion.Euler (-currentUp * 90) * currentForward;
									if (Physics.Raycast (currentRaycastPosition, currentCheckObstalceDirection, out rayhit, dynamicRaycastDistanceSides, dynamicObstacleLayer)) {

										obstacleDetected = true;

										avoidDirection = move.normalized;
									}
								}
							}
						}
					}
				}

				if (obstacleDetected) {
					obstacleOnForward = false;
					obstacleOnRight = false;
					obstacleOnLeft = false;
					obstacleOnRightForward = false;
					obstacleOnLeftForward = false;

					numberOfObstacleDirectionDetected = 0;

					obstacleDistance = 3;

					bool newObstacleFound = false;

					if (pathCorners.Count > 0) {
						currentForward = (pathCorners [pathCorners.Count - 1] - transform.position).normalized;
					} else {
						currentForward = move.normalized;
					}

					Vector3 rayDirection = currentForward;

					if (Physics.Raycast (currentRaycastPosition, rayDirection, out rayhit, dynamicRaycastDistanceForward, dynamicObstacleLayer)) {

						obstacleDistance = rayhit.distance;

						newObstacleFound = true;

						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.red, 2);

						obstacleOnForward = true;

						numberOfObstacleDirectionDetected++;
					} else {
						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.green, 2);
					}

					rayDirection = Quaternion.Euler (currentUp * 45) * currentForward;

					if (Physics.Raycast (currentRaycastPosition, rayDirection, out rayhit, dynamicRaycastDistanceAround, dynamicObstacleLayer)) {

						newObstacleFound = true;

						if (rayhit.distance < obstacleDistance) {
							obstacleDistance = rayhit.distance;
						}

						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.red, 2);

						obstacleOnRightForward = true;

						numberOfObstacleDirectionDetected++;
					} else {
						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.green, 2);
					}

					rayDirection = Quaternion.Euler (-currentUp * 45) * currentForward;

					if (Physics.Raycast (currentRaycastPosition, rayDirection, out rayhit, dynamicRaycastDistanceAround, dynamicObstacleLayer)) {

						newObstacleFound = true;

						if (rayhit.distance < obstacleDistance) {
							obstacleDistance = rayhit.distance;
						}

						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.red, 2);

						obstacleOnLeftForward = true;

						numberOfObstacleDirectionDetected++;
					} else {
						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.green, 2);
					}

					rayDirection = Quaternion.Euler (currentUp * 90) * currentForward;

					if (Physics.Raycast (currentRaycastPosition, rayDirection, out rayhit, dynamicRaycastDistanceAround, dynamicObstacleLayer)) {

						newObstacleFound = true;

						if (rayhit.distance < obstacleDistance) {
							obstacleDistance = rayhit.distance;
						}

						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.red, 2);

						obstacleOnRight = true;

						numberOfObstacleDirectionDetected++;
					} else {
						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.green, 2);
					}

					rayDirection = Quaternion.Euler (-currentUp * 90) * currentForward;

					if (Physics.Raycast (currentRaycastPosition, rayDirection, out rayhit, dynamicRaycastDistanceAround, dynamicObstacleLayer)) {

						newObstacleFound = true;

						if (rayhit.distance < obstacleDistance) {
							obstacleDistance = rayhit.distance;
						}

						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.red, 2);

						obstacleOnLeft = true;

						numberOfObstacleDirectionDetected++;
					} else {
						Debug.DrawRay (currentRaycastPosition, rayDirection, Color.green, 2);
					}

					if (obstacleOnLeft || obstacleOnLeftForward || obstacleOnForward) {
						moveToRight = true;
					} else {
						moveToRight = false;
					}

					if (!newObstacleFound) {
						obstacleDetected = false;

					} else {
						float avoidAngle = 30;
						if (obstacleDistance < 3) {
							avoidAngle = 45;
						} 

						if (obstacleDistance < 2) {
							avoidAngle = 65;
						} 

						if (obstacleDistance < 1) {
							avoidAngle = 85;
						}

						if (!moveToRight) {
							avoidAngle = -avoidAngle;
						}

						Vector3 newDirection = Quaternion.Euler (currentUp * avoidAngle) * avoidDirection;
						newDirection = newDirection.normalized;
						newMoveDirection = newDirection;
					}
				} else {
					newMoveDirection = move;
				}
			} else {
				newMoveDirection = move;
			}

			newMoveDirection.Normalize ();

			currentMoveDirection = Vector3.Lerp (currentMoveDirection, newMoveDirection, Time.deltaTime * dynamicAvoidSpeed);

			move = currentMoveDirection;
		} else {

			if (navMeshPaused) {
				move = Vector3.zero;
			}
		}

		if (move.magnitude < 0.01f) {
			move = Vector3.zero;
		}

		currentMovementDirection = move;

		AIMoveInput.moveInput = move;
		AIMoveInput.crouchInput = crouch;
		AIMoveInput.jumpInput = jump;

		if (debugMoveEnabled) {
			playerControllerManager.Move (AIMoveInput);
		}

		playerCameraManager.Rotate (rayCastPosition.forward);
	}

	public void pauseAI (bool state)
	{
		navMeshPaused = state;

		if (navMeshPaused) {
			if (agent.enabled) {
				agent.isStopped = true;
				agent.enabled = false;
			}
		} else {
			if (!agent.enabled) {
				agent.enabled = true;

				if (waitToResumeCoroutine != null) {
					StopCoroutine (waitToResumeCoroutine);
				}

				waitToResumeCoroutine = StartCoroutine (resumeCoroutine ());
			}
		}

		if (showPathRenderer) {
			lineRenderer.enabled = !state;
		}
	}

	IEnumerator resumeCoroutine ()
	{
		yield return new WaitForSeconds (0.0001f);

		agent.isStopped = false;

		checkingToResumeNavmesh = true;
	}

	public void recalculatePath ()
	{
		if (agent.enabled) {
			agent.isStopped = false;
		}
	}

	public void jumpStart ()
	{

	}

	public void jumpEnded ()
	{
		agent.enabled = true;

		agent.CompleteOffMeshLink ();

		//Resume normal navmesh behaviour
		agent.isStopped = false;

		lookingPathAfterJump = false;

		navMeshPaused = false;
	}

	public void setTarget (Transform currentTarget)
	{
		targetToReach = currentTarget;
	}

	public void removeTarget ()
	{
		targetToReach = null;
		followingTarget = false;
		targetSelected = false;
		targetToReach = null;
		setTargetType (false, false, false);

		if (showPathRenderer) {
			lineRenderer.enabled = false;
		}

		if (particlesInTransformToFollow != null) {
			particlesInTransformToFollow.SetActive (false);
		}

		canUseTargetParticles = true;
	}

	public void setCanUseTargetParticlesState (bool state)
	{
		canUseTargetParticles = state;
	}

	public void lookAtTaget (bool state)
	{
		//make the character to look or not to look towars its target, pointing the camera towards it
		AIMoveInput.lookAtTarget = state;
	}

	public bool isCharacterAttacking ()
	{
		return attacking;
	}

	public bool isCharacterFollowing ()
	{
		return following;
	}

	public bool isCharacterWaiting ()
	{
		return waiting;
	}

	public bool isCharacterHiding ()
	{
		return hiding;
	}

	public void setCharacterStates (bool attackValue, bool followValue, bool waitValue, bool hideValue)
	{
		attacking = attackValue;
		following = followValue;
		waiting = waitValue;
		hiding = hideValue;
	}

	public void setTargetType (bool isFriendly, bool isObject, bool isSurface)
	{
		targetIsFriendly = isFriendly;
		targetIsObject = isObject;
		targetIsSurface = isSurface;

		if (targetIsFriendly) {
			setCharacterStates (false, false, false, false);
		}
	}

	public bool isFollowingTarget ()
	{
		return followingTarget;
	}

	public void setShowCursorPausedState (bool state)
	{
		showCursorPaused = state;
	}

	bool showCursorPaused;

	public void setPlayerNavMeshEnabledState (bool state)
	{
		playerNavMeshEnabled = state;

		agent.enabled = playerNavMeshEnabled;

		playerControllerManager.setPlayerNavMeshEnabledState (playerNavMeshEnabled);
		playerCameraManager.setPlayerNavMeshEnabledState (playerNavMeshEnabled);

		pauseManager.usingPointAndClickState (playerNavMeshEnabled);

		if (!showCursorPaused) {
			pauseManager.showOrHideCursor (playerNavMeshEnabled);
			pauseManager.changeCursorState (!playerNavMeshEnabled);

			pauseManager.showOrHideMouseCursorController (playerNavMeshEnabled);
		}

		if (!playerNavMeshEnabled) {
			removeTarget ();
		}

		if (transformToFollow != null) {
			transformToFollow.gameObject.SetActive (playerNavMeshEnabled);
		}
		//print ("point change");
	}

	public void startToUseDevice ()
	{
		if (currentElementToUse != null) {
			devicesManager.useCurrentDevice (currentElementToUse);
		}

		if (currentElementUseOnOffDeviceState) {
			setStartAndStopUseDeviceButtonsState (false, true);
		}

		if (currentElementUseOnOffDeviceState) {
			stopToUseDeviceButtonText.text = "Cancel";
		} else {
			devicesManager.checkDeviceName ();

			stopToUseDeviceButtonText.text = devicesManager.getCurrentDeviceActionText ();
		}

		if (currentPointAndClickElement) {

			if (currentPointAndClickElement.disablePanelAfterUse) {
				if (usePointAndClickPanel) {
					pointAndClickPanel.SetActive (false);
				}
			}

			if (currentPointAndClickElement.checkIfRemove ()) {
				setStartAndStopUseDeviceButtonsState (false, false);

				if (usePointAndClickPanel) {
					pointAndClickPanel.SetActive (false);
				}

				currentPointAndClickElement.removeElement ();
			}
		}
	}

	public void stopToUseDevice ()
	{
		if (devicesManager.hasDeviceToUse ()) {
			devicesManager.useDevice ();		
		}	

		setStartAndStopUseDeviceButtonsState (true, false);
	}

	public void disablePanelInfo ()
	{
		if (usePointAndClickPanel) {
			pointAndClickPanel.SetActive (false);
		}

		setStartAndStopUseDeviceButtonsState (false, false);
	}

	public void setStartAndStopUseDeviceButtonsState (bool startState, bool stopState)
	{
		startUseDeviceButton.SetActive (startState);

		stopToUseDeviceButton.SetActive (stopState);
	}

	void OnTriggerEnter (Collider col)
	{
		checkTriggerInfo (col, true);
	}

	void OnTriggerExit (Collider col)
	{
		checkTriggerInfo (col, false);
	}

	public void checkTriggerInfo (Collider col, bool isEnter)
	{
		if ((1 << col.gameObject.layer & layerForElements.value) == 1 << col.gameObject.layer) {
			if (isEnter) {
				insideADeviceElement = true;
				col.GetComponent<pointAndClickElement> ().setCurrentPlayerNavMeshSystem (this);
			} else {
				insideADeviceElement = false;
			}
		}
	}

	public void setPlayerTargetPosition (Vector3 targetPosition)
	{
		positionToReach = targetPosition;

		lastTimePressed = Time.time;
		previousElementToUse = null;
		useCustomElementMinDistance = false;

		setTargetType (false, false, true);

		agent.enabled = true;

		currentNavMeshPath = new NavMeshPath ();

		pathCorners.Clear ();

		bool hasFoundPath = agent.CalculatePath (positionToReach, currentNavMeshPath);

		if (hasFoundPath) {
			targetSelected = true;
			transformToFollow.position = positionToReach;
			targetToReach = transformToFollow;
		} else {
			agent.enabled = false;
		}
	}

	public void enablePlayerNavmeshWithoutPointAndClick (bool state)
	{
		playerNavMeshEnabled = state;

		agent.enabled = playerNavMeshEnabled;

		playerControllerManager.setPlayerNavMeshEnabledState (playerNavMeshEnabled);
		playerCameraManager.setPlayerNavMeshEnabledState (playerNavMeshEnabled);

		if (!playerNavMeshEnabled) {
			removeTarget ();
		}
	}

	public void setUsingPlayerNavmeshExternallyState (bool state)
	{
		usingPlayerNavmeshExternally = state;
	}

	public void setUseDynamicObstacleDetectionState (bool state)
	{
		useDynamicObstacleDetection = state;
	}

	public void setOriginalUseDynamicObstacleDetection ()
	{
		setUseDynamicObstacleDetectionState (originalUseDynamicObstacleDetection);
	}

	public bool isUsingPlayerNavmeshExternallyActive ()
	{
		return usingPlayerNavmeshExternally;
	}

	public bool isPlayerNavMeshEnabled ()
	{
		return playerNavMeshEnabled;
	}

	public void enableCustomNavMeshSpeed (float newValue)
	{
		customNavMeshSpeed = newValue;

		useCustomNavMeshSpeed = true;
	}

	public void disableCustomNavMeshSpeed ()
	{
		useCustomNavMeshSpeed = false;
	}

	public void enableCustomMinDistance (float newValue)
	{
		useCustomMinDistance = true;

		customMinDistance = newValue;
	}

	public void disableCustomMinDistance ()
	{
		useCustomMinDistance = false;
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

	//draw the pivot and the final positions of every door
	void DrawGizmos ()
	{
		if (showGizmo) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere (lastPathTargetPosition, 1);
		}
	}
}