using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.NodeGraph
{
    [Serializable]
    public class AbstractNode : ISerializationCallbackReceiver, IGroupItem,IRectInterface
    {
        [SerializeField] private string m_Name;
        [SerializeField] private string m_GuidSerialized;
        [SerializeField] private string m_GroupGuidSerialized;

        [NonSerialized] private Guid m_Guid;
        [NonSerialized] private Guid m_GroupGuid;
        [SerializeField] private int m_NodeVersion;
        [SerializeReference] private List<AbstractSolt> m_Slots = new List<AbstractSolt>();
        [SerializeField][HideInInspector] private List<SerializationHelper.JsonObj> m_SerializableSlots = new List<SerializationHelper.JsonObj>();

        [NonSerialized] private GraphData m_owner;

        public GraphData Owner { get => m_owner; set => m_owner = value; }

        public List<AbstractSolt> Slots => m_Slots;

        public Guid guid => m_Guid;
        public Guid groupGuid { get => m_GroupGuid; set => m_GroupGuid = value; }
        public string name { get => m_Name; set => m_Name = value; }

        public virtual bool allowedInSubGraph => true;
        public virtual bool allowedInMainGraph => true;
        public virtual bool canCopyNode => true;

        [SerializeField] private Rect m_rect;
        public Rect rect { get => m_rect; internal set => m_rect = value; }

        protected AbstractNode()
        {
            m_Guid = Guid.NewGuid();
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(m_GuidSerialized))
                m_Guid = new Guid(m_GuidSerialized);
            else
                m_Guid = Guid.NewGuid();

            if (!string.IsNullOrEmpty(m_GroupGuidSerialized))
                m_GroupGuid = new Guid(m_GroupGuidSerialized);
            else
                m_GroupGuid = Guid.Empty;
                
            m_Slots = SerializationHelper.Deserialize<AbstractSolt>(m_SerializableSlots);
            m_SerializableSlots = null;
            foreach (var s in m_Slots)
            {
                s.Owner = this;
            }

            UpdateNodeAfterDeserialization();
        }

        protected virtual void InitPort() {
        
        }

        public virtual void UpdateNodeAfterDeserialization()
        {
        }

        public void OnBeforeSerialize()
        {
            m_GuidSerialized = m_Guid.ToString(); 
            m_GroupGuidSerialized = m_GroupGuid.ToString();
            m_SerializableSlots = SerializationHelper.Serialize<AbstractSolt>(m_Slots);
        }

        #region Slot

        public T FindSlot<T>(int slotId) where T : AbstractSolt
        {
            foreach (var slot in m_Slots)
            {
                if(slot.Id== slotId && slot is T target)
                    return target;
            }
            return default(T);
        }

        public T FindSlot<T>(Guid guid) 
            where T : AbstractSolt
        {
            foreach (var slot in m_Slots)
            {
                if (slot.guid == guid && slot is T target)
                    return target;
            }
            return default(T);
        }

        public T FindSlot<T>(string guidstr) 
            where T : AbstractSolt
        {
            if (string.IsNullOrEmpty(guidstr) || !Guid.TryParse(guidstr, out var guid))
                throw new ArgumentNullException(guidstr);
            return FindSlot<T>(guid);
        }

        public AbstractSolt AddSlot(AbstractSolt slot)
        {
            if (slot == null)
                throw new ArgumentNullException(nameof(slot));
            var foundSlot = FindSlot<AbstractSolt>(slot.Id);
            if (foundSlot != null)
                return foundSlot;

            m_Slots.Add(slot);
            slot.Owner = this;
            OnSlotsChanged();
            return slot;
        }

        private void OnSlotsChanged()
        {
            
        }

        public virtual string GetVariableNameForSlot(int slotId)
        {
            var slot = FindSlot<AbstractSolt>(slotId);
            if (slot == null)
                throw new ArgumentException($"Attempting to use AbstractSolt({slotId}) on node of type {this} where this slot can not be found!");
            return ""; 
        }

        #endregion

    }
}