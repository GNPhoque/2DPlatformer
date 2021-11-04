using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoomTrigger : MonoBehaviour
{
	[SerializeField]
	Transform cameraTransform;
	[SerializeField]
	Transform secretCameraPosition;

	bool status;
	CameraFollow cameraFollow;
	Vector3 oldPos = new Vector3(0f, 0f, 10f);

	private void Awake()
	{
		cameraFollow = cameraTransform.GetComponent<CameraFollow>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		cameraFollow.isInSecretRoom = true;
		Debug.Log(cameraFollow.isInSecretRoom);
		if (cameraFollow.isInSecretRoom)
		{
			oldPos = cameraTransform.position;
			cameraTransform.position = secretCameraPosition.position;
		}
		else
		{
			cameraTransform.position = oldPos;
			//cameraTransform.position = oldPos ?? new Vector3(0f, 0f, 10f);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		cameraFollow.isInSecretRoom = false;
		cameraTransform.position = oldPos;
	}
}
