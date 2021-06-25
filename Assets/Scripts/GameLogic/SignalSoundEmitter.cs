using Hearing;
using Signals;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(SoundEmitter))]
    public class SignalSoundEmitter: MonoBehaviour, ISignalHandler
    {
        [SerializeField]
        private float radius = 5f;
        
        private SoundEmitter _emitter;

        private void Start()
        {
            _emitter = GetComponent<SoundEmitter>();
        }

        public void HandleSignal(SignalType type)
        {
            _emitter.Emit(radius);
        }
    }
}