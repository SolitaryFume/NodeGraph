using UnityEngine;
namespace UnityLib.Graph
{
    [NodeMenu("ToNumber")]
    public class ToNumber : NodeData
    { 
        [Input][HideInInspector] public string @in;
        [Output] [HideInInspector] public long @out;
    }
}