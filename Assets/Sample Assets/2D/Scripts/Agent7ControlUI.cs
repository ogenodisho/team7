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
	bool fingerOnJump = false;
	
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

	// these two variables are just used for type checks
	private CircleCollider2D dummyCircleCollider;
	private BoxCollider2D dummyBoxCollider;
	private EdgeCollider2D dummyEdgeCollider;

	private int previouslyScaledWall = 0;

	private bool hasFireRatePickup = false;
	private float fireRateTimeLeft = 0f;

	public static bool hasInvulnerabilityPickup = false;
	private float invulnerabilityTimeLeft = 0f;

	//public static bool testingUsingUnityRemote = true;
	
	void Awake()
	{
		dummyCircleCollider = new CircleCollider2D();
		dummyBoxCollider = new BoxCollider2D();
		dummyEdgeCollider = new EdgeCollider2D();
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
			if (rightPressed) {
				character.Move (1, false, false);
			} else if (leftPressed) {
				character.Move (-1, false, false);
			}
			if (jumpPressed) {
				if (!fingerOnJump) {
					character.Jump ();
					fingerOnJump = true;
				}
			} else {
				fingerOnJump = false;
			}
			if (shootPressed) {
				if (hasFireRatePickup) {
					character.Shoot();
				} else 
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

	void FixedUpdate() {
		// count the timer down for the pickups if applicable
		if (hasFireRatePickup) {
			fireRateTimeLeft -= Time.deltaTime;
			if (fireRateTimeLeft <= 0) {
				hasFireRatePickup = false;
			}
		}
		if (hasInvulnerabilityPickup) {
			invulnerabilityTimeLeft -= Time.deltaTime;
			if (invulnerabilityTimeLeft <= 0) {
				hasInvulnerabilityPickup = false;
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

	// This method is called when Agent_7 collides with something
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == 12) {// Enemy layer
			if (collision.collider.GetType().IsAssignableFrom(dummyBoxCollider.GetType())) {
				Debug.Log ("You touched an enemy! Lost a life and score");
				character.LoseHealth ();
				character.LoseScore(10);
			} else if (collision.collider.GetType().IsAssignableFrom(dummyCircleCollider.GetType())) {
				Debug.Log ("You jumped on a Koopa's head");
				character.GainScore(50);
				Destroy (collision.gameObject);
			}
		} else if (collision.gameObject.layer == 11 &&
		           collision.collider.GetType().IsAssignableFrom(dummyEdgeCollider.GetType())) { // scalable world
			character.setScaling(true);
		} else {
			// reset previously scaled walls because you stopped scaling walls
			previouslyScaledWall = 0;
		}
	}
	void OnCollisionExit2D(Collision2D collision) {
		if (collision.collider.GetType().IsAssignableFrom(dummyEdgeCollider.GetType())) {
			// Make sure you can't keep jumping higher from the same wall.
			// This forces the player to jump from wall to wall in order to
			// gain height.
			character.setScaling(false);
			if (jumpPressed && collision.collider.GetInstanceID() != previouslyScaledWall) {
				character.ScaleJump ();
				previouslyScaledWall = collision.collider.GetInstanceID();
			}
		}
	}
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.name.Equals ("Fire")) {
			Debug.Log("-1 HP!");
			character.LoseHealth ();
			character.LoseScore(10);
		} else if (collider.gameObject.name.Equals("HealthPickup")) {
			// Agent_7 collided with a health pickup. Increment health
			// and destroy the pickup
			Debug.Log("+1 HP!");
			character.GainHealth ();
			Destroy (collider.gameObject, 0);
		} else if (collider.gameObject.name.Equals("x2Pickup")) {
				
		} else if (collider.gameObject.name.Equals("InvulnerabilityPickup")) {
			Debug.Log("INVULNERABLE!");
			hasInvulnerabilityPickup = true;
			Destroy (collider.gameObject, 0);
			// this pickup lasts for 10 seconds
			invulnerabilityTimeLeft = 10f;	
		} else if (collider.gameObject.name.Equals("FireRatePickup")) {
			Debug.Log("MORE FIREPOWER!");
			hasFireRatePickup = true;
			Destroy (collider.gameObject, 0);
			// this pickup lasts for 10 seconds
			fireRateTimeLeft = 10f;
		}
	}
}