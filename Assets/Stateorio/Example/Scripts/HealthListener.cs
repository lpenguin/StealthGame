using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Updates Text to show the player's current HP.
[RequireComponent (typeof (Text))]
public class HealthListener : MonoBehaviour {

	private Text healthText;
	public PlayerHealth PlayerHealth;

	// Use this for initialization
	void Start () {
		healthText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerHealth.IsDead ())
			healthText.text = "DEAD!!!";
		else
			healthText.text = "Health: " + PlayerHealth.Health;
	}
}
