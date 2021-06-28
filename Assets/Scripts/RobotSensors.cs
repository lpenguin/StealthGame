using System.Linq;
using Hearing;
using SimpleBT;
using UnityEngine;

public class RobotSensors : MonoBehaviour
{
    [SerializeField]
    Vision vision;

    [SerializeField] 
    private Vision peripheralVision;

    [SerializeField]
    private SoundListener soundListener;
    
    private Blackboard blackboard;
    

    private const string BB_CheckPosition = "Check Position";
    private const string BB_CheckPositionIsSet = "Check Position Is Set";
    private const string BB_CheckPositionSense = "Check Position Sense";
    private const string BB_CheckPositionSource= "Check Position Source";


    private Transform _detectedItem;
    private void Start()
    {
        blackboard = GetComponent<BehaviourTreeExecutor>().blackboard;
    }
    
    void Update()
    {
        try
        {
            Transform target;
            // target = vision.Targets.FirstOrDefault(t => t.CompareTag("Player"));
        
            // if (target != null)
            // {
            //     blackboard.SetValue(BB_CanSeeTarget, true);
            //     blackboard.SetValue(BB_TargetPosition, target.position);
            //     return;
            // }
        
            // blackboard.SetValue(BB_CanSeeTarget, false);
        
            // target = peripheralVision.Targets.FirstOrDefault(t => t.CompareTag("Player"));
            // if (target != null)
            // {
            //     blackboard.SetValue(BB_SawPeripheralTarget, true);
            //     blackboard.SetValue(BB_TargetPosition, target.position);
            //     return;
            // }
        
            target = soundListener.Emitters.Select(t => t.transform).FirstOrDefault();
            if (target != null)
            {
                blackboard.SetValue(BB_CheckPositionIsSet, true);
                blackboard.SetValue(BB_CheckPositionSense, "Hear");
                blackboard.SetValue(BB_CheckPosition, target.position);
                blackboard.SetValue(BB_CheckPositionSource, target);
            }
        }
        finally
        {
            soundListener.ClearEmitters();
        }
        

        
    }

    private void OnDrawGizmos()
    {
        if (blackboard == null || !blackboard.HasParameter(BB_CheckPosition))
        {
            return;
        }
        
        var position = blackboard.GetValue<Vector3>(BB_CheckPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position, 0.5f);
    }
}
