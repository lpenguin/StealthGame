using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
    public class Interrupt: Node
    {
        private StringParameter nodeId;

        protected override Status OnUpdate()
        {

            if(currentContext.BehaviourTree.nodeById.TryGetValue(nodeId.Value, out var node)){
                node.Reset();
                return Status.Success;
            }

            return Status.Failed;
            
        }
    }
}