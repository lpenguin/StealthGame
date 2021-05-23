using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
    [SerializeField]
    private Pathway _pathway;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private int _currentWaypoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_navMeshAgent.pathPending && !_navMeshAgent.hasPath){
            var waypoint = _pathway.NextPoint(_currentWaypoint);
            Debug.Log($"Destination reached, setting next waypoint: {waypoint} ({_currentWaypoint})");
            _navMeshAgent.SetDestination(waypoint);
            _currentWaypoint += 1;
        }
        
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude /  _navMeshAgent.speed);
    }
}
