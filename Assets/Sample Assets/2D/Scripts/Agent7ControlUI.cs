using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Agent7ControlUI : MonoBehaviour 
{
	private PlatformerCharacter2D character;
	Rect leftButton;
	Rect rightButton;
	Rect jumpButton;
	Rect shootButton;
	Rect menuButton;

	bool fingerOnTrigger = false;

	bool leftPressed = false;
	bool rightPressed = false;
	bool jumpPressed = false;
	bool shootPressed = false;
	bool menuPressed = false;
	
	bool paused = false;
	
	float lastShotTime = 0f;
	float shootingThreshold = 0.7f;

	void Awake()
	{
		// Initialise script and rectangles for the ui
		character = GetComponent<PlatformerCharacter2D>();
		leftButton = new Rect (          0           , Screen.width - 150, Screen.height / 5, 150);
		rightButton = new Rect(  Screen.height / 5   , Screen.width - 150, Screen.height / 5, 150);
		menuButton = new Rect ( 2 * Screen.height / 5, Screen.width - 150, Screen.height / 5, 150);
		shootButton = new Rect( 3 * Screen.height / 5, Screen.width - 150, Screen.height / 5, 150);
		jumpButton = new Rect ( 4 * Screen.height / 5, Screen.width - 150, Screen.height / 5, 150);
	}

	void Update () {

		leftPressed = false;
		rightPressed = false;
		jumpPressed = false;
		shootPressed = false;
		menuPressed = false;

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
			if (menuButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
				menuPressed = true;
			}
		}

		if (!paused) {
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
				if (!fingerOnTrigger && Time.realtimeSinceStartup - lastShotTime > shootingThreshold) {
					character.Shoot();
					lastShotTime = Time.realtimeSinceStartup;
					fingerOnTrigger = true;
				}
			} else {
				fingerOnTrigger = false;
			}
		}
		
		if (menuPressed) {
			if (paused) { // unpause
				paused = false;
				Time.timeScale = 1;
			} else {
				paused = true;
				Time.timeScale = 0;
			}
		}
	}

	// This method is called by Unity and just draws the boxes
	void OnGUI() {
		GUI.RepeatButton (leftButton, "<-");
		GUI.RepeatButton (rightButton, "->");
		GUI.Button (menuButton, "Menu");
		GUI.RepeatButton (jumpButton, "Jump");
		GUI.Button (shootButton, "Shoot");
	}
}
