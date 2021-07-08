using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Events
{
    [Name("Event.IsReceived")]
    public class EventReceived: Node
    {
        private StringParameter name = "";
        private Parameter<object> arg1;
        private Parameter<object> arg2;
        private Parameter<object> arg3;

        // private bool _handledEvent = false;
        private bool _registered = false;
        private BTEvent _handledEvent = null;

        protected override void OnStart(){
            // TODO: deregister callback. Probably need OnAwake function
            if(!_registered){
                currentContext.EventBus.RegisterCallback(name.Value, OnNewEvent);
                _registered = true;
            }
            
        }
        
        protected override Status OnUpdate()
        {
            if (_handledEvent != null)
            {
                if(_handledEvent.Arg1.HasValue){
                    arg1.Value = _handledEvent.Arg1.Value;
                }

                if(_handledEvent.Arg2.HasValue){
                    arg2.Value = _handledEvent.Arg2.Value;
                }

                if(_handledEvent.Arg3.HasValue){
                    arg3.Value = _handledEvent.Arg3.Value;
                }

                _handledEvent = null;
                return Status.Success;
            }

            return Status.Fail;
        }



        private void OnNewEvent(BTEvent ev){
            _handledEvent = ev;
        }
    }
}