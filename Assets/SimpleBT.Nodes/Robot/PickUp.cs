using SimpleBT.Parameters;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Robot
{
    public class PickUp: RobotNode
    {
        private TransformParameter target;
        

        protected override Status OnUpdate()
        {
            if (target.Value == null)
            {
                Debug.LogWarning($"PickUp: target is null");
                return Status.Failed;
            }
            
            robotController.Pickup(target.Value);
            return Status.Success;
        }
    }
}