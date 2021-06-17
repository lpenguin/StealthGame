using System.Collections.Generic;
using UnityEngine;

namespace Hearing
{
    public class SoundListener: MonoBehaviour
    {
        private HashSet<SoundEmitter> _emitters = new HashSet<SoundEmitter>();

        public IEnumerable<SoundEmitter> Emitters => _emitters;
        
        public void AddEmitter(SoundEmitter emitter)
        {
            _emitters.Add(emitter);
        }

        public void ClearEmitters()
        {
            _emitters.Clear();
        }

    }
}