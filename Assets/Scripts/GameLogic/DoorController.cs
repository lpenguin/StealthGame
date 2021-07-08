using System;
using Audio;
using Signals;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioManager))]
    public class DoorController: MonoBehaviour, ISignalHandler
    {
        [SerializeField]
        private bool isOpened;
        
        private Animator _animator;
        private AudioManager _audioManager;
        private static readonly int On = Animator.StringToHash("On");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioManager = GetComponent<AudioManager>();
            _animator.SetBool(On, isOpened);
        }
        
        public void HandleSignal(SignalType signal)
        {
            bool changed;
            switch (signal)
            {
                case SignalType.On:
                    changed = !isOpened;
                    isOpened = true;
                    break;
                case SignalType.Off:
                    changed = isOpened;
                    isOpened = false;
                    break;
                case SignalType.Toggle:
                    changed = true;
                    isOpened = !isOpened;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(signal), signal, null);
            }
            
            _animator.SetBool(On, isOpened);
            
            if (changed)
            {
                if (isOpened)
                {
                    _audioManager.PlayAudio("Open");
                }
                else
                {
                    _audioManager.PlayAudio("Close");
                }
            }
            
        }
    }
}