using System;
using Interactable;
using Signals;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

namespace GameLogic
{
    [RequireComponent(typeof(Animator))]
    public class Button: MonoBehaviour, IInteractable
    {
        [SerializeField]
        private bool isEnabled = true;
        
        [SerializeField]
        private UnityEvent onInteract;

        [SerializeField]
        private UnityEvent onDenied;


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
            if (isEnabled)
            {
                onInteract.Invoke();
            } else 
            {
                onDenied.Invoke();
            }
        }

        private void OnDrawGizmosSelected()
        {
            onInteract.DrawGizmos(transform.position);
            onDenied.DrawGizmos(transform.position);
        }
    }
}