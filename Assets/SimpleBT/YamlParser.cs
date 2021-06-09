using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using YamlDotNet.RepresentationModel;

namespace SimpleBT
{
    public class YamlParser
    {
        private Dictionary<Type, Func<YamlNode, object>> _converters = new Dictionary<Type, Func<YamlNode, object>>();

        public YamlParser()
        {
            var scalarTypes = new Type[]
            {
                typeof(string),
                typeof(int),
                typeof(bool),
                typeof(float),
                typeof(double),
            };
            foreach (var scalarType in scalarTypes)
            {
                _converters[scalarType] = node => System.Convert.ChangeType((node as YamlScalarNode).Value, scalarType);
            }

            _converters[typeof(Vector3)] = ParseVector3;
        }

        private static object ParseVector3(YamlNode node)
        {

            if (!(node is YamlMappingNode nodeMap))
            {
                throw new Exception($"Cannot parse Vector3, expected Mapping, got {node.NodeType}");
            }
            
            var children = nodeMap.Children;
            float GetComponent(string name)
            {
                if (!children.TryGetValue(new YamlScalarNode(name), out var xNode))
                {
                    throw new Exception($"Cannot parse Vector3, expected {name}");
                }

                if (!(xNode is YamlScalarNode xNodeScalar))
                {
                    throw new Exception($"Cannot parse Vector3.{name}, expected Scalar");
                }

                if (!float.TryParse(xNodeScalar.Value, out var value))
                {
                    throw new Exception($"Cannot parse Vector3.{name}, {xNodeScalar.Value}");
                }

                return value;
            }

            return new Vector3(
                GetComponent("x"),
                GetComponent("y"),
                GetComponent("z")
            );
        }
        
        public BehaviourTree ParseTree(TextReader reader)
        {
            
            var yaml = new YamlStream();
            yaml.Load(reader);
            var document = (YamlMappingNode)yaml.Documents[0].RootNode;
            var root =  (YamlMappingNode)(document[new YamlScalarNode("root")][0]);
            
            return new BehaviourTree(ParseNode(root));
        }

        private Type GetBtType(string name)
        {
            var assembly = GetType().Assembly;
            // var names = assembly.DefinedTypes.Select(x => x.Name).OrderBy(x => x).ToArray();
            foreach (var btType in assembly.DefinedTypes)
            {
                if (btType.Name == name)
                {
                    return btType;
                }
            }

            throw new Exception($"Cannot find type {name}");
        }

        private static void UpdateParameters(YamlMappingNode node, Type btType, List<IParameter> parameters)
        {
            var paramsDict = parameters.ToDictionary(p => p.Name, p => p);
            foreach (var kv in node.Children)
            {
                string paramName = (kv.Key as YamlScalarNode).Value;
                if (!paramsDict.TryGetValue(paramName, out var param))
                {
                    throw new Exception($"Cannot find parameter {paramName} in type {btType}");
                }


                if (kv.Value is YamlScalarNode scalarNode && scalarNode.Value.StartsWith("$"))
                {
                    string bbName = scalarNode.Value.Substring(1);
                    bbName = bbName.Trim('\"');
                    param.BbName = bbName;
                    continue;
                }
                
                param.Set(Convert(param.Type, kv.Value));
            }
        }
        
        private static void UpdateParameters(YamlScalarNode node, Type btType, List<IParameter> parameters)
        {
            var valueStr = node.Value;
            if (string.IsNullOrEmpty(valueStr))
            {
                return;
            }
         
            if (parameters.Count == 0)
            {
                throw new Exception($"Cannot pickup default parameter: {btType.Name}");
            }
            
            var param = parameters[0];
            if (valueStr.StartsWith("$"))
            {
                string bbName = valueStr.Substring(1);
                bbName = bbName.Trim('\"');
                param.BbName = bbName;
                return;
            }

            param.Set(Convert(param.Type, valueStr));
        }

        private static object Convert(Type type, YamlNode node)
        {
            // TODO
            try
            {
                return System.Convert.ChangeType(((YamlScalarNode) node).Value, type);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.ToString());
                throw;
            }
            
        }
        
        public Node ParseNode(YamlMappingNode node)
        {
            KeyValuePair<YamlNode,YamlNode>[] items = node.Children.ToArray();
            if (items.Length != 1)
            {
                throw new Exception("Invalid items count");
            }

            var name = items[0].Key;
            var value = items[0].Value;
            if (name.NodeType != YamlNodeType.Scalar)
            {
                throw new Exception("Invalid key type");
            }
            
            var nameS = ((YamlScalarNode) name).Value;
            var btType = GetBtType(nameS);
            if (btType == null)
            {
                throw new Exception($"Cannot find type {nameS}");
            }

            if (!btType.IsSubclassOf(typeof(Node)))
            {
                throw new Exception($"The type {nameS} is not a subclass BehaviourNode");
            }
            Node btNode = Activator.CreateInstance(btType) as Node;
            ;
            if (btNode == null)
            {
                throw new Exception($"Cannot create node instance of type {btType.Name}");
            }
            
            if (value is YamlSequenceNode valueSeq)
            {
                foreach (var childNode in valueSeq.Children)
                {
                    btNode.Children.Add(ParseNode(childNode as YamlMappingNode));
                }
                
                return btNode;
            }

            if (value is YamlMappingNode valueMap)
            {
                UpdateParameters(valueMap, btType, btNode.Parameters);
                return btNode;
            }

            if (value is YamlScalarNode valueScalar)
            {
                UpdateParameters(valueScalar, btType, btNode.Parameters);
                return btNode;
            }
            
            throw new Exception($"{name} has invalid YAML type: {value.NodeType}");
        }
    }
}