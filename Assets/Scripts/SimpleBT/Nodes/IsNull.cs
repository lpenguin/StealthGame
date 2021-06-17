namespace SimpleBT.Nodes
{
    public class IsNull: Node
    {
        private Parameter<object> value;
        protected override Status OnUpdate()
        {
            object v = value.Get();
            bool isNull;
            if (v is UnityEngine.Object unityV)
            {
                isNull = unityV == null;
            }
            else
            {
                isNull = v == null;
            }
            return isNull ? Status.Success : Status.Failed;
        }
    }
}