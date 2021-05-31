using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSearching : FsmState
{
    [SerializeField]
    private BehaviorExecutor _behaviour;

    private RobotBlackboard _blackboard;
    private FsmCore _fsm;

    public override void OnStateEnter()
    {
        _fsm = GetComponent<FsmCore>();
        _blackboard = GetComponent<RobotBlackboard>();
        _behaviour.enabled = true;
    }

    void Update(){
        if(_blackboard.target != null){
            _fsm.ChangeState(GetComponent<RobotChasing>());
        }
    }

    private void OnBehaviorComplete(){
        _fsm.ChangeState(GetComponent<RobotPatrolling>());
    }

    public override void OnStateLeave()
    {
        _behaviour.enabled = false;
    }
}
