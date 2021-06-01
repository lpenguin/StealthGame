using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Segment {
    private readonly Vector2 _start;
    private readonly Vector2 _end;
    private readonly Vector2 _diff;

    public Vector2 Start => _start;
    public Vector2 End => _end;

    public Segment(Vector2 start, Vector2 end){
        _start = start;
        _end = end;
        _diff = _end - _start;
    }

    public float Side(Vector2 point){
        // return (point.x - _start.x) / _diff.x - (point.y - _start.y) / _diff.y;
        var pdiff = point - _start;
        return Mathf.Sign(_diff.x * pdiff.y - _diff.y * pdiff.x);
    }
}

[Serializable]
public class ConvexHull2D
{
    [SerializeField]
    private Vector2[] _points;
    
    private Segment[] _segments;

    public ConvexHull2D(Vector2[] points)
    {
        SetPoints(points);
    }

    public void SetPoints(Vector2[] points)
    {
        _points = points;
        CalcSegments();
    }

    public IReadOnlyList<Vector2> Points => _points;
    public IReadOnlyList<Segment> Segments
    {
        get
        {
            if (_segments == null || _segments.Length == 0)
            {
                CalcSegments();
            }
            return _segments;
        }
    }

    public bool Contains(Vector2 point){
        if (_segments == null)
        {
            CalcSegments();
        }
        float side = _segments[0].Side(point);
        // Debug.Log($"side[0]: {side}");
        bool contains = true;
        for(int i = 1; i < _segments.Length; i++){
            
            float side1 = _segments[i].Side(point);
            // Debug.Log($"side[{i}]: {side1}");
            if(side1 != side)
            {
                return false;
            }
        }

        return true;
    }

    private void CalcSegments(){
        _segments = new Segment[_points.Length];
        for(int i = 0; i < _points.Length - 1; i++){
            _segments[i] = new Segment(_points[i], _points[i+1]);
        }
        _segments[_points.Length - 1] = new Segment(_points[_points.Length - 1], _points[0]);
    }
}
