using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
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