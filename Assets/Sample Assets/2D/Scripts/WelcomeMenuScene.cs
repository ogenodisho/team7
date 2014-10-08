// Attached to main camera
using UnityEngine;
using System.Collections;

public class WelcomeMenuScene : MonoBehaviour {

	public Texture Backgroundtexture;

	public float guiPlacementY1;
	public float guiPlacementY2;
	public float guiPlacementY3;
	public float guiPlacementY4;

	public float guiPlacementX1;
	public float guiPlacementX2;	
	public float guiPlacementX3;
	public float guiPlacementX4;

	void OnGUI(){

		//Display background texture
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Backgroundtexture);
	
		//Display 'Play' button
		if (GUI.Button (new Rect (Screen.width * .17f, Screen.height * .6f, Screen.width * .25f, Screen.height * .1f), "Play")){
			print("Clicked Play game");	
		}
		
		//Display 'Leaderboards' button
		if (GUI.Button (new Rect (Screen.width * .5f, Screen.height * .6f, Screen.width * .25f, Screen.height * .1f), "Leaderboards")){
			print("Clicked Leaderboards");	
		}
		
		//Display 'Options' button
		if (GUI.Button (new Rect (Screen.width * .17f, Screen.height * .78f, Screen.width * .25f, Screen.height * .1f), "Options")){
			print("Clicked Options");	
		}
		
		//Display 'Exit' button
		if (GUI.Button (new Rect (Screen.width * .5f, Screen.height * .78f, Screen.width * .25f, Screen.height * .1f), "Exit")){
			print("Clicked Exit");	
		}
	}
}