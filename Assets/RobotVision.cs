using System.Collections.Generic;
using System.Linq;
using SimpleBT;
using Unity.VisualScripting;
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
    private const string BB_Item = "Item";

    private Transform _detectedItem;
    private void Start()
    {
        blackboard = GetComponent<BehaviourTreeExecutor>().blackboard;
    }
    
    void Update()
    {
        IReadOnlyList<Transform> targets = vision.Targets.Where(t => t.CompareTag("Player")).ToArray();
        if (blackboard.HasParameter(BB_Player))
        {
            if(targets.Count == 0){
                blackboard.SetValue(BB_Player, null);
            }
    
            foreach(var target in targets){
                blackboard.SetValue(BB_Player, target);
                return;
            }
        }
        
 

        var items = vision.Targets.Where(t => t.CompareTag("Item")).ToHashSet();

        if (_detectedItem != null && !items.Contains(_detectedItem))
        {
            _detectedItem = null;
        }

        if (_detectedItem == null && items.Count > 0)
        {
            _detectedItem = items.First();
        }
        
        blackboard.SetValue(BB_Item, _detectedItem);
        
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
