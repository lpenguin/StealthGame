using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.EnableLookAt")]
    public class RobotEnableLookAt: RobotNode
    {
        private BoolParameter enable = true;
        protected override Status OnUpdate()
        {
            if (enable.Value)
            {
                robotController.StartLookAt();
            }
            else
            {
                robotController.StopLookAt();
            }

            return Status.Success;
        }
    }
}