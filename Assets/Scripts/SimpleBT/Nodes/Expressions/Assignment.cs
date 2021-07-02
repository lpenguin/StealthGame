using SimpleBT.Attributes;
using UnityEngine;

namespace SimpleBT.Nodes.Expressions
{
    public abstract class SetNode<T>: Node
    {
        protected Parameter<T> variable;
        protected Parameter<T> value;
        protected sealed override Status OnUpdate()
        {
            variable.Value = value.Value;
            return Status.Success;
        }
    }

    [Name("Obj.Set")]
    public class ObjSet : SetNode<object>
    {
    }
    
    [Name("Bool.Set")]
    public class BoolSet: SetNode<bool>
    {
    }
    
    [Name("Transform.Set")]
    public class TransformSet: SetNode<Transform>
    {
    }
    
    [Name("Str.Set")]
    public class StrSet: SetNode<string>
    {
    }
    
    [Name("Float.Set")]
    public class FloatSet: SetNode<float>
    {
    }
    
    [Name("Vector3.Set")]
    public class Vector3Set: SetNode<Vector3>
    {
    }
}