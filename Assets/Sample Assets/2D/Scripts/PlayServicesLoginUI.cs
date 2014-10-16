using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayServicesLoginUI : MonoBehaviour 
{
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
				Social.Active.localUser.Authenticate((bool success) => {
					// handle success or failure
					if (success) { // not reaching this code currently even though succesful login displayed in game
						isLoggedIn = true;
					}
				});
				isLoggedIn = true; // temporary fix
			} else {
				((PlayGamesPlatform) Social.Active).SignOut();
				isLoggedIn = false;
			}
		}
	}
}
