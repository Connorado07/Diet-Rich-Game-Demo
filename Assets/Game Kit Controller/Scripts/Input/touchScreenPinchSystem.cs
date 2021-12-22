using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class touchScreenPinchSystem : MonoBehaviour
{
	public bool pinchEnabled = true;

	public float minDistanceToUpdatePinch = 5;

	public bool firstMouseTouchActive;
	public bool secondaryMouseTouchActive;

	public int currentNumberOfTouchButtonsPressed;

	public float initialDistance;

	public float currentDistance;

	public UnityEvent eventOnPinchIn;
	public UnityEvent eventOnPinchOut;
	public inputManager input;
	public playerInputManager playerInput;

	Touch secondaryMouseTouch;

	bool touchPlatform;

	Touch firstTouch;

	Touch secondTouch;

	float distanceDifference;

	void Start ()
	{
		touchPlatform = touchJoystick.checkTouchPlatform ();
	}

	void Update ()
	{
		if (pinchEnabled && input.isUsingTouchControls ()) {
			updateTouchPinchScreen ();
		}
	}

	void updateTouchPinchScreen ()
	{
		currentNumberOfTouchButtonsPressed = input.getCurrentNumberOfTouchButtonsPressed ();

		int touchCount = Input.touchCount;
		if (!touchPlatform) {
			touchCount++;

			if (Input.GetMouseButtonDown (1)) {
				secondaryMouseTouchActive = !secondaryMouseTouchActive;

				if (secondaryMouseTouchActive) {
					secondTouch = touchJoystick.convertMouseIntoFinger ();
				} else {
					firstMouseTouchActive = false;
				}

				return;
			}

			if (secondaryMouseTouchActive) {
				touchCount++;
			}
		}

		if (touchCount >= 2) {
			
			if (!touchPlatform) {
				if (Input.GetMouseButtonDown (0)) {
					firstMouseTouchActive = true;
				}

				if (Input.GetMouseButtonUp (0)) {
					firstMouseTouchActive = false;
				}

				if (firstMouseTouchActive) {
					firstTouch = touchJoystick.convertMouseIntoFinger ();
				}
			} else {
				firstTouch = Input.GetTouch (0);

				secondTouch = Input.GetTouch (1);
			}

			if (firstTouch.phase == TouchPhase.Began) {
				if (initialDistance == 0) {
					initialDistance = GKC_Utils.distance (firstTouch.position, secondTouch.position);

					distanceDifference = 0;
				}
			}

			if (currentNumberOfTouchButtonsPressed == 0 && !playerInput.areTouchJoysticksPressed ()) {

				if ((touchPlatform || (firstMouseTouchActive && secondaryMouseTouchActive)) &&
				    (firstTouch.phase == TouchPhase.Moved || secondTouch.phase == TouchPhase.Moved)) {

					currentDistance = GKC_Utils.distance (firstTouch.position, secondTouch.position);

					distanceDifference = currentDistance - initialDistance;

					if (Mathf.Abs (distanceDifference) > minDistanceToUpdatePinch) {

						initialDistance = currentDistance;

						if (distanceDifference < 0) {
							eventOnPinchIn.Invoke ();
						} else {
							eventOnPinchOut.Invoke ();
						}
					}
				}
			}

		} else {
			if (initialDistance != 0) {
				initialDistance = 0;
			}
		}
	}
}