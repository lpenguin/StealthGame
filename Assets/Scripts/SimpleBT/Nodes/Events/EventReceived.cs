using SimpleBT.Attributes;
using SimpleBT.Nodes.Composites;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Events
{
    [Name("Selector.Active.HasReceivedEvent")]
    public class EventReceived: Node, IEventNode
    {
        private StringParameter name = "";
        private Parameter<object> arg1;
        private Parameter<object> arg2;
        private Parameter<object> arg3;

        private BTEvent _handledEvent;
        private EventBus _bus;
        private bool _registered;
        
        protected override void OnStart()
        {
            _bus = currentContext.EventBus;

            RegisterEvent();
        }

        public void RegisterEvent()
        {
            if (!_registered)
            {
                // Debug.Log($"{this} RegisterEvent");
                _registered = true;
                _bus.RegisterCallback(name.Value, OnNewEvent);
            } 
        }
        
        
        // TODO: not used
        public void DeregisterEvent()
        {
            // Debug.Log($"{this} DeregisterEvent");
            _bus.DeregisterCallback(name.Value, OnNewEvent);
            _registered = false;
            _handledEvent = null;
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

        private void OnNewEvent(BTEvent ev)
        {
            // Debug.Log($"OnNewEvent: {name.Value} {ev.Arg1}");
            _handledEvent = ev;
        }

        protected override void Destroy()
        {
            // Debug.Log($"{this} Destroy()");
            DeregisterEvent();
            base.Destroy();
        }
    }
}