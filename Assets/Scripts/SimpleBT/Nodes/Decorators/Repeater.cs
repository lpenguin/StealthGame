using System;

namespace SimpleBT.Nodes.Decorators
{
    public class Repeater: Node
    {
        protected override Status OnUpdate()
        {
            if (Children.Count != 1)
            {
                throw new Exception($"Repeater: Invalid number of children ({Children.Count})");
            }

            var child = Children[0];
            if (child.Status == Status.Success)
            {
                child.Reset();
            }
            
            child.Execute(currentContext);
            switch (child.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Fail:
                    return Status.Fail;
                case Status.Success:
                    return Status.Running;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}