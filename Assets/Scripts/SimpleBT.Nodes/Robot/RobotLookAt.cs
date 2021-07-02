using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.LookAt")]
    public class RobotLookAt: RobotNode
    {
        private Vector3Parameter target;
        protected override Status OnUpdate()
        {
            robotController.LookAt(target.Value);
            return Status.Success;
        }
    }
}