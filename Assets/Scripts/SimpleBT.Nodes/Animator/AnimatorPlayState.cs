using System;
using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Animator
{
    [Name("Animator.PlayState")]
    public class AnimatorPlayState: AnimatorNode
    {
        private StringParameter state;
        private StringParameter layer;
        private FloatParameter normalizedTime = float.NegativeInfinity;

        private int layerIndex;
        private int stateHash;
        private bool playing;

        private static int FindLayerIndex(UnityEngine.Animator animator, string layerName)
        {
            if (string.IsNullOrEmpty(layerName))
            {
                return -1;
            }

            int layerCount = animator.layerCount;
            for (int i = 0; i < layerCount; i++)
            {
                if (animator.GetLayerName(i) == layerName)
                {
                    return i;
                }
            }

            throw new Exception($"Cannot find layer name \"{layerName}\"");
        }
        protected override void OnStart()
        {
            base.OnStart();
            stateHash = UnityEngine.Animator.StringToHash(state.Value);
            playing = false;
            layerIndex = FindLayerIndex(animator, layer.Value);
            
            if (!animator.HasState(layerIndex, stateHash)) {
                Debug.LogError("Error: The Animator does not have the state " + state.Value + " on layer " + layer.Value);
            }
        }

        protected override Status OnUpdate()
        {
            if (!playing)
            {
                animator.Play(this.state.Value, layerIndex, normalizedTime.Value);    
                playing = true;
                return Status.Running;
            }

            var currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);
            if (currentState.shortNameHash != stateHash) {
                return Status.Success;
            }

            return Status.Running;
        }
    }
}