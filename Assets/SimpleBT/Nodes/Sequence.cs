using System;

namespace SimpleBT.Nodes
{
    public class Sequence: Node
    {
        protected int taskIndex = 0;

        protected override void OnStart()
        {
            taskIndex = 0;
        }
        
        protected override Status OnUpdate()
        {
            for (; taskIndex < Children.Count; taskIndex++)
            {
                var child = Children[taskIndex];
                child.Execute(currentContext);
                switch (child.Status)
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failed:
                        return Status.Failed;
                    case Status.Success:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Success;
        }
    }
}