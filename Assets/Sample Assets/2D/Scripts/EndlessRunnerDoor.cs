using UnityEngine;
using System.Collections;

public class EndlessRunnerDoor : MonoBehaviour {
	
	Animator anim;
	bool open;
	
	void Awake () {
		open = true;
		anim = GetComponent<Animator> ();
		anim.SetBool ("Open", true);
		GetComponent<Collider2D> ().isTrigger = true;
	}
	
	void FixedUpdate () {
		
	}
	
	public void OpenDoor () {
		anim.SetBool ("Open", true);
		GetComponent<Collider2D> ().isTrigger = true;
	}
}
