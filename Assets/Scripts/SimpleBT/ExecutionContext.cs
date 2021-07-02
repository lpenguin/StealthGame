using UnityEngine;

namespace SimpleBT
{
    public class ExecutionContext
    {
        public GameObject GameObject { get; set; }
        public Blackboard Blackboard { get; set; }
        public BehaviourTree BehaviourTree { get; set; }
        public EventBus EventBus { get; set; }
    }
}