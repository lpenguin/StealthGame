using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Robot
{
    public class Drop: RobotNode
    {
        protected override Status OnUpdate()
        {
            robotController.Drop();
            return Status.Success;
        }

    }
}