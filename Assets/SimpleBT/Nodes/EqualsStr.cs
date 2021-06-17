using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    [Name("Str.Equals")]
    public class EqualsStr: Node
    {
        public StringParameter value1;
        public StringParameter value2;
        
        protected override Status OnUpdate()
        {
            return value1.Value == value2.Value ? Status.Success : Status.Failed;
        }
    }
}