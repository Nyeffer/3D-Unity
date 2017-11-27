using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject gamePause;

	public void Pause () {
		Time.timeScale = 0f;
		gamePause.SetActive(true);
	}

	public void Resume () {
		Time.timeScale = 1f;
		gamePause.SetActive(false);
	}
	
	public void Quit () {
		Application.Quit();
	}
}
