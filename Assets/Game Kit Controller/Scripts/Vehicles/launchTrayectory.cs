using UnityEngine;
using System.Collections;

public class launchTrayectory : MonoBehaviour
{
	[Header ("Main Settings")]
	[Space]

	public LayerMask layer;

	public float width;
	public float numberOfPoints;
	public float animationSpeed;
	public float tillingOffset;
	public bool animateTexture;
	public Color textureColor;

	public bool useMaxDistanceWhenNoSurfaceFound;
	public float maxDistanceWhenNoSurfaceFound;

	public bool raycastCheckingEnabled = true;

	public bool checkIfLockedCameraActive;

	[Space]
	[Header ("Debug")]
	[Space]

	public bool parableEnabled = false;

	[Space]
	[Header ("Components")]
	[Space]

	public playerCamera mainPlayerCamera;
	public Transform shootPosition;
	public GameObject character;

	public Transform mainCameraTransform;
	public LineRenderer lineRenderer;

	public bool showGizmo;

	float currentAnimationOffset;
	Vector3 rayPoint;
	RaycastHit hit;
	bool rayColliding;

	float hitDistance;

	Vector3 startPosition;
	Vector3 endPosition;

	Vector3 currentRendererPosition;

	float currentRaycastDistance;

	Vector3 raycastDirection;
	Vector3 raycastOrigin;

	Ray newRay;

	void Start ()
	{
		changeParableState (false);
	}

	void Update ()
	{
		//if the player is using the barrel launcher
		if (parableEnabled) {
			//get the start position of the parable
			startPosition = shootPosition.position;

			if (raycastCheckingEnabled) {
				currentRaycastDistance = Mathf.Infinity;
			} else {
				currentRaycastDistance = maxDistanceWhenNoSurfaceFound;
			}

			raycastDirection = mainCameraTransform.TransformDirection (Vector3.forward);
			raycastOrigin = mainCameraTransform.position;

			if (checkIfLockedCameraActive) {
				if (mainPlayerCamera != null) {
					if (!mainPlayerCamera.isCameraTypeFree ()) {
						newRay = mainPlayerCamera.getCameraRaycastDirection ();
						raycastDirection = newRay.direction;
						raycastOrigin = newRay.origin;
					}
				}
			}

			//check where the camera is looking and 
			if (Physics.Raycast (raycastOrigin, raycastDirection, out hit, currentRaycastDistance, layer)) {
				//enable the linerender
				hitDistance = hit.distance;
				rayPoint = hit.point;

				rayColliding = true;

				if (!lineRenderer.enabled) {
					lineRenderer.enabled = true;
				}
			} else {
				if (useMaxDistanceWhenNoSurfaceFound) {
					
					hitDistance = maxDistanceWhenNoSurfaceFound;
					rayPoint = raycastOrigin + raycastDirection * maxDistanceWhenNoSurfaceFound;

					rayColliding = true;

					if (!lineRenderer.enabled) {
						lineRenderer.enabled = true;
					}
				} else {
					//disable it
					rayColliding = false;

					if (lineRenderer.enabled) {
						lineRenderer.enabled = false;
					}
				}
			}

			if (rayColliding) {
				//if the ray detects a surface, set the linerenderer positions and animated it
				endPosition = rayPoint;
				lineRenderer.positionCount = (int)numberOfPoints + 1;

				//get every linerendere position according to the number of points
				for (float i = 0; i < numberOfPoints + 1; i++) {
					currentRendererPosition = getParablePoint (startPosition, endPosition, i / numberOfPoints);

					lineRenderer.SetPosition ((int)i, currentRendererPosition);
				}

				//animate the texture of the line renderer by changing its offset texture
				lineRenderer.startWidth = width;
				lineRenderer.endWidth = width;

				int propertyNameID = Shader.PropertyToID ("_Color");

				if (animateTexture) {
					currentAnimationOffset -= animationSpeed * Time.deltaTime * hitDistance * 0.05f;
					lineRenderer.material.mainTextureScale = new Vector2 (tillingOffset * hitDistance * 0.2f, 1);
					lineRenderer.material.mainTextureOffset = new Vector2 (currentAnimationOffset, lineRenderer.material.mainTextureOffset.y);

					if (lineRenderer.material.HasProperty (propertyNameID)) {
						lineRenderer.material.color = textureColor;
					}
				}
			}
		}
	}

	public void changeParableState (bool state)
	{
		//enable or disable the barrel launcher parable
		parableEnabled = state;

		if (lineRenderer!= null) {
			if (parableEnabled) {
				lineRenderer.enabled = true;
			} else {
				lineRenderer.enabled = false;
			}
		}
	}

	Vector3 getParablePoint (Vector3 start, Vector3 end, float t)
	{
		//set the height of the parable according to the final position 
		float value = GKC_Utils.distance (start, end) / 65;
		float v0y = Physics.gravity.magnitude * value;
		float height = v0y;

		//translate to local position, to work correctly with the gravity control in the character
		float heightY = Mathf.Abs (transform.InverseTransformDirection (start).y - transform.InverseTransformDirection (end).y);
		if (heightY < 0.1f) {
			//start and end are roughly level
			Vector3 travelDirection = end - start;
			Vector3 result = start + t * travelDirection;
			result += Mathf.Sin (t * Mathf.PI) * height * character.transform.up;

			return result;
		} else {
			//start and end are not level
			Vector3 travelDirection = end - start;
			Vector3 startNew = start - transform.InverseTransformDirection (start).y * character.transform.up;
			startNew += transform.InverseTransformDirection (end).y * character.transform.up;

			Vector3 levelDirection = end - startNew;
			Vector3 right = Vector3.Cross (travelDirection, levelDirection);
			Vector3 up = Vector3.Cross (right, levelDirection);
			if (transform.InverseTransformDirection (end).y > transform.InverseTransformDirection (start).y) {
				up = -up;
			}

			Vector3 result = start + t * travelDirection;
			result += (Mathf.Sin (t * Mathf.PI) * height) * up.normalized;

			return result;
		}
	}

	public void setMainCameraTransform (Transform newCameraTransform)
	{
		mainCameraTransform = newCameraTransform;
	}

	void OnDrawGizmos ()
	{
		//draw the parable in the editor
		if (showGizmo && Application.isPlaying) {
			GUI.skin.box.fontSize = 16;
			Gizmos.color = Color.red;
			Gizmos.DrawLine (startPosition, endPosition);
			Vector3 lastP = startPosition;

			for (float i = 0; i < numberOfPoints + 1; i++) {
				Vector3 p = getParablePoint (startPosition, endPosition, i / numberOfPoints);
				Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
				Gizmos.DrawLine (lastP, p);
				lastP = p;
			}
		}
	}
}