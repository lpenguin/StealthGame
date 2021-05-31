using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State in which enemy doesn't see the player, but tries to find him.
/// This component shows an example of calling FsmCore.ChangeState() to change the state programmatically.
/// </summary>
[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (FsmCore))]
[RequireComponent (typeof (EnemyPatrolling))]
[RequireComponent (typeof (EnemyChasing))]
public class EnemyAlerted : FsmState {

	public float Epsilon = 0.5f;

	private NavMeshAgent agent;

	private FsmCore fsmCore;
	private EnemyPatrolling patrollingState;

	void Awake () {
		agent = GetComponent<NavMeshAgent> ();
		fsmCore = GetComponent<FsmCore> ();
		patrollingState = GetComponent<EnemyPatrolling> ();
	}

	public override void OnStateEnter () {
		agent.destination = GetComponent<EnemyChasing> ().GetLastKnownPlayerLocation ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!agent.pathPending && agent.remainingDistance < Epsilon) {
			fsmCore.ChangeState (patrollingState);
		}
	}

	public override void OnStateLeave () {
		agent.ResetPath ();
	}
}
