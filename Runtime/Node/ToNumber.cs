using UnityEngine;
namespace UnityLib.Graph
{
    [NodeMenu("ToNumber")]
    public class ToNumber : NodeData
    { 
        [Input][HideInInspector] public string In;
        [Output] [HideInInspector] public long Out;
    }
}