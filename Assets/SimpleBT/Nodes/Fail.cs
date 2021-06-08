using System;

namespace SimpleBT.Nodes
{
    public class Fail: Node
    {
        protected override Status OnUpdate()
        {
            if (Children.Count > 1)
            {
                throw new Exception($"Fail: Invalid number if children ({Children.Count})");
            }

            if (Children.Count == 0)
            {
                return Status.Failed;
            }

            var child = Children[0];
            child.Execute(currentContext);
            switch (child.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failed:
                case Status.Success:
                    return Status.Failed;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}