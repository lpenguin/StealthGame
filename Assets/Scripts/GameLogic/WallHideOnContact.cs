using System;
using UnityEngine;

namespace GameLogic
{
    public class WallHideOnContact : MonoBehaviour
    {
        [SerializeField]
        private ContactSensor3D sensor;

        private MeshRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            sensor.onEnter.AddListener(Sensor_OnContactEnter);
            sensor.onExit.AddListener(Sensor_OnContactExit);
        }

        private void OnDisable()
        {
            sensor.onEnter.RemoveListener(Sensor_OnContactEnter);
            sensor.onExit.RemoveListener(Sensor_OnContactExit);
            
        }
        
        private void Sensor_OnContactEnter(Collider collider)
        {
            _renderer.enabled = false;
        }
        
        private void Sensor_OnContactExit(Collider collider)
        {
            _renderer.enabled = true;

        }
    }
}
