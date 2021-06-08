using System;

namespace SimpleBT.Nodes
{
    public class Success: Node
    {
        protected override Status OnUpdate()
        {
            if (Children.Count > 1)
            {
                throw new Exception($"Success: Invalid number if children ({Children.Count})");
            }

            if (Children.Count == 0)
            {
                return Status.Success;
            }

            var child = Children[0];
            child.Execute(currentContext);
            switch (child.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failed:
                case Status.Success:
                    return Status.Success;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}