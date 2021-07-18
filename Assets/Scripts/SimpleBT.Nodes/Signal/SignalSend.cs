using System;
using System.Reflection;
using Signals;
using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes.Signal
{
    [Name("Method.Call")]
    public class SignalSend: Node
    {
        private TransformParameter target;
        private StringParameter signal;
        private MethodInfo _methodInfo;
        private Component _target;

        // private SignalType _signalType;
        // private ISignalHandler _handler;
        protected override void OnStart()
        {

            if(_target == null)
            {
                var tokens = signal.Value.Split('.');

                if(tokens.Length != 2)
                {
                    throw new Exception($"Invalid method signature: {signal.Value}");
                }

                string className = tokens[0];
                string methodName = tokens[1];

                foreach(var comp in target.Value.GetComponents<Component>())
                {
                    var compType = comp.GetType();
                    var compName = compType.Name;
                    if(compName == className)
                    {                        
                        _methodInfo = compType.GetMethod(methodName);
                        if(_methodInfo == null)
                        {
                            continue;
                        }

                        _target = comp;
                        break;
                    }
                }

                if(_target == null)
                {
                    throw new Exception($"Cannot find type: {className} with method {methodName}");
                }

            }


        }

        protected override Status OnUpdate()
        {
            _methodInfo.Invoke(_target, null);

            return Status.Success;
        }
    }
}