using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
    public class Break: Node
    {
        private BoolParameter enabled = false;
        protected override Status OnUpdate()
        {
            if (enabled.Value)
            {
                Debug.Break();
            }
            
            return Status.Success;
        }
    }
}