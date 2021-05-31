using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State in which enemy attacks the player (more like drains his HP).
/// If you look at this state in the FsmCore component of the enemy,
/// you will see it has two transition rules, one of which has a larger priority.
/// This is important, if enemy doesn't see the player, he shouldn't be able to chase him.
/// </summary>
public class EnemyAttacking : FsmState {

	public float Cooldown;
	public int Strength;

	public PlayerHealth PlayerHealth;

	private float cntdwn;

	// Use this for initialization
	void Start() {
		cntdwn = Cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (PlayerHealth.transform);

		if ((cntdwn -= Time.deltaTime) <= 0) {
			cntdwn += Cooldown;

			if (!PlayerHealth.IsDead())
				PlayerHealth.Health -= Strength;
		}
	}
}
