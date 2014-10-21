using UnityEngine;
using System.Collections;

public class KogDamageParticleScript : MonoBehaviour {
	
	public float moveSpeed = 1.5f;
	private bool facingRight = true;
	PlatformerCharacter2D playerScript;
	KogScript kogScript;

	private Vector3 pointToShootAt;
	Collider2D agent7;
	
	void Awake()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		GameObject kog = GameObject.Find("Kog");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		kogScript = kog.GetComponent<KogScript>();
		facingRight = playerScript.facingRight;

		// get the point to shoot at as soon as this particle is instantiated
		float rise = playerScript.transform.position.y - transform.position.y;
		float run = playerScript.transform.position.x - transform.position.x;
		float gradient = rise/run;
		float x;
		float y;
		float z;
		if (kogScript.facingRight) {
			x = playerScript.transform.position.x + 10f;
			y = playerScript.transform.position.y + (10f * gradient);
			z = playerScript.transform.position.z;
		} else {
			x = playerScript.transform.position.x - 10f;
			y = playerScript.transform.position.y - (10f * gradient);
			z = playerScript.transform.position.z;
		}
		pointToShootAt = new Vector3(x, y, z);
	}
	
	void Update () {
		// constantly shoot the missile towards agent_7s location at time of instantiation
		transform.Translate ((pointToShootAt - transform.position) * Time.deltaTime * moveSpeed);
		if (Mathf.Abs(pointToShootAt.x - transform.position.x) <= 1f) {
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		agent7 = other; //obtain reference to agent7
		playerScript.LoseHealth();
		Destroy (gameObject);
	}
}