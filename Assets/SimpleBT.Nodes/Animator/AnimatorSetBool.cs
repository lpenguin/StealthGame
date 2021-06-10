using System;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Animator
{
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