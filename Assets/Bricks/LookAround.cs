using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;

[Action("Custom/Look Around")]
public class LookAround : GOAction
{

    [InParam("degrees")]
    public float degrees { get; set; } = 90;

    [InParam("speed")]
    public float speed {get; set; } = 3f;

    private Quaternion _initialRotation;
    private Transform _transform;
    private bool _rotatingLeft = true;

    private const float EPSILON =  0.9999999f;

    // Start is called before the first frame update
    public override void OnStart()
    {
        _transform = gameObject.transform;
        _initialRotation = _transform.rotation;
    }

    public override TaskStatus OnUpdate()
    {
        var target = Quaternion.AngleAxis((_rotatingLeft ? -1 : 1) * degrees, _transform.up) * _initialRotation;
        _transform.rotation = Quaternion.Lerp(_transform.rotation, target, speed * Time.deltaTime);
        
        var abs = Mathf.Abs(Quaternion.Dot(target, _transform.rotation));
        if(abs >= EPSILON){
            if(_rotatingLeft){
                _rotatingLeft = false;
            } else {
                return TaskStatus.COMPLETED;
            }
        }

        return TaskStatus.RUNNING;
    }
}
