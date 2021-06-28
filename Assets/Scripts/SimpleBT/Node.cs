using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleBT.Attributes;
using UnityEngine;

namespace SimpleBT
{
    public enum Status
    {
        Empty,
        Running,
        Failed,
        Success,
        Interrupted
    }

    public class ExecutionContext
    {
        public GameObject GameObject { get; set; }
        public Blackboard Blackboard { get; set; }
        public BehaviourTree BehaviourTree { get; set; }
        public EventBus EventBus { get; set; }
        
    }
    
    public abstract class Node
    {
        public Status Status { get; protected set; } = Status.Empty;
        public IList<Node> Children { get; } = new List<Node>();
        public List<IParameter> Parameters { get; } = new List<IParameter>();
        public string Id {get; set;} = null;

        public string Comment { get; set; }
        
        protected ExecutionContext currentContext = null;
        
        public string Name => _name;

        private string _name;
        public void Execute(ExecutionContext context)
        {
            var blackboard = context.Blackboard;
            
            foreach (var parameter in Parameters)
            {
                if (!string.IsNullOrEmpty(parameter.BbName))
                {
                    parameter.Set(blackboard.GetValue(parameter.BbName));
                }
            }

            currentContext = context;

            if (Status == Status.Empty)
            {
                // Debug.Log($"{Name}.OnStart()");
                OnStart();
            }
            // Debug.Log($"{Name}.OnUpdate()");
            Status = OnUpdate();
            
            // Debug.Log($"{Name}.OnStart() -> {Status}");
            
            foreach (var parameter in Parameters)
            {
                if (!string.IsNullOrEmpty(parameter.BbName))
                {
                    blackboard.SetValue(parameter.BbName, parameter.Get());
                }
            }

            currentContext = null;
        }
        
        protected abstract Status OnUpdate();

        protected virtual void OnStart()
        {
        }

        protected virtual void OnAbort()
        {
        }
        
        public virtual void Reset()
        {
            // Debug.Log($"{Name}.Reset()");

            if (Status == Status.Empty)
            {
                return;
            }
            OnAbort();
            Status = Status.Empty;
            foreach (var child in Children)
            {
                child.Reset();
            }
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
                _name = nameAttr.Name;
            }
            else
            {
                _name = type.Name;
            }
        }


        public virtual void Interrupt()
        {
            Status = Status.Interrupted;
        }
    }
    

}