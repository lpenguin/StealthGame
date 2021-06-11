using System;
using SimpleBT;

namespace SimpleBT.Nodes
{
    public class Guard: Sequence
    {
        protected override void OnStart()
        {
            taskIndex = 1;
        }

        protected override Status OnUpdate()
        {
            if (Children.Count < 2)
            {
                throw new Exception($"Guard: invalid number of children ({Children.Count})");
            }

            var condition = Children[0];
            condition.Reset();
            condition.Execute(currentContext);
            
            switch (condition.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failed:
                    taskIndex = 1;
                    return Status.Failed;
                case Status.Success:
                    if (Children[taskIndex].Status == Status.Failed)
                    {
                        for (int i = 1; i < Children.Count; i++)
                        {
                            Children[i].Reset();
                        }
                    }
                    return base.OnUpdate();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}