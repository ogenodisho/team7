using UnityEngine;
using System.Collections;

public class MissileTrail : MonoBehaviour {
	
	public int moveSpeed = 230;
	private bool facingRight = true;

	void Start()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		PlatformerCharacter2D playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
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
		Destroy (gameObject, 1);
	}
}