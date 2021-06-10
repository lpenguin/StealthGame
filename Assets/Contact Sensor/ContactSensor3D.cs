using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;



public class ContactSensor3D: MonoBehaviour {
    [Serializable]
    public class Event : UnityEvent<Collider> { }

    [Header("Sensor")]
    [SerializeField]
    private Collider sensor;
    
    [SerializeField]
    private LayerMask layer;

    [SerializeField] 
    private QueryTriggerInteraction useTriggers = QueryTriggerInteraction.UseGlobal;

    [Header("Events")]
    [SerializeField]
    public Event onEnter;
    [SerializeField]
    public Event onStay;
    [SerializeField]
    public Event onExit;

    public IReadOnlyList<Collider> Contacts => _contacts;
    public Collider Collider => sensor;
    public LayerMask LayerMask => layer;
    public QueryTriggerInteraction QueryTriggerInteraction => useTriggers;
    private ArraySegment<Collider> _contacts = new ArraySegment<Collider>(new Collider[256], 0, 0);
    
    private int contactsCount;



    private void Start()
    {
        if (sensor == null)
        {
            sensor = GetComponent<Collider>();
        }
    }

    private void OnDisable()
    {
        _contacts = new ArraySegment<Collider>(new Collider[256], 0, 0);
    }


    private void UpdateContacts()
    {
        Collider[] contactsArr = _contacts.Array;
        
        int contactsCount;
        while ((contactsCount = OverlapColliderNonAlloc(sensor, layer, useTriggers, contactsArr)) == contactsArr.Length)
        {
            contactsArr = new Collider[(int) (contactsArr.Length * 2.5) + 1];
        }

        _contacts = new ArraySegment<Collider>(contactsArr, 0, contactsCount);
    }
    
    private static int OverlapColliderNonAlloc(Collider collider, int mask, QueryTriggerInteraction queryTriggerInteraction, Collider[] results)
    {
        var tr = collider.transform;
        var pos = tr.position;
        var rot = tr.rotation;
        
        if (collider is BoxCollider boxCollider)
        {
            Vector3 scale = tr.localScale;
            Vector3 size = boxCollider.size / 2;
            size.x *= scale.x;
            size.y *= scale.y;
            size.z *= scale.z;
            return Physics.OverlapBoxNonAlloc(
                pos + boxCollider.center, 
                size, 
                results, 
                rot, 
                mask, 
                queryTriggerInteraction);
        }

        if (collider is SphereCollider sphereCollider)
        {
            return Physics.OverlapSphereNonAlloc(
                pos + sphereCollider.center,
                sphereCollider.radius,
                results,
                mask,
                queryTriggerInteraction);

        }

        if (collider is CapsuleCollider capsuleCollider)
        {
            // TODO: direction
            Vector3 scale = tr.localScale;
            Vector3 offset = Vector3.up * capsuleCollider.height/2 * scale.y;
            Vector3 center = capsuleCollider.center;
            return Physics.OverlapCapsuleNonAlloc(
                pos + center - offset,
                pos + center + offset,
                capsuleCollider.radius * scale.x,
                results,
                mask,
                queryTriggerInteraction
            );
        }

        throw new ArgumentException($"Invalid collider: {collider}");
    }

    public void ForceUpdate()
    {
        FixedUpdate();
    }
    private void FixedUpdate()
    {
        var prevContacts = new HashSet<Collider>(_contacts);
        UpdateContacts();

        
        if (onStay != null || onEnter != null || onExit != null)
        {
            foreach (var contact in _contacts)
            {
                if (prevContacts.Contains(contact))
                {
                    onStay?.Invoke(contact);
                }
                else
                {
                    onEnter?.Invoke(contact);
                }

                prevContacts.Remove(contact);
            }

            foreach (var contact in prevContacts)
            {
                onExit?.Invoke(contact);
            }
        }

        contactsCount = Contacts.Count;
    }
}