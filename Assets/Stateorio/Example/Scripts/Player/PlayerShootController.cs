using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controller for killing enemies by left-clicking them.
public class PlayerShootController : MonoBehaviour {

	public float Range;
	public float BulletRadius;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			bool oldSetting = Physics.queriesHitTriggers;
			Physics.queriesHitTriggers = false;

			RaycastHit hitInfo;
			Physics.SphereCast (transform.position, BulletRadius, transform.forward, out hitInfo, Range);

			if (hitInfo.collider != null) {
				Killed killed = hitInfo.collider.gameObject.GetComponent<Killed> ();
				if (killed != null)
					killed.Dead = true;
			}

			Physics.queriesHitTriggers = oldSetting;
		}
	}
}
