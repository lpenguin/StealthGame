using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleBT.Attributes;

namespace SimpleBT
{
    [Flags]
    public enum Status
    {
        Empty = 0b00001,
        Running = 0b00010,
        Fail = 0b00100,
        Success = 0b01000,
        // Interrupted = 0b10000
    }

    public abstract class Node
    {
        public Status Status { get; private set; } = Status.Empty;
        public IList<Node> Children { get; } = new List<Node>();
        public List<IParameter> Parameters { get; } = new List<IParameter>();
        public string Id {get; set;}

        public string Comment { get; set; }
        
        protected ExecutionContext currentContext;
        
        public string Name { get; }

        public void Execute(ExecutionContext context)
        {
            currentContext = context;
            
            var blackboard = context.Blackboard;
            LoadParameters(blackboard);
            
            if (Status == Status.Empty)
            {
                OnStart();
            }
            
            Status = OnUpdate();

            if ((Status & (Status.Fail | Status.Success)) != 0)
            {
                OnEnd();
            }
            SaveParameters(blackboard);
            currentContext = null;
        }

        protected virtual void OnEnd()
        {
            
        }

        private void SaveParameters(Blackboard blackboard)
        {
            foreach (var parameter in Parameters)
            {
                if (!string.IsNullOrEmpty(parameter.BbName))
                {
                    blackboard.SetValue(parameter.BbName, parameter.Get());
                }
            }
        }

        private void LoadParameters(Blackboard blackboard)
        {
            foreach (var parameter in Parameters)
            {
                if (!string.IsNullOrEmpty(parameter.BbName))
                {
                    parameter.Set(blackboard.GetValue(parameter.BbName));
                }
            }
        }

        protected abstract Status OnUpdate();

        protected virtual void OnStart()
        {
        }

        protected virtual void OnAbort()
        {
        }

        protected static void OnAbort(Node node)
        {
            node.OnAbort();
        }
        
        protected static void OnEnd(Node node)
        {
            node.OnEnd();
        }
        
        public virtual void Reset()
        {
            if (Status == Status.Empty)
            {
                return;
            }

            if (Status == Status.Running)
            {
                OnAbort();
            }
            
            Status = Status.Empty;
            foreach (var child in Children)
            {
                child.Reset();
            }
        }

        public Node Clone()
        {
            var type = GetType();
            Node clone = Activator.CreateInstance(type) as Node;
            
            for (int i = 0; i < Parameters.Count; i++)
            {
                var param = Parameters[i];
                var cloneParam = clone.Parameters[i];
                cloneParam.BbName = param.BbName;
                cloneParam.Set(param.Get());
            }

            foreach (var child in Children)
            {
                clone.Children.Add(child.Clone());
            }
            
            return clone;
        }
        
        protected Node()
        {
            Type iParameterType = typeof(IParameter);
            var type = GetType();

            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType.GetInterfaces().Any(t => t == iParameterType))
                {
                    IParameter parameter = fieldInfo.GetValue(this) as IParameter ?? Activator.CreateInstance(fieldInfo.FieldType) as IParameter;
                    fieldInfo.SetValue(this, parameter);
                    if (parameter == null)
                    {
                        throw new Exception($"Cannot create parameter instance of type {fieldInfo.FieldType}");
                    }

                    parameter.Name = fieldInfo.Name;
                    Parameters.Add(parameter);
                }
            }
            
            var nameAttrObj = type.GetCustomAttributes(typeof(NameAttribute), false).FirstOrDefault();
            if (nameAttrObj is NameAttribute nameAttr)
            {
                Name = nameAttr.Name;
            }
            else
            {
                Name = type.Name;
            }
        }


        // public virtual void Interrupt()
        // {
        //     Status = Status.Interrupted;
        // }
    }
    

}