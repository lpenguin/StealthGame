using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    [Name("Transform.Set")]
    public class TransformSet: Node
    {
        private TransformParameter variable;
        private TransformParameter value;

        protected override Status OnUpdate()
        {
            variable.Value = value.Value;
            return Status.Success;
        }
    }
}