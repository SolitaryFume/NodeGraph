using UnityEditor.Experimental.GraphView;

namespace UnityEditor.NodeGraph
{
    public class LinkEdge : Edge
    { 
        public new NodeLink userData { get; set; }
    }
}
