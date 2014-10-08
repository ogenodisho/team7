using UnityEngine;

public class AchievementUI : MonoBehaviour 
{
	private Rect achBackground;
	private string message;

	private float timer = 0.0f;
	private float timerMax = 3.0f; // 3 seconds for displaying the achievement message
	
	void Awake() {
		//if (Agent7ControlUI.testingUsingUnityRemote) {
			achBackground = new Rect(Screen.width / 4, Screen.height / 6, Screen.width / 2, Screen.height / 7);
		//} else {
		//	achBackground = new Rect(Screen.height / 4, Screen.width / 6, Screen.height / 2, Screen.width / 7);
		//}
	}

	void Update(){
		if (AchievementManager.Instance.getQueue().Count > 0) {
			timer += Time.deltaTime;

			if (timer >= timerMax) { // 3 seconds for displaying the achievement message
				AchievementManager.Instance.queueDequeue(); // remove achievement message
				Debug.Log("timerMax reached!");
				
				// reset timer
				timer = 0.0f;
			}
		}
	}

	void OnGUI() {
		if (AchievementManager.Instance.getQueue().Count > 0) { // there is a achievement message in the queue
			GUI.Box(achBackground, AchievementManager.Instance.getQueue()[0]); // display message at 0 index
		}
	}
}