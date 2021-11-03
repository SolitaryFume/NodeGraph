using System;

namespace UnityEditor.NodeGraph.Lua
{
    [NodeMenu("Base/dofile")]
    [Serializable]
    public class Lua_Dofile : FunctionNode
    {
        public Lua_Dofile()
        {
            name = "dofile";
            funName = nameof(dofile);
            InitPort();
        }

        private string dofile([Slot(0, SlotValueLuaType.String)] string filename,[Slot(1,SlotValueLuaType.All)]out object Out)
        {
            Out = default;
            return string.Empty;
        }
    }
}