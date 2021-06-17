using System;

namespace SimpleBT
{
    public interface IParameter
    {
        string Name { get; set; }
        string BbName { get; set; }
        Type Type { get; }
        object Get();
        void Set(object value);
    }

    public class Parameter<T> : IParameter
    {
        public T Value { get; set; }
        
        public string Name { get; set; }
        public string BbName { get; set; }
        
        public virtual Type Type => typeof(T);

        public Parameter(T value = default)
        {
            Value = value;
        }

        public Parameter()
        {
            Value = default;
        }
        
        public object Get()
        {
            return Value;
        }

        public void Set(object value)
        {
            Value = (T)value;
        }
    }
}