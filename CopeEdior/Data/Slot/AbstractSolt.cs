using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public interface ISlotHasValue<T>
    {
        T defaultValue { get; }
        T value { get; }
    }

    [Serializable]
    public abstract class AbstractSolt:ISerializationCallbackReceiver
    {
        protected const string k_NotInit = "Not Initilaized";

        [SerializeField] private string m_DisplayName = k_NotInit;
        [SerializeField] private SlotType m_SlotType = SlotType.Input;
        [SerializeField] private string m_GuidSerialized;
        [SerializeField] private int m_Id;

        [SerializeField] private bool m_Hidden;
        [SerializeField] private bool m_HasError;

        private Guid m_guid;
        public Guid guid => m_guid;

        public int Id => m_Id;
        public bool IsInputSlot=> m_SlotType == SlotType.Input;
        public bool IsOutputSlot => m_SlotType == SlotType.Output;
        public SlotType SlotType => m_SlotType;
        public bool IsHidden { get => m_Hidden; set { m_Hidden = value; onHiddenChange?.Invoke(value); } }
        public AbstractNode Owner { get; set; }
        public string DisplayName => m_DisplayName;

        public abstract bool isDefaultValue { get; }
        public virtual Color Color { get; } = Color.white;

        protected AbstractSolt(int slotId,string displayName,SlotType slotType,bool hidden = false) : this()
        { 
            m_Id = slotId;
            m_DisplayName = displayName;
            m_SlotType = slotType;
            m_Hidden = hidden;
        }
        public AbstractSolt() 
        {
            m_guid = Guid.NewGuid();
        }

        public virtual string GetDefaultValue()
        {
            return string.Empty;
        }

        public virtual VisualElement InstantiateControl()
        {
            return null;
        }

        public virtual void OnBeforeSerialize()
        {
            m_GuidSerialized = m_guid.ToString();
        }

        public virtual void OnAfterDeserialize()
        {
            if (!Guid.TryParse(m_GuidSerialized, out m_guid))
            {
                m_guid = Guid.NewGuid();
            }
        }

        public event Action<bool> onHiddenChange;
    }
}