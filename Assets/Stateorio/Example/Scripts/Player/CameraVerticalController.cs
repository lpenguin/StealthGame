using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Moving the mouse up/down causes camera to move.
public class CameraVerticalController : MonoBehaviour {

	public float Sensitivity;

	[Range(0, 360)]
	public float AngularRange;

	private float startingPitch;
	private float upper, lower;

	// Use this for initialization
	void Start() {
		startingPitch = Pitch;

		if (AngularRange <= 0 || AngularRange >= 360) {
			lower = upper = startingPitch;
		} else {
			upper = startingPitch + AngularRange / 2;
			lower = startingPitch - AngularRange / 2;
		}

		// should be at most once, but loop just in case
		while (upper >= 360) {
			upper -= 360;
			lower -= 360;
		}
	}

	// Update is called once per frame
	void Update () {
		float pitchMovement = Input.GetAxis ("Mouse Y") * (-Sensitivity);

		if (pitchMovement != 0) {
			float pitchDesired = Pitch + pitchMovement;

			while (pitchDesired >= 360)
				pitchDesired -= 360;

			while (pitchDesired < 0)
				pitchDesired += 360;

			// upper is nonnegative, lower can be negative
			if (!(pitchDesired >= lower && pitchDesired <= upper) && !(pitchDesired - 360 >= lower && pitchDesired - 360 <= upper))
				pitchDesired = pitchMovement > 0 ? upper : lower;

			transform.rotation = Quaternion.Euler (pitchDesired, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		}
	}

	public float Pitch {
		get {
			return transform.rotation.eulerAngles.x;
		}
	}
}
