using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(NavMeshAgent))]
public class RobotAlerted : FsmState
{

    private NavMeshAgent _navMeshAgent;
    private RobotBlackboard _blackboard;
    private FsmCore _fsm;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("RobotAlerted");

    }

    public override void OnStateEnter()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _fsm = GetComponent<FsmCore>();
        _blackboard = GetComponent<RobotBlackboard>();
        Assert.IsNotNull(_navMeshAgent, "NavMeshAgent is required");
        _navMeshAgent.SetDestination(_blackboard.lastSeenPosition);
    }
    
    void Update()
    {
        if(_blackboard.target != null){
            _fsm.ChangeState(GetComponent<RobotChasing>());
            return;
        }

        if(!_navMeshAgent.pathPending && !_navMeshAgent.hasPath){
            _fsm.ChangeState(GetComponent<RobotSearching>());
        }

    }

    public override void OnStateLeave () {
		_navMeshAgent.ResetPath ();
	}
}
