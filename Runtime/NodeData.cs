using UnityEngine;
using System;

namespace UnityLib.Graph
{
    public abstract class NodeData: ScriptableObject
    {
        [SerializeField] [HideInInspector] private string m_guid = Guid.NewGuid().ToString();
        [SerializeField] [HideInInspector] private Vector2 m_position;
        public string guid => m_guid;
        public Vector2 Position { get => m_position; set => m_position = value; }
    }

    [System.Serializable]
    public struct JsonScriptableObject
    {
        public string type;
        public string data;

        public static JsonScriptableObject ToJson(object obj)
        {
            return new JsonScriptableObject()
            {
                type = obj.GetType().FullName,
                data = UnityEditor.EditorJsonUtility.ToJson(obj,true),
            };

        }

        public static T ToObj<T>(JsonScriptableObject jsonObj)
            where T : ScriptableObject
        {
            var type = Type.GetType(jsonObj.type);
            var obj = ScriptableObject.CreateInstance(type);
            UnityEditor.EditorJsonUtility.FromJsonOverwrite(jsonObj.data, obj);
            return (T)obj;
        }
    }

    [Serializable]
    public class NodeLink
    {
        public NodeLink(string guide = null)
        {
            if (string.IsNullOrEmpty(guide))
            {
                this.m_guid = Guid.NewGuid().ToString();
            }
            else
            {
                m_guid = guide;
            }
        }

        [SerializeField] [HideInInspector] private string m_guid;
        public string guid => m_guid;
        public string startNode;
        public string endNode;
        public string inputPort;
        public string outputPort;
    }
}