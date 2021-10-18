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
    }
}