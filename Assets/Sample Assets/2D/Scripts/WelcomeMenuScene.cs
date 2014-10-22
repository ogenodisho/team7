using UnityEngine; 
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class WelcomeMenuScene : MonoBehaviour{ 
  	
	// Declare variables
	private string demoLabel;
	public Texture Backgroundtexture;
	int mode = 0;

	bool pleaseLogin = false;
	private float timer = 0.0f;
	private float timerMax = 3.0f;
	

	private bool sound = true;
	public const string SOUND_ON = "Sound: ON";
	public const string SOUND_OFF = "Sound: OFF";
	private string soundBtnText = SOUND_ON;
	
	void Update(){
		if (pleaseLogin) {
			timer += Time.deltaTime;
			if (timer >= timerMax) { // 3 seconds
				//Debug.Log("timerMax reached!");
				pleaseLogin = false;
				// reset timer
				timer = 0.0f;
			}


		}
	}

	void Start(){

		Screen.orientation = ScreenOrientation.LandscapeLeft;

		if (AudioListener.volume.Equals(0f)) { // if sound is currently off, then set buttons appropriately.. etc.
			sound = false;
			soundBtnText = WelcomeMenuScene.SOUND_OFF;
			
		}

	}

	// toggle between two scenes
	public void OnGUI(){
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Backgroundtexture);

		switch (mode) {
		case 0:
			showMainDialog();
			break;
		case 1:
			ShowPlayDialog();
			break;
		case 2:
			//showLeaderBoardDialog();
			break;
		case 3:
			//showOptionsDialog();
			break;
		case 4:
			ShowExitDialogBox();
			break;
		}
	}

	// Displays the main menu
	public void showMainDialog(){
		//Display background texture

		
		//Display 'Play' button
		if (GUI.Button (new Rect (Screen.width * .17f, Screen.height * .6f, Screen.width * .25f, Screen.height * .1f), "Play")){
			mode = 1;
		}
		
		//Display 'Leaderboards' button
		if (GUI.Button (new Rect (Screen.width * .5f, Screen.height * .6f, Screen.width * .25f, Screen.height * .1f), "Leaderboards")){
			print("Clicked Leaderboards");

			// show leaderboard UI
			//((PlayGamesPlatform) Social.Active).ShowLeaderboardUI();
			if (Social.localUser.authenticated) {
				Social.ShowLeaderboardUI();
			} else {
				pleaseLogin = true;
			}
		}
		
		//Display 'Options' button
		if (GUI.Button (new Rect (Screen.width * .17f, Screen.height * .78f, Screen.width * .25f, Screen.height * .1f), "Achievements")){
			print("Clicked Options");

			// show achievements UI for testing
			if (Social.localUser.authenticated) {
				Social.ShowAchievementsUI();
			} else {
				pleaseLogin = true;
			}
		}
		
		//Display 'Exit' button
		if (GUI.Button (new Rect (Screen.width * .5f, Screen.height * .78f, Screen.width * .25f, Screen.height * .1f), "Exit")){
			mode = 4;	
		}

		// user tries to view leaderboards and achievements without having logged in
		if (pleaseLogin) {
			GUI.Box (new Rect(Screen.width / 4, Screen.height / 6, Screen.width / 2, Screen.height / 7), "Please login");
		}

		// sound options
		if (GUI.Button (new Rect (Screen.width * .887f, Screen.height * .91f, 150, 50), soundBtnText)) {
			if (sound) { // sound is currently on, so turn off
				soundBtnText = SOUND_OFF;
				sound = false;
				AudioListener.volume = 0f;

			} else { // sound is currently off, so turn on
				soundBtnText = SOUND_ON;
				sound = true;
				AudioListener.volume = 1f;

			}
		}
	}

	// Displays the dialog message asking whether the player wants to play the game in Story mode or 
	// Endless Runner mode
	public void ShowPlayDialog(){

		// Create GUI group
		GUI.BeginGroup (new Rect ((Screen.width / 2) - 200, (Screen.height / 2) - 100, 400, 200));

		// Creates a dialog box
		GUI.Box (new Rect (0, 0, 400, 200), "");

		// Display dialog message asking what the player wants to play
		GUI.Label (new Rect ((400 - 230) / 2, (100 - 30) / 2, 260, 310), "Which game mode do you want to play?");

		// Displays 'Story' button 
		if (GUI.Button (new Rect ((280 - 230) / 2, (200 - 30) / 2 + 40, 150, 30), "Story")) {
				Application.LoadLevel (1);	
		}

		// Displays 'Endless Runner' button 
		if (GUI.Button (new Rect ((280 - 230) / 2 + 200, (200 - 30) / 2 + 40, 150, 30), "Endless Runner")) {
				Application.LoadLevel (6);	
		}

		GUI.EndGroup ();

	}



	// Displays the dialog message when exit button is clicked
	public void ShowExitDialogBox(){

		// Create GUI group
		GUI.BeginGroup (new Rect ((Screen.width/ 2) - 200, (Screen.height/ 2) - 100, 400, 200));

		// Creates and dialog box
		GUI.Box (new Rect (0, 0, 400, 200), "");

		// Displays dialog message
		GUI.Label(new Rect((400 - 230)/2, (200 - 30)/2, 230, 310), "Are you sure you want to exit Escape from Dr. Vile's lab?");

		// Displays 'Yes' button 
		if (GUI.Button (new Rect ((400 - 230) / 2, (200 - 30) / 2 + 40, 100, 30), "Yes")) 
		{
			Application.Quit ();
		}

		// Displays 'cancel' button 
		if (GUI.Button (new Rect ((400 - 230) / 2 + 130, (200 - 30) / 2 + 40, 100, 30), "Cancel")) 
		{
			mode = 0;
		}
		GUI.EndGroup ();

	}
}