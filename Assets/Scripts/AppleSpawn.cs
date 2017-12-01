using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawn : MonoBehaviour {

	public GameObject fallApple;
	GameObject appleClone;


	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			StartCoroutine(AppleSpawnDelay());
			appleClone = Instantiate(fallApple, transform.position, Quaternion.identity) as GameObject;
			StartCoroutine(AppleSpawnDelay());	
		}
	}

	public IEnumerator AppleSpawnDelay() {
		yield return new WaitForSeconds(1);
	}
		
}
