using SimpleBT.Attributes;

namespace SimpleBT.Nodes
{
    [Name("Obj.Equals")]
    public class EqualsObject: Node
    {
        public Parameter<object> value1;
        public Parameter<object> value2;
        
        protected override Status OnUpdate()
        {
            return value1.Value == value2.Value ? Status.Success : Status.Failed;
        }
    }
}