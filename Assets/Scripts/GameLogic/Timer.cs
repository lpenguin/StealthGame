using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Extensions;

namespace GameLogic
{
    [ExecuteInEditMode]
    public class Timer: MonoBehaviour, ISignalHandler
    {
        
        [Header("Timer")]
        [SerializeField]
        private bool isStarted = false;
        
        [SerializeField]
        private float time = 1f;
        
        // [SerializeField]
        private float _remaining;
        
        [Header("Signals")]
        [SerializeField]
        private UnityEvent onTimerEnd;

        [Header("Image")]
        [SerializeField]
        private bool showImage = true;
        
        [SerializeField]
        private Image timerImage;
        
        private void Update()
        {
            if (isStarted && Application.isPlaying)
            {
                _remaining -= Time.deltaTime;

                if (_remaining < 0)
                {
                    onTimerEnd.Invoke();
                    StopTimer();
                }
            }

            if (timerImage == null)
            {
                return;
            }
            
            timerImage.enabled = showImage && (!Application.isPlaying || isStarted);
            timerImage.fillAmount = 1 - _remaining / time;

        }

        public void StartTimer()
        {
            if(isStarted)
            {
                return;
            }

            isStarted = true;
            _remaining = time;
        }

        public void StopTimer()
        {
            isStarted = false;
        }

        public void HandleSignal(SignalType signal)
        {
            switch(signal)
            {
                case SignalType.On:
                    StartTimer();
                    break;
                case SignalType.Off:
                    StopTimer();
                    break;
                case SignalType.Toggle:
                    if (isStarted)
                    {
                        StopTimer();
                    }
                    else
                    {
                        StartTimer();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(signal), signal, null);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            onTimerEnd.DrawGizmos(transform.position);
        }
    }
}