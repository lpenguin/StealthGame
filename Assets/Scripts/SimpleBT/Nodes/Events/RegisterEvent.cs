using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Events
{
    [Name("Event.Register")]
    public class RegisterEvent: Node
    {
        private StringParameter eventName;
        protected override Status OnUpdate()
        {
            Debug.Log($"Event.Register ({NumericId}) \"{eventName.Value}\"");
            currentContext.EventBus.RegisterEvent(eventName.Value);
            return Status.Success;
        }
    }
}