using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Robot
{
    public abstract class PickControllerNode: Node
    {
        protected PickupController _pickupController;
        protected override void OnStart()
        {
            _pickupController = currentContext.GameObject.GetComponent<PickupController>();
            Assert.IsNotNull(_pickupController, "_pickupController != null");
        }
    }
}