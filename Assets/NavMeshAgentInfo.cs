using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NavMeshAgentInfo : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    private Text _text;
    
    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        if (agent == null)
        {
            return;
        }

        _text.text = $"Agent:\n" +
                     $"Is Stopped: {agent.isStopped}\n" +
                     $"Path Pending: {agent.pathPending}\n" +
                     $"Has Path: {agent.hasPath}\n" +
                     $"Path Status: {agent.pathStatus}\n" +
                     $"Remaining Distance: {agent.remainingDistance}\n" +
                     $"Is Path Stale: {agent.isPathStale}\n" +
                     $"Destination: {agent.destination}\n" +
                     $"Next Position: {agent.nextPosition}\n" +
                     $"Speed: {agent.speed}\n" +
                     "";
    }
}
