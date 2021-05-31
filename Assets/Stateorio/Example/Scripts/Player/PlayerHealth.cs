using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Holds the current player's HP value.
public class PlayerHealth : MonoBehaviour {

	public int StartingHealth;

	private int health;

	// Use this for initialization
	void Start () {
		health = StartingHealth;
	}

	public int Health {
		get { return health; }
		set {
			health = value;
			if (IsDead ()) {
				PlayerMovementController movement = GetComponent<PlayerMovementController> ();
				if (movement != null)
					movement.enabled = false;

				PlayerShootController shoot = GetComponent<PlayerShootController> ();
				if (shoot != null)
					shoot.enabled = false;
			}
		}
	}

	public bool IsDead() {
		return Health <= 0;
	}
}
