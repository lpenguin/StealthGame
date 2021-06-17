using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Zone
{
    [Name("Zone.Contains")]
    public class ZoneContains: ZoneNode
    {
        private TransformParameter obj;
        protected override Status OnUpdate()
        {
            return zoneController.Contains(obj.Value) ? Status.Success : Status.Failed;
        }
    }
}