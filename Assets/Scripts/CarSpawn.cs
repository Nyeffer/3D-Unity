using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour {

	public GameObject car;

	float counter = 0;
	void Update() {
		if(counter >= 3) {
			Instantiate(car, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z), gameObject.transform.rotation);
			counter = 0;
		} else {
			counter += Time.deltaTime;
		}
	}
}
