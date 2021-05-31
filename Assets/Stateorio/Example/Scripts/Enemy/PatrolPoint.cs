using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates of the enemy's patrol route point.
/// Isn't shown during play.
/// </summary>
public class PatrolPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Renderer R in GetComponents<Renderer>())
			R.enabled = false;
	}

	public Vector3 Position {
		get { return gameObject.transform.position; }
	}
}
