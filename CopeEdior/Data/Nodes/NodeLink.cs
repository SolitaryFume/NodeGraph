using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.NodeGraph
{
    [Serializable]
    public sealed class NodeLink:ISerializationCallbackReceiver, IEquatable<NodeLink>
    {
        public NodeLink(AbstractSolt outputSolt, AbstractSolt inputSlot)
        {
            guid = Guid.NewGuid();
            FromNode = outputSolt.Owner.guid;
            ToNode = inputSlot.Owner.guid;
            OutPort = outputSolt.guid;
            InputPort = inputSlot.guid;

            //m_guid = m_SerializationFromNode = m_SerializationToNode = m_SerializationOutPort = m_SerializationInputPort = String.Empty;
        }

        public Guid guid;

        public Guid FromNode;
        public Guid ToNode;
        public Guid OutPort;
        public Guid InputPort;

        [SerializeField] private string m_guid;
        [SerializeField] private string m_SerializationFromNode;
        [SerializeField] private string m_SerializationToNode;
        [SerializeField] private string m_SerializationOutPort;
        [SerializeField] private string m_SerializationInputPort;

        public void OnAfterDeserialize()
        {
            guid = Guid.Parse(m_guid);
            FromNode = Guid.Parse(m_SerializationFromNode);
            ToNode = Guid.Parse(m_SerializationToNode);
            OutPort = Guid.Parse(m_SerializationOutPort);
            InputPort = Guid.Parse(m_SerializationInputPort);
        }

        public void OnBeforeSerialize()
        {
            m_SerializationFromNode = FromNode.ToString();
            m_SerializationToNode = ToNode.ToString();
            m_SerializationOutPort = OutPort.ToString();
            m_SerializationInputPort = InputPort.ToString();
            m_guid = guid.ToString();
        }

        public bool Equals(NodeLink other)
        {
            return FromNode == other.FromNode && ToNode == other.ToNode 
                && OutPort == other.OutPort && InputPort == other.InputPort;
        }
    }
}