using System;
using SimpleBT.Attributes;

namespace SimpleBT.Nodes.Composites
{
    [Name("Selector.Active")]
    public class ActiveSelector: Node
    {
        private int taskIndex = 0;

        // private bool[] markForReset;
        protected override void OnStart()
        {
            taskIndex = 0;
            // markForReset = new bool[Children.Count];
        }

        protected override Status OnUpdate()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                // Debug.Log($"SelectorI: Checking Child[{i}]");
                var child = Children[i];
                child.Execute(currentContext);
                
                switch (child.Status)
                {
                    case Status.Fail:
                        continue;
                    case Status.Running:
                        if (taskIndex != i)
                        {
                            // Children[taskIndex].Reset(true);
                            // markForReset[taskIndex] = true;
                            Children[taskIndex].Interrupt();
                            taskIndex = i;
                        }

                        return Status.Running;
                    case Status.Success:
                        if (taskIndex != i)
                        {
                            Children[taskIndex].Interrupt();
                            // markForReset[taskIndex] = true;
                            // Children[taskIndex].Reset(true);    
                        }

                        continue;
                        // return Status.Success;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Fail;
        }
    }
}