using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	Transform playerTransform;
	[SerializeField]
	float speed;

	Transform t;

	private void Awake()
	{
		t = GetComponent<Transform>();
	}

	private void FixedUpdate()
	{
		//Vector3 newPos = Vector2.Lerp(t.position, playerTransform.position, speed * Time.deltaTime);
		//newPos.z = t.position.z;
		//t.position = newPos;
		if (playerTransform.position.x < -16 || playerTransform.position.x > 0)
		{
			float x = Mathf.Lerp(t.position.x, playerTransform.position.x, speed * Time.deltaTime);
			t.position = new Vector3(x, t.position.y, t.position.z); 
		}
	}
}
