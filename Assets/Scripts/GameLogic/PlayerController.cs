using System.Linq;
using Hearing;
using Interactable;
using UnityEngine;

namespace GameLogic
{
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

        private bool isCrouched = false;
    
        private CharacterController _controller;
        private Animator _animator;
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
            _pickupController = GetComponent<PickupController>();
            _animator = GetComponent<Animator>();
            _interactSensor = GetComponent<ContactSensor3D>();
            _soundEmitter = GetComponent<SoundEmitter>();
            _health = GetComponent<Health>();
            
            _camera = Camera.main;
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
                return;
            }
            
            var dx = Input.GetAxis("Horizontal");
            var dy =  Input.GetAxis("Vertical");
            isCrouched = Input.GetButton("Crouch");

            var direction = new Vector3(dx, 0, dy);
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

            if (Input.GetKeyDown("e"))
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
        
            _soundEmitter.Emit(runNoiseRadius);
        }


    }
}
