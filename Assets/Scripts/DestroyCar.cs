using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCar : MonoBehaviour {


	void OnCollisionEnter( Collision col) {
		if(col.gameObject.tag == "Car") {
			Destroy(col.gameObject);
		}

	}
}
