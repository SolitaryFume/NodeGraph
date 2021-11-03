using System;

namespace UnityEditor.NodeGraph.Lua
{
    [NodeMenu("Base/error")]
    [Serializable]
    public class Lua_Error : FunctionNode
    {
        public Lua_Error()
        {
            name = "error";
            funName = nameof(error);
            InitPort();
        }

        private string error([Slot(0, SlotValueLuaType.String)] string message, [Slot(1, SlotValueLuaType.Nmuber)] int level)
        { 
            return string.Empty;
        }
    }
}