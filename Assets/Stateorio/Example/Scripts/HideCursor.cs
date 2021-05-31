using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Hides the cursor when the game starts.
public class HideCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
