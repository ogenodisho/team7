﻿using UnityEngine;
using System.Collections;

public class Enemy1Behaviour : MonoBehaviour {

	// Variable determining the direction the enemy is facing, initially 'left'
	bool facingRight = true;							

	// The fastest the player can travel in the x axis.
	[SerializeField] float maxSpeed = 4f;				
	
	// A mask determining what is ground to the character
	[SerializeField] LayerMask whatIsGround;	

	public Transform ExplosionPrefab;
	private Transform explosion;

	PlatformerCharacter2D playerScript;

	public int health = 2;

	int counter = 0;

	void Awake() {
		GameObject thePlayer = GameObject.Find("Agent_7");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
	}


	// Update is called once per frame (FixedUpdate for rigidbody)
	void FixedUpdate () {
		//Flip ();
		if (facingRight) {
			transform.Translate(new Vector3(0.02f, 0, 0));
		} else {
			transform.Translate(new Vector3(-0.02f, 0, 0));
		}
		counter++;
		if (counter > 100) {
			counter = 0;
			Flip();
		}

	}
	
	// Setter for top speed of enemy
	void setTopSpeed(float speed) {
		maxSpeed = speed;
	}

	// Invert the character's position about its vertical axis
	void Flip ()
	{
		facingRight = !facingRight;
		//transform.Rotate(new Vector3(transform.position.x,1,0), new Vector3(transform.position.x,1,0), 180);
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.name.StartsWith("Missile")) {
			// Destroy the missile and generate the explosion at the missile's position
			explosion = (Transform)Instantiate (ExplosionPrefab, collision.collider.transform.position, collision.collider.transform.rotation);
			Destroy (explosion.gameObject, 0.5f);
			Destroy(collision.collider.gameObject);

			// decrement health
			health -= 1;
			if (health == 0) {
				// Destroy the enemy if his health is 0 and gain score
				Destroy (transform.gameObject);
				playerScript.GainScore(50);
			}
		}
	}
	
}