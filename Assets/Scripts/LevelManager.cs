using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	Transform start;
	[SerializeField]
	Transform end;
	[SerializeField]
	BoxCollider2D[] deathzones;
}
