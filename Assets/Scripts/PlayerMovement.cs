using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	int jumps;
	[SerializeField]
	float speed;
	[SerializeField]
	float jumpForce;
	[SerializeField]
	float fallMultiplier;
	[SerializeField]
	float maxWallSlideFallSpeed;
	[SerializeField]
	Rigidbody2D rb;

	HashSet<Collider2D> groundColliders;
	HashSet<Collider2D> wallColliders;
	int remainingJumps;
	bool pressingJump;
	bool holdingJump;
	bool isGrounded;
	bool isAgainstWall;
	Transform t;
	Vector3 direction;
	Animator animator;
	[SerializeField]

	void Awake()
	{
		groundColliders = new HashSet<Collider2D>();
		wallColliders = new HashSet<Collider2D>();
		t = GetComponent<Transform>();
		animator = GetComponent<Animator>();
	}

	void Start()
	{
		remainingJumps = jumps;
	}

	void Update()
	{
		//GET JUMP INPUT
		if (Input.GetButtonUp("Jump"))
		{
			holdingJump = false;
		}
		//GET MOVE INPUT
		direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
		//FLIP PLAYER SPRITE
		if (direction.normalized.x != 0)
		{
			t.localScale = new Vector3(direction.normalized.x, 1f, 1f);
		}
	}

	void FixedUpdate()
	{
		//JUMP
		if (pressingJump)
		{
			pressingJump = false;
			if (CanJump())
			{
				rb.velocity = new Vector2(rb.velocity.x, jumpForce);
				remainingJumps--;
			}
		}
		//MOVE
		rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
		//BETTER JUMP
		if (rb.velocity.y < 0 || (rb.velocity.y > 0 && !holdingJump))
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
			if (remainingJumps == jumps && !IsWallSliding())
			{
				remainingJumps = jumps - 1;
			}
		}
		if (IsWallSliding() && rb.velocity.y < maxWallSlideFallSpeed)
		{
			rb.velocity = new Vector2(rb.velocity.x, maxWallSlideFallSpeed);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			foreach (ContactPoint2D contactPoint in collision.contacts)
			{
				Debug.Log(contactPoint.normal);
				if (contactPoint.normal.y > 0)
				{
					//ON TOP OF GROUND
					Debug.Log("ON GROUND");
					groundColliders.Add(collision.collider);
					isGrounded = true;
					ResetJumpCounter();
				}
				if (!Mathf.Approximately(contactPoint.normal.x, 0f))
				{
					//CONTACT WITH SIDE OF WALL
					Debug.Log("WALL SLIDE");
					wallColliders.Add(collision.collider);
					isAgainstWall = true;
					ResetJumpCounter();
				}
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{				
			if (wallColliders.Contains(collision.collider))
			{
				Debug.Log("OUT OF WALL SLIDE");
				wallColliders.Remove(collision.collider);
				if (wallColliders.Count == 0)
				{
					isAgainstWall = false;
				}
			}
			if (groundColliders.Contains(collision.collider))
			{
				Debug.Log("OUT OF GROUND");
				groundColliders.Remove(collision.collider);
				if (groundColliders.Count == 0)
				{
					isGrounded = false;
				}
			}
		}
	}

	public bool CanJump()
	{
		return remainingJumps > 0;
	}

	public void ResetJumpCounter()
	{
		remainingJumps = jumps;
	}

	public bool IsGrounded()
	{
		return isGrounded;
	}

	public bool IsWallSliding()
	{
		return !isGrounded && isAgainstWall;
	}

	public void Jump()
	{
		pressingJump = true;
		holdingJump = true;
	}

	public void EndJump()
	{
		holdingJump = false;
	}

	public float GetVerticalVelocity()
	{
		return rb.velocity.y;
	}
}