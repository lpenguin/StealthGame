using System;

namespace SimpleBT.Nodes
{
    public class Repeater: Node
    {
        protected override Status OnUpdate()
        {
            if (Children.Count != 1)
            {
                throw new Exception($"Fail: Invalid number if children ({Children.Count})");
            }

            var child = Children[0];
            child.Execute(currentContext);
            switch (child.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failed:
                    return Status.Failed;
                case Status.Success:
                    child.Reset();
                    return Status.Running;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}