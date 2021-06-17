using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    [Name("Str.Set")]
    public class SetValue: Node
    {
        private StringParameter variable;
        private StringParameter value;
        protected override Status OnUpdate()
        {
            variable.Value = value.Value;
            return Status.Success;
        }
    }
}