using System;
using System.Collections;
using System.Collections.Generic;
using SimpleBT;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    [SerializeField]
    Vision vision;

    private Blackboard blackboard;

    private void Start()
    {
        blackboard = GetComponent<BehaviourTreeExecutor>().blackboard;
        blackboard.SetValue("Initial Position", transform.position);
    }
    
    void Update()
    {
        var targets = vision.Targets;
        if(targets.Count == 0){
            blackboard.SetValue("Player", null);
            // _blackboard.lastSeenPosition = Vector3.zero;
            return;
        }
    
        foreach(var target in targets){
            blackboard.SetValue("Player", target);
            break;
        }
    }
}
