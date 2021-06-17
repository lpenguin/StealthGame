using System;

namespace SimpleBT.Types
{
    [Serializable]
    public struct NullableInt
    {
        public int Value;
        public bool HasValue;

        public NullableInt(int value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableInt(int value) => new NullableInt(value);

        public static implicit operator NullableInt(NullableNull value) => new NullableInt();

        public static implicit operator int(NullableInt value) => value.Value;

        public static implicit operator int? (NullableInt value) => value.HasValue ? value.Value : new int?();
    }
    
    public sealed class NullableNull
    {
        private NullableNull()
        { }
    }
}