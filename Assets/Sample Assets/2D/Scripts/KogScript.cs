using UnityEngine;
using System.Collections;

public class KogScript : MonoBehaviour {
	
	// Variable determining the direction the enemy is facing, initially 'left'
	bool facingRight = true;							
	
	// The fastest the player can travel in the x axis.
	[SerializeField] float maxSpeed = 3f;				
	
	// A mask determining what is ground to the character
	[SerializeField] LayerMask whatIsGround;

	public Transform OozePrefab;

	Transform oozeFirePoint;

	PlatformerCharacter2D playerScript;

	DoorScript door;

	public Transform ExplosionPrefab;
	private Transform explosion;

	private int health = 100;

	public Texture hpFull;
	public Texture hpEmpty;

	float attackTimer = 4.8f;
	float animTimer = 0.8f;
	bool startAnim = false;
	bool instantiated = false;

	Animator anim;	

	public int getHealth() {
		return health;
	}

	void Awake() {
		anim = GetComponent<Animator>();
		GameObject thePlayer = GameObject.Find("Agent_7");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		thePlayer = GameObject.Find ("Door");
		door = thePlayer.GetComponent<DoorScript> ();
		oozeFirePoint = transform.FindChild ("OozeFirePoint");
		if (playerScript.transform.position.x > transform.position.x) {
			facingRight = true; //agent in front of kog
		} else {
			Flip ();
		}
	}

	// Update is called once per frame (FixedUpdate for rigidbody)
	void Update () {
		if (playerScript.transform.position.x > transform.position.x) { // Agent 7 to right of Kog
			if (!facingRight) { // Kog should be facing right
				Flip ();
			}
		} else { // Agent 7 to left of Kog
			if (facingRight) { // Kog should be facing left
				Flip ();
			}
		}
	}

	void FixedUpdate() {
		// Dont interact if too far away or if agent_7 is already oozed
		if (Mathf.Abs(playerScript.transform.position.x - transform.position.x) >= 20f 
		    || playerScript.getOozed()) {
			anim.SetBool("Attack", false);
			return;
		}

		// control how often kog attacks. In this case its 3 seconds.
		// count three seconds then start the animation.
		attackTimer -= Time.deltaTime;
		if (attackTimer <= 0) {
			attackTimer = 4.8f; // reset the attack timer
			anim.SetBool("Attack", true);
			startAnim = true;
		}

		// When the animation has started, Instantiate the goo prefab half
		// way through the animation because it looks more realistic that way.
		// Instantiate it once other wise many will be instantiated from 0 to 0.4f seconds.
		// Also, don't instantiate it if agent7 is already oozed because that's too strong
		if (startAnim) {
			animTimer -= Time.deltaTime;
			if (animTimer <= 0.4f && !instantiated) {
				Instantiate (OozePrefab, oozeFirePoint.position, oozeFirePoint.rotation);
				instantiated = true;
			} else if (animTimer <= 0f) { // The animation has ended, set attacking to false
				animTimer = 0.8f; // reset the animation timer
				anim.SetBool("Attack", false);
				startAnim = false;
				instantiated = false;
			}
		}
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
			explosion = (Transform)Instantiate (ExplosionPrefab, collision.transform.position, collision.transform.rotation);
			Destroy (explosion.gameObject, 0.4f);
			Destroy(collision.collider.gameObject);

			// decrement the health
			if (collision.collider.name.Contains("Super")) {
				health -= 2;
			} else {
				health -= 1;
			}
			if (health <= 0) {
				// Destroy the enemy if his health is 0 and gain score
				Destroy (transform.gameObject);
				playerScript.GainScore(200);
				door.OpenDoor();
			}
		}
	}

	void OnGUI() {
		Vector3 screenpos = GameObject.Find("Agent_7Camera").camera.WorldToScreenPoint(
			GameObject.Find (gameObject.name).transform.position);
		int currHP = GameObject.Find (gameObject.name).GetComponent<KogScript>().getHealth();
		for (int i = 0; i < 10; i++) {
			if (i * 10 < currHP) {
				GUI.DrawTexture(new Rect(screenpos.x - 30 + ((50 / 6) * i), screenpos.y - 120, 50/6, 10), hpFull);
			} else {
				GUI.DrawTexture(new Rect(screenpos.x - 30 + ((50 / 6) * i), screenpos.y - 120, 50/6, 10), hpEmpty);
			}
		}
	}

}