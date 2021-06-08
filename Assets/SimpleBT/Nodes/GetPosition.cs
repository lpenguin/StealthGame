using UnityEngine;

namespace SimpleBT.Nodes
{
    public class GetPosition: Node
    {
        private Parameter<Transform> transform;
        private Parameter<Vector3> toStore;
        
        protected override Status OnUpdate()
        {
            toStore.Value = transform.Value.position;
            return Status.Success;
        }
    }
}