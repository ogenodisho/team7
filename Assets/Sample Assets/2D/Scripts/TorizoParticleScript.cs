using UnityEngine;
using System.Collections;

public class TorizoParticleScript : MonoBehaviour {
	
	public float moveSpeed = 1f;
	private bool facingRight = true;
	PlatformerCharacter2D playerScript;
	TorizoScript torizoScript;
	
	private Vector3 pointToShootAt;
	Collider2D agent7;
	
	void Awake()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		GameObject kog = GameObject.Find("Torizo");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		torizoScript = kog.GetComponent<TorizoScript>();
		facingRight = playerScript.facingRight;
		
		// get the point to shoot at as soon as this particle is instantiated
		float rise = playerScript.transform.position.y - transform.position.y;
		float run = playerScript.transform.position.x - transform.position.x;
		float gradient = rise/run;
		float x;
		float y;
		float z;
		if (torizoScript.facingRight) {
			x = playerScript.transform.position.x + 15f;
			y = playerScript.transform.position.y + (15f * gradient);
			z = playerScript.transform.position.z;
		} else {
			x = playerScript.transform.position.x - 15f;
			y = playerScript.transform.position.y - (15f * gradient);
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