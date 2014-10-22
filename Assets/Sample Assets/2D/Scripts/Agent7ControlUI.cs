	using UnityEngine;
	using GooglePlayGames;
	using UnityEngine.SocialPlatforms;

	[RequireComponent(typeof(PlatformerCharacter2D))]
	public class Agent7ControlUI : MonoBehaviour 
	{
		private PlatformerCharacter2D character;
		private Agent7StatsUI statsUi;
		Rect leftButton;
		Rect rightButton;
		Rect jumpButton;
		Rect shootButton;
		Rect menuButton;
		Rect resumeButton;
		Rect soundButton;
		Rect quitButton;
		Rect menuBackground;
		Rect nextButton;
		Rect againButton;
		Rect backButton;
		Rect submitButton;
		
		private float m_waitInSeconds = 30.0f;
		private float StartTime = 0.0f; 
		
		bool fingerOnTrigger = false;
		bool fingerOnJump = false;
		
		bool leftPressed = false;
		bool rightPressed = false;
		bool jumpPressed = false;
		bool shootPressed = false;
		bool menuPressed = false;
		bool resumePressed = false;
		bool soundPressed = false;
		bool quitPressed = false;
		bool nextPressed = false;
		bool againPressed = false;
		bool submitPresssed = false;
		bool backPressed = false;
		
		bool paused = false;
		bool dead = false;
		bool end = false;
		bool scout = false; // gyro scout
		
		float lastShotTime = 0f;
		float shootingThreshold = 0.7f;

		public Texture gameoverBg;
		public Texture lvlclearedBg;

		// these two variables are just used for type checks
		private CircleCollider2D dummyCircleCollider;
		private BoxCollider2D dummyBoxCollider;
		private EdgeCollider2D dummyEdgeCollider;

		private int previouslyScaledWall = 0;

		private bool hasFireRatePickup = false;
		private float fireRateTimeLeft = 0f;

		public static bool hasInvulnerabilityPickup = false;
		private float invulnerabilityTimeLeft = 0f;

		public static bool hasX2MissilesPickup = false;
		private float X2MissilesPickupTimeLeft = 0f;

		private bool exitSceneWait = false;
		private float timer = 0.0f;
		private float timerMax = 1.0f;

		private bool submitted = false;
		private string submitText = "Submit Score";

		private bool sound = true;
		private string soundBtnText = WelcomeMenuScene.SOUND_ON;

		private System.Random rng = new System.Random ();

		GUIStyle scoreFont;

		int currentLevel;	// Used to store current level 

		//public static bool testingUsingUnityRemote = true;

		public AudioSource audio;
		public AudioClip DNApickup;
		public AudioClip pickupFX;
		
		void Awake()
		{
			dummyCircleCollider = new CircleCollider2D();
			dummyBoxCollider = new BoxCollider2D();
			dummyEdgeCollider = new EdgeCollider2D();
			// Initialise script and rectangles for the ui
			character = GetComponent<PlatformerCharacter2D>();
			statsUi = GetComponent<Agent7StatsUI> ();
			
			scoreFont = new GUIStyle();
			scoreFont.fontSize = 50;
			scoreFont.normal.textColor = Color.white;
			scoreFont.alignment = TextAnchor.UpperCenter;

			// achievements
			AchievementManager.Instance.RegisterEvent (AchievementType.Play);
			//if (testingUsingUnityRemote) {
				leftButton = new Rect (          0          , Screen.height - 150, Screen.width / 4, 150);
				rightButton = new Rect(  Screen.width / 4   , Screen.height - 150, Screen.width / 4, 150);
				menuButton = new Rect ( Screen.width - 150, 0, 150, 50);
				shootButton = new Rect( 2 * Screen.width / 4, Screen.height - 150, Screen.width / 4, 150);
				jumpButton = new Rect ( 3 * Screen.width / 4, Screen.height - 150, Screen.width / 4, 150);

				menuBackground = new Rect(Screen.width / 4     ,             Screen.height / 6             , 
	Screen.width / 2     , 2 * Screen.height / 3);
				resumeButton = new Rect  (Screen.width / 4 + 10,         Screen.height / 6 + 20 + 10       , 
	Screen.width / 2 - 20,   Screen.height / 6  );
			soundButton = new Rect (Screen.width / 4 + 10,       Screen.height / 3 + 20 + 10 + 10    , 
	Screen.width / 2 - 20,   Screen.height / 6  );
				quitButton = new Rect    (Screen.width / 4 + 10,    Screen.height / 2 + 20 + 10 + 10 + 10  , 
	Screen.width / 2 - 20,   Screen.height / 6  );
			
				nextButton = new Rect  (Screen.width * .7f, Screen.height * .8f, Screen.width * .25f, Screen.
	height * .1f);
				againButton = new Rect  (Screen.width * .7f, Screen.height * .8f, Screen.width * .25f, Screen
	.height * .1f);
				submitButton = new Rect  (Screen.width * .375f, Screen.height * .8f, Screen.width * .25f, 
	Screen.height * .1f);
				backButton = new Rect (Screen.width * .05f, Screen.height * .8f, Screen.width * .25f, Screen.
	height * .1f);
				
				currentLevel = Application.loadedLevel;

				
			/*} else {
				leftButton = new Rect (          0          , Screen.width - 150, Screen.height / 4, 150);
				rightButton = new Rect(  Screen.height / 4   , Screen.width - 150, Screen.height / 4, 150);
				menuButton = new Rect ( Screen.height - 150, 0, 150, 50);
				shootButton = new Rect( 2 * Screen.height / 4, Screen.width - 150, Screen.height / 4, 150);
				jumpButton = new Rect ( 3 * Screen.height / 4, Screen.width - 150, Screen.height / 4, 150);

				menuBackground = new Rect(Screen.height / 4     ,             Screen.width / 6             , 
	Screen.height / 2     , 2 * Screen.width / 3);
				resumeButton = new Rect  (Screen.height / 4 + 10,         Screen.width / 6 + 20 + 10       , 
	Screen.height / 2 - 20,   Screen.width / 6  );
				soundButton = new Rect (Screen.height / 4 + 10,       Screen.width / 3 + 20 + 10 + 10    , 
	Screen.height / 2 - 20,   Screen.width / 6  );
				quitButton = new Rect    (Screen.height / 4 + 10,    Screen.width / 2 + 20 + 10 + 10 + 10  , 
	Screen.height / 2 - 20,   Screen.width / 6  );
			}*/
		}
		
		void Update () {
					
			leftPressed = false;
			rightPressed = false;
			jumpPressed = false;
			shootPressed = false;
			menuPressed = false;
			resumePressed = false;
			soundPressed = false;
			quitPressed = false;
			nextPressed = false;
			againPressed = false;
			backPressed = false;
			submitPresssed = false;

			if (statsUi.getHp () == 0 && !dead) {
				die();
			}

			// when dead wait one second so accidental button presses can be avoided
			if (exitSceneWait) {
				timer += Time.unscaledDeltaTime;
				if (timer >= timerMax) {
					//Debug.Log("timerMax reached!");
					exitSceneWait = false;
					// reset timer
					timer = 0.0f;
				}
			}

			// Iterate through the touches to determine
			// which buttons are currently being pressed
			for (int i = 0; i < Input.touchCount; i++) {
				if (!paused) {
					if (leftButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height - Input.
	GetTouch(i).position.y, 0))) {
						leftPressed = true;
					}
					if (rightButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						rightPressed = true;
					}
					if (jumpButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						jumpPressed = true;
					}
					if (shootButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						shootPressed = true;
					}
					if (menuButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						menuPressed = true;
					}
				} else if (dead) {
					if (againButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						againPressed = true;
					}
					if (backButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						backPressed = true;
					}
					if (submitButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						submitPresssed = true;
					}
				} else if (end) {
					if (nextButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						nextPressed = true;
					}
					if (backButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						backPressed = true;
					}
				} else {
					if (resumeButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						resumePressed = true;
					}
					if (soundButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
						soundPressed = true;
					}
					if (quitButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.
	GetTouch(i).position.y, 0))) {
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
			} else if (dead && !exitSceneWait) {
				if (againPressed) {
					Time.timeScale = 1;
					Application.LoadLevel(currentLevel);
				} else if (backPressed) {
					Time.timeScale = 1;
					Application.LoadLevel(0);
				} else if (submitPresssed && !submitted) {
					submitted = true;
					Social.ReportScore(statsUi.getScore(), "CgkIltz5q7wNEAIQCg", (bool success) => {
						// handle success or failure
						if (success) {
							submitText = "Submitted!";
						} else {
							submitted = false;
							submitText = "Failed, submit again?";
						}
					});
				}
			} else if (end && !exitSceneWait) {
				if (nextPressed) {
					Time.timeScale = 1;
					if(++currentLevel > 4) {
						currentLevel = 1;
					}
					Application.LoadLevel(currentLevel);
				} else if (backPressed) {
					Time.timeScale = 1;
					Application.LoadLevel(0);
				}
			} else {
				if (resumePressed) {
					paused = false;
					Time.timeScale = 1;
				}
				else if (soundPressed) {

				}
				else if (quitPressed) {
					// quit to title
					Time.timeScale = 1;
					Application.LoadLevel(0);
				}
			}
		}
		
		void Start(){
			// The time at this very moment, plus the 30 seconds we want to wait 
			StartTime = Time.time + m_waitInSeconds;

			if (AudioListener.volume.Equals(0f)) { // if sound is currently off, then set buttons appropriately.. etc.
				sound = false;
				soundBtnText = WelcomeMenuScene.SOUND_OFF;
				
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
			if (hasX2MissilesPickup) {
				X2MissilesPickupTimeLeft -= Time.deltaTime;
				if (X2MissilesPickupTimeLeft <= 0) {
					hasX2MissilesPickup = false;
				}
			}
		}

		// This method is called by Unity and just draws the boxes
		void OnGUI() {
			if (!paused && !scout) {
				GUI.Button (leftButton, "<-");
				GUI.Button (rightButton, "->");
				GUI.Button (menuButton, "Menu");
				GUI.Button (jumpButton, "Jump");
				GUI.Button (shootButton, "Shoot");
				if (GUI.Button (new Rect ( Screen.width - 150, 55, 150, 50), "Scout")) {
					scout = true;
					PlayerPrefs.SetInt("SCOUT", 1);
					Time.timeScale = 0;
				}
			} else if (scout) {
				if (GUI.Button (new Rect ( Screen.width - 150, 55, 150, 50), "Un-Scout")) {
					scout = false;
					PlayerPrefs.SetInt("SCOUT", 0);
					Time.timeScale = 1;
				}
			} else if (dead) {
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), gameoverBg);
				GUI.Label (new Rect (0, Screen.height * .5f, Screen.width, Screen.height *.2f), "" + statsUi.
	getScore(), scoreFont);
				GUI.Button (againButton, "Try Again");
				GUI.Button (submitButton, submitText);
				GUI.Button (backButton, "Quit to Title");
			} else if (end) {
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), lvlclearedBg);
				GUI.Label (new Rect (0, Screen.height * .5f, Screen.width, Screen.height *.2f), "" + statsUi.
	getScore(), scoreFont);
				GUI.Button (nextButton, "Next level");
				GUI.Button (backButton, "Quit to Title");
			} else {
				GUI.Box(menuBackground, "PAUSED");
				GUI.Button (resumeButton, "Resume");
				if (GUI.Button (soundButton, soundBtnText)) {
					if (sound) { // sound is currently on, so turn off
						soundBtnText = WelcomeMenuScene.SOUND_OFF;
						sound = false;

						
					} else { // sound is currently off, so turn on
						soundBtnText = WelcomeMenuScene.SOUND_ON;
						sound = true;

						
					}
				}
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
					   collision.collider.GetType().IsAssignableFrom(dummyEdgeCollider.GetType())) { 
				//scalable world
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
			if (collider.gameObject.name.Equals ("HealthPickup")) {
					// Agent_7 collided with a health pickup. Increment health
					// and destroy the pickup
					if (statsUi.getHp() == 3){
						Debug.Log ("CAN'T GAIN HEALTH");
					} else {
						Debug.Log ("+1 HP!");
						character.GainHealth ();
						Destroy (collider.gameObject, 0);
					}

			// Door collision logic; has different outcome depending on whether game is in Story
			// mode or Endless Runner mode
			} else if (collider.gameObject.name.Equals ("Door")) {
					Debug.Log ("End of level reached...");
					// For an endless runner level...
					character.LoseHealth (); //debug, remove eventually (testing why/where collision event not occuring)
					if (Application.loadedLevelName.StartsWith("EndlessRunner")) {
						// Go to a random level
						Application.LoadLevel (rng.Next(5,7));
					} else {
						Debug.Log("LEVEL END!");
						end = true;
						paused = true;
						Time.timeScale = 0;
						exitSceneWait = true;
					}
			} else if (collider.gameObject.name.Equals ("X2MissilesPickup")) {
					Debug.Log ("X2 Missile Damage!");
					hasX2MissilesPickup = true;
					PickupScript pus =(PickupScript) collider.gameObject.GetComponent(typeof(PickupScript));
					if(!pus.getCollected()){
						audio.PlayOneShot(pickupFX,0.5f);
					}
					pus.setCollected();
					X2MissilesPickupTimeLeft = 10f;
			} else if (collider.gameObject.name.Equals ("InvulnerabilityPickup")) {
					Debug.Log ("INVULNERABLE!");
					hasInvulnerabilityPickup = true;
					PickupScript pus =(PickupScript) collider.gameObject.GetComponent(typeof(PickupScript));
					if(!pus.getCollected()){
						audio.PlayOneShot(pickupFX,0.5f);
					}
					pus.setCollected();
					invulnerabilityTimeLeft = 10f;	
			} else if (collider.gameObject.name.Equals ("FireRatePickup")) {
					Debug.Log ("MORE FIREPOWER!");
					hasFireRatePickup = true;
					PickupScript pus =(PickupScript) collider.gameObject.GetComponent(typeof(PickupScript));
					if(!pus.getCollected()){
						audio.PlayOneShot(pickupFX,0.5f);
					}
					pus.setCollected();
					// this pickup lasts for 10 seconds
					fireRateTimeLeft = 10f;
			} else if (collider.gameObject.name.Equals ("DNACollectible")) {
					Debug.Log ("Got a DNACollectible");
					Destroy (collider.gameObject, 0);
					character.GainScore(10);
					AchievementManager.Instance.RegisterEvent(AchievementType.Collectable);
					audio.PlayOneShot(DNApickup);
			} 
		}

		// This method is called when hp equals 0
		void die() {
			AchievementManager.Instance.RegisterEvent(AchievementType.Die);
			dead = true;
			paused = true;
			exitSceneWait = true;
			submitText = "Submit Score";
			Time.timeScale = 0;
		}
		
	}
