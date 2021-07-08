using System;
using UnityEngine;

public class PickupController: MonoBehaviour
{
    [SerializeField] 
    private Transform pickupPoint;

    [SerializeField] 
    private Transform dropPoint;
    
    private Item _pickedObject;

    public Item PickedObject => _pickedObject;
    
    public void Pickup(Item item)
    {
        _pickedObject = item;
        if (_pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

        item.owner = gameObject;

        var itemTransform = _pickedObject.transform;
        itemTransform.rotation = Quaternion.identity;
        itemTransform.position = pickupPoint.position;
    }
    
    public void Drop()
    {
        _pickedObject.transform.position = dropPoint.position;
        
        if (_pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }

        _pickedObject.owner = null;
        _pickedObject = null;
    }

    private void Update()
    {
        if (_pickedObject != null)
        {
            var itemTransform = _pickedObject.transform;
            itemTransform.position = pickupPoint.position;
            itemTransform.rotation *= Quaternion.AngleAxis(100 * Time.deltaTime, Vector3.up);
        }
    }
}