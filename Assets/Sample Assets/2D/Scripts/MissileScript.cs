using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {
	
	public int moveSpeed = 20;
	private bool facingRight = true;
	public Transform ExplosionPrefab;
	private Transform explosion;
	PlatformerCharacter2D playerScript;

	void Start()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		facingRight = playerScript.facingRight;
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
		// destroy the missile after 2 seconds, this is its time-out period
		// so as it doesnt travel forever
		Destroy (gameObject, 2);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == 12) {// the enemy layer

			string enemyName = collision.collider.name;

			Debug.Log ("Your missile hit an enemy!");

			/*if (enemyName.Equals("Koopa")) {
				playerScript.GainScore(50);
			} else if (enemyName.Equals("Kog")) {
				playerScript.GainScore(100);
			}
			// destroy the enemies that were hit
			/*foreach(ContactPoint2D c in collision.contacts) {
				Destroy(c.otherCollider.gameObject);
			}*/

			// instantiate the explosion animation and destroy it after 0.5 seconds.
			// Then destory the missile because it exploded
			/*Destroy (transform.gameObject);
			explosion = (Transform)Instantiate (ExplosionPrefab, transform.position, transform.rotation);
			Destroy (explosion.gameObject, 0.5f);*/
			//Destroy(collision.collider.gameObject);*/
		} else if (collision.gameObject.layer == 11) {
			Debug.Log ("Your missle hit world!");
			// instantiate the explosion animation and destroy it after 0.5 seconds.
			// Then destory the missile because it exploded
			explosion = (Transform)Instantiate (ExplosionPrefab, transform.position, transform.rotation);
			Destroy (explosion.gameObject, 0.5f);
			Destroy(gameObject);
		}
	}
}