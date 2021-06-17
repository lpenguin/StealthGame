using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
    [Name("Animator.SetInt")]
    public class AnimatorSetInt : AnimatorNode
    {
        private StringParameter name;
        private IntParameter value;

        protected override Status OnUpdate()
        {
            animator.SetInteger(name.Value, value.Value);
            return Status.Success;
        }
    }
}