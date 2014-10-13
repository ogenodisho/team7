using UnityEngine;

/** This class takes care of Agent_7's functionality.
 * It also handles his collisions with other objects.
 * 
 * */
public class PlatformerCharacter2D : MonoBehaviour 
{
	public bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	

	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character

	public Transform MissilePrefab;

	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.


	// shooting variables
	public float fireRate = 0;
	public float Damage = 10;
	public LayerMask whatToHit;

	private bool oozed = false;
	
	float timeToFire = 0;
	Transform idleFirePoint;
	Transform runningFirePoint;


	public AudioSource audio;
	public AudioClip jump;


	private Quaternion rotateOneEightyAroundZ;
	
	Agent7StatsUI statsUi;

	//// Following taken from this forum answer - http://answers.unity3d.com/questions/427191/how-to-shake-phone-using-c.html
	public float avrgTime = 0.5f;
	public float peakLevel = 0.6f;
	public float endCountTime = 0.6f;
	public int shakeDir;
	public int shakeCount;
	
	Vector3 avrgAcc = Vector3.zero;
	int countPos;
	int countNeg;
	int lastPeak;
	int firstPeak;
	bool counting;
	float timer;
	bool ShakeDetector(){
		// read acceleration:
		Vector3 curAcc = Input.acceleration; 
		// update average value:
		avrgAcc = Vector3.Lerp(avrgAcc, curAcc, avrgTime * Time.deltaTime);
		// calculate peak size:
		curAcc -= avrgAcc;
		// variable peak is zero when no peak detected...
		int peak = 0;
		// or +/- 1 according to the peak polarity:
		if (curAcc.y > peakLevel) peak = 1;
		if (curAcc.y < -peakLevel) peak = -1;
		// do nothing if peak is the same of previous frame:
		if (peak == lastPeak) 
			return false;
		// peak changed state: process it
		lastPeak = peak; // update lastPeak
		if (peak != 0){ // if a peak was detected...
			timer = 0; // clear end count timer...
			if (peak > 0) // and increment corresponding count
				countPos++;
			else
				countNeg++;
			if (!counting){ // if it's the first peak...
				counting = true; // start shake counting
				firstPeak = peak; // save the first peak direction
			} else {
			}
		} 
		else // but if no peak detected...
		if (counting){ // and it was counting...
			timer += Time.deltaTime; // increment timer
			if (timer > endCountTime){ // if endCountTime reached...
				counting = false; // finish counting...
				shakeDir = firstPeak; // inform direction of first shake...
				if (countPos > countNeg) // and return the higher count
					shakeCount = countPos;
				else
					shakeCount = countNeg;
				// zero counters and become ready for next shake count
				countPos = 0;
				countNeg = 0;
				return true; // count finished
			}
		}
		return false;
	}
	///////


	void Start() {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");

		anim = GetComponent<Animator>();

		// set up shooting
		idleFirePoint = transform.FindChild ("IdleFirePoint");
		runningFirePoint = transform.FindChild ("RunningFirePoint");

		if (idleFirePoint == null) {
			Debug.LogError ("No idleFirePoint? WHAT?!");
		}
		if (runningFirePoint == null) {
			Debug.LogError ("No runningFirePoint? WHAT?!");
		}

		statsUi = GetComponent<Agent7StatsUI>();

		rotateOneEightyAroundZ = new Quaternion(0, 0, 1, 0);



	}

	void Update() {
		ShakeDetector ();
		if (counting) {
			setOozed (false);
			counting = false;
		}
	}

	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		if (grounded) {
			anim.SetBool("ScaleJump", false);
		}

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

		anim.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));

	}

	public void Move(float move, bool crouch, bool jump)	{
		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(move > 0 && !facingRight) {
			// ... flip the player.
			Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
		} else if(move < 0 && facingRight)
			// ... flip the player.
			Flip();
	}

	public void Jump() {
		// If the player should jump...

		if (grounded) {
			// Add a vertical force to the player.
			anim.SetBool("Ground", false);
			
			rigidbody2D.AddForce(new Vector2(0f, 1200f));
			
			AchievementManager.Instance.RegisterEvent (AchievementType.Jump);
			audio.PlayOneShot (jump);
		}
	}

	public void setScaling(bool scaling) {
		anim.SetBool("Scale", scaling);
		if (scaling) {
			anim.SetBool("ScaleJump", false);
		} else {
			anim.SetBool("ScaleJump", true);
		}
	}

	public void setOozed(bool oozed) {
		this.oozed = oozed;
		if (oozed) {
			// restrict movement
			transform.rigidbody2D.drag = 100f;
		} else {
			// revert movement
			transform.rigidbody2D.drag = 3f;
		}
	}
	public bool getOozed() {
		return oozed;
	}


	public void ScaleJump() {
		anim.SetBool("Ground", false);
		//anim.SetBool("ScaleJump", true);
		if (facingRight) {
			rigidbody2D.AddForce(new Vector2(1500f, 1000f));
		} else {
			rigidbody2D.AddForce(new Vector2(-1500f, 1000f));
		}
		
		// achievements register one jump - maybe make this a scale jump achievement
		AchievementManager.Instance.RegisterEvent (AchievementType.Jump);
		audio.PlayOneShot (jump);
	}

	public void Shoot () {
		// Agent_7's gun is higher up when he's running
		// so calculate his speed to determine the height
		// to fire the missile from for realism
		float currSpeed = anim.GetFloat("Speed");
		Transform firePoint;

		if (currSpeed > 2) {
			firePoint = runningFirePoint;
		} else {
			firePoint = idleFirePoint;
		}

		// Create the missile.
		if (facingRight) {
			Instantiate (MissilePrefab, firePoint.position, firePoint.rotation);
		} else {
			// If Agent_7 is not facing right, rotate the missile prefab 180 around the z-axis
			Instantiate (MissilePrefab, firePoint.position, rotateOneEightyAroundZ * firePoint.rotation);
		}
		audio.Play ();
		// achievements
		AchievementManager.Instance.RegisterEvent (AchievementType.Shoot);
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

	// The following 4 methods are for
	// increasing and decreasing hp and score
	public void LoseHealth() {
		// only lose health if agent7 is not invulnerable
		if (!Agent7ControlUI.hasInvulnerabilityPickup) {
			statsUi.setHp (statsUi.getHp() - 1);
		}
	}

	public void GainHealth() {
		statsUi.setHp (statsUi.getHp() + 1);
	}

	public void LoseScore(int amount) {
		if (!Agent7ControlUI.hasInvulnerabilityPickup) {
			statsUi.setScore (statsUi.getScore() - amount);
		}
	}
	
	public void GainScore(int amount) {
		statsUi.setScore (statsUi.getScore() + amount);
	}
}
