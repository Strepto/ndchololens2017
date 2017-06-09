using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour {

	public GameObject ScoreText;
	public GameObject TimeText;
	public GameObject PlaceText;
	// Use this for initialization
	void Start () {
		if(!ScoreText || !TimeText || !PlaceText){
			Debug.LogWarning("Missing properties in GameOverMenu");
		}
	}

	public void UpdateGameOverScreen(int score, TimeSpan time, int position, int numPlayers){
		this.ScoreText.GetComponent<TextMesh>().text = "Score: " + score;
		this.TimeText.GetComponent<TextMesh>().text = "You used " + time.TotalSeconds + "sec";
		this.PlaceText.GetComponent<TextMesh>().text = "Number " + position + " of " + numPlayers;

	}
	
}
