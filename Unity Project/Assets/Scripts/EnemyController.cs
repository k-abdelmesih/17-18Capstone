using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private Rigidbody2D rb2d;
	public PlayerController playercontroller;
	public float moveForce = 100f;
	public int EnemyCount;
	private float hitForce = 200f;
	private Animator anim;
	public float health = 100;
	public bool isDead = false;
	public bool attacking = false;
	public bool facingRight = true;
	public bool inHitStun = false;
	public float enemyDistance = 50f;
	public float enemyNear = 2.5f;
	public float delta;
	public BoxCollider2D attack;
	private bool inLauncher = false;
	public float deltaY;
	// Update is called once per frame
	void Update ()
	{
		
		GameObject thePlayer2 = GameObject.Find ("GW");
		PlayerController playercontroller2 = thePlayer2.GetComponent<PlayerController> ();
		deltaY = Mathf.Abs (playercontroller2.rb2d.position.y - rb2d.position.y);
		delta = Mathf.Abs (playercontroller2.rb2d.position.x - rb2d.position.x);

		if (delta < enemyNear && !inHitStun && !attacking) {
			
			StartCoroutine (Attack ());

		} 	
		 
		if ((playercontroller2.rb2d.position.x > rb2d.position.x) && !inHitStun && delta < 10 && deltaY < 7) {
			if (!facingRight) {
				flip ();
			}
			anim.SetTrigger ("Walk");
			rb2d.velocity = (new Vector2 (0, rb2d.velocity.y));
			rb2d.AddForce (new Vector2 (100f, 0f));
		}
		if (playercontroller2.rb2d.position.x < rb2d.position.x && !inHitStun && delta < 10 && deltaY < 7) {
			if (facingRight) {
				flip ();
			}
			anim.SetTrigger ("Walk");
			rb2d.velocity = (new Vector2 (0, rb2d.velocity.y));
			rb2d.AddForce (new Vector2 (-100f, 0f));
		}



	}

	void Awake ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		rb2d.simulated = true;
		rb2d.freezeRotation = true;
		rb2d.mass = 1;
		rb2d.tag = "Enemy";
		attack.enabled = false;
		attack.isTrigger = true;
		attack.tag = "eAttack";
	}

	void flip ()
	{
		facingRight = !facingRight; 
		Vector3 theScale = transform.localScale; //teScale is temporary variable
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void enemyMoveRight ()
	{
		if (!attacking) {
			anim.SetFloat ("Speed", 1);
		}
		rb2d.velocity = (new Vector2 (0, rb2d.velocity.y));
		rb2d.AddForce (new Vector2 (moveForce, 0));
	}

	public void enemyMoveLeft ()
	{
		if (!attacking) {
			anim.SetFloat ("Speed", 1);
		}
		rb2d.velocity = (new Vector2 (0, rb2d.velocity.y));
		rb2d.AddForce (new Vector2 (-moveForce, 0));
	}

	IEnumerator death (Collider2D coll)
	{
		anim.SetTrigger ("Death");
		FindObjectOfType<SoundManagerScript> ().PlaySound ("enemyDeath");
		yield return new WaitForSeconds (1);
		FindObjectOfType<GameManager> ().increaseScrap (53);
		rb2d.gameObject.SetActive (false);
		isDead = true;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.CompareTag ("Player")) {
			coll.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 0));
		}  
	}

	IEnumerator waitForHitstun ()
	{
		inHitStun = true;
		yield return new WaitForSeconds (2);
		if (!inLauncher) {
			inHitStun = false;
		}
	}

	IEnumerator waitForHitstunL ()
	{
		inHitStun = true;
		inLauncher = true; 
		yield return new WaitForSeconds (5);
		inLauncher = false; 
		inHitStun = false;
	}

	IEnumerator Attack ()
	{
		attacking = true;
		FindObjectOfType<SoundManagerScript> ().PlaySound ("attack");
		anim.SetTrigger ("Attack");
		attack.enabled = true;
		yield return new WaitForSeconds (0.5f);
		attack.enabled = false;
		attacking = false;
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Attack")) {
			anim.SetTrigger ("Damage");
			GameObject thePlayer = GameObject.Find ("GW");
			PlayerController playercontroller = thePlayer.GetComponent<PlayerController> ();
			StartCoroutine (waitForHitstun ());
			float d = getDirection (playercontroller.facingRight);
			rb2d.AddForce (new Vector2 (rb2d.velocity.x + d * hitForce, rb2d.velocity.y + d * hitForce));
			health -= 25;
			if (health <= 0) {
				StartCoroutine (death (coll));
			}
		}
		if (coll.gameObject.CompareTag ("Attack2") || coll.gameObject.CompareTag ("Attack3")) {
			anim.SetTrigger ("Damage");
			StartCoroutine (waitForHitstun ());
			rb2d.AddForce (new Vector2 (0, 0));
		}
		if (coll.gameObject.CompareTag ("Launcher")) {
			anim.SetTrigger ("Damage");
			StartCoroutine (waitForHitstunL ());
			rb2d.AddForce (new Vector2 (10f, 400f));
		}

		if (coll.gameObject.CompareTag ("Jump Attack")) {
			anim.SetTrigger ("Damage");
			StartCoroutine (waitForHitstun ());
			rb2d.AddForce (new Vector2 (0f, 0f));
		}




	}

	float getDirection (bool direction)
	{
		float s;
		if (direction) {
			s = -1.0f;
		} else {
			s = 1.0f;
		}
		return -s;
	}

}
