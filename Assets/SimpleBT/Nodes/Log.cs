using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
    public class Log: Node
    {
        private StringParameter message = "";
        
        protected override Status OnUpdate()
        {
            Debug.Log(message.Value);
            return Status.Success;
        }
    }
}