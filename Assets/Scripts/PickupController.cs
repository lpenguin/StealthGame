using System;
using UnityEngine;

public class PickupController: MonoBehaviour
{
    [SerializeField] 
    private Transform pickupPoint;

    [SerializeField] 
    private Transform dropPoint;
    
    private Transform _pickedObject;

    public Transform PickedObject => _pickedObject;
    
    public void Pickup(Transform obj)
    {
        _pickedObject = obj;
        if (_pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

        if (_pickedObject.TryGetComponent<Item>(out var item))
        {
            item.owner = gameObject;
        }
        
        _pickedObject.transform.rotation = Quaternion.identity;
        
        _pickedObject.position = pickupPoint.position;
    }
    
    public void Drop()
    {
        _pickedObject.position = dropPoint.position;
        
        if (_pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }
        if (_pickedObject.TryGetComponent<Item>(out var item))
        {
            item.owner = null;
        }
        _pickedObject = null;
    }

    private void Update()
    {
        if (_pickedObject != null)
        {
            _pickedObject.position = pickupPoint.position;
            _pickedObject.rotation *= Quaternion.AngleAxis(100 * Time.deltaTime, Vector3.up);
        }
    }
}