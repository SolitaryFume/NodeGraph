using UnityEngine;

namespace UnityEditor.NodeGraph
{
    public sealed class NodeGraphWindow : EditorWindow
    {
        private AbstractNodeGraph graph;
        private NodeGraphView nodeGraphView;

        public void Initialize(AbstractNodeGraph graph)
        {
            this.graph = graph;
            this.nodeGraphView = new NodeGraphView(this,graph);
            this.rootVisualElement.Add(nodeGraphView);

            UpdateTitle();
        }


        public void UpdateTitle()
        {
            if (graph == null)
                return;
            titleContent = new GUIContent(graph.name);
        }
    }
}
