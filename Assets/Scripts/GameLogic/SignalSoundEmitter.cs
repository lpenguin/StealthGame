using Hearing;
using Signals;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(SoundEmitter))]
    public class SignalSoundEmitter: MonoBehaviour, ISignalHandler
    {
       
        private SoundEmitter _emitter;

        private void Start()
        {
            _emitter = GetComponent<SoundEmitter>();
        }

        public void HandleSignal(SignalType signal)
        {
            _emitter.Emit();
        }
    }
}