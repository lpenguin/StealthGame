using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
    [Name("Animator.SetTrigger")]
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