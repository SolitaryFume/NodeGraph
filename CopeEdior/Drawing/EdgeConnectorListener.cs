using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Linq;

namespace UnityEditor.NodeGraph
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        private NodeGraphView nodeGraphView;
        private AbstractNodeGraph graph;

        public EdgeConnectorListener(NodeGraphView nodeGraphView, AbstractNodeGraph graph)
        {
            this.nodeGraphView = nodeGraphView;
            this.graph = graph;
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            var output = edge.output as SlotPort;
            var input = edge.input as SlotPort;

            if (input.connected)
            {
                var list = input.connections.ToList();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var linkEdge = list[i] as LinkEdge;
                    nodeGraphView.RemoveEdge(linkEdge.userData);
                }
            }

            var link = graph.Connect(output.userData,input.userData);
            nodeGraphView.CrateEdge(in link);
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            //Debug.LogError("释放");
        }
    }
}