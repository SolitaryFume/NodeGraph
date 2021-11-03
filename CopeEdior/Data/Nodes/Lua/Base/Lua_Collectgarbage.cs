using System;
using System.Reflection;
using UnityEngine;

namespace UnityEditor.NodeGraph.Lua
{

    [NodeMenu("Base/collectgarbage")]
    [Serializable]
    public class Lua_Collectgarbage : FunctionNode
    {
        public Lua_Collectgarbage()
        {
            name = "collectgarbage";
            funName = nameof(collectgarbage);
            InitPort();
        }

        private string collectgarbage([Slot(0, SlotValueLuaType.String)] string opt, [Slot(1, SlotValueLuaType.String)] string arg)
        {
            return string.Empty;
        }
    }
}