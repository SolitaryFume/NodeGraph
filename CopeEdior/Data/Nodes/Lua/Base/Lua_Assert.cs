
using System;

namespace UnityEditor.NodeGraph.Lua
{
    [Serializable]
    [NodeMenu("Base/assert")]
    public class Lua_Assert : FunctionNode
    {
        public Lua_Assert()
        { 
            name = "assert";
            funName = nameof(assert);
            InitPort();
        }

        private string assert([Slot(0,SlotValueLuaType.Boolean)] bool v,[Slot(1,SlotValueLuaType.String)]string message)
        {
            return string.Empty;
        }
    }
}