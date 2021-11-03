using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public class SlotPort : Port
    {
        protected SlotPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }

        public static SlotPort Create(AbstractSolt slot, IEdgeConnectorListener connectorListener)
        {
            if (slot == null)
                throw new ArgumentNullException(nameof(slot));
            if (connectorListener == null)
                throw new ArgumentNullException(nameof(connectorListener));
            var port = new SlotPort(Orientation.Horizontal, slot.IsInputSlot ? Direction.Input : Direction.Output, Capacity.Single, null)
            {
                m_EdgeConnector = new EdgeConnector<Edge>(connectorListener)
            };
            port.AddManipulator(port.m_EdgeConnector);
            port.portName = slot.DisplayName;
            port.userData = slot;
            port.portColor = slot.Color;
            port.portType = slot.GetType();
            return port;
        }

        public new AbstractSolt userData { get; set; }

        public Action<Port> OnDisconnect;

        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);
            OnDisconnect?.Invoke(this);
            if (!this.IsCopiable())
                this.userData.IsHidden = false;
        }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            this.userData.IsHidden = true;
        }

        public override void DisconnectAll()
        {
            base.DisconnectAll();
            this.userData.IsHidden = false;
        }

        
    }
}
