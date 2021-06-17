using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Zone
{
    [Name("Zone.RandomPosition")]
    public class RandomPositionInZone: ZoneNode
    {
        private Vector3Parameter position;
        protected override Status OnUpdate()
        {
            position.Value = GetRandomPointInsideCollider(collider);
            return Status.Success;
        }
        
        public static Vector3 GetRandomPointInsideCollider( BoxCollider boxCollider )
        {
            Vector3 extents = boxCollider.size / 2f;
            Vector3 point = new Vector3(
                Random.Range( -extents.x, extents.x ),
                Random.Range( -extents.y, extents.y ),
                Random.Range( -extents.z, extents.z )
            );
 
            return boxCollider.transform.TransformPoint( point );
        }
    }
}