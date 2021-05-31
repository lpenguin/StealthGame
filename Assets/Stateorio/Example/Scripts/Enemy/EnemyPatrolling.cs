using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State in which the enemy goes through its patrol points.
/// When transitioning back to this state, the enemy continues its patrol route where he left of.
/// </summary>
[RequireComponent (typeof (NavMeshAgent))]
public class EnemyPatrolling : FsmState {

	public PatrolPoint[] Points;
	public float Epsilon = 0.5f;

	private int destPoint = 0;
	private NavMeshAgent agent;

	private Vector3? returnPoint = null;

	void Awake () {
		agent = GetComponent<NavMeshAgent> ();
		if (Points.Length > 1) agent.autoBraking = false;
	}

	private void gotoNextPoint () {
		if (Points.Length == 0)
			return;

		if (returnPoint != null) {
			agent.destination = returnPoint.Value;
			returnPoint = null;
		} else {
			agent.destination = Points [destPoint].Position;
			destPoint = (destPoint + 1) % Points.Length;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Points.Length > 1 && !agent.pathPending && agent.remainingDistance < Epsilon)
			gotoNextPoint ();
	}

	public override void OnStateEnter () {
		gotoNextPoint ();
	}

	public override void OnStateLeave () {
		if (Points.Length == 0)
			return;
		
		agent.ResetPath ();

		if (Points.Length > 1)
			destPoint = destPoint > 0 ? destPoint - 1 : Points.Length - 1;

		returnPoint = transform.position;
	}
}
