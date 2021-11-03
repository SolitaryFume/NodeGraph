using System;

namespace UnityEditor.NodeGraph.Lua
{
    [NodeMenu("Base/getmetatable")]
    [Serializable]
    public class Lua_Getmetatable : FunctionNode
    {
        public Lua_Getmetatable()
        {
            name = "getmetatable";
            funName = nameof(getmetatable);
            InitPort();
        }

        private string getmetatable([Slot(0, SlotValueLuaType.All)] string @object,[Slot(1,SlotValueLuaType.Table|SlotValueLuaType.Nil)] out object Out) 
        {
            Out = string.Empty;
            return string.Empty;
        }
    }
}