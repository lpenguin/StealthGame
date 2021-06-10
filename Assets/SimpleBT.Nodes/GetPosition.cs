using SimpleBT.Attributes;
using UnityEngine;

namespace SimpleBT.Nodes
{
    [Name("Transform.GetPosition")]
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