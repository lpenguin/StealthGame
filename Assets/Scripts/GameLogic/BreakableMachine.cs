using System;
using Interactable;
using Signals;
using SimpleBT;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    [RequireComponent(typeof(Animator))]
    public class BreakableMachine: MonoBehaviour, IInteractable, ISignalHandler
    {
        [SerializeField]
        private string fixTree = "Wait";
        
        [SerializeField]
        private BehaviourTreeExecutor[] notifyExecutors;

        public UnityEvent onBreak = new UnityEvent();
        
        private bool _isBroken = false;
        private Animator _animator;
        private static readonly int Animator_IsBroken = Animator.StringToHash("Is Broken");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Interact()
        {
            Break();
        }

        public void HandleSignal(SignalType signal)
        {
            switch(signal)
            {
                case SignalType.On:
                    Fix();
                    break;
                case SignalType.Off:
                    Break();
                    break;
                case SignalType.Toggle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(signal), signal, null);
            }

            
        }

        public void Fix()
        {
            _isBroken = false;
            _animator.SetBool(Animator_IsBroken, false);
        }
        
        private void Break()
        {
            if (_isBroken)
            {
                return;
            }

            _animator.SetBool(Animator_IsBroken, true);
            onBreak.Invoke();
            _isBroken = true;

            foreach (var executor in notifyExecutors)
            {
                executor.EventBus.SendEvent("Object Is Broken", new BTEvent()
                {
                    Arg1 = transform,
                    Arg2 = fixTree
                });
            }
        }
    }
}