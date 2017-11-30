using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public float m_moveSpeed;

	void Start() {
		m_moveSpeed = 25.0f;
	}


	void Update() {
		Vector3 pos = gameObject.transform.position;
		pos.z = pos.z + m_moveSpeed * Time.deltaTime;
		gameObject.transform.position = pos;
	}
}
