using System;
using UnityEngine;

namespace UnityLib.Graph
{
    [Serializable]
    [NodeMenu("LuaAsset")]
    public class LuaForAsset:NodeData
    {
        public TextAsset luaFile;
    }
}