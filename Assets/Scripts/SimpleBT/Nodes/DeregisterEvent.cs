using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    public class DeregisterEvent: Node
    {
        private StringParameter eventName;
        protected override Status OnUpdate()
        {
            currentContext.EventBus.DeregisterEvent(eventName.Value);
            return Status.Success;
        }
    }
}