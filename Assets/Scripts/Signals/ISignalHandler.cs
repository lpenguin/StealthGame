namespace Signals
{
    public interface ISignalHandler
    {
        void HandleSignal(SignalType type);
    }
}