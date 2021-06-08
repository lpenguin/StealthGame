using System;

namespace SimpleBT.Nodes
{
    public class SelectorI: Selector
    {
        protected override Status OnUpdate()
        {
            for (int i = 0; i < taskIndex; i++)
            {
                // Debug.Log($"SelectorI: Checking Child[{i}]");
                var child = Children[i];
                child.Reset();
                child.Execute(currentContext);
                
                switch (child.Status)
                {
                    case Status.Failed:
                        continue;
                    case Status.Running:
                        // Debug.Log($"SelectorI: Child[{i}] resumed, stopping active");
                        for (int j = i + 1; j <= taskIndex; j++)
                        {
                            Children[j].Reset();
                        }

                        taskIndex = i;

                        return Status.Running;
                    case Status.Success:
                        taskIndex = i + 1;
                        return base.OnUpdate();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return base.OnUpdate();
        }
    }
}