using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    public class CheckBool: Node
    {
        private BoolParameter value1;
        
        private BoolParameter value2 = true;

        protected override Status OnUpdate()
        {
            return value1.Value == value2.Value ? Status.Success : Status.Failed;
        }
    }
}