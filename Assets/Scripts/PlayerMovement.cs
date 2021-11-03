using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    Rigidbody2D rb;

    Vector3 direction;

	void Update()
    {
        float dir = Input.GetAxisRaw("Horizontal");
        direction = new Vector3(dir, 0f, 0f);
    }

	void FixedUpdate()
	{
		rb.velocity = direction * speed;
	}
}
