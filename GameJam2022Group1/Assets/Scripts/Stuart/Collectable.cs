using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace StuartH
{

	/// <summary>
	///Collectable - Provide collectable movement
	/// </summary>
	public class Collectable : MonoBehaviour
	{
		[SerializeField] private float speed = 10f;

		private void Start() => transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360));

		private void Update()=>transform.Rotate(Vector3.up, speed * Time.deltaTime);
		
	}
}
