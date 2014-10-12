using UnityEngine;
using System.Collections;

public class OozeScript : MonoBehaviour {
	
	public float moveSpeed = 2f;
	private bool facingRight = true;
	private float agentSevensPreviousDrag;
	PlatformerCharacter2D playerScript;

	Collider2D agent7;

	bool agent7GotOozed = false;

	void Awake()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		facingRight = playerScript.facingRight;
		// destroy the ooze after 2 seconds, this is its time-out period
		// so as it doesnt travel forever
	}
	
	void Update () {
		if (agent7GotOozed) {
			if (!playerScript.getOozed()) {
				Destroy (gameObject);
			}
		}
		transform.Translate ((playerScript.transform.position - transform.position) * Time.deltaTime * moveSpeed);
	}

	void OnTriggerEnter2D(Collider2D other) {
		agent7 = other; //obtain reference to agent7

		// increase the speed of the ooze so it sticks to agent7
		moveSpeed = 100f;

		agent7GotOozed = true;

		// set the player as oozed
		playerScript.setOozed(true);
	}
}