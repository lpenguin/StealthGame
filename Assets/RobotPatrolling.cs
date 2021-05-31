using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotPatrolling : FsmState
{
    [SerializeField]
    private Pathway _pathway;

    private NavMeshAgent _navMeshAgent;
    private RobotBlackboard _blackboard;
    private FsmCore _fsm;

    private int _currentWaypoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _fsm = GetComponent<FsmCore>();
        _blackboard = GetComponent<RobotBlackboard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_blackboard.target != null){
            _fsm.ChangeState(GetComponent<RobotChasing>());

            return;
        }

        if(!_navMeshAgent.pathPending && !_navMeshAgent.hasPath){
            var waypoint = _pathway.NextPoint(_currentWaypoint);
            Debug.Log($"Destination reached, setting next waypoint: {waypoint} ({_currentWaypoint})");
            _navMeshAgent.SetDestination(waypoint);
            _currentWaypoint += 1;
        }
    }

    public override void OnStateLeave () {
		_navMeshAgent.ResetPath ();
	}

}
