using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityLib.Graph
{

    [Serializable]
    public class NodeGraph : ScriptableObject
    {
        private GraphData m_data;

        public GraphData Data { get => m_data; set => m_data = value;}

        public string ToJson() 
        {
            return UnityEditor.EditorJsonUtility.ToJson(m_data,true);
        }

        public bool FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("josn is null or empty !");
                return false;
            }
            m_data = new GraphData();
            UnityEditor.EditorJsonUtility.FromJsonOverwrite(json, m_data);
            return true;
        }
    }
}