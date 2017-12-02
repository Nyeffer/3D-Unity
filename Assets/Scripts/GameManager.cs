using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float m_startTime;
	public Text m_countDown;
	public Image m_gameOver;
	public GameObject m_pc;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		m_startTime -= Time.deltaTime;
		m_countDown.text = "" + Mathf.Floor(m_startTime);

		if(m_startTime < 1) {
			GameOver();
			m_startTime = 0;
			
		}
	}

	public void GameOver () {
		Time.timeScale = 0f;
		m_gameOver.gameObject.SetActive(true);
	}
}
