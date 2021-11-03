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
    Rigidbody2D rb;
    [SerializeField]
    bool isGrounded;

    int remainingJumps;
	bool jumpInput;
    Vector3 direction;

	void Start()
	{
		remainingJumps = jumps;
	}

	void Update()
    {
		if (Input.GetButtonDown("Jump"))
		{
			Debug.Log("Jump input down");
			jumpInput = true;
		}
        float dir = Input.GetAxisRaw("Horizontal");
        direction = new Vector3(dir, 0f, 0f);
    }

	void FixedUpdate()
	{
		if (jumpInput)
		{
			Debug.Log("Fixed update Jump ");
			if (remainingJumps > 0)
			{
				rb.AddForce(Vector2.up * jumpForce); 
				Debug.Log("AddForce");
			}
			jumpInput = false;
		}
		rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
            isGrounded = true;
			remainingJumps = jumps;
		}
	}
}
