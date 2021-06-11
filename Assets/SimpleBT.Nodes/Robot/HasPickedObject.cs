using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.HasObject")]
    public class HasPickedObject: PickControllerNode
    {
        private TransformParameter obj;
        protected override Status OnUpdate()
        {
            return _pickupController.PickedObject == obj.Value ? Status.Success : Status.Failed;
        }
    }
}