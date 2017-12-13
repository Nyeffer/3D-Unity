using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Right : MonoBehaviour {

 	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Car") {
			other.transform.Rotate(0, 90, 0);
		}
	} 

}