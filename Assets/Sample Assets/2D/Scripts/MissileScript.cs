using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {
	
	public int moveSpeed = 20;
	private bool facingRight = true;

	void Start()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		PlatformerCharacter2D playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		facingRight = playerScript.facingRight;

		// When you jump, you have an upward force, when you're falling, you have a downward force.
		// This force is being applied to the missile because it collides with you when it fires.
		// This produces an undesired effect so we will manually set the y force of the missile to 0.
		/*Vector2 xyMissileForce = new Vector2(transform.rigidbody2D.velocity.x, transform.rigidbody2D.velocity.y);
		xyMissileForce.y = 0;
		transform.rigidbody2D.velocity = xyMissileForce;*/
	}

	void Update () {
		if (facingRight) {
			transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		} else {
			// If not facing right - still translate to the "right" because
			// the PlatformerCharacter2D script flipped the prefab so "right"
			// is actually left.
			transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		}
		Destroy (gameObject, 1);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name.Equals("Enemy")) {
			Debug.Log ("You hit an enemy!");
		}
	}
}