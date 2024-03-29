using UnityEngine;
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

	public Transform damagePickup;
	public Transform fireRatePickup;
	public Transform invulnerabilityPickup;
	public Transform healthPickup;

	PlatformerCharacter2D playerScript;
	Agent7ControlUI controlUI;

	DoorScript door;

	public int health = 2;

	public Texture tex;

	int counter = 0;	

	Animator anim;

	public int getHealth() {
		return health;
	}

	void Awake() {
		GameObject thePlayer = GameObject.Find("Agent_7");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		controlUI = thePlayer.GetComponent<Agent7ControlUI> ();
		anim = GetComponent<Animator>();
		thePlayer = GameObject.Find ("Door");
		door = thePlayer.GetComponent<DoorScript> ();
	}


	// Update is called once per frame (FixedUpdate for rigidbody)
	void FixedUpdate () {
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

	/*void OnGUI() {
		Vector3 screenpos = GameObject.Find("Agent_7Camera").camera.WorldToScreenPoint(
			GameObject.Find (gameObject.name).transform.position);
		for (int i = 0; i < GameObject.Find (gameObject.name).GetComponent<Enemy1Behaviour>().getHealth(); i++) {
			GUI.DrawTexture(new Rect(screenpos.x + ((50 / 6) * i), screenpos.y, 50/6, 10), tex);
		}
	}*/

	// Invert the character's position about its vertical axis
	void Flip ()
	{
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.name.StartsWith("Missile")) {
			// Destroy the missile and generate the explosion at the missile's position
			explosion = (Transform)Instantiate (ExplosionPrefab, collision.collider.transform.position, collision.collider.transform.rotation);
			Destroy (explosion.gameObject, 0.4f);
			Destroy(collision.collider.gameObject);

			// decrement health
			if (collision.collider.name.Contains("Super")) {
				health -= 2;
			} else {
				health -= 1;
			}
			anim.SetBool ("GotHit", true);
			if (health <= 0) {
				// Destroy the enemy if his health is 0 and gain score
				Destroy (transform.gameObject);
				playerScript.GainScore(50);
				AchievementManager.Instance.RegisterEvent(AchievementType.Enemy);
				if(transform.localScale.y == 2 && controlUI.getCurrentLevel() == 1){
					door.OpenDoor();
				}
				if(transform.localScale.y == 2.5 && controlUI.getCurrentLevel() == 2) {
					door.OpenDoor();
				}
				//Drop random item
				System.Random rng = new System.Random();
				switch (rng.Next(1,11)) {
				case 1:
					Instantiate (healthPickup, transform.position, transform.rotation);
					break;
				case 2:
					Instantiate (damagePickup, transform.position, transform.rotation);
					break;
				case 3:
					Instantiate (fireRatePickup, transform.position, transform.rotation);
					break;
				case 4:
					Instantiate (invulnerabilityPickup, transform.position, transform.rotation);
					break;
				default:
					break;
				}

			}
		}
	}
	
}
