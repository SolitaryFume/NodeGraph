using System;
using UnityEngine;

namespace UnityEditor.NodeGraph
{

    [Serializable]
    public abstract class LuaSlot : AbstractSolt
    {
        public abstract SlotValueLuaType ValueType { get; }
        protected LuaSlot(int slotId, string displayName, SlotType slotType, bool hidden = false) : base(slotId, displayName, slotType, hidden) { }

        public override Color Color {
            get {
                switch (ValueType)
                {
                    case SlotValueLuaType.Nil:
                        return Color.gray;
                    case SlotValueLuaType.Boolean:
                        return Color.red;
                    case SlotValueLuaType.Function:
                        return new Color(118 / 255f, 104 / 255f, 154 / 255f);
                    case SlotValueLuaType.Nmuber:
                        return Color.blue;
                    case SlotValueLuaType.String:
                        return Color.cyan;
                    case SlotValueLuaType.Table:
                        return Color.green;
                    case SlotValueLuaType.UserData:
                        return Color.magenta;
                    case SlotValueLuaType.Thread:
                        return Color.yellow;
                    default:
                        return Color.white;
                }
            }
        }

        public static LuaSlot CrateSlot(int slotId, string displayName, SlotType slotType, SlotValueLuaType valueType, bool hidden = false)
        {
            switch (valueType)
            {
                case SlotValueLuaType.Boolean:
                    return new SlotBool(slotId, displayName, slotType, hidden);
                case SlotValueLuaType.Nmuber:
                    return new NumberSlot(slotId, displayName, slotType, hidden);
                case SlotValueLuaType.String:
                    return new StringSlot(slotId, displayName, slotType, hidden);
                default:
                    return new GeneralLuaSlot(slotId, displayName, slotType, valueType, hidden);
            }
        }
    }

    public class GeneralLuaSlot : LuaSlot
    {
        public GeneralLuaSlot(int slotId, string displayName, SlotType slotType,SlotValueLuaType slotValueLuaType, bool hidden = false) : base(slotId, displayName, slotType, hidden)
        {
            m_slotValueLuaType = slotValueLuaType;
        }

        [SerializeField] private SlotValueLuaType m_slotValueLuaType;
        public override SlotValueLuaType ValueType => m_slotValueLuaType;

        public override bool isDefaultValue => false;
    }
}