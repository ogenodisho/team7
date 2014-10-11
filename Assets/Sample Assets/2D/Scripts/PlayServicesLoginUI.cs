using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayServicesLoginUI : MonoBehaviour 
{
	private bool isLoggedIn = false;
	private bool failed = false;

	void Awake() {
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
	}

	void Start(){
		// authenticate user:
		if (!isLoggedIn) {
			Social.localUser.Authenticate((bool success) => {
				// handle success or failure
				if (success) {
					isLoggedIn = true;
				} else {
					failed = true;
				}
			});
		}
	}

	void OnGUI() {
		if (failed) { // TODO not working currently
			GUI.Box (new Rect(Screen.width / 4, Screen.height / 6, Screen.width / 2, 2 * Screen.height / 3), "Don't ask me again?");
			if (GUI.Button (new Rect (Screen.width / 4 + 10, Screen.height / 6 + 20 + 10, Screen.width / 2 - 20,   Screen.height / 6 ), "OK")){
				// TODO
				isLoggedIn = true;
				failed = false;
			}
			if (GUI.Button (new Rect (Screen.width / 4 + 10, Screen.height / 3 + 20 + 10 + 10, Screen.width / 2 - 20, Screen.height / 6), "Ask again later")){
				failed = false;
			}
		}
	}
}
