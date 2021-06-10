using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
    public class AnimatorSetTrigger: AnimatorNode
    {
        private StringParameter name;
        
        protected override Status OnUpdate()
        {
            animator.SetTrigger(name.Value);
            return Status.Success;
        }
    }
}