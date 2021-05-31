using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Pressing ESCAPE will trigger application quit.
public class EscapeQuitsGame : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit();
	}
}
