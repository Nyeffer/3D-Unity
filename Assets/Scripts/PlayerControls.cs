using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {

	public string m_moveStatus = "Idle";
	public bool m_walkByDefault = true;
	public float m_gravity = 20.0f;
	
	// Movement Speeds
	public float m_jumpSpeed;
	public float m_sprintSpeed;
	public float m_runSpeed;
	public float m_turnSpeed;
	public float m_moveBackwardsMultiplier;
	public GameManager m_gm;

	// Internal Variables
	private float m_speedMultiplier = 0.0f;
	private bool m_grounded = false;
	private Vector3 m_moveDirection = Vector3.zero;
	private bool m_isRunning = false;
	private bool m_jumping = false;
	private bool m_mouseSideDown = false;
	private CharacterController m_controller;
	private Animator m_animationControl;
	

	// Stamina
	public bool m_isSprinting;
	public float m_currentStamina;
	public float m_maxStamina;
	public Slider staminaBar;

	void Awake() {
		// get the controllers
		m_controller = GetComponent<CharacterController>();
		m_animationControl = GetComponent<Animator>();
		
	}

	void Start() {
		m_currentStamina = m_maxStamina;
	}

	void Update() {
		m_moveStatus = "idle";
		m_isRunning = m_walkByDefault;

//--------------------------------------------------------------------------------//

		if(Input.GetAxis("Run") != 0 && Input.GetAxis("Horizontal") != 0) {
			m_isRunning = !m_walkByDefault;
			m_isSprinting = true;
			m_animationControl.SetBool("isSprinting", true);
			staminaBar.value = m_currentStamina/m_maxStamina;
		} else if(Input.GetAxis("Run") != 0 && Input.GetAxis("Vertical") != 0) {
			m_isRunning = !m_walkByDefault;
			m_isSprinting = true;
			m_animationControl.SetBool("isSprinting", true);
			staminaBar.value = m_currentStamina/m_maxStamina;
		} else {
			m_isSprinting = false;
			m_animationControl.SetBool("isSprinting", false);
			staminaBar.value = m_currentStamina/m_maxStamina;
		}

		if(m_isSprinting) {
			m_currentStamina -= Time.deltaTime;
			if(m_currentStamina < 0) {
				m_currentStamina = 0;
				m_isRunning = m_walkByDefault;
				m_animationControl.SetBool("isSprinting", false);
			}
		} else if(m_currentStamina < m_maxStamina) {
			m_currentStamina += Time.deltaTime;
		}

		
//--------------------------------------------------------------------------------//
		




		if(m_grounded) {
			// if the player is steering with the right mouse button ... A/D strafe
			if(Input.GetMouseButton(1)) {
				m_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			} else {
				m_moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
			}

			// auto-move button pressed
			if(Input.GetButtonDown("Toggle Move")) {
				m_mouseSideDown = !m_mouseSideDown;
			}

			// player moved or otherwise interrupted auto-move
			if(m_mouseSideDown && (Input.GetAxis("Vertical") != 0 || Input.GetButton("Jump")) || (Input.GetMouseButton(0) && Input.GetMouseButton(1))) {
				m_mouseSideDown = false;
			}

			// L+R MouseButton Movement
			if((Input.GetMouseButton(0) && Input.GetMouseButton(1)) || m_mouseSideDown) {
				m_moveDirection.z = 1;
			}

			if(((Input.GetMouseButtonDown(1) && Input.GetAxis("Horizontal") !=0) && Input.GetAxis("Vertical") != 0)) {
				m_moveDirection *= 0.707f; // TODO: Fuck you magic numbers
			}

			// apply the move backwards multiplier if not moving forwards only
			if((Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0) || Input.GetAxis("Vertical") < 0) {
				m_speedMultiplier = m_moveBackwardsMultiplier;
			} else {
				m_speedMultiplier = 1f;
			}

			// use the run or the walkspeed
			m_moveDirection *= m_isRunning ? m_runSpeed * m_speedMultiplier : m_sprintSpeed * m_speedMultiplier;

			// Jump
			if(Input.GetButtonDown("Jump")) {
				m_jumping = true;
				m_moveDirection.y = m_jumpSpeed;
				m_animationControl.SetBool("isJumping", true);
			} else {
				m_animationControl.SetBool("isJumping", false);
			}

			// tell the animator whats going on
			if(m_moveDirection.magnitude > 0.05f) { // TODO: Fuck you magic numbers
				m_animationControl.SetBool("isRunning", true);
			} else {
				m_animationControl.SetBool("isRunning", false);
			}

			m_animationControl.SetFloat("Speed", m_moveDirection.z);
			m_animationControl.SetFloat("Direction", m_moveDirection.x);
			// transform direction
			m_moveDirection = transform.TransformDirection(m_moveDirection);
		} // end if grounded 


		// Character must face the same direction as the camera when the right mouse button is down
		if(Input.GetMouseButton(1)) {
			transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
		} else {
			transform.Rotate(0, Input.GetAxis("Horizontal") * m_turnSpeed * Time.deltaTime, 0);
		}

		// apply gravity
		m_moveDirection.y -= m_gravity * Time.deltaTime;

		// move charactercontroller and check if grounded
		m_grounded = ((m_controller.Move(m_moveDirection * Time.deltaTime)) & CollisionFlags.Below) != 0;

		m_jumping = m_grounded ? false : m_jumping;

		if(m_jumping) {
			m_moveStatus = "jump";
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Goal") {
			m_gm.WinOver();
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Car") {
			m_animationControl.SetBool("isHit", true);
			m_jumpSpeed = 0;
			m_sprintSpeed = 0;
			m_runSpeed = 0;
			m_turnSpeed = 0;
			m_moveBackwardsMultiplier = 0;
			Debug.Log("BANG!");
			StartCoroutine(GameOverDelay());
			
		}

		if(other.gameObject.tag == "Obstacle") {
			m_animationControl.SetTrigger("isStumbled");
			StartCoroutine(SlowDown());
		} 
	}

	IEnumerator GameOverDelay() {
		yield return new WaitForSeconds(3);
		m_gm.GameOver();
	}

	IEnumerator SlowDown() {
		m_jumpSpeed = 4.0f;
		m_sprintSpeed = 10.0f;
		m_runSpeed = 7.5f;
		m_turnSpeed = 125.0f;
		m_moveBackwardsMultiplier = 0.375f;
		yield return new WaitForSeconds(1);
		m_jumpSpeed = 8.0f;
		m_sprintSpeed = 20.0f;
		m_runSpeed = 15.0f;
		m_turnSpeed = 250.0f;
		m_moveBackwardsMultiplier = 0.75f;
	}	
}
