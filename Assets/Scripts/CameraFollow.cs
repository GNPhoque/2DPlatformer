using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	Transform playerTransform;
	[SerializeField]
	float speed;

	public bool isInSecretRoom { get; set; }
	Transform t;

	private void Awake()
	{
		t = GetComponent<Transform>();
	}

	private void Start()
	{
		isInSecretRoom = false;
	}

	private void FixedUpdate()
	{
		if (!isInSecretRoom)
		{
			if (playerTransform.position.x > 0)
			{
				float x = Mathf.Lerp(t.position.x, playerTransform.position.x, speed * Time.deltaTime);
				t.position = new Vector3(x, t.position.y, t.position.z);
			}

		}
	}
}
