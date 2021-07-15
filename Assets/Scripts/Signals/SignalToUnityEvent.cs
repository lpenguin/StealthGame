using System;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class SignalToUnityEvent: MonoBehaviour, ISignalHandler
    {
        public UnityEvent on;
        public UnityEvent off;
        public UnityEvent toggle;
        public void HandleSignal(SignalType signal)
        {
            switch(signal)
            {
                case SignalType.On:
                    on.Invoke();
                    break;
                case SignalType.Off:
                    off.Invoke();
                    break;
                case SignalType.Toggle:
                    toggle.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(signal), signal, null);
            }
        }
    }
}