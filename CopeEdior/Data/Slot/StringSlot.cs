using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public class StringSlot : LuaSlot, ISlotHasValue<string>
    {
        [SerializeField] private string m_Value;
        [SerializeField] private string m_DefaultValue;

        public string defaultValue => m_DefaultValue;
        public string value { get => m_Value; set => m_Value = value; }

        public StringSlot(int slotId, string displayName, SlotType slotType,bool hidden = false, string defaultValue = default) : base(slotId, displayName, slotType)
        {
            if (string.IsNullOrEmpty(defaultValue))
                m_DefaultValue = string.Empty;
            else
                m_DefaultValue = defaultValue;
            m_Value = m_DefaultValue;
        }

        public override SlotValueLuaType ValueType => SlotValueLuaType.String;

        public override bool isDefaultValue => value.Equals(defaultValue);

        public override VisualElement InstantiateControl()
        {
            var field = new TextField();

            field.value = defaultValue;
            field.RegisterValueChangedCallback(OnValueChange);
            return field;
        }

        public override string GetDefaultValue()
        {
            return $"\"{defaultValue}\"";
        }
        private void OnValueChange(ChangeEvent<string> evt)
        {
            m_DefaultValue = evt.newValue;
            AssetDatabase.SaveAssets();
        }
    }
}