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
		Vector3 newPos = Vector2.Lerp(t.position, playerTransform.position, speed * Time.deltaTime);
		newPos.z = t.position.z;
		t.position = newPos;
	}
}
