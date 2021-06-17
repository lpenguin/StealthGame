using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Animator
{
    public abstract class AnimatorNode: Node
    {
        protected UnityEngine.Animator animator;

        protected override void OnStart()
        {
            animator = currentContext.GameObject.GetComponent<UnityEngine.Animator>();
            Assert.IsNotNull(animator, "animator != null");
        }
    }
}