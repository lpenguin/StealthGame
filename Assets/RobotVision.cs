using SimpleBT;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    [SerializeField]
    Vision vision;

    [SerializeField] 
    private Vision peripheralVision;
    
    private Blackboard blackboard;

    private const string BB_SuspiciousObjectPosition = "Suspicious Object Position";
    private const string BB_SuspiciousObjectDetected = "Suspicious Object Detected";
    
    private const string BB_Player = "Player";
    private void Start()
    {
        blackboard = GetComponent<BehaviourTreeExecutor>().blackboard;
    }
    
    void Update()
    {
        var targets = vision.Targets;
        if(targets.Count == 0){
            blackboard.SetValue(BB_Player, null);
        }
    
        foreach(var target in targets){
            blackboard.SetValue(BB_Player, target);
            return;
        }

        if (peripheralVision == null)
        {
            return;
        }
        
        targets = peripheralVision.Targets;
        // if(targets.Count == 0){
        //     blackboard.SetValue(BB_SuspiciousObject, null);
        // }
    
        foreach(var target in targets){
            blackboard.SetValue(BB_SuspiciousObjectDetected, true);
            blackboard.SetValue(BB_SuspiciousObjectPosition, target.position);
            break;
        }
    }
}
