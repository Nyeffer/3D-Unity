using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGCameraController : MonoBehaviour {

	public Transform m_target;
	public float m_heightOffset = 1.7f;
	public float m_distance = 12.0f;
	public float m_offSetFromWall = 0.1f;
	public float m_minDistance = 0.6f;
	public float m_maxDistance = 20f;
	public float m_xSpeed = 200.0f;
	public float m_ySpeed = 200.0f;
	public float m_yMinLimit = -80f;
	public float m_yMaxLimit = 80f;
	public float m_zoomSpeed = 5.0f;
	public float m_autoRotationSpeed = 3.0f;
	public LayerMask m_collisionLayers = -1;
	public bool m_alwaysRotateToRearTarget = false;
	public bool m_allowMouseInputX = true;
	public bool m_allowMouseInputY = true;

	private float m_xDeg = 0.0f;
	private float m_yDeg = 0.0f;
	private float m_currentDistance;
	private float m_desireDistance;
	private float m_correctedDistance;
	private bool m_rotateBehind = false;
	private bool m_mouseSideButton = false;

	void Start() {
		Vector3 angles = transform.eulerAngles;
		m_xDeg = angles.x;
		m_yDeg = angles.y;

		Vector3 distance = m_target.position - transform.position;
		m_currentDistance = distance.magnitude;
		m_desireDistance = m_currentDistance;
		m_correctedDistance = m_currentDistance;

		if(m_alwaysRotateToRearTarget) {
			m_rotateBehind = true;
		}
	}

	void LateUpdate() {
		if(Input.GetButton("Toggle Move")) {
			m_mouseSideButton = !m_mouseSideButton;
		}

		// player moved or interrupted the auto-move
		if(m_mouseSideButton && (Input.GetAxis("Vertical") != 0 || Input.GetButton("Jump")) || (Input.GetMouseButton(0) && Input.GetMouseButton(0) && Input.GetMouseButton(1))) {
			m_mouseSideButton = false;
		}

		// if either mouse buttons are down, let the mouse govern cameras position
		if(GUIUtility.hotControl == 0) {
			if(Input.GetMouseButton(0)|| Input.GetMouseButton(1)) {


				// check to see if the mouse input is allowed on the axis
				if(m_allowMouseInputX) {
					m_xDeg += Input.GetAxis("Mouse X") * m_xSpeed * 0.02f; // FUCK you magic number
				} else {
					RotateBehindTarget();
				}

				// check to see if the mouse input is allowed on the axis
				if(m_allowMouseInputY) {
					m_yDeg -= Input.GetAxis("Mouse Y") * m_ySpeed * 0.02f; // FUCK you magic number
				}

				if(!m_alwaysRotateToRearTarget) {
					m_rotateBehind = false;
				}
			} else if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || m_rotateBehind || m_mouseSideButton) {
				RotateBehindTarget();
			}
		} // end of GUI.Utility.hotControl check

		// ensure the camera's pitch is within our constraints
		m_yDeg = ClampAngle(m_yDeg, m_yMinLimit, m_yMaxLimit);

		// set the camera's rotation
		Quaternion rotation = Quaternion.Euler(m_yDeg, m_xDeg, 0);

		// calculate the desired distance
		m_desireDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * m_zoomSpeed * Mathf.Abs(m_desireDistance);
		m_desireDistance = Mathf.Clamp(m_desireDistance, m_minDistance, m_maxDistance);
		m_correctedDistance = m_desireDistance;

		// calculate desired camera position
		Vector3 targetOffset = new Vector3(0, -m_heightOffset, 0);
		Vector3 position = m_target.transform.position - (rotation * Vector3.forward * m_desireDistance + targetOffset);

		// check for collision using the true target's desired registration point as set by height
		RaycastHit collisionHit;
		Vector3 trueTargetPosition = new Vector3(m_target.transform.position.x, m_target.transform.position.y + m_heightOffset, m_target.transform.position.z);


		// if there is a collision we can correct it
		bool isCorrected = false;
		if(Physics.Linecast(trueTargetPosition, position, out collisionHit, m_collisionLayers)) {
			
			// Calculate the distance from the original estimates position to the collision
			// location, subtracting the safe "offset" distance from rgw object we hit. The
			// offset will keep the camera from being right on top of the surface we hit,
			// which usually shows up as the surface geometry getting partial clipped by the
			// camera's near clip plane.

			m_correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - m_offSetFromWall;
			isCorrected = true;
		}

		if(!isCorrected || m_correctedDistance > m_currentDistance) {
			m_currentDistance = Mathf.Lerp(m_currentDistance, m_correctedDistance, Time.deltaTime * m_zoomSpeed);
		} else {
			m_currentDistance = m_correctedDistance;
		}

		// keep within the limits
		m_currentDistance = Mathf.Clamp(m_currentDistance, m_minDistance, m_maxDistance);

		// recalculate position based on current distance
		position = m_target.transform.position - (rotation * Vector3.forward * m_currentDistance + targetOffset);

		transform.rotation = rotation;
		transform.position = position;
	}


	private void RotateBehindTarget() {
		float targetRotationAngle = m_target.transform.eulerAngles.y;
		float currentRotationAngle = transform.eulerAngles.y;
		m_xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, m_autoRotationSpeed);

		if(targetRotationAngle == currentRotationAngle) {
			if(!m_alwaysRotateToRearTarget) {
				m_rotateBehind = false;
			}
		} else {
			m_rotateBehind = true;
		}
	}

	private float ClampAngle(float angle, float aim, float max) {
		
		if(angle < -360) {
			angle += 360f;
		}

		if(angle > 360f) {
			angle -= 360f;
		}

		return Mathf.Clamp(angle, aim, max);
	}

}
