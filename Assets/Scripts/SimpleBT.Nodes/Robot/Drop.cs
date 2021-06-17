using SimpleBT.Attributes;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.Drop")]
    public class Drop: PickControllerNode
    {
        protected override Status OnUpdate()
        {
            _pickupController.Drop();
            return Status.Success;
        }

    }
}