using UnityEngine.Events;
using UnityEngine;

namespace Extensions
{
	public static class UnityEventExtensions 
	{
		public static void DrawGizmos(this UnityEvent unityEvent, Vector3 position)
		{
            Gizmos.color = Color.green;
            int nSignals = unityEvent.GetPersistentEventCount();

            if (nSignals == 0)
            {
                return;
            }

            for(int i = 0; i < nSignals; i++)
            {
            	var target = unityEvent.GetPersistentTarget(i);

                if (target == null)
                {
                    continue;
                }

                if(target is Component comp){
					Gizmos.DrawLine(position, comp.transform.position);
                }
                
            }
		}
	}
}