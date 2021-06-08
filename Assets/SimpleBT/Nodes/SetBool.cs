using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    public class SetBool: Node
    {
        private BoolParameter parameter;
        
        private BoolParameter value;

        protected override Status OnUpdate()
        {
            parameter.Value = value.Value;
            return Status.Success;
        }
    }
}