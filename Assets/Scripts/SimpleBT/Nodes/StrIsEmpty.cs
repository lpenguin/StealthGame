using System.Linq;
using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    [Name("Str.IsEmpty")]
    public class StrIsEmpty: Node
    {
        private StringParameter variable;

        protected override Status OnUpdate()
        {
            return string.IsNullOrEmpty(variable.Value) ? Status.Success : Status.Failed;
        }
    }
}