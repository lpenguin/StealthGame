using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    [SerializeField]
    Vision vision;

    // RobotBlackboard _blackboard;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     _blackboard = GetComponent<RobotBlackboard>();
    // }
    //
    // void Update()
    // {
    //     var targets = vision.Targets;
    //     if(targets.Count == 0){
    //         _blackboard.target = null;
    //         // _blackboard.lastSeenPosition = Vector3.zero;
    //         return;
    //     }
    //
    //     foreach(var target in targets){
    //         _blackboard.target = target;
    //         _blackboard.lastSeenPosition = target.position;
    //         break;
    //     }
    // }
}
