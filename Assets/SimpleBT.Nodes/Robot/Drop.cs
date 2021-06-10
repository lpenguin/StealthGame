using SimpleBT.Attributes;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.Drop")]
    public class Drop: Node
    {
        private PickupController _pickupController;
        protected override void OnStart()
        {
            _pickupController = currentContext.GameObject.GetComponent<PickupController>();
        }

        protected override Status OnUpdate()
        {
            _pickupController.Drop();
            return Status.Success;
        }

    }
}