using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Sensor
{
    [Name("Vision.HasTarget")]
    public class VisionHasTarget: Node
    {
        private Parameter<Transform> sensor;
        private Parameter<Transform> target;
        
        private Vision _vision;

        protected override void OnStart()
        {
        	_vision = sensor.Value?.GetComponent<Vision>();
            Assert.IsNotNull(_vision, "_vision != null");
        }

        protected override Status OnUpdate()
        {
        	foreach(var contact in _vision.Contacts){
        		if(contact == target.Value){
        			return Status.Success;
        		}
        	}
        	return Status.Fail;
        }
    }
}