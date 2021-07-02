using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Expressions
{
    [Name("Bool.IsSet")]
    public class BoolIsSet: Node
    {
        private BoolParameter value;
        
        protected override Status OnUpdate()
        {
            return value.Value ? Status.Success : Status.Failed;
        }
    }
    
    [Name("Bool.IsNotSet")]
    public class BoolIsNotSet: Node
    {
        private BoolParameter value;
        
        protected override Status OnUpdate()
        {
            return !value.Value ? Status.Success : Status.Failed;
        }
    }
}