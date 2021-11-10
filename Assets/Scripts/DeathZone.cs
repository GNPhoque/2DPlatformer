using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	[SerializeField]
	Transform respawnPoint;
	[SerializeField]
	Transform player;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			player.position = respawnPoint.position;
		}
	}
}
