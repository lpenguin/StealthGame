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

        [SerializeField]
        private ContactSensor3D robotSensor;
        
        private Animator _animator;
        private AudioManager _audioManager;
        private static readonly int On = Animator.StringToHash("On");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioManager = GetComponent<AudioManager>();
            _animator.SetBool(On, isOpened);

        }

        private void OnEnable()
        {
            if (robotSensor != null)
            {
                robotSensor.onEnter.AddListener(OnRobotEnter);
                robotSensor.onExit.AddListener(OnRobotExit);
            }
        }

        private void OnDisable()
        {
            if (robotSensor != null)
            {
                robotSensor.onEnter.RemoveListener(OnRobotEnter);
                robotSensor.onExit.RemoveListener(OnRobotExit);
            }
        }
        
        private void OnRobotEnter(Collider _)
        {
            OpenDoor();
        }
        
        private void OnRobotExit(Collider _)
        {
            CloseDoor();
        }
        
        private bool IsRobotNearby()
        {
            if (robotSensor == null)
            {
                return false;
            }

            return robotSensor.Contacts.Count > 0;
        }
        
        private void OpenDoor()
        {
            if (isOpened)
            {
                return;
            }

            isOpened = true;
            _animator.SetBool(On, true);
            _audioManager.PlayAudio("Open");
        }

        private void CloseDoor()
        {
            if (!isOpened)
            {
                return;
            }


            if (IsRobotNearby())
            {
                return;
            }
            isOpened = false;
            _animator.SetBool(On, false);
            _audioManager.PlayAudio("Close");
        }
        
        public void HandleSignal(SignalType signal)
        {
            switch (signal)
            {
                case SignalType.On:
                    OpenDoor();
                    break;
                case SignalType.Off:
                    CloseDoor();
                    break;
                case SignalType.Toggle:
                    if (isOpened)
                    {
                        CloseDoor();
                    }
                    else
                    {
                        OpenDoor();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(signal), signal, null);
            }
        }
    }
}