using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	Animator anim;
	bool open;
	
	void Awake () {
		open = false;
		anim = GetComponent<Animator> ();
		GetComponent<Collider2D> ().isTrigger = false;
	}

	void FixedUpdate () {

	}

	public void OpenDoor () {
		anim.SetBool ("Open", true);
		GetComponent<Collider2D> ().isTrigger = true;
	}
}
