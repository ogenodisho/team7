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
	Rect resumeButton;
	Rect optionsButton;
	Rect quitButton;
	Rect menuBackground;
	
	bool fingerOnTrigger = false;
	
	bool leftPressed = false;
	bool rightPressed = false;
	bool jumpPressed = false;
	bool shootPressed = false;
	bool menuPressed = false;
	bool resumePressed = false;
	bool optionsPressed = false;
	bool quitPressed = false;
	
	bool paused = false;
	
	float lastShotTime = 0f;
	float shootingThreshold = 0.7f;
	
	//public static bool testingUsingUnityRemote = true;
	
	void Awake()
	{
		// Initialise script and rectangles for the ui
		character = GetComponent<PlatformerCharacter2D>();
		// achievements
		AchievementManager.Instance.RegisterEvent (AchievementType.Play);
		//if (testingUsingUnityRemote) {
			leftButton = new Rect (          0          , Screen.height - 150, Screen.width / 4, 150);
			rightButton = new Rect(  Screen.width / 4   , Screen.height - 150, Screen.width / 4, 150);
			menuButton = new Rect ( Screen.width - 150, 0, 150, 50);
			shootButton = new Rect( 2 * Screen.width / 4, Screen.height - 150, Screen.width / 4, 150);
			jumpButton = new Rect ( 3 * Screen.width / 4, Screen.height - 150, Screen.width / 4, 150);

			menuBackground = new Rect(Screen.width / 4     ,             Screen.height / 6             , Screen.width / 2     , 2 * Screen.height / 3);
			resumeButton = new Rect  (Screen.width / 4 + 10,         Screen.height / 6 + 20 + 10       , Screen.width / 2 - 20,   Screen.height / 6  );
			optionsButton = new Rect (Screen.width / 4 + 10,       Screen.height / 3 + 20 + 10 + 10    , Screen.width / 2 - 20,   Screen.height / 6  );
			quitButton = new Rect    (Screen.width / 4 + 10,    Screen.height / 2 + 20 + 10 + 10 + 10  , Screen.width / 2 - 20,   Screen.height / 6  );
		/*} else {
			leftButton = new Rect (          0          , Screen.width - 150, Screen.height / 4, 150);
			rightButton = new Rect(  Screen.height / 4   , Screen.width - 150, Screen.height / 4, 150);
			menuButton = new Rect ( Screen.height - 150, 0, 150, 50);
			shootButton = new Rect( 2 * Screen.height / 4, Screen.width - 150, Screen.height / 4, 150);
			jumpButton = new Rect ( 3 * Screen.height / 4, Screen.width - 150, Screen.height / 4, 150);

			menuBackground = new Rect(Screen.height / 4     ,             Screen.width / 6             , Screen.height / 2     , 2 * Screen.width / 3);
			resumeButton = new Rect  (Screen.height / 4 + 10,         Screen.width / 6 + 20 + 10       , Screen.height / 2 - 20,   Screen.width / 6  );
			optionsButton = new Rect (Screen.height / 4 + 10,       Screen.width / 3 + 20 + 10 + 10    , Screen.height / 2 - 20,   Screen.width / 6  );
			quitButton = new Rect    (Screen.height / 4 + 10,    Screen.width / 2 + 20 + 10 + 10 + 10  , Screen.height / 2 - 20,   Screen.width / 6  );
		}*/
	}
	
	void Update () {
		
		leftPressed = false;
		rightPressed = false;
		jumpPressed = false;
		shootPressed = false;
		menuPressed = false;
		resumePressed = false;
		optionsPressed = false;
		quitPressed = false;
		
		// Iterate through the touches to determine
		// which buttons are currently being pressed
		for (int i = 0; i < Input.touchCount; i++) {
			if (!paused) {
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
			} else {
				if (resumeButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
					resumePressed = true;
				}
				if (optionsButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
					optionsPressed = true;
				}
				if (quitButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
					quitPressed = true;
				}
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
				// achievements
				AchievementManager.Instance.RegisterEvent (AchievementType.Jump);
			}
			if (shootPressed) {
				// Alter the finger on trigger boolean so you
				// have to release the button in order to shoot again.
				// This fixes the problem of being able to shoot non stop
				if (!fingerOnTrigger && Time.realtimeSinceStartup - lastShotTime > shootingThreshold) {
					character.Shoot();
					lastShotTime = Time.realtimeSinceStartup;
					fingerOnTrigger = true;
					// achievements
					AchievementManager.Instance.RegisterEvent (AchievementType.Shoot);
				}
			} else {
				fingerOnTrigger = false;
			}
			if (menuPressed && !paused) {
				paused = true;
				Time.timeScale = 0;
			}
		} else {
			if (resumePressed) {
				paused = false;
				Time.timeScale = 1;
			}
			else if (optionsPressed) {
				// options menu
			}
			else if (quitPressed) {
				// quit to title
				paused = false;
				Time.timeScale = 1;
				Application.LoadLevel("HomeScreenScene");
			}
		}
	}
	
	// This method is called by Unity and just draws the boxes
	void OnGUI() {
		if (!paused) {
			GUI.Button (leftButton, "<-");
			GUI.Button (rightButton, "->");
			GUI.Button (menuButton, "Menu");
			GUI.Button (jumpButton, "Jump");
			GUI.Button (shootButton, "Shoot");
		} else {
			GUI.Box(menuBackground, "PAUSED");
			GUI.Button (resumeButton, "Resume");
			GUI.Button (optionsButton, "Options");
			GUI.Button (quitButton, "Quit to Title");
		}
	}
}