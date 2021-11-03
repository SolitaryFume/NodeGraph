using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UnityEditor.NodeGraph
{
    [Serializable]
    public class NumberSlot : LuaSlot, ISlotHasValue<float>
    {
        [SerializeField] private float m_Value;
        [SerializeField] private float m_DefaultValue;

        public float defaultValue => m_DefaultValue;
        public float value { get => m_Value; set => m_Value = value; }
        public override bool isDefaultValue => value.Equals(defaultValue);

        public override SlotValueLuaType ValueType => SlotValueLuaType.Nmuber;

        public NumberSlot(int slotId,string displayName, SlotType slotType, bool hidden =false, float defaultValue = default) :base(slotId, displayName, slotType,false)
        {
            this.m_DefaultValue = defaultValue;
            this.m_Value = defaultValue;
        }

        public override VisualElement InstantiateControl()
        {
            var field = new FloatField();
            field.value = defaultValue;
            field.RegisterValueChangedCallback(OnValueChange);
            return field;
        }

        private void OnValueChange(ChangeEvent<float> evt)
        {
            m_DefaultValue = evt.newValue;
            AssetDatabase.SaveAssets();
        }

        public override string GetDefaultValue()
        {
            return m_DefaultValue.ToString();
        }
    }

    public class FlowSlot : LuaSlot
    {
        public FlowSlot(int slotId, string displayName, SlotType slotType, bool hidden = false)
            : base(slotId, displayName, slotType, hidden)
        {
            
        }

        public override SlotValueLuaType ValueType => SlotValueLuaType.Flow;
        public override bool isDefaultValue => false;
    }
}