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

	int remainingJumps;
	bool pressingJump;
	bool holdingJump;
	Transform t;
	Vector3 direction;

	void Awake()
	{
		t = GetComponent<Transform>();
	}

	void Start()
	{
		remainingJumps = jumps;
	}

	void Update()
	{
		//GET JUMP INPUT
		if (Input.GetButtonDown("Jump"))
		{
			pressingJump = true;
		}
		holdingJump = Input.GetButton("Jump");
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
			if (remainingJumps > 0)
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
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			remainingJumps = jumps;
		}
	}
}