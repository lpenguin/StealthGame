using Robot;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Robot
{
    public abstract class RobotNode: Node
    {
        protected RobotController robotController;
        
        protected override void OnStart()
        {
            robotController = currentContext.GameObject.GetComponent<RobotController>();
            Assert.IsNotNull(robotController, "_robotController != null");
        }

    }
}