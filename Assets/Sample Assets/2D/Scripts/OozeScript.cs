using UnityEngine;
using System.Collections;

public class OozeScript : MonoBehaviour {
	
	public float moveSpeed = 2f;
	private bool facingRight = true;
	private float agentSevensPreviousDrag;
	PlatformerCharacter2D playerScript;

	Collider2D agent7;

	float timeUntilDestroy = 2f;
	

	void Start()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		facingRight = playerScript.facingRight;
		// destroy the ooze after 2 seconds, this is its time-out period
		// so as it doesnt travel forever
	}
	
	void Update () {
		transform.Translate ((playerScript.transform.position - transform.position) * Time.deltaTime * moveSpeed);
	}

	void FixedUpdate() {
		timeUntilDestroy -= Time.deltaTime;
		if (timeUntilDestroy <= 0) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		agent7 = other; //obtain reference to agent7

		// restrict his movement
		agent7.rigidbody2D.drag = 100f;

		// increase the speed of the ooze so it sticks to agent7
		moveSpeed = 100f;

		// refresh the ooze duration
		timeUntilDestroy = 2f;

		playerScript.setOozed(true);
	}

	void OnDestroy() {
		// Reset agent7's drag
		if (agent7 != null) {
			agent7.rigidbody2D.drag = 3f;
		}
		playerScript.setOozed(false);
	}
}