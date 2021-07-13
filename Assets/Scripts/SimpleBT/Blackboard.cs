using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleBT
{
    public enum BBParameterType
    {
        Bool,
        Float,
        Int,
        String,
        Vector3,
        Quaternion,
        Transform
    }
    [Serializable]
    public class BBParameter
    {

        [SerializeField]
        private BBParameterValue value;
        
        [HideInInspector]
        public string Name;
        
        public BBParameterType Type;

        internal BBParameter(string name, BBParameterType type, BBParameterValue value)
        {
            Name = name;
            Type = type;
            this.value = value;
        }
        internal BBParameter(string name, BBParameterType type)
        {
            Name = name;
            Type = type;
        }

        
        public object Value
        {
            get
            {
                switch (Type)
                {
                    case BBParameterType.Bool:
                        return value.boolValue;
                    case BBParameterType.Float:
                        return value.floatValue;
                    case BBParameterType.Int:
                        return value.intValue;
                    case BBParameterType.String:
                        return value.stringValue;
                    case BBParameterType.Vector3:
                        return value.vector3Value;
                    case BBParameterType.Quaternion:
                        return value.quaternionValue;
                    case BBParameterType.Transform:
                        return value.transformValue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (Type)
                {
                    case BBParameterType.Bool:
                        this.value.boolValue = (bool) value;
                        break;
                    case BBParameterType.Float:
                        this.value.floatValue = (float) value;
                        break;
                    case BBParameterType.Int:
                        this.value.intValue = (int) value;
                        break;
                    case BBParameterType.String:
                        this.value.stringValue = (string) value;
                        break;
                    case BBParameterType.Vector3:
                        this.value.vector3Value = (Vector3) value;
                        break;
                    case BBParameterType.Quaternion:
                        this.value.quaternionValue = (Quaternion) value;
                        break;
                    case BBParameterType.Transform:
                        this.value.transformValue = (Transform) value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    [Serializable]
    internal struct BBParameterValue
    {
        public string stringValue;
        public bool boolValue;
        public int intValue;
        public float floatValue;
        public Vector3 vector3Value;
        public Quaternion quaternionValue;
        public Transform transformValue;
    }

    [Serializable]
    public class BBParameterDictionary : SerializableDictionary<string, BBParameter>
    {
        
    }
    [Serializable]
    public class Blackboard
    {
        public BBParameterDictionary parameters;

        private static BBParameter FromValue(string name, object value)
        {
            return value switch
            {
                int intValue => new BBParameter(name, BBParameterType.Int, 
                    new BBParameterValue{intValue = intValue}),
                bool boolValue => new BBParameter(name, BBParameterType.Bool,
                    new BBParameterValue {boolValue = boolValue}),
                string stringValue => new BBParameter(name, BBParameterType.String,
                    new BBParameterValue {stringValue = stringValue}),
                float floatValue => new BBParameter(name, BBParameterType.Float,
                    new BBParameterValue {floatValue = floatValue}),
                Vector3 vector3Value => new BBParameter(name, BBParameterType.Vector3,
                    new BBParameterValue {vector3Value = vector3Value}),
                Quaternion quaternionValue => new BBParameter(name, BBParameterType.Quaternion,
                    new BBParameterValue {quaternionValue = quaternionValue}),
                Transform transformValue => new BBParameter(name, BBParameterType.Transform,
                    new BBParameterValue {transformValue = transformValue}),
                _ => throw new Exception($"Invalid value type: {value}")
            };
        }
        public void SetValue(string name, object value)
        {
            if (!parameters.ContainsKey(name))
            {
                parameters[name] = FromValue(name, value);
                return;
            } 
            var p = parameters[name];
            p.Value = value;
        }

        public object GetValue(string name)
        {
            if (!parameters.TryGetValue(name, out var parameter))
            {
                throw new Exception($"Blackboard parameter doesn't exist: {name}");
            }
            
            return parameter.Value;
        }

        public T GetValue<T>(string name)
        {
            return (T) GetValue(name);
        }

        public void AddParameter(string name, Type type)
        {
            object paramValue = null;
            if (type == typeof(string) || type.IsValueType)
            {
                paramValue = Activator.CreateInstance(type);
            }

            var param = FromValue(name, paramValue);
            parameters[name] = param;
        }

        public void RenameParameter(string oldName, string newName)
        {
            var param = parameters[oldName];
            param.Name = newName;
            parameters.Remove(oldName);
            parameters[newName] = param;
        }

        public void DeleteParameter(string name)
        {
            var param = parameters[name];
            parameters.Remove(name);
        }

        public IEnumerable<BBParameter> Parameters => parameters.Values;


        public Blackboard Copy()
        {
            return this;
        }

        public bool HasParameter(string parameterName)
        {
            return parameters.ContainsKey(parameterName);
        }

        public void AddParameter(BBParameter parameter)
        {
            parameters[parameter.Name] = parameter;
        }
    }
}