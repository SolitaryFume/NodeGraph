namespace UnityLib.Graph
{

    public abstract class LuaValue<T> : NodeData
    {
        [Input][Output] public T value;
    }
}