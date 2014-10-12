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
		Destroy (gameObject, timeUntilDestroy);
	}
	
	void Update () {
		transform.Translate ((playerScript.transform.position - transform.position) * Time.deltaTime * moveSpeed);

		if (agent7 != null) {
			// if agent7 is not null it means onTriggerEnter got called
			// once he lands on the ground, apply the slow
			if (agent7.gameObject.GetComponent<Animator>().GetBool("Ground")) {
				agent7.rigidbody2D.drag = 100f;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		agent7 = other; //obtain reference to agent7

		// if agent7 is on the ground, restrict his movement
		if (other.gameObject.GetComponent<Animator>().GetBool("Ground")) {
			agent7.rigidbody2D.drag = 100f;
		}

		// increase the speed of the ooze so it sticks to agent7
		moveSpeed = 100f;
	}

	void OnDestroy() {
		// Reset agent7's drag it he got hit
		if (agent7 != null) {
			agent7.rigidbody2D.drag = 3f;
		}
	}
}