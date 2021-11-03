using System.Reflection;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public interface IControlAttribute
    {
        VisualElement InstantiateControl(AbstractNode node, PropertyInfo propertyInfo);
    }
}