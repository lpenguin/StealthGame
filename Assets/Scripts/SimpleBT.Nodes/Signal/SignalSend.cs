using System;
using Signals;
using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.Signal
{
    [Name("Signal.Send")]
    public class SignalSend: Node
    {
        private TransformParameter target;
        private StringParameter signal;
        private SignalType _signalType;
        private ISignalHandler _handler;
        protected override void OnStart()
        {
            if (!Enum.TryParse(signal, out _signalType))
            {
                throw new Exception($"Invalid signal type: {signal.Value}");
            }

            if (!target.Value.TryGetComponent(out _handler))
            {
                throw new Exception($"Target has no signal handler attached ({target.Value.gameObject.name})");
            }
        }

        protected override Status OnUpdate()
        {
            _handler.HandleSignal(_signalType);
            return Status.Success;
        }
    }
}