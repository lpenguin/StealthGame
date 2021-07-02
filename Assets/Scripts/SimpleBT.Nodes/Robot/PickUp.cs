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
                return Status.Fail;
            }

            if (target.Value.TryGetComponent<Item>(out var item))
            {
                _pickupController.Pickup(item);
                return Status.Success;
            }
            
            Debug.LogWarning($"PickUp: target is not item");
            return Status.Fail;
        }
    }
}