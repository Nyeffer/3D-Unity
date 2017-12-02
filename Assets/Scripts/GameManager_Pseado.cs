using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Pseado : MonoBehaviour {

	public GameObject Spawner;


	public GameObject[] goals;
	// Use this for initialization
	void Awake () {
		Spawner.SetActive(true);
		for(int i = 0; i < goals.Length; i++) {
			goals[i].SetActive(false);
		}
		
	}

	void Start() {
		int rand = Random.Range(0, goals.Length);
		goals[rand].SetActive(true);
		// Debug.Log(rand);
		// switch(rand) {
		// 	case 0:
		// 		goals[0].SetActive(true);
		// 	break;
		// 	case 1:
		// 		goals[1].SetActive(true);
		// 	break;
		// 	case 2:
		// 		goals[2].SetActive(true);
		// 	break;
		// 	case 3:
		// 		goals[3].SetActive(true);
		// 	break;
		// 	case 4:
		// 		goals[4].SetActive(true);
		// 	break;
		// 	case 5:
		// 		goals[5].SetActive(true);
		// 	break;
		// 	case 6:
		// 		goals[6].SetActive(true);
		// 	break;
		// 	case 7:
		// 		goals[7].SetActive(true);
		// 	break;
		// 	case 8:
		// 		goals[8].SetActive(true);
		// 	break;
		// 	case 9:
		// 		goals[9].SetActive(true);
		// 	break;
		// 	case 10:
		// 		goals[10].SetActive(true);
		// 	break;
		// 	case 11:
		// 		goals[11].SetActive(true);
		// 	break;
		// 	case 12:
		// 		goals[12].SetActive(true);
		// 	break;
		// 	case 13:
		// 		goals[13].SetActive(true);
		// 	break;
		// 	case 14:
		// 		goals[14].SetActive(true);
		// 	break;
		// 	case 15:
		// 		goals[15].SetActive(true);
		// 	break;
		// }
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
