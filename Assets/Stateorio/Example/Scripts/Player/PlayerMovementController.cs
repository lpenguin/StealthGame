using UnityEngine;
using System.Collections;

/// Controller for player's walking and turning around.
[RequireComponent (typeof (CharacterController))]
public class PlayerMovementController : MonoBehaviour {

	public float SpeedWalk;
	public float SpeedTurn;

	private CharacterController characterController;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
	}

	private float getSidewaysAxis () {
		bool positive = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
		bool negative = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

		if (positive && !negative) return 1.0f;
		else if (!positive && negative) return -1.0f;
		else return 0.0f;
	}

	private float getFrontbackAxis () {
		bool positive = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
		bool negative = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

		if (positive && !negative) return 1.0f;
		else if (!positive && negative) return -1.0f;
		else return 0.0f;
	}

	// Update is called once per frame
	void Update () {
		Vector3 movement = new Vector3 (0.0f, 0.0f, 0.0f);

		// Walk
		movement.x = getSidewaysAxis ();
		movement.z = getFrontbackAxis ();
		movement = movement.normalized * SpeedWalk;

		// Turn
		float rotationYaw = Input.GetAxis ("Mouse X");
		transform.Rotate (0.0f, rotationYaw * SpeedTurn, 0.0f);
		movement = transform.TransformDirection (movement);

		// Final move
		characterController.Move (movement * Time.deltaTime);
	}
}
