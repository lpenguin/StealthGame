using System;
using System.Collections.Generic;
using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace SimpleBT.Nodes.Navigation
{
    [Name("Nav.RandomPoint")]
    public class RandomPointInArea: Node
    {
        private Vector3Parameter position;
        
        private FloatParameter distance = 3;
        private FloatParameter minDistance = 3;
        private FloatParameter grid = .5f;
        private NavMeshAgent _agent;
        protected override void OnStart()
        {
            _agent = currentContext.GameObject.GetComponent<NavMeshAgent>();
        }

        protected override Status OnUpdate()
        {
            var curPos = currentContext.GameObject.transform.position;
            var gridV = grid.Value;
            var distanceV = distance.Value;
            NavMeshPath path = new NavMeshPath();

            List<Vector3> positions = new List<Vector3>();
            for (float x = - distanceV; x < distanceV; x += gridV)
            {
                for (float y = -distanceV; y < distanceV; y += gridV)
                {
                    float pointDistance = Mathf.Sqrt(x * x + y * y);
                    if (pointDistance > distanceV)
                    {
                        continue;
                    }

                    if (pointDistance < minDistance)
                    {
                        continue;
                    }

                    Vector3 position = new Vector3(x, 0, y) + curPos;
                    if (!_agent.CalculatePath(position, path))
                    {
                        continue;
                    }

                    if (path.status == NavMeshPathStatus.PathInvalid)
                    {
                        continue;
                    }

                    float dist = 0;
                    var corners = path.corners;
                    var prev = corners[0];
                    for (int i = 1; i < corners.Length; i++)
                    {
                        dist += Vector3.Distance(corners[i], prev);
                        prev = corners[i];
                    }

                    if (dist > distanceV)
                    {
                        continue;
                    }
                    positions.Add(position);
                }
            }

            if (positions.Count == 0)
            {
                return Status.Fail;
            }

            position.Value = positions[Random.Range(0, positions.Count)];
            Debug.Log($"Selected position: {position.Value}");

            return Status.Success;
        }
    }
}