using UnityEngine;

namespace SimpleBT.Parameters
{
    public class StringParameter : Parameter<string>
    {
        public static implicit operator StringParameter(string value)
        {
            return new StringParameter{Value = value};
        }
    }
    
    public class FloatParameter : Parameter<float>
    {
        public static implicit operator FloatParameter(float value)
        {
            return new FloatParameter{Value = value};
        }
    }
    
    public class Vector3Parameter : Parameter<Vector3>
    {
        public static implicit operator Vector3Parameter(Vector3 value)
        {
            return new Vector3Parameter{Value = value};
        }
    }

    public class BoolParameter : Parameter<bool>
    {
        public static implicit operator BoolParameter(bool value)
        {
            return new BoolParameter {Value = value};
        }
    }

    public class GameObjectParameter : Parameter<GameObject>
    {
        public static implicit operator GameObjectParameter(GameObject value)
        {
            return new GameObjectParameter{Value = value};
        }
    }
}