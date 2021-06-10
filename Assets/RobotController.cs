using System.Collections;
using SimpleBT;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class RobotController : MonoBehaviour
{
    [SerializeField] 
    private Transform lookAtSource;

    [SerializeField] 
    private Rig lookAtRig;
    
    [SerializeField]
    private float switchLookAtSpeed = 5;

    [SerializeField] 
    private Transform pickedObject;

    [SerializeField] 
    private Transform pickupLocation;
    
    [SerializeField] 
    private Transform dropLocation;
    
    [Header("Bark")]
    [SerializeField]
    [InspectorName("Text")]
    private TextMeshProUGUI barkText;

    [SerializeField] 
    [InspectorName("Duration")]
    private float barkDuration;


    public Transform PickedObject => pickedObject;
    
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private bool _isLookingAt = false;
    private Coroutine _barkCoro;
    
    private Blackboard blackboard;
    private const string BB_InitialPosition = "Initial Position";
    private const string BB_InitialRotation = "Initial Rotation";

    public void Pickup(Transform target)
    {
        if (pickedObject != null)
        {
            Drop();
        }

        pickedObject = target;
        if (pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

        pickedObject.position = pickupLocation.position;
    }

    public void Drop()
    {
        pickedObject.position = dropLocation.position;
        
        if (pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }

        pickedObject = null;
    }
    public void Bark(string text)
    {
        barkText.text = text;

        IEnumerator TimeOutCoro()
        {
            yield return new WaitForSeconds(barkDuration);
            barkText.text = "";
        }

        if (_barkCoro != null)
        {
            StopCoroutine(_barkCoro);
        }

        _barkCoro = StartCoroutine(TimeOutCoro());
    }
    public void LookAt(Vector3 point)
    {
        lookAtSource.position = point;
        _isLookingAt = true;
    }

    public void StopLookAt()
    {
        _isLookingAt = false;
    }
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        lookAtRig.weight = 0;
        blackboard = GetComponent<BehaviourTreeExecutor>().blackboard;
        blackboard.SetValue(BB_InitialPosition, transform.position);
        blackboard.SetValue(BB_InitialRotation, transform.rotation);
    }

    void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude /  _navMeshAgent.speed);
        lookAtRig.weight = Mathf.Lerp(lookAtRig.weight, _isLookingAt ? 1 : 0, Time.deltaTime * switchLookAtSpeed);

        if (pickedObject != null)
        {
            pickedObject.position = pickupLocation.position;
            pickedObject.rotation *= Quaternion.AngleAxis(100 * Time.deltaTime, Vector3.up);
        }
    }
}
