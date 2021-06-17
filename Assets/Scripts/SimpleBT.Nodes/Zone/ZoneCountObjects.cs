using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Zone
{
    [Name("Zone.Count")]
    public class ZoneCountObjects: ZoneNode
    {
        private IntParameter count;
        
        protected override Status OnUpdate()
        {
            count.Value = zoneController.NumberOfObjects();
            return Status.Success;
        }
    }
}