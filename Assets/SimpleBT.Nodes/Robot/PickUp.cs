using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.PickUp")]
    public class PickUp: Node
    {
        private TransformParameter target;
        
        private PickupController _pickupController;
        protected override void OnStart()
        {
            _pickupController = currentContext.GameObject.GetComponent<PickupController>();
        }

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