using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtCamera : MonoBehaviour
{
    private Transform _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera == null)
        {
            _camera = Camera.main.transform;
        }

        var direction = _camera.position - transform.position;
        direction.x = 0;
        
        var xRot = _camera.rotation.eulerAngles.x;
        transform.rotation = Quaternion.AngleAxis(xRot, Vector3.right);
        transform.rotation = Quaternion.LookRotation(-direction); 
    }
}
