using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using Hearing;
using SimpleBT;
using Unity.VisualScripting;
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
    

    private const string BB_TargetPosition = "Target Position";
    private const string BB_CanSeeTarget = "Can See Target";
    private const string BB_SawPeripheralTarget = "Saw Peripheral Target";
    private const string BB_HeardTarget = "Heard Target";

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
            target = vision.Targets.FirstOrDefault(t => t.CompareTag("Player"));
        
            if (target != null)
            {
                blackboard.SetValue(BB_CanSeeTarget, true);
                blackboard.SetValue(BB_TargetPosition, target.position);
                return;
            }
        
            blackboard.SetValue(BB_CanSeeTarget, false);
        
            target = peripheralVision.Targets.FirstOrDefault(t => t.CompareTag("Player"));
            if (target != null)
            {
                blackboard.SetValue(BB_SawPeripheralTarget, true);
                blackboard.SetValue(BB_TargetPosition, target.position);
                return;
            }
        
            target = soundListener.Emitters.Select(t => t.transform).FirstOrDefault(t => t.CompareTag("Player"));
            if (target != null)
            {
                blackboard.SetValue(BB_HeardTarget, true);
                blackboard.SetValue(BB_TargetPosition, target.position);
            }
        }
        finally
        {
            soundListener.ClearEmitters();
        }
        

        
    }

    private void OnDrawGizmos()
    {
        if (blackboard == null || !blackboard.HasParameter(BB_TargetPosition))
        {
            return;
        }
        
        var position = blackboard.GetValue<Vector3>(BB_TargetPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position, 0.5f);
    }
}
