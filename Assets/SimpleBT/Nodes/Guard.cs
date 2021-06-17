using System;

namespace SimpleBT.Nodes
{
    public class Guard: Node
    {
        protected override Status OnUpdate()
        {
            if (Children.Count < 2)
            {
                throw new Exception($"Guard: invalid number of children ({Children.Count}), must be 2 exactly");
            }

            var condition = Children[0];
            condition.Reset();
            condition.Execute(currentContext);

            var action = Children[1];
            
            switch (condition.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failed:
                    return Status.Failed;
                case Status.Success:
                    if (action.Status == Status.Failed)
                    {
                        action.Reset();
                    }
                    
                    return ExecuteAction(action);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Status ExecuteAction(Node action)
        {
            action.Execute(currentContext);
            switch (action.Status)
            {
                case Status.Running:
                case Status.Success:
                case Status.Failed:
                    return action.Status;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}