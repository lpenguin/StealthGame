using System;
using SimpleBT.Attributes;

namespace SimpleBT.Nodes
{
    public abstract class GreaterThen<T> : Node where T: IComparable<T>
    {
        protected Parameter<T> value1;
        protected Parameter<T> value2;
        
        protected override Status OnUpdate()
        {
            return value1.Value.CompareTo(value2.Value) > 0 ? Status.Success : Status.Fail;
        }
    }
    
    public abstract class LessThen<T> : Node where T: IComparable<T>
    {
        protected Parameter<T> value1;
        protected Parameter<T> value2;
        
        protected override Status OnUpdate()
        {
            return value1.Value.CompareTo(value2.Value) < 0 ? Status.Success : Status.Fail;
        }
    }
    
    [Name("Int.GreaterThen")]
    public class IntGreaterThen: GreaterThen<int>
    {
    }
    
    [Name("Float.GreaterThen")]
    public class FloatGreaterThen: GreaterThen<float>
    {
    }
    
    [Name("Int.LessThen")]
    public class IntLessThen: LessThen<int>
    {
    }
    
    [Name("Float.LessThen")]
    public class FloatLessThen: LessThen<float>
    {
    }
}