using UnityEngine;

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
	
	float timeToFire = 0;
	Transform idleFirePoint;
	Transform runningFirePoint;

	private Quaternion rotateOneEightyAroundZ;

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

		rotateOneEightyAroundZ = new Quaternion(0, 0, 1, 0);

	}

	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

		anim.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));

		//Debug.Log (firePoint.position.x + " " + firePoint.position.y);
	}


	public void Move(float move, bool crouch, bool jump)
	{


		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);

		//only control the player if grounded or airControl is turned on
		if(grounded || airControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move * crouchSpeed : move);

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			//anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

        // If the player should jump...
        if (grounded && jump) {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);
            rigidbody2D.AddForce(new Vector2(0f, 100f));
        }
	}

	public void Jump() {
		// If the player should jump...
		if (grounded) {
			// Add a vertical force to the player.
			anim.SetBool("Ground", false);
			rigidbody2D.AddForce(new Vector2(0f, 120f));
		}
	}

	public void Shoot () {
		float currSpeed = anim.GetFloat("Speed");
		Transform firePoint;
		if (currSpeed > 2) {
			firePoint = runningFirePoint;
		} else {
			firePoint = idleFirePoint;
		}
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		Vector2 mousePosition = new Vector2 (facingRight ? firePoint.position.x + 100 : firePoint.position.x - 100, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, 100, whatToHit);
		Effect();
		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
			Debug.Log ("We hit " + hit.collider.name + " and did " + Damage + " damage.");
		} else {
			Debug.DrawLine (firePointPosition, (mousePosition-firePointPosition)*100, Color.red);
		}
	}

	void Effect() {
		float currSpeed = anim.GetFloat("Speed");
		Transform firePoint;
		if (currSpeed > 2) {
			firePoint = runningFirePoint;
		} else {
			firePoint = idleFirePoint;
		}
		if (facingRight) {
			Instantiate (MissilePrefab, firePoint.position, firePoint.rotation);
		} else {
			// If Agent_7 is not facing right, rotate the prefab 180 around the z-axis
			Instantiate (MissilePrefab, firePoint.position, rotateOneEightyAroundZ * firePoint.rotation);
		}
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
