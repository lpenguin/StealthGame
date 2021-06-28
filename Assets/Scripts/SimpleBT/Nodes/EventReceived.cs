using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
    public class EventReceived: Node
    {
        private StringParameter name = "";
        private Parameter<object> arg1;
        private Parameter<object> arg2;
        private Parameter<object> arg3;

        private bool _handledEvent = false;

        protected override void OnStart(){
            // TODO: deregister callback. Probably need OnAwake function
            currentContext.EventBus.RegisterCallback(name.Value, OnNewEvent);
            _handledEvent = false;
        }
        
        protected override Status OnUpdate()
        {
            var result = _handledEvent ? Status.Success : Status.Failed;
            _handledEvent = false;
            return result;
        }



        private void OnNewEvent(BTEvent ev){
            _handledEvent = true;
            if(ev.Arg1.HasValue){
                arg1.Value = ev.Arg1.Value;
            }

            if(ev.Arg2.HasValue){
                arg2.Value = ev.Arg2.Value;
            }

            if(ev.Arg3.HasValue){
                arg3.Value = ev.Arg2.Value;
            }
        }
    }
}