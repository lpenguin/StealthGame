using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Whether the player is close enough for the enemy to attack.
/// </summary>
public class PlayerInRange : FsmCondition {

	public float Range;

	public Transform Player;

	public override bool IsSatisfied (FsmState curr, FsmState next) {
		return (transform.position - Player.position).sqrMagnitude <= Range * Range;
	}
}
