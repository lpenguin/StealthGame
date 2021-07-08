using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Events
{
    [Name("Event.Wait")]
    public class Event: Node
    {
        private StringParameter name;
        private Parameter<object> arg1;
        private Parameter<object> arg2;
        private Parameter<object> arg3;

        private BTEvent _handledEvent = null;
        private EventBus _bus;
        protected override void OnStart()
        {
            _handledEvent = null;

            _bus = currentContext.EventBus;
            _bus.RegisterCallback(name.Value, OnNewEvent);
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

            
            return Status.Running;
        }

        protected override void OnEnd()
        {
             _bus?.DeregisterCallback(name.Value, OnNewEvent);
             _bus = null;
             _handledEvent = null;
        }

        protected override void OnAbort()
        {
            _bus?.DeregisterCallback(name.Value, OnNewEvent);
            _bus = null;
            _handledEvent = null;
        }


        private void OnNewEvent(BTEvent ev){
            _handledEvent = ev;
        }
    }
}