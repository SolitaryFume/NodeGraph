using UnityEngine;
using UnityEditor;
using System;

namespace UnityEditor.NodeGraph
{
    [Serializable]
    [CreateAssetMenu(fileName = "LuaNodeGraph",menuName = "NodeGraph/Lua",order = 1)]
    public sealed class LuaNodeGraph : AbstractNodeGraph
    { 

    }

    [CustomEditor(typeof(LuaNodeGraph))]
    public sealed class LuaNodeGraphInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("EDITOR"))
            {
                var window = EditorWindow.CreateWindow<NodeGraphWindow>(typeof(SceneView));
                window.Initialize(target as LuaNodeGraph);
            }
        }
    }
}