using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Text;

using Object = UnityEngine.Object;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using Node = UnityEditor.Experimental.GraphView.Node;
using UnityEngine.UIElements;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace UnityEditor.NodeGraph
{

    public class NodeGraphView : GraphView
    {
        NodeGraphWindow window;
        AbstractNodeGraph nodeGraph;
        private IEdgeConnectorListener edgeConnectorListener;

        public NodeGraphView(NodeGraphWindow window, AbstractNodeGraph nodeGraph)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));
            if(window==null)
                throw new ArgumentNullException(nameof(nodeGraph));

            edgeConnectorListener = new EdgeConnectorListener(this, nodeGraph);

            this.window = window;
            this.nodeGraph = nodeGraph;
            this.Add(InitToolbar());
            this.ViewInit();
            this.StretchToParentSize();
        }

        protected virtual VisualElement InitToolbar()
        {
            var toolbar = new IMGUIContainer(() =>
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button("Save", EditorStyles.toolbarButton,GUILayout.Width(100)))
                {
                    EditorUtility.SetDirty(nodeGraph);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                GUILayout.Space(8);
                if (GUILayout.Button("编译", EditorStyles.toolbarButton, GUILayout.Width(100)))
                {
                    var entranceNode = nodeGraph.Nodes.OfType<EntranceNode>().FirstOrDefault();
                    if (entranceNode == null)
                        throw new Exception("no find entrance node !");
                    var sb = new StringBuilder();
                    entranceNode.GenerateNodeCode(sb);
                    Debug.Log(sb);
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Blackboard", EditorStyles.toolbarButton, GUILayout.Width(100)))
                { 

                }
                if (GUILayout.Button("Inspector", EditorStyles.toolbarButton, GUILayout.Width(100)))
                {

                }
                GUILayout.EndHorizontal();
            });
            return toolbar;
        }

        private NodeMenuProvider nodeMenuProvider;
        protected void ViewInit()
        {
            this.Add(new Bl());

            this.SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            nodeMenuProvider = ScriptableObject.CreateInstance<NodeMenuProvider>();
            nodeMenuProvider.OnSelectEntryHandler = OnMenuSelectEntry;
            this.nodeCreationRequest = OnNodeCreationRequest;
            this.deleteSelection += OnDeleteSelection;
            this.graphViewChanged += OnGraphViewChanged;

            InitNode();
            InitEdge();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            foreach (NodeView nodeView in graphViewChange.movedElements)
            {
                nodeView.userData.rect = nodeView.GetPosition();
            }
            return graphViewChange;
        }

        private void OnDeleteSelection(string operationName, AskUser askUser)
        {
            for (int i = this.selection.Count - 1; i >= 0; i--)
            {
                var element = this.selection[i];
                RemoveFromSelection(element);
            }
        }

        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            GraphElement element = selectable as GraphElement;
            if (element != null)
            {
                switch (element)
                {
                    case NodeView nodeView:
                        this.RemoveNodeView(nodeView.userData);
                        break;
                    case LinkEdge edge:
                        this.RemoveEdge(edge.userData);
                        break;
                    default:
                        break;
                }
                //this.nodeGraph.Remove(element.viewDataKey);
            }
        }

        //public new void RemoveElement(GraphElement graphElement)
        //{
        //    if (graphElement == null)
        //        return;
        //    base.RemoveElement(graphElement);
        //    switch (graphElement)
        //    {
        //        case NodeView nodeView:
        //            nodeGraph.RemoveNode(nodeView.userData);
        //            break;
        //        case LinkEdge edge:
        //            nodeGraph.RemoveLink(edge.userData);
        //            break;
        //        default:
        //            break;
        //    }
        //    //this.nodeGraph.Remove(graphElement.viewDataKey);
        //}

        private void InitNode()
        {
            if (nodeGraph == null)
                return;
            foreach (var item in nodeGraph.Nodes)
            {
                CrateNodeView(item);
            }
        }

        private void InitEdge()
        {
            if (nodeGraph == null)
                return;
            foreach (var item in nodeGraph.Links)
            {
                CrateEdge(item);
            }
        }

        private bool OnMenuSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as Type;

            Vector2 mousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
            var localPost = this.WorldToLocal(mousePosition);

            var node = Activator.CreateInstance(type,false) as AbstractNode;
            node.rect = new Rect(localPost, Vector2.zero);
            nodeGraph.AddNode(node);

            CrateNodeView(node);
            return true;
        }

        private NodeView CrateNodeView(AbstractNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var nodeView = new NodeView();
            nodeView.Initialize(node, edgeConnectorListener, this);
            this.AddElement(nodeView);
            nodeView.RefreshExpandedState();
            return nodeView;
        }

        private void RemoveNodeView(AbstractNode node)
        {
            if (node == null)
                throw new ArgumentException(nameof(node));
            var nodeview = this.GetNodeByGuid(node.guid.ToString());
            if(nodeview==null)
                throw new Exception("no find node !");
            this.RemoveElement(nodeview);
            nodeGraph.RemoveNode(node);

            var list = nodeGraph.FindLink(node);
            foreach (var item in list)
            {
                RemoveEdge(item);
            }
        }

        public void RemoveEdge(NodeLink link)
        {
            if (link == null)
                throw new ArgumentException(nameof(link));
            var edge = this.GetEdgeByGuid(link.guid.ToString());
            if (edge.input != null)
            {
                edge.input.Disconnect(edge);
            }
            if (edge.output != null)
            {
                edge.output.Disconnect(edge);
            }

            this.RemoveElement(edge);
            nodeGraph.RemoveLink(link);
        }

        public LinkEdge CrateEdge(in NodeLink link)
        {
            var outPort = GetPortByGuid(link.OutPort.ToString());
            if (outPort == null)
                throw new Exception("no find out port");
            var inPort = GetPortByGuid(link.InputPort.ToString());
            if (inPort == null)
                throw new Exception("no find input prot !");

            var edge = new LinkEdge()
            {
                input = inPort,
                output = outPort
            };
            edge.viewDataKey = link.guid.ToString();
            edge.userData = link;
            inPort.Connect(edge);
            outPort.Connect(edge);
            this.AddElement(edge);
            return edge;
        }

        private void OnNodeCreationRequest(NodeCreationContext context)
        {
            if (EditorWindow.focusedWindow == this.window)
            {
                var searchWindowContext = new SearchWindowContext(context.screenMousePosition);
                SearchWindow.Open<NodeMenuProvider>(searchWindowContext, nodeMenuProvider);
            }
        }

        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var start = startAnchor as SlotPort;
            foreach (var port in ports.ToList())
            {
                if (startAnchor.node == port.node ||
                    startAnchor.direction == port.direction ||
                    startAnchor.portType != port.portType)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }
    }
}
