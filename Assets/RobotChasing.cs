using UnityEngine.AI;
using UnityEngine;

public class RobotChasing : FsmState
{
    
    private NavMeshAgent _navMeshAgent;
    private RobotBlackboard _blackboard;
    private FsmCore _fsm;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _fsm = GetComponent<FsmCore>();
        _blackboard = GetComponent<RobotBlackboard>();
    }

    void Update()
    {
        if(_blackboard.target == null){
            _fsm.ChangeState(GetComponent<RobotAlerted>());
            return;
        }

        var dest = _blackboard.lastSeenPosition;
        dest.y = transform.position.y;

        _navMeshAgent.SetDestination(dest);
    }

    public override void OnStateLeave () {
		_navMeshAgent.ResetPath ();
	}
}
