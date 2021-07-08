using SimpleBT.Attributes;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.StopAttack")]
    public class RobotStopAttack : RobotNode
    {
        protected override Status OnUpdate()
        {
            robotController.StopAttack();
            return Status.Success;
        }
    }
}