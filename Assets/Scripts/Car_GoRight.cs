using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_GoRight : MonoBehaviour {

	public float m_moveSpeed;
	private Rigidbody rb;
	public Vector3 direction;
	public bool changed;
	void Start() {
		rb = GetComponent<Rigidbody>();
		m_moveSpeed = 25.0f;
		direction = Vector3.right;
		changed = false;
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Turn") {
			if(direction == Vector3.back && !changed) {
				changed = true;
				direction = Vector3.right;
				gameObject.transform.Rotate(0,-90,0);
			}
			if(direction == Vector3.right && !changed) {
				changed = true;
				direction = Vector3.forward;
				gameObject.transform.Rotate(0,-90,0);
			}

			if(direction == Vector3.forward && !changed) {
				changed = true;
				direction = Vector3.left;
				gameObject.transform.Rotate(0,-90,0);
			}

			if(direction == Vector3.left && !changed) {
				changed = true;
				direction = Vector3.back;
				gameObject.transform.Rotate(0,-90,0);
			}
		}
		if(col.gameObject.tag == "return") {
			if(direction == Vector3.back && !changed) {
				changed = true;
				direction = Vector3.left;
				gameObject.transform.Rotate(0,90,0);
			}
			if(direction == Vector3.right && !changed) {
				changed = true;
				direction = Vector3.back;
				gameObject.transform.Rotate(0,90,0);
			}

			if(direction == Vector3.forward && !changed) {
				changed = true;
				direction = Vector3.right;
				gameObject.transform.Rotate(0,90,0);
			}

			if(direction == Vector3.left && !changed) {
				changed = true;
				direction = Vector3.forward;
				gameObject.transform.Rotate(0,90,0);
			}
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.gameObject.tag == "Turn") {
			changed = false;
		}

		if(col.gameObject.tag == "return") {
			changed = false;
		}
	}

	void FixedUpdate() {
        rb.MovePosition(transform.position + direction * (Time.deltaTime * m_moveSpeed));
    }


	// void Update() {
	// 	Vector3 pos = gameObject.transform.position;
	// 	pos.x = pos.x - m_moveSpeed * Time.deltaTime;
	// 	gameObject.transform.position = pos;
	// }
}
