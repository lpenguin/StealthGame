using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.SetEmotion")]
    public class SetEmotion: RobotNode
    {
        private StringParameter emotion;
        protected override Status OnUpdate()
        {
            robotController.currentEmotion = emotion;
            return Status.Success;
        }
    }
}