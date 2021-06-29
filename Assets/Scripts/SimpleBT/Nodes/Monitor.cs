using System;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    public class Monitor: Node
    {
        private NodeParameter condition;
        private NodeParameter action;
        protected override Status OnUpdate()
        {
            var conditionV = condition.Value;
            var actionV = action.Value;
            
            if (conditionV.Status != Status.Running)
            {
                conditionV.Reset();
            }
            conditionV.Execute(currentContext);
            switch (conditionV.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failed:
                    return Status.Failed;
                case Status.Success:
                    if (actionV.Status == Status.Failed || actionV.Status == Status.Interrupted)
                    {
                        actionV.Reset();
                    }

                    return ExecuteAction(actionV);
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Reset()
        {
            condition.Value.Reset();
            action.Value.Reset();
            base.Reset();
        }

        private Status ExecuteAction(Node actionNode)
        {
            actionNode.Execute(currentContext);
            switch (actionNode.Status)
            {
                case Status.Running:
                case Status.Success:
                case Status.Failed:
                    return actionNode.Status;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}