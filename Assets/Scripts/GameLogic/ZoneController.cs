using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZoneController: MonoBehaviour
{
    [SerializeField]
    private ContactSensor3D sensor;

    [SerializeField] 
    private bool ownObjects = false;
    
    public Transform GetRandomObject()
    {
        var contacts = sensor.Contacts;
        if (contacts.Count == 0)
        {
            return null;
        }
        
        return sensor.Contacts[Random.Range(0, sensor.Contacts.Count - 1)].transform;
    }

    public int NumberOfObjects()
    {
        return sensor.Contacts.Count;
    }

    public bool Contains(Transform obj)
    {
        if (sensor.Contacts.Count > 0)
        {
            int a = 0;
        }
        return sensor.Contacts.Any(c => c.transform == obj);
    }

    private void Update()
    {
        if (!ownObjects)
        {
            return;
        }
        
        foreach (var contact in sensor.Contacts)
        {
            if (contact.TryGetComponent<Item>(out var item))
            {
                if (item.owner == null)
                {
                    item.owner = gameObject;
                }
            }
        }
    }
}