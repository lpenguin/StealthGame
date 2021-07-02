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

    
    [Header("Bark")]
    [SerializeField]
    [InspectorName("Text")]
    private TextMeshProUGUI barkText;

    [SerializeField] 
    [InspectorName("Duration")]
    private float barkDuration;


    
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private bool _isLookingAt = false;
    private Coroutine _barkCoro;
    private Vector3 _initialLookAtPosition;
    
    private Blackboard blackboard;
    private const string BB_InitialPosition = "Initial Position";
    private const string BB_InitialRotation = "Initial Rotation";
    
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

    public void StartLookAt()
    {
        _isLookingAt = true;
    }
    public void StopLookAt()
    {
        _isLookingAt = false;
        lookAtSource.position = _initialLookAtPosition;
    }
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        lookAtRig.weight = 0;
        blackboard = GetComponent<BehaviourTreeExecutor>().blackboard;
        blackboard.SetValue(BB_InitialPosition, transform.position);
        blackboard.SetValue(BB_InitialRotation, transform.rotation);
        _initialLookAtPosition = lookAtSource.position;
    }

    void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude /  _navMeshAgent.speed);
        lookAtRig.weight = Mathf.Lerp(lookAtRig.weight, _isLookingAt ? 1 : 0, Time.deltaTime * switchLookAtSpeed);
    }
}
