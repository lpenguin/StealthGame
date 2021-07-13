using UnityEngine;

namespace Hearing
{
    public class SoundEmitter: MonoBehaviour
    {
        [SerializeField]
        private LayerMask emitMask;

        [SerializeField]
        private SoundEmitterVfx vfx;
        
        [SerializeField]
        private float radius = 10f;

        private const int MaxColliders = 256;
        
        private Collider[] _colliders = new Collider[MaxColliders];

        public void Emit()
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, _colliders, emitMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < size; i++)
            {
                if (_colliders[i].TryGetComponent<SoundListener>(out var listener))
                {
                    listener.AddEmitter(this);
                }
            }

            var go = Instantiate(this.vfx.gameObject, transform.position, Quaternion.identity);

            if (go.TryGetComponent<SoundEmitterVfx>(out var vfx))
            {
                vfx.radius = radius;
            }
        }
    }
}