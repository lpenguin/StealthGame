using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    [Name("Bool.Set")]
    public class SetBool: Node
    {
        private BoolParameter variable;
        
        private BoolParameter value;

        protected override Status OnUpdate()
        {
            variable.Value = value.Value;
            return Status.Success;
        }
    }
}