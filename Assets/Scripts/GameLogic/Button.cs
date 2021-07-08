using System;
using Interactable;
using Signals;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(Animator))]
    public class Button: MonoBehaviour, IInteractable
    {
        [SerializeField]
        private bool isEnabled = true;
        
        [SerializeField]
        private SignalCollection toggle;

        private Animator _animator;
        private static readonly int Enabled = Animator.StringToHash("Enabled");
        private static readonly int Trigger = Animator.StringToHash("Activate");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            SetEnabled(isEnabled);
        }
        public void SetEnabled(bool isEnabled)
        {
            this.isEnabled = isEnabled;
            _animator.SetBool(Enabled, this.isEnabled);
        }
        
        public void Interact()
        {
            _animator.SetTrigger(Trigger);
            toggle.Emit();
        }

        private void OnDrawGizmosSelected()
        {
            toggle.DrawGizmos(transform.position);
        }
    }
}