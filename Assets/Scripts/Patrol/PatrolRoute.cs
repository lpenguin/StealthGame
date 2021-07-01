using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using Utils.Array;


namespace Patrol
{

    [Serializable]
    public class PatrolRouteEditorInfo {
        public bool editingRoute = false;
        public int selectedIndex = -1;
    } 

    [Serializable]
    public class PatrolPoint {
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public bool rotate = false;
        public string treeName;
    }

    
    public class PatrolRoute: MonoBehaviour
    {
        public enum PatrolMode
        {
            PingPong,
            Cycle,
        }
        
        [FormerlySerializedAs("points")]
        public PatrolPoint[] rawPoints;
        public PatrolMode patrolMode = PatrolMode.PingPong;

        public IReadOnlyList<PatrolPoint> Points;
        
        [HideInInspector]
        public PatrolRouteEditorInfo editorInfo = new PatrolRouteEditorInfo();

        private void Start()
        {
            int[] indices;
            switch(patrolMode){
                case PatrolMode.PingPong:
                    indices = Enumerable.Range(0, rawPoints.Length)
                        .Concat(
                            Enumerable.Range(1, rawPoints.Length - 2)
                                .Reverse()
                        )
                        .ToArray();
                    break;
                case PatrolMode.Cycle:
                    indices = Enumerable.Range(0, rawPoints.Length).ToArray();
                    break;       
                default:
                    throw new Exception($"Invalid patrol mode {patrolMode}");         
            }

            Points = new IndexedView<PatrolPoint>(rawPoints, indices);
        }

        public int NextIndex(int currentIndex)
        {
            return (currentIndex + 1) % Points.Count;
        }   
    }
}