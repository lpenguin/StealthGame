using System.Linq;
using Hearing;
using SimpleBT.Nodes.Robot;
using UnityEngine;

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
    private ContactSensor3D _sensor;
    private PickupController _pickupController;
    private SoundEmitter _soundEmitter;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _pickupController = GetComponent<PickupController>();
        _animator = GetComponent<Animator>();
        _sensor = GetComponent<ContactSensor3D>();
        _soundEmitter = GetComponent<SoundEmitter>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
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

        _animator.SetFloat("Speed", direction.magnitude);

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
                var contact = _sensor.Contacts.FirstOrDefault();
                if (contact != null)
                {
                    _pickupController.Pickup(contact.transform);
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
