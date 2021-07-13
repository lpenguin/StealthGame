using System;
using UnityEngine;

namespace Signals
{
    [Serializable]
    public class SignalCollection
    {
        [SerializeField]
        public Signal[] signals;

        public void Emit()
        {
            if (signals == null)
            {
                return;
            }

            foreach (var signal in signals)
            {
                signal.Emit();
            }
        }
        
        public void DrawGizmos(Vector3 position)
        {
            Gizmos.color = Color.green;
            if (signals == null)
            {
                return;
            }
            foreach (var signal in signals)
            {
                if (signal.target == null)
                {
                    continue;
                }
                Gizmos.DrawLine(position, signal.target.transform.position);
            }
        }
    }
    
    
    [Serializable]
    public struct Signal
    {
        public SignalType type;
        
        // [InterfaceType(typeof(ISignalHandler))]
        public GameObject target;

        public void Emit()
        {
            if (target.TryGetComponent<ISignalHandler>(out var handler))
            {
                handler.HandleSignal(type);
            }
        }
    }
}