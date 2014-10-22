using UnityEngine;
using System.Collections.Generic;

public class GyroControl : MonoBehaviour
{
	private Gyroscope gs;

	//private List<SceneBounds> sceneBounds;

	private float tiltCalibrationX = 0.0f;
	private float tiltCalibrationY = 0.3f;
	private float slowGravityFactorX = 1.1f;
	private float slowGravityFactorY = 1.6f;

	private SceneBounds currentBounds;
	
	// Use this for initialization
	void Start () {
		if (SystemInfo.supportsGyroscope) {
			gs = Input.gyro;
			gs.enabled = true;
			PlayerPrefs.SetInt("GYRO_SUPPORT", 1);
		} else {
			PlayerPrefs.SetInt("GYRO_SUPPORT", 0);
		}

		if (Application.loadedLevelName == "TutorialLevelScene") {
			currentBounds = new SceneBounds(-10.0f, 115.0f, 25.0f, -1.0f);
		} else if (Application.loadedLevelName == "EasyLevelScene") {
			currentBounds = new SceneBounds(-20.0f, 60.0f, 15.0f, -1.0f);
		} else if (Application.loadedLevelName == "NormalLevelScene") {
			currentBounds = new SceneBounds(-15.0f, 100.0f, 70.0f, -1.0f);
		} else if (Application.loadedLevelName == "HardLevelScene") {
			currentBounds = new SceneBounds(-20.0f, 60.0f, 15.0f, -1.0f);
		} else if (Application.loadedLevelName == "EasyKogScene") {
			currentBounds = new SceneBounds(-15.0f, 445.0f, 25.0f, 0.0f);
		} else if (Application.loadedLevelName == "EndlessRunnerBeginning") {
			currentBounds = new SceneBounds(-5.0f, 25.0f, 15.0f, 0.0f);
		} else if (Application.loadedLevelName == "EndlessRunnerClimb") {
			currentBounds = new SceneBounds(0.0f, 130.0f, 65.0f, 0.0f);
		} else if (Application.loadedLevelName == "EndlessRunnerSpacious") {
			currentBounds = new SceneBounds(0.0f, 220.0f, 15.0f, -15.0f);
		} else if (Application.loadedLevelName == "EndlessRunnerCorridors") {
			currentBounds = new SceneBounds(0.0f, 545.0f, 15.0f, -340.0f);
		} else if (Application.loadedLevelName == "EndlessRunnerValley") {
			currentBounds = new SceneBounds(-10.0f, 140.0f, 15.0f, -95.0f);
		} else { // default
			currentBounds = new SceneBounds(-20.0f, 220.0f, 70.0f, -15.0f);
		}
	}

	public Vector3 transformPosition(Vector3 position) {
		position.x += ((gs.gravity.x + tiltCalibrationX) / slowGravityFactorX);
		position.y += ((gs.gravity.y + tiltCalibrationY) / slowGravityFactorY);

		// bounds checking
		if (position.x >= currentBounds.right) { // right
			position.x = currentBounds.right;
		} else if (position.x <= currentBounds.left) { // left
			position.x = currentBounds.left;
		}

		if (position.y >= currentBounds.top) { // top
			position.y = currentBounds.top;
		} else if (position.y <= currentBounds.bottom) { // bottom
			position.y = currentBounds.bottom;
		}

		return position;
	}

	private class SceneBounds {
		public float left;
		public float right;
		public float top;
		public float bottom;

		public SceneBounds (float left, float right, float top, float bottom) {
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}
	}
}

