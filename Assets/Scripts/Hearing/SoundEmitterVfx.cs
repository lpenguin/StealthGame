using System;
using DG.Tweening;
using UnityEngine;

namespace Hearing
{
    public class SoundEmitterVfx: MonoBehaviour
    {
        [SerializeField]
        private float duration = 0.3f;

        public float radius = 5f;
        
        private LineRenderer _lineRenderer;
        private Ring _ring;
        private void Start()
        {
            _ring = GetComponent<Ring>();
            _lineRenderer = GetComponent<LineRenderer>();
            Emit();
        }

        private void Emit()
        {
            _ring.radius = radius;
            var seq = DOTween.Sequence();

            // _lineRenderer.DOColor(
            //     new Color2(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 0f)),
            //     new Color2(Color.white, Color.white),
            //     0.05f);
            //     
            var startColor = _lineRenderer.startColor;
            seq
                .Append(DOTween
                .To(() => _ring.radius, value => _ring.radius = value, radius, duration)
                .From(0.5f)
                )
                .Append(_lineRenderer.DOColor(
                new Color2(startColor, startColor),
                new Color2(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 0f)),
                0.5f))
                .OnComplete(() =>
                {
                    Destroy(gameObject, 0.1f);
                });
        }
    }
}