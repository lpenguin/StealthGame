using System.Linq;
using Audio;
using Hearing;
using SimpleBT;
using UnityEngine;

namespace Robot
{
    public class RobotSensors : MonoBehaviour
    {
        [SerializeField]
        Vision vision;

        [SerializeField] 
        private Vision peripheralVision;

        [SerializeField]
        private SoundListener soundListener;
    
        private Blackboard blackboard;
        private BehaviourTreeExecutor treeExecutor;


        private const string BB_CheckPosition = "Check Position";
        // private const string BB_CheckPositionIsSet = "Check Position Is Set";
        private const string BB_CheckPositionSense = "Check Position Sense";
        private const string BB_CheckPositionSource= "Check Position Source";


        private Transform _detectedItem;
        private void Start()
        {
            treeExecutor = GetComponent<BehaviourTreeExecutor>();
            blackboard = treeExecutor.blackboard;
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
                    treeExecutor.EventBus.SendEvent("Check Position", new BTEvent{
                        Arg1 = target,
                        Arg2 = "Hear",
                        Arg3 = target.position,
                    });
                    // blackboard.SetValue(BB_CheckPositionIsSet, true);
                    // blackboard.SetValue(BB_CheckPositionSense, "Hear");
                    // blackboard.SetValue(BB_CheckPosition, target.position);
                    // blackboard.SetValue(BB_CheckPositionSource, target);
                }
            }
            finally
            {
                soundListener.ClearEmitters();
            }
        }
    }
}
