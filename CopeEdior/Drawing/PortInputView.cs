using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public class PortInputView : GraphElement, IDisposable
    {
        EdgeControl m_EdgeControl;
        VisualElement m_Container;
        VisualElement m_Control;

        AbstractSolt slot;

        public PortInputView(AbstractSolt slot)
        {
            var styleSheet = Resources.Load<StyleSheet>("Styles/PortInputView");
            if (styleSheet == null)
                throw new Exception("No find Config !");
            styleSheets.Add(styleSheet);
            this.style.marginTop = 0;
            this.style.marginBottom = 0;

            this.slot = slot;

            pickingMode = PickingMode.Ignore;

            m_EdgeControl = new EdgeControl
            {
                @from = new Vector2(232f - 21f, 11.5f),
                to = new Vector2(232f, 11.5f),
                edgeWidth = 2,
                pickingMode = PickingMode.Ignore
            };
            Add(m_EdgeControl);

            m_Container = new VisualElement { name = "container" };
            {
                CreateControl();

                var slotElement = new VisualElement { name = "slot" };
                {
                    slotElement.Add(new VisualElement { name = "dot" });
                }
                m_Container.Add(slotElement);
            }
            Add(m_Container);
            m_Container.Add(new VisualElement() { name = "disabledOverlay", pickingMode = PickingMode.Ignore });
            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);

            slot.onHiddenChange += hidden => this.style.display = hidden?DisplayStyle.None:DisplayStyle.Flex;
        }


        void CreateControl()
        {
            m_Control = slot.InstantiateControl();
            if (m_Control != null)
            {
                m_Control.style.minWidth = 50;
                m_Control.style.minWidth = 25;
                m_Container.Insert(0, m_Control);;
            }
            else
            {
                m_Container.visible = m_EdgeControl.visible = false;
            }
        }

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            
        }

        public void Dispose()
        {
            if (m_Control is IDisposable disposable)
                disposable.Dispose();
        }
    }
}