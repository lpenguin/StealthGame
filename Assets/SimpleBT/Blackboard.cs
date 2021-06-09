using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleBT
{
    [Serializable]
    public class BBParameter
    {
        public string Name;
        public object Value;
        public Type Type;
        public string ValueStr;
        public string TypeStr;
        public Transform transformField;
        
    }
    
    [Serializable]
    public class Blackboard: ISerializationCallbackReceiver
    {
        public static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>()
        {
            {"bool", typeof(bool)},
            {"float", typeof(float)},
            {"string", typeof(string)},
            {"Vector3", typeof(Vector3)},
            {"Quaternion", typeof(Quaternion)},
            {"Transform", typeof(Transform)},
        };

        public static readonly Dictionary<Type, string> TypesInv = InverseTypes();

        static Dictionary<Type, string> InverseTypes()
        {
            var res = new Dictionary<Type, string>();
            foreach (var kv in Types)
            {
                res[kv.Value] = kv.Key;
            }

            return res;
        }
        
        [SerializeField]
        private List<BBParameter> _parametersList;
        
        private Dictionary<string, BBParameter> _parameters;
        
        
        public void SetValue(string name, object value)
        {
            _parameters[name].Value = value;
        }

        public object GetValue(string name)
        {
            return _parameters[name].Value;
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

            var param = new BBParameter()
            {
                Name = name,
                Type = type,
                Value = paramValue
            };
            _parameters[name] = param;
        }

        public void RenameParameter(string oldName, string newName)
        {
            var param = _parameters[oldName];
            param.Name = newName;
            _parameters.Remove(oldName);
            _parameters[newName] = param;
        }

        public void DeleteParameter(string name)
        {
            var param = _parameters[name];
            _parameters.Remove(name);
        }

        public IEnumerable<BBParameter> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new Dictionary<string, BBParameter>();
                }
                return _parameters.Values;
            }
        }

        public Blackboard Copy()
        {
            return this;
        }

        public void OnBeforeSerialize()
        {
            if (_parametersList == null)
            {
                _parametersList = new List<BBParameter>();
            }
            _parametersList.Clear();
            if (_parameters == null)
            {
                return;
            }
            
            foreach (var parameter in _parameters.Values)
            {
                SerializeParam(parameter);
                _parametersList.Add(parameter);
            }
            
        }



        public void OnAfterDeserialize()
        {
            if (_parameters == null)
            {
                if (_parametersList == null)
                {
                    _parametersList = new List<BBParameter>();
                }
                _parameters = new Dictionary<string, BBParameter>();
                foreach (var parameter in _parametersList)
                {
                    _parameters[parameter.Name] = parameter;
                    DeserializeParam(parameter);
                }
            }
        }

        private void DeserializeParam(BBParameter parameter)
        {
            if (!Types.TryGetValue(parameter.TypeStr, out parameter.Type))
            {
                throw new Exception($"Cannot find type: {parameter.TypeStr}");
            }
            
            if (parameter.Type == typeof(Transform))
            {
                parameter.Value = parameter.transformField;
                return;
            }
            
            parameter.Value = DeserializeValue(parameter.Type, parameter.ValueStr);
        }

        private void SerializeParam(BBParameter parameter)
        {
            parameter.TypeStr = TypesInv[parameter.Type];
            if (parameter.Type == typeof(Transform))
            {
                parameter.transformField = (Transform) parameter.Value;
                parameter.ValueStr = null;
                return;
            }
            parameter.ValueStr = SerializeValue(parameter.Type, parameter.Value);
        }

        private string SerializeValue(Type type, object value)
        {
            if (type == typeof(Vector3))
            {
                Vector3 v = (Vector3) value;
                return $"{v.x};{v.y};{v.z}";
            }

            if (type == typeof(Quaternion))
            {
                return JsonUtility.ToJson(value);
            }
            return value.ToString();
        }

        private object DeserializeValue(Type type, string valueStr)
        {
            if (type == typeof(Vector3))
            {
                var tokens = valueStr.Split(';');
                return new Vector3(
                    float.Parse(tokens[0]),
                    float.Parse(tokens[1]),
                    float.Parse(tokens[2])
                );
            }

            if (type == typeof(Quaternion))
            {
                return JsonUtility.FromJson<Quaternion>(valueStr);
            }
            return Convert.ChangeType(valueStr, type);
        }
    }
}