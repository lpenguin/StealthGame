using System;
using UnityEngine;

namespace Signals
{
    [Serializable]
    public struct SignalCollection
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