using System.IO;
using UnityEditor;
using UnityLib.Graph;

namespace UnityLib.GraphEditor
{
    public sealed class NodeGraphWindow:EditorWindow
    {
        private NodeGraphView nodeGraphView;
        private string editorAssetGuide;

        public static void EditorGarph(string assetguide)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetguide);

            var fileName = Path.GetFileName(path);
            var window = CreateWindow<NodeGraphWindow>(fileName);
            window.editorAssetGuide = assetguide;
            window.nodeGraphView = new NodeGraphView(window, assetguide);
            window.Init();
        }

        private void Init()
        {
            rootVisualElement.Add(nodeGraphView);
        }
    }
}
