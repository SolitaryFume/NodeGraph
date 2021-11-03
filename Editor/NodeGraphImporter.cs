using UnityLib.Graph;
using UnityEngine;
using UnityEditor;

using System.IO;
using System.Text;

namespace UnityLib.GraphEditor
{
    [UnityEditor.AssetImporters.ScriptedImporter(31, Extension, 3)]
    public class NodeGraphImporter: UnityEditor.AssetImporters.ScriptedImporter
    {
        public const string Extension = "nodegraph";
        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
        {
            var path = ctx.assetPath;
            var textData = File.ReadAllText(path,Encoding.UTF8);
            var nodeGraph = ScriptableObject.CreateInstance<NodeGraph>();
            nodeGraph.FromJson(textData);
            nodeGraph.name = "mainObj";

            ctx.AddObjectToAsset("mainObj", nodeGraph);

            var text = new TextAsset(textData);
            text.name = "json";
            ctx.AddObjectToAsset("jsondata", text);
            ctx.SetMainObject(nodeGraph);
        }
    }

    [CustomEditor(typeof(NodeGraphImporter))]
    public class NodeGraphImporterEditor:UnityEditor.AssetImporters.ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("编辑"))
            {
                var path = AssetDatabase.GetAssetPath(target);
                var guide = AssetDatabase.AssetPathToGUID(path);
                NodeGraphWindow.EditorGarph(guide);
            }

            this.ApplyRevertGUI();
        }
    }
}
    