using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public class SlotBool : LuaSlot, ISlotHasValue<bool>
    {
        [SerializeField] private bool m_Value;
        [SerializeField] private bool m_DefaultValue;

        public SlotBool(int slotId, string displayName, SlotType slotType,bool defaultValue = false, bool hidden = false) : base(slotId, displayName, slotType, hidden)
        {
            m_DefaultValue = defaultValue;
            m_Value = m_DefaultValue;
        }

        public bool defaultValue => m_DefaultValue;
        public bool value { get => m_Value; set => m_Value = value; }
        public override SlotValueLuaType ValueType => SlotValueLuaType.Boolean;
        public override bool isDefaultValue => value.Equals(defaultValue);

        public override VisualElement InstantiateControl()
        {
            var field = new Toggle();

            field.value = defaultValue;
            field.RegisterValueChangedCallback(OnValueChange);
            return field;
        }

        private void OnValueChange(ChangeEvent<bool> evt)
        {
            m_DefaultValue = evt.newValue;
            AssetDatabase.SaveAssets();
        }

        public override string GetDefaultValue()
        {
            return m_DefaultValue.ToString();
        }
    }
}