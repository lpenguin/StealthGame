using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Robot
{
    public class Bark : RobotNode
    {
        private Parameter<string> text;
        
        protected override Status OnUpdate()
        {
            robotController.Bark(text.Value);

            return Status.Success;
        }
    }
}