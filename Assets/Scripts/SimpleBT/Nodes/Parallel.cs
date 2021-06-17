using System;

namespace SimpleBT.Nodes
{
    public class Parallel: Node
    {
        protected override Status OnUpdate()
        {
            foreach (var child in Children)
            {
                child.Execute(currentContext);
                switch (child.Status)
                {
                    case Status.Running:
                        break;
                    case Status.Failed:
                    case Status.Success:
                        return child.Status;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Running;
        }
    }
}