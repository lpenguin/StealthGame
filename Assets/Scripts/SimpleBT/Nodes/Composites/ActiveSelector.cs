using System;
using SimpleBT.Attributes;
using SimpleBT.Nodes.Events;

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
                
                if(i != taskIndex && (child.Status & (Status.Empty | Status.Running)) == 0)
                {
                    child.Reset();
                }
                
                child.Execute(currentContext);
                
                switch (child.Status)
                {
                    case Status.Fail:
                        continue;
                    case Status.Running:
                        if (taskIndex != i)
                        {
                            var taskIndexChild = Children[taskIndex];
                            if (taskIndexChild.Status == Status.Running)
                            {
                                taskIndexChild.Reset();
                                DeregisterEvents(taskIndexChild);
                            }
                            
                            taskIndex = i;
                        }

                        return Status.Running;
                    case Status.Success:
                        if (taskIndex != i)
                        {
                            var taskIndexChild = Children[taskIndex];
                            if (taskIndexChild.Status == Status.Running)
                            {
                                taskIndexChild.Reset();    
                                DeregisterEvents(taskIndexChild);
                            }
                        }
                        
                        DeregisterEvents(child);
                        // continue;
                        return Status.Success;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Fail;
        }

        private void DeregisterEvents(Node node)
        {
            if (node is IEventNode eventNode)
            {
                eventNode.DeregisterEvent();
            }

            foreach (var child in node.Children)
            {
                DeregisterEvents(child);
            }
        }
    }
}