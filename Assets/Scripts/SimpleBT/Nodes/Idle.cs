namespace SimpleBT.Nodes
{
    public class Idle: Node
    {
        protected override Status OnUpdate()
        {
            return Status.Running;
        }
    }
}