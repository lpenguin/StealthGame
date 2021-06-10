using System.Linq;
using UnityEngine;

public class ZoneController: MonoBehaviour
{
    [SerializeField]
    private ContactSensor3D sensor;

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
}