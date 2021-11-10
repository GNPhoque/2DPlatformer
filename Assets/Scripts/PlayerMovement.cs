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
	Rigidbody2D rb;

	HashSet<Collider2D> groundColliders;
	int remainingJumps;
	bool pressingJump;
	bool holdingJump;
	bool isGrounded;
	bool isWallSliding;
	Transform t;
	Vector3 direction;
	Animator animator;
	[SerializeField]
	PlayerAnimState animState;

	void Awake()
	{
		groundColliders = new HashSet<Collider2D>();
		t = GetComponent<Transform>();
		animator = GetComponent<Animator>();
	}

	void Start()
	{
		remainingJumps = jumps;
		animState = PlayerAnimState.IDLE;
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
		//Debug.Log("isGrounded : " + isGrounded);
		//Debug.Log("JUMP : " + holdingJump);
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
			if (remainingJumps == jumps && !isGrounded)
			{
				remainingJumps = jumps - 1;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		foreach (ContactPoint2D contactPoint in collision.contacts)
		{
			Debug.Log(contactPoint.normal);
			if (contactPoint.normal.y > 0)
			{
				//ON TOP OF GROUND
				Debug.Log("ON GROUND");
			}
			else if (!Mathf.Approximately(contactPoint.normal.x, 0f))
			{
				//CONTACT WITH SIDE OF WALL
				Debug.Log("WALL SLIDE");
			}
		}
		if (collision.gameObject.CompareTag("Ground"))
		{
			groundColliders.Add(collision.collider);
			isGrounded = true;
			ResetJumpCounter();
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			if (groundColliders.Contains(collision.collider))
			{
				groundColliders.Remove(collision.collider);
				if (groundColliders.Count == 0)
				{
					isGrounded = false;
				}
			}
		}
	}

	void SetAnimation()
	{
		PlayerAnimState tmp = animState;
		if (remainingJumps == jumps)//Au sol
		{
			if ((rb.velocity.x < -.1f || rb.velocity.x > .1f) && animState != PlayerAnimState.RUN)
			{
				tmp = PlayerAnimState.RUN;
			}
			else if (rb.velocity.x > -.1f && rb.velocity.x < .1f && animState != PlayerAnimState.IDLE)
			{
				tmp = PlayerAnimState.IDLE;
			}
		}
		else
		{
			if (rb.velocity.y > .01f && animState != PlayerAnimState.JUMP)
			{
				tmp = PlayerAnimState.JUMP;
			}
		}
		if (rb.velocity.y < -.01f && animState != PlayerAnimState.FALL)
		{
			tmp = PlayerAnimState.FALL;
		}
		if (tmp != animState)
		{
			animState = tmp;
			switch (animState)
			{
				case PlayerAnimState.IDLE:
					animator.SetTrigger("Idle");
					break;
				case PlayerAnimState.RUN:
					animator.SetTrigger("Run");
					break;
				case PlayerAnimState.JUMP:
					animator.SetTrigger("Jump");
					break;
				case PlayerAnimState.FALL:
					animator.SetTrigger("Fall");
					break;
				default:
					break;
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