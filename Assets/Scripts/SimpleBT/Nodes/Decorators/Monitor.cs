using System;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Decorators
{
    public class Monitor: Node
    {
        protected override void OnStart()
        {
            if (Children.Count != 2)
            {
                throw new Exception($"Monitor: invalid child nodes count: {Children.Count}");
            }
        }

        protected override Status OnUpdate()
        {
            var condition = Children[0];
            var action = Children[1];
            
            if (condition.Status != Status.Running)
            {
                condition.Reset();
            }
            condition.Execute(currentContext);
            switch (condition.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Fail:
                    action.Reset();
                    return Status.Fail;
                case Status.Success:
                    return ExecuteAction(action);
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private Status ExecuteAction(Node actionNode)
        {
            if ((actionNode.Status & (Status.Empty | Status.Running)) == 0)
            {
                Reset();
            }

            actionNode.Execute(currentContext);
            switch (actionNode.Status)
            {
                case Status.Running:
                case Status.Success:
                case Status.Fail:
                    return actionNode.Status;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}