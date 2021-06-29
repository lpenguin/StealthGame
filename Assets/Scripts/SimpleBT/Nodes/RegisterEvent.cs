using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    public class RegisterEvent: Node
    {
        private StringParameter eventName;
        protected override Status OnUpdate()
        {
            currentContext.EventBus.RegisterEvent(eventName.Value);
            return Status.Success;
        }
    }
}