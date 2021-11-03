using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public sealed class NodeView : Node
    {
        private NodeGraphView m_graphView;
        private IEdgeConnectorListener m_connectorListener;
        public new AbstractNode userData { get; set; }

        public void Initialize(AbstractNode node, IEdgeConnectorListener connectorListener, NodeGraphView graphView)
        {
            this.m_graphView = graphView;
            this.m_connectorListener = connectorListener;

            VisualElement visualElement = this.Q("node-border");
            visualElement.style.overflow = Overflow.Visible;

            this.title = node.name;

            userData = node;
            this.viewDataKey = node.guid.ToString();
            this.SetPosition(userData.rect);
            AddSlots(node.Slots);
            this.mainContainer.Add(OnInspector());
        }

        void AddSlots(IEnumerable<AbstractSolt> slots)
        {
            foreach (var slot in slots)
                AddPortForSlot(slot);
        }

        private Port AddPortForSlot(AbstractSolt slot)
        {
            var port = SlotPort.Create(slot, m_connectorListener);
            if (slot.IsOutputSlot)
                outputContainer.Add(port);
            else
            {
                var portContainer = new VisualElement();
                portContainer.style.flexDirection = FlexDirection.Row;
                var portInputView = new PortInputView(slot) { style = { position = Position.Absolute } };
                portContainer.Add(portInputView);
                portContainer.Add(port);
                inputContainer.Add(portContainer);
            }
            port.viewDataKey = slot.guid.ToString();
            port.OnDisconnect = OnEdgeDisconnected;
            return port;
        }

        private void OnEdgeDisconnected(Port obj)
        {
            
        }


        private VisualElement m_ControlsDivider;
        private VisualElement m_ControlItems;
        private VisualElement OnInspector()
        {
            var controlsContainer = new VisualElement { name = "controls" };
            {
                m_ControlsDivider = new VisualElement { name = "divider" };
                m_ControlsDivider.AddToClassList("horizontal");
                controlsContainer.Add(m_ControlsDivider);
                m_ControlItems = new VisualElement { name = "items" };
                //m_ControlItems.style.paddingTop = 4;
                //m_ControlItems.style.paddingBottom = 4;
                controlsContainer.Add(m_ControlItems);

                foreach (var propertyInfo in userData.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    foreach (IControlAttribute attribute in propertyInfo.GetCustomAttributes(typeof(IControlAttribute), false))
                        m_ControlItems.Add(attribute.InstantiateControl(userData, propertyInfo));
            }
            return controlsContainer;
        }
    }
}