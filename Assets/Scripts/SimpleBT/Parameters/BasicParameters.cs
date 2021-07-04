using SimpleBT.Types;
using UnityEngine;

namespace SimpleBT.Parameters
{
    public class StringParameter : Parameter<string>
    {
        public static implicit operator StringParameter(string value)
        {
            return new StringParameter{Value = value};
        }
        
        public static implicit operator string(StringParameter p)
        {
            return p.Value;
        }
    }
    
    public class StringArrayParameter : Parameter<string[]>
    {
        public static implicit operator StringArrayParameter(string[] value)
        {
            return new StringArrayParameter{Value = value};
        }
        
        public static implicit operator string[](StringArrayParameter p)
        {
            return p.Value;
        }
    }
    
    public class FloatParameter : Parameter<float>
    {
        public static implicit operator FloatParameter(float value)
        {
            return new FloatParameter{Value = value};
        }
        
        public static implicit operator float(FloatParameter p)
        {
            return p.Value;
        }
    }
    
    public class IntParameter : Parameter<int>
    {
        public static implicit operator IntParameter(int value)
        {
            return new IntParameter{Value = value};
        }
        
        public static implicit operator int(IntParameter p)
        {
            return p.Value;
        }
    }
    
    public class NullableIntParameter : Parameter<NullableInt>
    {
        public static implicit operator NullableIntParameter(NullableInt value)
        {
            return new NullableIntParameter{Value = value};
        }
        
        public static implicit operator NullableInt(NullableIntParameter p)
        {
            return p.Value;
        }
    }
    
    public class Vector3Parameter : Parameter<Vector3>
    {
        public static implicit operator Vector3Parameter(Vector3 value)
        {
            return new Vector3Parameter{Value = value};
        }
        
        public static implicit operator Vector3(Vector3Parameter p)
        {
            return p.Value;
        }
    }

    public class BoolParameter : Parameter<bool>
    {
        public static implicit operator BoolParameter(bool value)
        {
            return new BoolParameter {Value = value};
        }

        public static implicit operator bool(BoolParameter p)
        {
            return p.Value;
        }
    }

    public class GameObjectParameter : Parameter<GameObject>
    {
        public static implicit operator GameObjectParameter(GameObject value)
        {
            return new GameObjectParameter{Value = value};
        }
    }
    
    public class TransformParameter : Parameter<Transform>
    {
        public static implicit operator TransformParameter(Transform value)
        {
            return new TransformParameter{Value = value};
        }
        
        public static implicit operator Transform(TransformParameter p)
        {
            return p.Value;
        }
    }
    
    public class QuaternionParameter : Parameter<Quaternion>
    {
        public static implicit operator QuaternionParameter(Quaternion value)
        {
            return new QuaternionParameter{Value = value};
        }
        
        public static implicit operator Quaternion(QuaternionParameter p)
        {
            return p.Value;
        }
    }

    public class NodeParameter : Parameter<Node>
    {
        public static implicit operator NodeParameter(Node value)
        {
            return new NodeParameter{Value = value};
        }
        
        public static implicit operator Node(NodeParameter p)
        {
            return p.Value;
        }
    }
}