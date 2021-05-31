using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Vision: MonoBehaviour {
    [SerializeField]
    private ContactSensor3D _sensor;

    [SerializeField]
    [Range(0, 360)]
    private float fov = 90;

    [SerializeField]
    private float distance = 10f;

    [SerializeField]
    private LayerMask obstacles;

    public IReadOnlyList<Transform> Targets => _contacts;

    private Transform _transform;

    private Transform[] _contacts = new Transform[0];
    private void Start(){
        _transform = GetComponent<Transform>();

    }

    private void Update() {
        _contacts = _sensor.Contacts
            .Where(CanSee)
            .Select(c => c.transform)
            .ToArray();

    }

    private static Vector3 DropY(Vector3 vector){
        vector.y = 0;
        return vector;
    }

    private bool CanSee(Collider collider){
        var pos = collider.transform.position;
        var distance = DropY(pos - transform.position);
  
        var angle = Vector3.Angle(distance, DropY(transform.forward));
   
        if(!(angle < fov/2f && distance.magnitude < this.distance)){
            return false;
        }

        RaycastHit hit;
        return !Physics.Raycast(transform.position, distance.normalized, out hit, this.distance, obstacles);
        // return hit.collider == collider;
    }



    private void OnDrawGizmos(){
        if(_transform == null){
            return;
        }

        var rot = _transform.rotation;
        
        var leftRot = rot * Quaternion.AngleAxis(-fov/2, _transform.up);
        var rightRot = rot * Quaternion.AngleAxis(fov/2, _transform.up);

        var forward = _transform.forward;
        var left = Quaternion.AngleAxis(-fov/2, _transform.up) * forward;
        var right = Quaternion.AngleAxis(fov/2, _transform.up) * forward;

        Gizmos.DrawRay(_transform.position, left * distance);
        Gizmos.DrawRay(_transform.position, right * distance);

        Gizmos.color = Color.red;

        foreach(var contact in _contacts){
            Gizmos.DrawWireSphere(contact.position, .5f);
        }

    }
}