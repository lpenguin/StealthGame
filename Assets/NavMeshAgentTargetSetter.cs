using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentTargetSetter : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private bool updateOnlyIfChanged = false;
    
    private NavMeshAgent _agent;
    private Vector3 _prevPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!updateOnlyIfChanged || target.position != _prevPosition)
        {
            if (!_agent.SetDestination(target.position))
            {
                Debug.LogWarning($"{Time.frameCount}: Cannot request SetDestination");
            }
        }

        if (!_agent.hasPath)
        {
            Debug.LogWarning("Agent has no path");
        }

        _prevPosition = target.position;
    }
    
    private void OnDrawGizmos()
    {
        if (_agent == null)
        {
            return;
        }
        
        
        var pos = _agent.transform.position;
        var prevPos = pos;
        foreach (var vector3 in _agent.path.corners)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(prevPos, vector3);
            prevPos = vector3;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, _agent.destination);

    }
}
