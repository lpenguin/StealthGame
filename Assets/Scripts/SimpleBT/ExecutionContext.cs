using UnityEngine;
using UnityEngine.Events;

namespace SimpleBT
{
    public class NodeUnityEvent : UnityEvent<Node>
    {
    }

    public class ExecutionContext
    {
        public GameObject GameObject { get; set; }
        public Blackboard Blackboard { get; set; }
        public BehaviourTree BehaviourTree { get; set; }
        public EventBus EventBus { get; set; }

        public NodeUnityEvent OnContentsChanged { get; set; } = new NodeUnityEvent();
    }
}