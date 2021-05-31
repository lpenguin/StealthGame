using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Whether the enemy has been killed by the player.
/// The condition is false if the enemy is already dying,
/// so the enemy doesn't reenter EnemyDying state over and over again.
/// </summary>
public class Killed : FsmCondition {

	private bool dead;

	public bool Dead {
		get { return dead; }
		set { dead = value; }
	}

	public override bool IsSatisfied (FsmState curr, FsmState next) {
		return Dead && !(curr is EnemyDying);
	}
}
