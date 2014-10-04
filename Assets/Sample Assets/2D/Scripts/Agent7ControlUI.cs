using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Agent7ControlUI : MonoBehaviour 
{
	private PlatformerCharacter2D character;
	Rect leftButton;
	Rect rightButton;
	Rect jumpButton;
	Rect shootButton;

	void Awake()
	{
		character = GetComponent<PlatformerCharacter2D>();
		leftButton = new Rect (          0         , Screen.height - 80, Screen.width / 4, 80);
		rightButton = new Rect(  Screen.width / 4  , Screen.height - 80, Screen.width / 4, 80);
		jumpButton = new Rect (  Screen.width / 2  , Screen.height - 80, Screen.width / 4, 80);
		shootButton = new Rect(3 * Screen.width / 4, Screen.height - 80, Screen.width / 4, 80);
	}
	
	void Update ()
	{
		for (int i = 0; i < Input.touchCount; i++) {
			if (leftButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height - Input.GetTouch(i).position.y, 0))) {
				character.Move(-1, false, false);
			}
			if (rightButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
				character.Move(1, false, false);
			}
			if (jumpButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
				character.Jump();
			}
			if (shootButton.Contains(new Vector3(Input.GetTouch(i).position.x, Screen.height-Input.GetTouch(i).position.y, 0))) {
				
			}
		}
	}
	
	void OnGUI() {
		GUI.RepeatButton (leftButton, "<-");
		GUI.RepeatButton (rightButton, "->");
		GUI.RepeatButton (jumpButton, "Jump");
		GUI.RepeatButton (shootButton, "Shoot");
	}
}
