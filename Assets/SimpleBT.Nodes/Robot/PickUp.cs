using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.PickUp")]
    public class PickUp: PickControllerNode
    {
        private TransformParameter target;

        protected override Status OnUpdate()
        {
            if (target.Value == null)
            {
                Debug.LogWarning($"PickUp: target is null");
                return Status.Failed;
            }
            
            _pickupController.Pickup(target.Value);
            return Status.Success;
        }
    }
}