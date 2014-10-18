using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour {

	private bool collected = false;
	private float respawnTimer = 30f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (collected) {
			respawnTimer -= Time.deltaTime;
			if (respawnTimer <= 0f) {
				respawnTimer = 30f;
				gameObject.renderer.enabled = true;
				collected = false;
			}
		}
	}

	public void setCollected () {
		collected = true;
		gameObject.renderer.enabled = false;
	}
}
