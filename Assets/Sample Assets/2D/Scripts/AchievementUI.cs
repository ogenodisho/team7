using UnityEngine;

public class AchievementUI : MonoBehaviour 
{
	private Rect achBackground;
	private string message;

	private float timer = 0.0f;
	private float timerMax = 3.0f;
	
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

			if (timer >= timerMax) {
				AchievementManager.Instance.queueDequeue();
				Debug.Log("timerMax reached!");
				
				// reset timer
				timer = 0.0f;
			}
		}
	}

	void OnGUI() {
		if (AchievementManager.Instance.getQueue().Count > 0) {
			GUI.Box(achBackground, AchievementManager.Instance.getQueue()[0]);
		}
	}
}