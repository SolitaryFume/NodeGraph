using UnityLib.Graph;
using UnityEngine;
using UnityEditor;

namespace UnityLib.GraphEditor
{
    public class CreateNodeGraph
    {
        [MenuItem("Assets/Create/NodeGraph/Base Graph", false, 208)]

        public static void CreateBaseNodeGraph()
        {
            var graphItem = ScriptableObject.CreateInstance<NewGraphAction>();
            graphItem.Graph = ScriptableObject.CreateInstance<NodeGraph>();
            graphItem.Graph.Data = new GraphData();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, graphItem,
                string.Format("NodeGraph.{0}", NodeGraphImporter.Extension), null, null);
        }
    }
}
    