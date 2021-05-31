using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConvexHullCollider2D : MonoBehaviour
{
    private ConvexHull2D _convexHull;
    void Start()
    {
        _convexHull = new ConvexHull2D(new Vector2[]{new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1)});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected(){
        Vector3 Vector2ToVector3(Vector2 v){
            return new Vector3(v.x, 0, v.y);
        }

        Gizmos.color = Color.green;

        var pos = transform.position;
        foreach(var segment in _convexHull.Segments){
            Gizmos.DrawLine(pos + Vector2ToVector3(segment.Start), pos + Vector2ToVector3(segment.End));
        }
    }
}
