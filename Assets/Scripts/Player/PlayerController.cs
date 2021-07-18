using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameLogic;
using Hearing;
using Interactable;
using UnityEngine;
using UnityEngine.InputSystem;
using Audio;

namespace Player
{
    [Serializable]
    public struct PlayerInput
    {
        [SerializeField]
        public InputAction move;

        [SerializeField]
        public InputAction use;
        
        [SerializeField]
        public InputAction crouch;
    }
    
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;

        [SerializeField]
        private float crouchSpeedFactor = 0.5f;
    
        [SerializeField]
        private float turnSpeed = 10f;

        [SerializeField]
        private float cameraFollowSpeed = 10f;

        [SerializeField]
        private float runNoiseRadius = 5f;

        private MainControls input;

        private bool isCrouched = false;
    
        private CharacterController _controller;
        private Animator _animator;
        private AudioManager _audioManager;
        private Camera _camera;
        private ContactSensor3D _interactSensor;
        private PickupController _pickupController;
        private SoundEmitter _soundEmitter;
        private Health _health;
        private bool _isDead;
        private static readonly int Animator_Speed = Animator.StringToHash("Speed");
        private static readonly int Animator_Dead = Animator.StringToHash("IsDead");

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _audioManager = GetComponent<AudioManager>();
            _pickupController = GetComponent<PickupController>();
            _animator = GetComponent<Animator>();
            _interactSensor = GetComponent<ContactSensor3D>();
            _soundEmitter = GetComponent<SoundEmitter>();
            _health = GetComponent<Health>();
            
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            input = new MainControls();
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
            input.Dispose();
        }

        private IEnumerator LevelFailedCoro()
        {
            yield return new WaitForSeconds(0.5f);
            var gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager?.LevelFailed();
        }
        // Update is called once per frame
        void Update()
        {
            if (_isDead)
            {
                _animator.SetFloat(Animator_Speed, 0);
                return;
            }

            if (_health.CurrentHealth == 0)
            {
                _isDead = true;
                _animator.SetBool(Animator_Dead, true);
                gameObject.layer = LayerMask.NameToLayer("Ignore");
                StartCoroutine(LevelFailedCoro());
                return;
            }
            

            isCrouched = input.Main.Crouch.ReadValue<float>() > 0;

            var inputDir = input.Main.Move.ReadValue<Vector2>();
            var direction = inputDir.ToVector3();
            
            if(direction.magnitude > 1){
                direction = direction.normalized;
            }

            if (isCrouched)
            {
                direction *= crouchSpeedFactor;
            }
        
            if(direction.magnitude != 0){
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        targetRotation,
                        Time.deltaTime * turnSpeed)
                    ;

                _controller.SimpleMove(direction * speed);
            }
        
            _animator.SetFloat(Animator_Speed, direction.magnitude);

            var cameraPos = _camera.transform.position;
            var targetPos = transform.position;
            targetPos.y = cameraPos.y;
        
            _camera.transform.position = Vector3.Lerp(cameraPos, targetPos, Time.deltaTime * cameraFollowSpeed);

            if (input.Main.Use.triggered)
            {
                if (_pickupController.PickedObject != null)
                {
                    _pickupController.Drop();
                }
                else
                {
                    var contact = _interactSensor.Contacts.FirstOrDefault();


                    if (contact != null)
                    {
                        if (contact.TryGetComponent<IInteractable>(out var interactable))
                        {
                            interactable.Interact();
                        }
                        else if(contact.TryGetComponent<Item>(out var item))
                        {
                            _pickupController.Pickup(item);    
                        }
                    }
                }
            }

        }

        void FootStep()
        {
            if (isCrouched)
            {
                return;
            }
        
            _soundEmitter.Emit();
            _audioManager.PlayAudio("Footstep");
        }


    }
}
