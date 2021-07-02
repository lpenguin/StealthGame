using System;
using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleBT.Nodes
{
    public abstract class EqualsNodeStruct<T> : Node where T: struct
    {
        protected Parameter<T> value1;
        protected Parameter<T> value2;
        
        protected override Status OnUpdate()
        {
            return value1.Value.Equals(value2.Value) ? Status.Success : Status.Failed;
        }
    }
    
    public abstract class EqualsNodeObject<T> : Node where T: class
    {
        protected Parameter<T> value1;
        protected Parameter<T> value2;
        
        protected override Status OnUpdate()
        {
            var v1 = value1.Value;
            var v2 = value2.Value;
            if (v1 == null)
            {
                return v2 == null ? Status.Success : Status.Failed;
            }
            
            return value1.Value.Equals(value2.Value) ? Status.Success : Status.Failed;
        }
    }

    public abstract class NotEqualsNodeStruct<T> : Node where T: struct
    {
        protected Parameter<T> value1;
        protected Parameter<T> value2;
        
        protected override Status OnUpdate()
        {
            return !value1.Value.Equals(value2.Value) ? Status.Success : Status.Failed;
        }
    }
    
    public abstract class NotEqualsNodeObject<T> : Node where T: class
    {
        protected Parameter<T> value1;
        protected Parameter<T> value2;
        
        protected override Status OnUpdate()
        {
            return !value1.Value.Equals(value2.Value) ? Status.Success : Status.Failed;
        }
    }
    
    // [Name("Obj.Equals")]
    // public class ObjectEquals: EqualsNodeObject<object>
    // {
    // }
    
    [Name("Str.Equals")]
    public class StrEquals: EqualsNodeObject<string>
    {
    }
    
    [Name("Int.Equals")]
    public class IntEquals: EqualsNodeStruct<int>
    {
    }
    
    [Name("Float.Equals")]
    public class FloatEquals: EqualsNodeStruct<float>
    {
    }
    
    [Name("Transform.Equals")]
    public class TransformEquals: EqualsNodeObject<Transform>
    {
    }
    
    [Name("Bool.Equals")]
    public class BoolEquals: EqualsNodeStruct<bool>
    {
    }
    
    [Name("Str.NotEquals")]
    public class StrNotEquals: NotEqualsNodeObject<string>
    {
    }
    
    [Name("Int.NotEquals")]
    public class IntNotEquals: NotEqualsNodeStruct<int>
    {
    }
    
    [Name("Float.NotEquals")]
    public class FloatNotEquals: NotEqualsNodeStruct<float>
    {
    }
    
    [Name("Transform.NotEquals")]
    public class TransformENotquals: NotEqualsNodeObject<Transform>
    {
    }
    
    [Name("Bool.NotEquals")]
    public class BoolENotquals: NotEqualsNodeStruct<bool>
    {
    }
    

}