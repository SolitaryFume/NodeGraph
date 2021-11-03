using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.NodeGraph
{
    [Serializable]
    public sealed class GraphData : ISerializationCallbackReceiver
    {

        #region Serialize
        [SerializeField] [HideInInspector] private List<SerializationHelper.JsonObj> serializenodes = new List<SerializationHelper.JsonObj>();
        [SerializeField] private List<NodeLink> m_Links = new List<NodeLink>();
        #endregion

        [SerializeField] private List<AbstractNode> m_Nodes = new List<AbstractNode>();
        internal List<AbstractNode> Nodes => m_Nodes;
        public List<NodeLink> Links => m_Links;
        private Dictionary<Guid, AbstractSolt> m_Solts = new Dictionary<Guid, AbstractSolt>();

        public AbstractSolt GetConnectSolt(AbstractSolt solt)
        {
            if (solt == null)
                throw new ArgumentNullException(nameof(solt));

            var link = FindFirstLink(solt);
            if (link != null && m_Solts.TryGetValue(solt.IsInputSlot ? link.OutPort : link.InputPort, out var connectSlot))
            { 
                return connectSlot;
            }
            return default(AbstractSolt);

        }

        public IEnumerable<NodeLink> FindAllLink(AbstractSolt solt)
        {
            if (solt == null)
                throw new ArgumentNullException(nameof(solt));

            if (solt.IsInputSlot)
                return Links.Where(l => l.InputPort == solt.guid);
            else
                return Links.Where(l =>l.OutPort==solt.guid);
        }

        public NodeLink FindFirstLink(AbstractSolt solt)
        {
            if (solt == null)
                throw new ArgumentNullException(nameof(solt));
            if (solt.IsInputSlot)
                return Links.FirstOrDefault(l => l.InputPort == solt.guid);
            else
                return Links.FirstOrDefault(l => l.OutPort == solt.guid);
        }

        public void AddNode(AbstractNode node)
        {
            if(node==null)
                throw new ArgumentNullException(nameof(node));
            m_Nodes.Add(node);
            node.Owner = this;
            foreach (var slot in node.Slots)
            {
                m_Solts.Add(slot.guid, slot);
                Debug.Log("Add Solt :" + slot.guid);
            }
        }

        public void OnAfterDeserialize()
        {
            if (m_Solts == null)
                m_Solts = new Dictionary<Guid, AbstractSolt>();
            m_Solts.Clear();
            m_Nodes.Clear();
            var list = SerializationHelper.Deserialize<AbstractNode>(serializenodes);
            foreach (var node in list)
            {
                AddNode(node);
            }
        }

        public void OnBeforeSerialize()
        {
            serializenodes = SerializationHelper.Serialize(m_Nodes);
        }
    }

    interface IRectInterface
    {
        Rect rect { get;}
    }
}