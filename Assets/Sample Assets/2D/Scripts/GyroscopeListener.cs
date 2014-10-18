using UnityEngine;
using System.Collections;

public class GyroscopeListener : MonoBehaviour {

	private Gyroscope gyro;
	private bool gyroCheck;

	// Low pass filter used for ignoring gyroscope positions smaller than a
	// certain amount
	private const float lowPassFilterFactor = 0.2f;

	// Rotation values used in game 
	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);
	private readonly Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	private readonly Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	private readonly Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);
	
	void Awake() {

		//GameObject thePlayer = GameObject.Find ("Agent_7");
		//playerScript = thePlayer.GetComponent<PlatformerCharacter2D> ();


	}

	void Start() {
		// Check to see if the system supports use of the gyroscope
		gyroCheck = SystemInfo.supportsGyroscope;

		// Enable 
		if (gyroCheck) {
			gyro = Input.gyro;
			gyro.enabled = true;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public static Vector3 getCurrentGyroCoordinates() {
		return new Vector3 (0, 0, 0); // DEFAULT RETURN, NOT FINAL
	}
}
