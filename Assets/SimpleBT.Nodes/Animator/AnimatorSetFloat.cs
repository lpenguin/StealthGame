using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
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