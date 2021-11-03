using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TextureControlAttribute : Attribute, IControlAttribute
    {
        string m_Label;

        public TextureControlAttribute(string label = null) { 
            m_Label = label;
        }

        public VisualElement InstantiateControl(AbstractNode node, PropertyInfo propertyInfo)
        {
            return new StringControlView(m_Label, node, propertyInfo);
        }
    }
}