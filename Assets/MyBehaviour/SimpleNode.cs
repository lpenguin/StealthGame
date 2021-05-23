using XNode;
using UnityEngine;

public class SimpleNode : Node { 
  [Input] public float a;
  [Output] public float b;

  [SerializeField] 
  int field = 10;


  [SerializeField] 
  public SimpleGraph otherGraph;
  public override object GetValue(NodePort port) {
    if (port.fieldName == "b") return GetInputValue<float>("a", a);
    else return null;
  }
}