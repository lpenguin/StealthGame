using System.Linq;
using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    [Name("Str.In")]
    public class StrIn: Node
    {
        private StringParameter variable;
        private StringArrayParameter value;
        protected override Status OnUpdate()
        {
            return value.Value.Contains(variable.Value) ? Status.Success : Status.Failed;
        }
    }
}