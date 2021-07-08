using SimpleBT.Attributes;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.StartAttack")]
    public class RobotStartAttack: RobotNode
    {
        protected override Status OnUpdate()
        {
            robotController.StartAttack();
            return Status.Success;
        }
    }
}