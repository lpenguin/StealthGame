using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Vision: MonoBehaviour {
    private class DelayedVisible
    {
        public float remaining;
        public Transform transform;
    }

    [SerializeField]
    private ContactSensor3D _sensor;

    [SerializeField] 
    private ConvexHullCollider2D _convexHullCollider;

    [SerializeField] 
    private float visibilityDelay = 3;
    // [SerializeField]
    // [Range(0, 360)]
    // private float fov = 90;
    //
    // [SerializeField]
    // private float distance = 10f;

    [SerializeField]
    private LayerMask obstacles;

    [SerializeField] 
    private Transform trackHead;
    
    public IReadOnlyList<Transform> Targets => _contacts;

    private Transform _transform;

    private Transform[] _contacts = new Transform[0];
    private Dictionary<int, DelayedVisible> _delayedSee = new Dictionary<int, DelayedVisible>();
    private void Start(){
        _transform = GetComponent<Transform>();

    }

    private void Update() {
        // _contacts = _sensor.Contacts
            // .Where(CanSee)
            // .Select(c => c.transform)
            // .ToArray();
        float deltaTime = Time.deltaTime;
        foreach (var kv in _delayedSee.ToArray())
        {
            kv.Value.remaining -= deltaTime;
            if (kv.Value.remaining < 0)
            {
                _delayedSee.Remove(kv.Key);
            }
        }

        var currentlySee = _sensor.Contacts
            .Where(CanSee)
            .Select(c => c.transform);
        foreach (var see in currentlySee)
        {
            int id = see.GetInstanceID();
            if (!_delayedSee.TryGetValue(id, out var delayedSee))
            {
                delayedSee = new DelayedVisible {transform = see};
                _delayedSee[id] = delayedSee;
            }
            delayedSee.remaining = visibilityDelay;
        }

        _contacts = _delayedSee.Values.Select(dv => dv.transform).ToArray();
        // .ToDictionary(c => c.GetInstanceID(), c => c);

        if (trackHead != null)
        {
            var angles = trackHead.localRotation.eulerAngles;
            var selfAngles = transform.localRotation.eulerAngles;
            selfAngles.y = angles.y;
            transform.localRotation = Quaternion.Euler(selfAngles);
        }
    }

    private static Vector3 DropY(Vector3 vector){
        vector.y = 0;
        return vector;
    }

    private bool CanSee(Collider collider){
        var pos = collider.transform.position;
        var distance = DropY(pos - transform.position);
  
        var angle = Vector3.Angle(distance, DropY(transform.forward));
   
        // if(!(angle < fov/2f && distance.magnitude < this.distance)){
        //     return false;
        // }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, distance.normalized, out hit, distance.magnitude, obstacles))
        {
            return false;
        }

        var convexContains = _convexHullCollider.ContainsPoint(pos);
        if (!convexContains)
        {
            return false;
        }
        
        return true;
        // return hit.collider == collider;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_transform == null){
            return;
        }
        
        foreach(var contact in _contacts){
            Gizmos.DrawSphere(contact.position, .5f);
        }
    }

    // private void OnDrawGizmos(){
    //     if(_transform == null){
    //         return;
    //     }
    //
    //     var rot = _transform.rotation;
    //     
    //     var leftRot = rot * Quaternion.AngleAxis(-fov/2, _transform.up);
    //     var rightRot = rot * Quaternion.AngleAxis(fov/2, _transform.up);
    //
    //     var forward = _transform.forward;
    //     var left = Quaternion.AngleAxis(-fov/2, _transform.up) * forward;
    //     var right = Quaternion.AngleAxis(fov/2, _transform.up) * forward;
    //
    //     Gizmos.DrawRay(_transform.position, left * distance);
    //     Gizmos.DrawRay(_transform.position, right * distance);
    //
    //     Gizmos.color = Color.red;
    //
    //     foreach(var contact in _contacts){
    //         Gizmos.DrawWireSphere(contact.position, .5f);
    //     }
    //
    // }
}