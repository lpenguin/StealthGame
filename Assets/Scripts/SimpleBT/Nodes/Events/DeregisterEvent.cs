using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Events
{
    [Name("Event.Deregister")]
    public class DeregisterEvent: Node
    {
        private StringParameter eventName;
        protected override Status OnUpdate()
        {
            Debug.Log($"Event.Deregister ({NumericId}) \"{eventName.Value}\"");
            currentContext.EventBus.DeregisterEvent(eventName.Value);
            return Status.Success;
        }
    }
}