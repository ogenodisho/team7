using UnityEngine; 
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class WelcomeMenuScene : MonoBehaviour{ 
  	
	// Declare variables
	private string demoLabel;
	public Texture Backgroundtexture;
	int mode = 0;
	
	void update(){

	}

	void Start(){

		Screen.orientation = ScreenOrientation.LandscapeLeft;

	}

	// toggle between two scenes
	public void OnGUI(){
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Backgroundtexture);

		switch (mode) {
		case 0:
			showMainDialog();
			break;
		case 1:
			//showPlayDialog();
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
			Application.LoadLevel("InGameScene");	
		}
		
		//Display 'Leaderboards' button
		if (GUI.Button (new Rect (Screen.width * .5f, Screen.height * .6f, Screen.width * .25f, Screen.height * .1f), "Leaderboards")){
			print("Clicked Leaderboards");

			// show leaderboard UI
			Social.ShowLeaderboardUI();
		}
		
		//Display 'Options' button
		if (GUI.Button (new Rect (Screen.width * .17f, Screen.height * .78f, Screen.width * .25f, Screen.height * .1f), "Options")){
			print("Clicked Options");

			// show achievements UI for testing
			Social.ShowAchievementsUI();
		}
		
		//Display 'Exit' button
		if (GUI.Button (new Rect (Screen.width * .5f, Screen.height * .78f, Screen.width * .25f, Screen.height * .1f), "Exit")){
			mode = 4;	
		}
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