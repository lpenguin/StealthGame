using System;

namespace SimpleBT.Nodes
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
                // Debug.Log($"Selector: Checking Child[{taskIndex}]");
                var child = Children[taskIndex];
                child.Execute(currentContext);
                // Debug.Log($"Selector: Result Child[{taskIndex}]: {child.Status}");
                switch (child.Status)
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        return Status.Success;
                    case Status.Failed:
                        if (taskIndex + 1 < Children.Count)
                        {
                            Children[taskIndex + 1].Reset();
                        }
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Failed;
        }
    }
}