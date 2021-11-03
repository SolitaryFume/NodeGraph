using System;

namespace UnityEditor.NodeGraph.Lua
{
    [NodeMenu("Base/ipairs")]
    [Serializable]
    public class Lua_Ipairs : FunctionNode
    {
        public Lua_Ipairs()
        {
            name = "ipairs";
            funName = nameof(ipairs);
            InitPort();
        }

        private string ipairs([Slot(0, SlotValueLuaType.All)] object t,[Slot(1,SlotValueLuaType.Nmuber)]out int index,[Slot(2,SlotValueLuaType.NotNil)] out int value)
        {
            index = default;
            value = default;
            return default;
        }
    }
}