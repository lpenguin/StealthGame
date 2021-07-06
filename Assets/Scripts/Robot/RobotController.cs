using System;
using System.Collections;
using Audio;
using SimpleBT;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;


namespace Robot
{
    [Serializable]
    public class StringColorDictionary : SerializableDictionary<string, Color> {}

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


    [Header("Emotions")]
    [SerializeField]
    private StringColorDictionary emotions = new StringColorDictionary();

    public Light emotionLight;
    public string currentEmotion;
    
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private AudioManager _audioManager;
    private bool _isLookingAt = false;
    private Coroutine _barkCoro;
    private Vector3 _initialLookAtPosition;
    
    private Blackboard blackboard;
    private const string BB_InitialPosition = "Initial Position";
    private const string BB_InitialRotation = "Initial Rotation";
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _audioManager = GetComponent<AudioManager>();
        lookAtRig.weight = 0;
        blackboard = GetComponent<BehaviourTreeExecutor>().blackboard;
        blackboard.SetValue(BB_InitialPosition, transform.position);
        blackboard.SetValue(BB_InitialRotation, transform.rotation);
        _initialLookAtPosition = lookAtSource.localPosition;
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

    public void StartLookAt()
    {
        _isLookingAt = true;
    }
    
    public void StopLookAt()
    {
        _isLookingAt = false;
        lookAtSource.localPosition = _initialLookAtPosition;
    }
    
    void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude /  _navMeshAgent.speed);
        lookAtRig.weight = Mathf.Lerp(lookAtRig.weight, _isLookingAt ? 1 : 0, Time.deltaTime * switchLookAtSpeed);
        if (emotions.TryGetValue(currentEmotion, out var color))
        {
            emotionLight.color = color;
        }
    }
    
    void Footstep()
    {
        _audioManager.PlayAudio("Step");
    }
    
    private void OnDrawGizmos()
    {
        if (_navMeshAgent == null)
        {
            return;
        }
        
        
        var pos = _navMeshAgent.transform.position;
        var prevPos = pos;
        foreach (var vector3 in _navMeshAgent.path.corners)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(prevPos, vector3);
            prevPos = vector3;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, _navMeshAgent.destination);

    }
}
}
