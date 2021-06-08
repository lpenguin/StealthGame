using UnityEngine.Assertions;

namespace SimpleBT.Nodes
{
    public class BarkAction : Node
    {
        private Parameter<string> text;
        
        private RobotController _robotController;
        
        protected override void OnStart()
        {
            _robotController = currentContext.GameObject.GetComponent<RobotController>();
            Assert.IsNotNull(_robotController);
        }

        protected override Status OnUpdate()
        {
            _robotController.Bark(text.Value);

            return Status.Success;
        }
    }
}