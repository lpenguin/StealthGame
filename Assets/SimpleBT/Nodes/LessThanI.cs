using SimpleBT.Attributes;

namespace SimpleBT.Nodes
{
    [Name("Int.LessThan")]
    public class LessThanI: Node
    {
        public Parameter<int> value1;
        public Parameter<int> value2;
        
        protected override Status OnUpdate()
        {
            return value1.Value < value2.Value ? Status.Success : Status.Failed;
        }
    }
}