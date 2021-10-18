using UnityLib.Graph;
using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.ProjectWindowCallback;

namespace UnityLib.GraphEditor
{
    public class NewGraphAction : EndNameEditAction
    {
        public NodeGraph Graph { get; set; }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var graph = ScriptableObject.CreateInstance<NodeGraph>();

            graph.Data = new GraphData()
            {
                AssetGuide = AssetDatabase.AssetPathToGUID(pathName)
            };
            File.WriteAllText(pathName, graph.ToJson());
            AssetDatabase.Refresh();
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<NodeGraph>(pathName);
            Selection.activeObject = obj;
        }
    }
}
