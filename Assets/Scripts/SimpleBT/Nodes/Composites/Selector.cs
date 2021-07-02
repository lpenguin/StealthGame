using System;

namespace SimpleBT.Nodes.Composites
{
    public class Selector: Node
    {
        protected int taskIndex;

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
                    case Status.Success:
                        return Status.Success;
                    case Status.Fail:
                        if (taskIndex + 1 < Children.Count)
                        {
                            Children[taskIndex + 1].Reset();
                        }
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Fail;
        }
    }
}