﻿using UnityEngine;
using System.Collections;

public class Enemy1Behaviour : MonoBehaviour {

	// Variable determining the direction the enemy is facing, initially 'left'
	bool facingRight = false;							

	// The fastest the player can travel in the x axis.
	[SerializeField] float maxSpeed = 4f;				
	
	// A mask determining what is ground to the character
	[SerializeField] LayerMask whatIsGround;			
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Setter for top speed of enemy
	void setTopSpeed(float speed) {
			maxSpeed = speed;
	}
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
}
