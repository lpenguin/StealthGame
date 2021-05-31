using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return (point.x - _start.x) / _diff.x - (point.y - _start.y) / _diff.y;
    }
}

public class ConvexHull2D
{


    private Vector2[] _points;
    private Segment[] _segments;

    public ConvexHull2D(Vector2[] points){
        _points = points;
        CalcSegments();
    }
    

    public IReadOnlyList<Vector2> Points => _points;
    public IReadOnlyList<Segment> Segments => _segments;

    public bool Contains(Vector2 point){

        float side = Mathf.Sign(_segments[0].Side(point));
        for(int i = 1; i < _segments.Length; i++){
            if(Mathf.Sign(_segments[i].Side(point)) != side){
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
