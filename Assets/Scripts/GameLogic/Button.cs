using System;
using Interactable;
using Signals;
using UnityEngine;

namespace GameLogic
{
    public class Button: MonoBehaviour, IInteractable
    {
        [SerializeField]
        private SignalCollection toggle;

        public void Interact()
        {
            toggle.Emit();
        }

        private void OnDrawGizmosSelected()
        {
            var pos = transform.position;
            
            Gizmos.color = Color.green;
            
            foreach (var signal in toggle.signals)
            {
                Gizmos.DrawLine(pos, signal.target.transform.position);
            }
        }
    }
}