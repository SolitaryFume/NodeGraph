using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.NodeGraph
{
    [Serializable]
    public abstract class AbstractNodeGraph : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] [HideInInspector] private string m_json;
        [SerializeField] private GraphData graphData = new GraphData();

        public IEnumerable<AbstractNode> Nodes => graphData.Nodes;
        public IEnumerable<NodeLink> Links => graphData.Links;

        public virtual void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(m_json))
                return;
            else
                JsonUtility.FromJson<GraphData>(m_json);
        }

        public virtual NodeLink Connect(AbstractSolt output, AbstractSolt input)
        {
            var link = new NodeLink(output, input);
            if (!graphData.Links.Contains(link))
                graphData.Links.Add(link);
            return link;
        }

        public virtual void OnBeforeSerialize()
        {
            JsonUtility.ToJson(graphData, true);
        }

        public void AddNode(AbstractNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            node.Owner = graphData;
            graphData.AddNode(node);
        }

        public void Remove(string viewDataKey)
        {
            var guid = Guid.Parse(viewDataKey);
            var node = graphData.Nodes.FirstOrDefault(n => n.guid == guid);
            if (node != null)
            {
                graphData.Nodes.Remove(node);
                return;
            }

            var edge = graphData.Links.FirstOrDefault(l => l.guid == guid);
            if (edge != null)
            {
                graphData.Links.Remove(edge);
                return;
            }
        }

        internal void RemoveNode(AbstractNode node)
        {
            graphData.Nodes.Remove(node);
        }

        internal void RemoveLink(NodeLink link)
        {
            graphData.Links.Remove(link);
        }

        public AbstractNode FindNode(Guid guide)
        {
            return graphData.Nodes.FirstOrDefault(node => node.guid == guide);
        }

        internal IEnumerable<NodeLink> FindLink(AbstractNode node)
        {
            var list = new List<NodeLink>();
            foreach (var item in Links)
            {
                if(item.FromNode==node.guid || item.ToNode == node.guid)
                    list.Add(item);
            }
            return list;
        }
    }

}