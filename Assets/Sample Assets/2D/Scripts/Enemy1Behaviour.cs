using UnityEngine;
using System.Collections;

public class Enemy1Behaviour : MonoBehaviour {

	// Variable determining the direction the enemy is facing, initially 'left'
	bool facingRight = true;							

	// The fastest the player can travel in the x axis.
	[SerializeField] float maxSpeed = 4f;				
	
	// A mask determining what is ground to the character
	[SerializeField] LayerMask whatIsGround;	

	int counter = 0;
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
	
}
