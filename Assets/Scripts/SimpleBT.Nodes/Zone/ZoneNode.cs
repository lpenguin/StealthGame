using SimpleBT.Parameters;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Zone
{
    public abstract class ZoneNode: Node
    {
        protected TransformParameter zone;
        protected BoxCollider collider;
        protected ZoneController zoneController;

        protected override void OnStart()
        {
            Assert.IsNotNull(zone.Value, "zone.Value != null");
            ;
            if (!zone.Value.TryGetComponent(out collider))
            {
                Debug.LogWarning($"The zone doesn't have a BoxCollider");
            }
            
            if (!zone.Value.TryGetComponent<ZoneController>(out zoneController))
            {
                Debug.LogWarning($"No ZoneController associated");
            }
        }
    }
}