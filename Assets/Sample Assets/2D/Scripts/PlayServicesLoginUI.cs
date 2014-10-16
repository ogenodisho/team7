using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayServicesLoginUI : MonoBehaviour 
{
	private string LOGGED_KEY = "Logged_in";

	private bool isLoggedIn = false;
	private bool isWorking = false;

	private string button = "Login";

	private float timer = 0.0f;
	private float timerMax = 2.0f; // 2 seconds for working...

	void Awake() {
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();

		// already logged in from previous session
		if (PlayerPrefs.GetInt(LOGGED_KEY, 0) == 1) {
			isLoggedIn = true;
			button = "Logout";
			Social.Active.localUser.Authenticate(ProcessAuthentication);
		}
	}

	void Update() {
		if (isWorking) {
			timer += Time.deltaTime;
			button = "Working...";
			if (timer >= timerMax) { // 2 seconds
				//Debug.Log("timerMax reached!");
				isWorking = false;
				if (isLoggedIn) {
					button = "Logout";
				} else {
					button = "Login";
				}
				// reset timer
				timer = 0.0f;
			}
		}
	}

	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width - 150, 0, 150, 50), button) && !isWorking) {
			isWorking = true;
			if (!isLoggedIn) {
				Social.Active.localUser.Authenticate(ProcessAuthentication);

				// temporary fix
				isLoggedIn = true;
				PlayerPrefs.SetInt(LOGGED_KEY, 1);
			} else {
				((PlayGamesPlatform) Social.Active).SignOut();
				isLoggedIn = false;
				PlayerPrefs.SetInt(LOGGED_KEY, 0);
			}
		}
	}

	void ProcessAuthentication(bool success) {
		if (success) { // not reaching this code currently even though succesful login displayed in game
			isLoggedIn = true;
			PlayerPrefs.SetInt(LOGGED_KEY, 1);
			print("successfully authenicated");
		} else {
			print("fail to authenicate");
		}
	}
}
