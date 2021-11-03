using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    class StringControlView : VisualElement
    {
        AbstractNode m_Node;
        PropertyInfo m_PropertyInfo;

        public StringControlView(string label, AbstractNode node, PropertyInfo propertyInfo)
        {
            this.style.flexDirection = FlexDirection.Row;
            style.paddingLeft = 8;
            style.paddingRight = 8;
            style.paddingTop = 4;
            style.paddingBottom = 4;
            style.unityTextAlign = TextAnchor.MiddleCenter;

            m_Node = node;
            m_PropertyInfo = propertyInfo;
            if (propertyInfo.PropertyType != typeof(string))
                throw new ArgumentException("Property must be of type string.", "propertyInfo");
            label = label ?? ObjectNames.NicifyVariableName(propertyInfo.Name);
            if (!string.IsNullOrEmpty(label))
                Add(new Label(label));

            var strField = new TextField { value = (string)m_PropertyInfo.GetValue(m_Node) };
            strField.RegisterValueChangedCallback(OnChange);
            strField.style.flexGrow = 1;
            Add(strField);
        }

        private void OnChange(ChangeEvent<string> evt)
        {
            m_PropertyInfo.SetValue(m_Node, evt.newValue, null);
            this.MarkDirtyRepaint();
        }
    }
}