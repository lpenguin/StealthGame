using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Zone
{
    [Name("Zone.RandomObject")]
    public class RandomObjectInZone: ZoneNode
    {
        private TransformParameter obj;
        protected override Status OnUpdate()
        {
            Assert.IsNotNull(zone.Value);
            if (!zone.Value.TryGetComponent<ZoneController>(out var zoneController))
            {
                Debug.LogWarning($"No ZoneController associated");
                return Status.Failed;
            }

            obj.Value = zoneController.GetRandomObject();
            return Status.Success;
        }
    }
}