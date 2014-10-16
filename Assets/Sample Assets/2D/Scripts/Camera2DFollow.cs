using UnityEngine;
using System.Collections;

/**
 * Taken from Brackeys 2D Platformer tutorial. Just follows a target.
 * */
public class Camera2DFollow : MonoBehaviour {
	
	public Transform target;
	public float damping = 0.3f;
	public float lookAheadFactor = 1.5f;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;
	public GameObject gameObject;

	public GyroscopeListener gyroInput;
	
	float offsetZ;
	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;

	// Gyroscope variable
	Vector3 gyroPos;
	
	// Use this for initialization
	void Start () {
		// Instantiate a new gameobject so that components may
		// be added and add a GyroscopeListener component
		gameObject = new GameObject ();
		gameObject.AddComponent<GyroscopeListener> ();

		lastTargetPosition = target.position;
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;

	}
	
	// Update is called once per frame
	void Update () {
		
		// only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (target.position - lastTargetPosition).x;

	    bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadTarget) {
			lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		} else {
			lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);	
		}
		
		Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

		// Consider gyroscope values (not used yet)
		gyroPos = GyroscopeListener.getCurrentGyroCoordinates();
		
		transform.position = newPos;
		
		lastTargetPosition = target.position;	
	}
}
