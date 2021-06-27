using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleBT.Attributes;
using UnityEngine;
using YamlDotNet.RepresentationModel;

namespace SimpleBT
{
    public class YamlParser
    {
        private Dictionary<Type, Func<YamlNode, object>> _converters = new Dictionary<Type, Func<YamlNode, object>>();
        private Dictionary<string, Type> _types;
        public Dictionary<string, Node> _nodeById;

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
            _types = LoadNodeTypes();
        }

        private Dictionary<string,Type> LoadNodeTypes()
        {
            var res = new Dictionary<string, Type>();
            var assembly = GetType().Assembly;
            // var names = assembly.DefinedTypes.Select(x => x.Name).OrderBy(x => x).ToArray();
            foreach (var btType in assembly.DefinedTypes)
            {
                if (btType.IsSubclassOf(typeof(Node)))
                {
                    res[btType.Name] = btType;
                    var nameAttrObj = btType.GetCustomAttributes(typeof(NameAttribute), false).FirstOrDefault();
                    if (nameAttrObj is NameAttribute nameAttr)
                    {
                        res[nameAttr.Name] = btType;
                    }
                }
            }

            return res;
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

        private static object ParseNode(YamlNode node){
            if(!(node is YamlMappingNode mappingNode)){
                throw new Exception("Node must be a YamlMappingNode");
            }

            return ParseNode(mappingNode);
        }
        
        public BehaviourTree ParseTree(TextReader reader)
        {
            _nodeById =  new Dictionary<string, Node>();

            var yaml = new YamlStream();
            yaml.Load(reader);
            var document = (YamlMappingNode)yaml.Documents[0].RootNode;
            var root =  (YamlMappingNode)(document["root"]);

            var treeName = ParseTreeName(document);

            var subTress = ParseTrees(document);

            var parameters = ParseBBParameters(document);
            var rootNode = ParseNode(root);
            return new BehaviourTree
            {
                name = treeName,
                subTrees = subTress,
                root = rootNode,
                blackboardParameters = parameters,
                nodeById = _nodeById,
            };
        }

        private List<BBParameter> ParseBBParameters(YamlMappingNode document)
        {
            BBParameter ParseBBParameter(KeyValuePair<YamlNode, YamlNode> pair)
            {
                BBParameter res = new BBParameter();
                
                var name = ((YamlScalarNode) pair.Key).Value;

                res.Name = name;
                var descrNode = pair.Value;
                if (!(descrNode is YamlMappingNode descrNodeMap))
                {
                    throw new Exception($"blackboard parameter {name}: expected Mapping here, got {descrNode.NodeType}");
                }

                if (!descrNodeMap.Children.TryGetValue("type", out var typeNode))
                {
                    throw new Exception($"blackboard parameter {name}: expected type");
                }

                var typeStr = ((YamlScalarNode) typeNode).Value;
                if (!Blackboard.Types.TryGetValue(typeStr, out var bbType))
                {
                    throw new Exception($"blackboard parameter {name}: invalid type {typeStr}");
                }

                res.Type = bbType;
                
                
                if (descrNodeMap.Children.TryGetValue("value", out var valueNode))
                {
                    res.Value = Blackboard.DeserializeValue(bbType, ((YamlScalarNode) valueNode).Value);
                }
                else
                {
                    if (bbType.IsClass)
                    {
                        res.Value = null;
                    }
                    else
                    {
                        res.Value = Activator.CreateInstance(bbType);
                    }
                }

                return res;
                
            }
            
            var res = new List<BBParameter>();
            if (!document.Children.TryGetValue("blackboard", out var bbNode))
            {
                return res;
            }

            if (!(bbNode is YamlMappingNode bbNodeMap))
            {
                throw new Exception($"blackboard: expected Mapping here, got {bbNode.NodeType}");
            }

            foreach (var kv in bbNodeMap.Children)
            {
                res.Add(ParseBBParameter(kv));
            }

            return res;
        }

        private string ParseTreeName(YamlMappingNode document)
        {
            if(document.Children.TryGetValue("name", out var nameNode))
            {
                return ((YamlScalarNode) nameNode).Value;    
            }

            return null;
        }

        private Dictionary<string, Node> ParseTrees(YamlMappingNode document)
        {
            Dictionary<string, Node> res = new Dictionary<string, Node>();
            if (!document.Children.TryGetValue("trees", out var treesNode))
            {
                return res;
            }
            
            if (!(treesNode is YamlMappingNode treesNodeMap))
            {
                throw new Exception($"trees: Expected mapping");
            }

            foreach (var kv in treesNodeMap.Children)
            {
                var name = ((YamlScalarNode) kv.Key).Value;
                if (!(kv.Value is YamlMappingNode valueMap))
                {
                    throw new Exception($"Sub tree {name}: Expected mapping here got: {kv.Value.NodeType}");
                }

                res[name] = ParseNode(valueMap);
            }

            return res;
        }

        private Type GetBtType(string name)
        {
            if (!_types.TryGetValue(name, out var btType))
            {
                throw new Exception($"Cannot find type {name}");
            }

            return btType;
        }

        private void UpdateParameters(YamlMappingNode node, Type btType, List<IParameter> parameters)
        {
            var paramsDict = parameters.ToDictionary(p => p.Name, p => p);
            int positionalIndex = 0;
            bool metNamed = false;
            
            foreach (var kv in node.Children)
            {
                string paramName = (kv.Key as YamlScalarNode).Value;
                IParameter param = null;

                YamlNode paramValueNode = kv.Value;

                if(paramValueNode is YamlScalarNode scalarValue){
                    string paramValue = scalarValue.Value;

                    // BB parameter
                    if (paramValue.StartsWith("$"))
                    {
                        string bbName = paramValue.Substring(1);
                        bbName = bbName.Trim('\"');
                        param.BbName = bbName;
                        continue;
                    }

                    // Indexed parameter
                    if (scalarValue.Start.Equals(scalarValue.End))
                    {
                        if (metNamed)
                        {
                            throw new Exception("Cannot handle positional parameter after named one");
                        }
                        
                        param = parameters[positionalIndex];
                        positionalIndex += 1;
                        paramValueNode = kv.Key;
                    }
                }

                // named parameter
                if(param == null){
                    if (!paramsDict.TryGetValue(paramName, out param))
                    {
                        throw new Exception($"Cannot find parameter {paramName} in type {btType}");
                    }

                    metNamed = true;
                }

                Convert(param, paramValueNode);
            }
        }
        
        private void UpdateParameters(YamlScalarNode node, Type btType, List<IParameter> parameters)
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

            param.Set(Convert(param, node));
        }

        private object Convert(IParameter parameter, YamlNode value)
        {
            if(!_converters.TryGetValue(parameter.Type, out var converter)){
                throw new Exception($"Cannot find a suitable converter for a type {parameter.Type}");
            }

            try
            {
                return converter(value);
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
            
            string id = null;
            var nameS = ((YamlScalarNode) name).Value;
            if(nameS.Contains("#")){
                var nameSTokens = nameS.Split('#');
                nameS = nameSTokens[0];
                id = nameSTokens[1];

            }
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
            btNode.Id = id;
            if(!string.IsNullOrEmpty(id))
            {
                btNode.Id = id;
                _nodeById[id] = btNode;
            }

            ;
            if (btNode == null)
            {
                throw new Exception($"Cannot create node instance of type {btType.Name}");
            }
            
            if (value is YamlSequenceNode valueSeq)
            {
                List<YamlNode> children = valueSeq.Children.ToList();
                if (children.Count > 0 && children[0] is YamlScalarNode commentNode)
                {
                    btNode.Comment = commentNode.Value;
                    children.RemoveAt(0);
                }
                
                foreach (var childNode in children)
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