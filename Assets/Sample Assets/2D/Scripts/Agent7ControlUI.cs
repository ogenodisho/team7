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
		// Initialise script and rectangles for the ui
		character = GetComponent<PlatformerCharacter2D>();
		leftButton = new Rect (          0          , Screen.width - 150, Screen.height / 4, 150);
		rightButton = new Rect(  Screen.height / 4  , Screen.width - 150, Screen.height / 4, 150);
		shootButton = new Rect(  Screen.height / 2  , Screen.width - 150, Screen.height / 4, 150);
		jumpButton = new Rect (3 * Screen.height / 4, Screen.width - 150, Screen.height / 4, 150);
	}

	void Update () {

		leftPressed = false;
		rightPressed = false;
		jumpPressed = false;
		shootPressed = false;

		// Iterate through the touches to determine
		// which buttons are currently being pressed
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

		// Perform appropriate logic based on pressed buttons
		if (leftPressed) {
			character.Move (-1, false, false);
		}
		if (rightPressed) {
			character.Move (1, false, false);
		}
		if (jumpPressed) {
			character.Jump ();
		}
		if (shootPressed) {
			// Alter the finger on trigger boolean so you
			// have to release the button in order to shoot again.
			// This fixes the problem of being able to shoot non stop
			if (!fingerOnTrigger) {
				character.Shoot();
				fingerOnTrigger = true;
			}
		} else {
			fingerOnTrigger = false;
		}
	}

	// This method is called by Unity and just draws the boxes
	void OnGUI() {
		GUI.RepeatButton (leftButton, "<-");
		GUI.RepeatButton (rightButton, "->");
		GUI.RepeatButton (jumpButton, "Jump");
		GUI.Button (shootButton, "Shoot");
	}
}
