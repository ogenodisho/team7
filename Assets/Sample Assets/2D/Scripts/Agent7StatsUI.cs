using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Agent7StatsUI : MonoBehaviour {
	
	private PlatformerCharacter2D character;
	public float fallingThreshold = -100;

	public Texture threeHp;
	public Texture twoHp;
	public Texture oneHp;
	public Texture zeroHp;

	Rect healthDisplay;
	Rect scoreDisplay;
	Rect powerupDisplay;

	// Initial hp and score
	int hp = 2;
	int score = 0;

	GUIStyle customScoreText;
	GUIStyle customHealth;

	// Getters and setters for status variables

	public void setHp(int newHp) {
		hp = newHp;

		// stop hp going below zero or above three...
		if (hp < 0) {
			hp = 0;
		} else if (hp > 3) {
			hp = 3;
		}
	}

	public int getHp() {
		return hp;
	}

	public int getScore() {
		return score;
	}

	public void setScore(int newScore) {
		score = newScore;
		
		// stop score going below zero...
		if (score < 0) {
			score = 0;
		}
	}

	void Awake() {
		character = GetComponent<PlatformerCharacter2D>();

		if (Agent7ControlUI.testingUsingUnityRemote) {
			healthDisplay = new Rect ((Screen.width / 2) - (Screen.width / 5 / 2), 0, Screen.width / 5, 50);
			scoreDisplay = new Rect (0, 0, Screen.width / 5, 50);
		} else {
			healthDisplay = new Rect ((Screen.height / 2) - (Screen.height / 5 / 2), 0, Screen.height / 5, 50);
			scoreDisplay = new Rect (0, 0, Screen.height / 5, 50);
		}

		customScoreText = new GUIStyle();
		customScoreText.normal.textColor = Color.white;
		customScoreText.fontSize = 50;

		// this empty gui style will remove the black
		// tint from behind the default GUI.Box()
		customHealth = new GUIStyle();
	}
	
	void Update() {
		if (character.transform.position.y < fallingThreshold) {
			hp = 0;
		}
	}

	// Everytime this method is called, check the health value
	// so you can display the appropriate health texture
	void OnGUI() {
		GUI.Box(scoreDisplay, score.ToString(), customScoreText);
		switch (hp) {
		case 3:
			GUI.Box(healthDisplay, threeHp, customHealth);
			break;
		case 2:
			GUI.Box(healthDisplay, twoHp, customHealth);
			break;
		case 1:
			GUI.Box(healthDisplay, oneHp, customHealth);
			break;
		case 0:
			GUI.Box(healthDisplay, zeroHp, customHealth);
			Die(true);
			break;
		}
	}
	
	
	// this method is called when Agent_7 dies
	// The boolean is for when you are just testing the
	// code. It returns Agent_7 to the starting point
	// instantly for convenience.
	void Die(bool test) {
		if (test) {
			character.transform.position = new Vector3(0f, 1.6f, 0f);
			hp = 3;
		} else {
			// initiate normal death sequence
		}
	}
}
