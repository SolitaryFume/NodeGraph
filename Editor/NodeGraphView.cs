using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;
using UnityLib.Graph;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace UnityLib.GraphEditor
{
    public class NodeGraphView : GraphView
    {
        private static Dictionary<Type, Type> m_nodeViewMap;

        public static Dictionary<Type, Type> noedViewMap {
            get 
            {
                if (m_nodeViewMap==null)
                { 
                    m_nodeViewMap = new Dictionary<Type, Type>();
                    var all = typeof(NodeView).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(NodeView)));
                    foreach (var viewty in all)
                    {
                        if (viewty.GetCustomAttributes(typeof(CustomViewAttribute), false) is CustomViewAttribute[] arrs && arrs.Length>0)
                        {
                            var custom = arrs[0];
                            m_nodeViewMap.Add(custom.type, viewty);
                        }
                    }
                }
                return m_nodeViewMap;
            }
        }


        private EditorWindow editorWindow;
        private string assetguide;
        private GraphData graphData;
        private NodeGraph graph;

        public NodeGraphView(EditorWindow ew, string assetguide)
        {
            this.assetguide = assetguide;
            editorWindow = ew;
            var assetPath = AssetDatabase.GUIDToAssetPath(assetguide);
            graph = AssetDatabase.LoadAssetAtPath<NodeGraph>(assetPath);
            graph.FromJson(File.ReadAllText(assetPath,Encoding.UTF8));
            graphData = graph.Data;

            Toolbar();
            this.StretchToParentSize();
            var menuProvider = ScriptableObject.CreateInstance<NodeGraphMenuWindowProvider>();
            menuProvider.OnSelectEntryHandler = OnMenuSelectEntry;

            nodeCreationRequest += context =>
            {
                var searchWindowContext = new SearchWindowContext(context.screenMousePosition);
                SearchWindow.Open<NodeGraphMenuWindowProvider>(searchWindowContext, menuProvider);
            };

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            InitView();
        }

        private void InitView()
        {
            foreach (var nodedata in graphData.NodeDic.Values)
            {
                AddNode(nodedata);
            }

            foreach (var link in graphData.LinkDir.Values)
            {
                AddLink(link);
            }
        }

        private void AddLink(NodeLink link)
        {
            if (link == null)
                throw new ArgumentNullException(nameof(link));

            var startNode = GetNodeByGuid(link.startNode);
            var endNode = GetNodeByGuid(link.endNode);
            var inputPort = endNode.inputContainer.Query<Port>(link.inputPort);
            var outPort = startNode.outputContainer.Q<Port>(link.outputPort);

            var edge = new Edge()
            {
                input = inputPort,
                output = outPort,
            };
            this.Add(edge);
        }

        private void Toolbar()
        {
            var toolbar = new IMGUIContainer(() =>
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button("保存",GUILayout.Width(80)))
                {
                    Save();
                }
                GUILayout.EndHorizontal();
            });
            Add(toolbar);
        }

        private bool OnMenuSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as Type;
            
            Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position);
            var localPost = this.WorldToLocal(mousePosition);

            var nodeData = graphData.CreateNode(type, localPost);
            AddNode(nodeData);
            return true;
        }

        private void AddNode(NodeData nodeData)
        {
            if (!noedViewMap.TryGetValue(nodeData.GetType(), out var nodeViewTy))
            {
                nodeViewTy = typeof(NodeView);
            }

            var node = Activator.CreateInstance(nodeViewTy, new object[] { nodeData }) as NodeView;
            var localPost = nodeData.Position;
            node.SetPosition(new Rect(localPost, Vector2.zero));
            node.RefreshExpandedState();
            this.AddElement(node);
        }

        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
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

        public virtual void Save()
        {
            nodes.ForEach(node =>
            {
                if (node.userData is NodeData nodeData)
                {
                    nodeData.Position = node.GetPosition().position;
                }
            });

            this.graphData.LinkDir.Clear();
            edges.ForEach(edge =>
            {
                if (edge.input != null && edge.output != null)
                {
                    var link = new NodeLink(edge.viewDataKey) {
                        startNode = edge.output.node.viewDataKey,
                        endNode = edge.input.node.viewDataKey,
                        inputPort = edge.input.portName,
                        outputPort = edge.output.portName,
                    };
                    this.graphData.LinkDir.Add(link.guid,link);
                };
            });

            var assetPath = AssetDatabase.GUIDToAssetPath(assetguide);
            File.WriteAllText(assetPath, graph.ToJson());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
