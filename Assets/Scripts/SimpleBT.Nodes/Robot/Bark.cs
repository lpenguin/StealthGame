using SimpleBT.Attributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.Bark")]
    public class Bark : RobotNode
    {
        private Parameter<string> text;
        
        protected override Status OnUpdate()
        {
            robotController.Bark(text.Value);
            Debug.Log(text.Value);
            return Status.Success;
        }
    }
}