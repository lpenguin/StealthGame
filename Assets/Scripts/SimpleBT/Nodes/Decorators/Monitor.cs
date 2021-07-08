using System;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Decorators
{
    public class Monitor: Node
    {
        private NodeParameter condition;
        private NodeParameter action;

        protected override void OnStart()
        {
            condition.Value.Parent = this;
            action.Value.Parent = this;
        }

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
                case Status.Fail:
                    actionV.Reset();
                    return Status.Fail;
                case Status.Success:
                    return ExecuteAction(actionV);
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Reset()
        {
            condition.Value.Reset();
            action.Value.Reset();
            
            condition.Value.Parent = null;
            action.Value.Parent = null;

            base.Reset();
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