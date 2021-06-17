using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
    [Name("Animator.SetFloat")]
    public class AnimatorSetFloat: AnimatorNode
    {
        private StringParameter name;
        private FloatParameter value;
        
        protected override Status OnUpdate()
        {
            animator.SetFloat(name.Value, value.Value);
            return Status.Success;
        }
    }
}