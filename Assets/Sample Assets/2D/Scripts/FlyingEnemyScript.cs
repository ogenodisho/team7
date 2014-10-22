using UnityEngine;
using System.Collections;

public class FlyingEnemyScript : MonoBehaviour {
	
	public float moveSpeed = 1f;
	private bool facingRight = true;
	private float agentSevensPreviousDrag;
	PlatformerCharacter2D playerScript;
	public Transform ExplosionPrefab;
	private Transform explosion;
	private Vector3 translation;
	Animator anim;
	public int health = 1;
	private bool activated = false;
	bool agent7GotOozed = false;
	private bool skyfall = false;

	private bool soundOnceDROP = false;
	private bool soundOnceFLY = false;

	public Transform damagePickup;
	public Transform fireRatePickup;
	public Transform invulnerabilityPickup;
	public Transform healthPickup;

	public AudioSource audio;
	public AudioClip fly;
	public AudioClip drop;
	public int getHealth() {
		return health;
	}

	void Awake()
	{
		GameObject thePlayer = GameObject.Find("Agent_7");
		playerScript = thePlayer.GetComponent<PlatformerCharacter2D>();
		facingRight = playerScript.facingRight;
		anim = GetComponent<Animator>();
		if (playerScript.transform.position.x > transform.position.x) {
			facingRight = true; //agent in front of kog
		} else {
			Flip ();
		}
	}
	
	void Update () {
		if (Mathf.Abs(playerScript.transform.position.x - transform.position.x) <= 1f) {
			skyfall = true;
			anim.SetBool("skyfall", true);
		}
	}

	void FixedUpdate() {
		if (Mathf.Abs(playerScript.transform.position.x - transform.position.x) > 10f || (transform.position.y - playerScript.transform.position.y) > 5f) {
			if (!activated) {
				return;
			}
		} else {
			activated = true;
		}
		if (!skyfall) {
			if(!audio.isPlaying){
				audio.PlayOneShot(fly,0.25f);
				//		audio.Play ();
			}
			translation = playerScript.transform.position - transform.position;
			translation.x = facingRight ? 5f : -5f;
			translation.y = 0;
			transform.Translate (translation * Time.deltaTime * moveSpeed);
		} else {
			while(soundOnceDROP == false){
				audio.Stop();
				audio.PlayOneShot(drop, 0.25f);
				soundOnceDROP = true;
			}
			transform.Translate (new Vector3(0, -5f, 0) * Time.deltaTime * moveSpeed);
		}
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
			if (health <= 0) {
				// Destroy the enemy if his health is 0 and gain score
				Destroy (transform.gameObject);
				playerScript.GainScore(50);
				AchievementManager.Instance.RegisterEvent(AchievementType.Enemy);
				audio.Stop ();

				//Drop random item
				System.Random rng = new System.Random();
				switch (rng.Next(1,11)) {
				case 1:
					Instantiate (damagePickup, transform.position, transform.rotation);
					break;
				case 2:
					Instantiate (healthPickup, transform.position, transform.rotation);
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
		} else if (collision.collider.name.Equals("Agent_7")) {
			// the flying enemy suicided into agent 7. You lose hp and you get no score.
			explosion = (Transform)Instantiate (ExplosionPrefab, collision.collider.transform.position, collision.collider.transform.rotation);
			Destroy (explosion.gameObject, 0.4f);
			Destroy (transform.gameObject);
			audio.Stop ();
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
}
