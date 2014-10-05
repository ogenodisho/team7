using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Agent7ControlUI : MonoBehaviour 
{
	private PlatformerCharacter2D character;
	Rect leftButton;
	Rect rightButton;
	Rect jumpButton;
	Rect shootButton;

	bool fingerOnTrigger = false;

	bool leftPressed = false;
	bool rightPressed = false;
	bool jumpPressed = false;
	bool shootPressed = false;

	void Awake()
	{
		character = GetComponent<PlatformerCharacter2D>();
		leftButton = new Rect (          0         , Screen.height - 80, Screen.width / 4, 80);
		rightButton = new Rect(  Screen.width / 4  , Screen.height - 80, Screen.width / 4, 80);
		jumpButton = new Rect (  Screen.width / 2  , Screen.height - 80, Screen.width / 4, 80);
		shootButton = new Rect(3 * Screen.width / 4, Screen.height - 80, Screen.width / 4, 80);
	}

	void Update () {

		leftPressed = false;
		rightPressed = false;
		jumpPressed = false;
		shootPressed = false;

		for (int i = 0; i < Input.touchCount; i++) {
			if (leftButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height - Input.GetTouch(i).position.y, 0))) {
				leftPressed = true;
			}
			if (rightButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
				rightPressed = true;
			}
			if (jumpButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
				jumpPressed = true;
			}
			if (shootButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
				shootPressed = true;
			}
		}

		if (leftPressed) {
			character.Move (-1, false, false);
			character.LoseScore(1);
		}
		if (rightPressed) {
			character.Move (1, false, false);
			character.GainScore(1);
		}
		if (jumpPressed) {
			character.Jump ();
			character.GainHealth();
		}
		if (shootPressed) {
			if (!fingerOnTrigger) {
				character.Shoot();
				character.LoseHealth();
				fingerOnTrigger = true;
			}
		} else {
			fingerOnTrigger = false;
		}
	}
	
	void OnGUI() {
		GUI.RepeatButton (leftButton, "<-");
		GUI.RepeatButton (rightButton, "->");
		GUI.RepeatButton (jumpButton, "Jump");
		GUI.Button (shootButton, "Shoot");
	}
}
