using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
    [Name("Animator.SetBool")]
    public class AnimatorSetBool: AnimatorNode
    {
        private StringParameter name;
        private BoolParameter value = true;
        
        protected override Status OnUpdate()
        {
            animator.SetBool(name.Value, value.Value);
            return Status.Success;
        }
    }
}