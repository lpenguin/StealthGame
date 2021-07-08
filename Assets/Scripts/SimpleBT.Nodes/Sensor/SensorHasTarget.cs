using System.Linq;
using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Sensor
{
    [Name("Sensor.HasTarget")]
    public class SensorHasTarget: Node
    {
        private TransformParameter sensor;
        private TransformParameter target;
        
        private ContactSensor3D _sensor;

        protected override void OnStart()
        {
            _sensor = sensor.Value.GetComponent<ContactSensor3D>();
            Assert.IsNotNull(_sensor, $"{nameof(_sensor)} != null");
        }

        protected override Status OnUpdate()
        {
            return _sensor.Contacts.Any(c => c.transform == target.Value) ? Status.Success : Status.Fail;
        }
    }
}