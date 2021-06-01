using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ConvexHullCollider2D : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private ConvexHull2D _convexHull;

    public readonly UnityEvent ConvexHullChanged = new UnityEvent(); 
    public ConvexHull2D ConvexHull
    {
        get
        {
            CreateHullIfNeeded();
            return _convexHull;
        }
    }

    void Start()
    {
        CreateHullIfNeeded();
    }

    private void CreateHullIfNeeded()
    {
        _convexHull ??= new ConvexHull2D(new[] {new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1)});
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
       Vector3 Vector2ToVector3(Vector2 v){
            return new Vector3(v.x, 0, v.y);
        }

        Gizmos.color = Color.green;

        var pos = transform.position;
        var transformation = Matrix4x4.Translate(pos) * Matrix4x4.Rotate(transform.rotation) ;
        foreach(var segment in _convexHull.Segments){
            Gizmos.DrawLine(
                transformation.MultiplyPoint3x4(Vector2ToVector3(segment.Start)), 
                transformation.MultiplyPoint3x4(Vector2ToVector3(segment.End)));
        }
    }

    public bool ContainsPoint(Vector3 pos)
    {
        var localPos = transform.InverseTransformPoint(pos);
        return _convexHull.Contains(localPos.ToVector2());
    }
}
