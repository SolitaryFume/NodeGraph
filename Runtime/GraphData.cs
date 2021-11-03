using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace UnityLib.Graph
{

    [Serializable]
    public class GraphData : ISerializationCallbackReceiver
    {
        #region Serialization
        [SerializeField] [HideInInspector] private string assetGuide;
        [SerializeField] [HideInInspector] private List<JsonScriptableObject> nodes = new List<JsonScriptableObject>();
        [SerializeField] [HideInInspector] private List<NodeLink> links = new List<NodeLink>();
        #endregion

        public string AssetGuide { get => assetGuide; set => assetGuide = value; }
        private Dictionary<string, NodeData> m_nodeDic;
        public Dictionary<string, NodeData> NodeDic
        {
            get {
                if (m_nodeDic == null)
                { 
                    m_nodeDic= new Dictionary<string, NodeData>();
                }
                return m_nodeDic;
            }
        }

        private Dictionary<string, NodeLink> m_linkDir;

        public Dictionary<string, NodeLink> LinkDir
        {
            get {
                if (m_linkDir == null)
                {
                    m_linkDir = new Dictionary<string, NodeLink>();
                }
                return m_linkDir;
            }
        }

        public NodeData CreateNode(Type type, Vector2 localPost)
        {
            var nodeData = ScriptableObject.CreateInstance(type) as NodeData;
            nodeData.Position = localPost;
            NodeDic.Add(nodeData.guid, nodeData);
            return nodeData;
        }

        public void OnAfterDeserialize()
        {
            NodeDic.Clear();
            var collection = nodes.Select(jsonobj => JsonScriptableObject.ToObj<NodeData>(jsonobj));
            foreach (var nodeData in collection)
            {
                NodeDic.Add(nodeData.guid, nodeData);
            }

            LinkDir.Clear();
            foreach (var link in links)
            {
                LinkDir.Add(link.guid, link);
            }
        }

        public void OnBeforeSerialize()
        {
            nodes = NodeDic.Values.Select(node => JsonScriptableObject.ToJson(node)).ToList();
            links = LinkDir.Values.ToList();
        }

        public bool QueryConnect(NodeData from, string formPort,out NodeData to, out string toPort)
        { 
            if(from==null)
                throw new ArgumentNullException(nameof(from));
            if (string.IsNullOrEmpty(formPort))
                throw new ArgumentException($"{nameof(formPort)} is null or empty !");
            var fastLink = LinkDir.Values.Where(link => link.startNode == from.guid && link.outputPort == formPort || link.endNode == from.guid && link.inputPort == formPort).FirstOrDefault();
            if (fastLink == null)
            {
                to = default;
                toPort = default;
                return false;
            }

            if (fastLink.startNode == from.guid)
            {
                to = NodeDic[fastLink.endNode];
                toPort = fastLink.inputPort;
            }
            else
            {
                to = NodeDic[fastLink.startNode];
                toPort = fastLink.outputPort;
            }
            return true;
        }
    }

    public static class GraphData_ForLua
    {
        //public static string ToLua(this GraphData self)
        //{
        //    if (self == null)
        //        throw new ArgumentNullException(nameof(self));

        //    var headnode = self.NodeDic.Values.OfType<Flow_Head>().FirstOrDefault();
        //    if (headnode == null)
        //        throw new Exception("no find Flow_Head !");

        //    if (self.QueryConnect(headnode, nameof(Flow_Head.To), out var to, out var toPort))
        //    { 
                
        //    }
        //}

        public static string ToCode(this GraphData self,LuaNodeData node)
        {
            return node.ToLuaCode(self);
        }
    }
}