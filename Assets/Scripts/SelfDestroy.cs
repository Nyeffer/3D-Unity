using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

	
	public GameObject m_apple;
	
	// Update is called once per frame
	void Update () {
		Destroy(m_apple, 3);
	}
}
