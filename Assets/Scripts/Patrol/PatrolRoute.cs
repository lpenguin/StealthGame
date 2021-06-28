using UnityEngine;

namespace Patrol
{
    public class PatrolRoute: MonoBehaviour
    {
        public enum PatrolMode
        {
            PingPong,
            Cycle,
        }
        public Vector3[] points;
        public PatrolMode patrolMode = PatrolMode.PingPong;
    }
}