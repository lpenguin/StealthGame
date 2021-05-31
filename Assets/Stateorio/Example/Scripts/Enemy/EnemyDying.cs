using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State in which the enemy death animation goes on.
/// This state is accessed into with the only global state transition rule.
/// This transition rule has the largest priority in the FSM,
/// and the enemy won't transition to other states after getting killed.
/// </summary>
public class EnemyDying : FsmState {

	public float FallSpeed;
	public float RollSpeed;
	public float Bottom;

	public override void OnStateEnter () {
		NavMeshAgent agent = GetComponent<NavMeshAgent> ();
		if (agent != null)
			agent.enabled = false;

		CharacterController controller = GetComponent<CharacterController> ();
		if (controller != null)
			controller.enabled = false;

		foreach (Collider c in GetComponents<Collider>())
			c.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		transform.position = transform.position - new Vector3 (0, FallSpeed, 0) * Time.deltaTime;

		transform.Rotate (0, RollSpeed * Time.deltaTime, 0);

		if (transform.position.y <= Bottom)
			Destroy (gameObject);
	}
}
