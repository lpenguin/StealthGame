using System;

namespace SimpleBT.Nodes
{
    public class SelectorI: Node
    {
        private int taskIndex = 0;

        protected override void OnStart()
        {
            taskIndex = 0;
        }

        protected override Status OnUpdate()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                // Debug.Log($"SelectorI: Checking Child[{i}]");
                var child = Children[i];
                if (i != taskIndex && (child.Status == Status.Success || child.Status == Status.Running))
                {
                    child.Reset();
                }
                child.Execute(currentContext);
                
                switch (child.Status)
                {
                    case Status.Failed:
                        child.Reset();
                        continue;
                    case Status.Running:
                        for (int j = i + 1; j <= taskIndex; j++)
                        {
                            Children[j].Reset();
                        }

                        taskIndex = i;

                        return Status.Running;
                    case Status.Success:
                        for (int j = i + 1; j <= taskIndex; j++)
                        {
                            Children[j].Reset();
                        }

                        return Status.Success;
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Success;
        }
    }
}