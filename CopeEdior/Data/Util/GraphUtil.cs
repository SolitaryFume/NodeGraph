using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Globalization;
using UnityEditor.ProjectWindowCallback;

namespace UnityEditor.NodeGraph
{

    static class SerializationHelper
    {
        private static Type GetTypeFromSerializedString(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(nameof(fullName));

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(fullName);
                if (type != null)
                    return type;
            }

            throw new InvalidCastException($"No find Type of typefullname : {fullName}");
        }

        public static JsonObj Serialize(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return new JsonObj
            {
                typeFullName = obj.GetType().FullName,
                jsonData = JsonUtility.ToJson(obj,true)
            };
        }

        public static object Deserialize(JsonObj jsonObj)
        {
            var type = GetTypeFromSerializedString(jsonObj.typeFullName);
            return JsonUtility.FromJson(jsonObj.jsonData, type);
        }

        public static T Deserialize<T>(JsonObj jsonObj)
        {
            var obj = Deserialize(jsonObj);
            return (T) obj;
        }

        public static List<object> Deserialize(List<JsonObj> jsonObjs)
        { 
            if(jsonObjs==null)
                throw new ArgumentNullException(nameof(jsonObjs));
            var list = new  List<object>(jsonObjs.Count);
            foreach (var jsonObj in jsonObjs)
                list.Add(Deserialize(jsonObj));
            return list;
        }

        public static List<T> Deserialize<T>(List<JsonObj> jsonObjs)
        {
            if (jsonObjs == null)
                throw new ArgumentNullException(nameof(jsonObjs));
            var list = new List<T>(jsonObjs.Count);
            foreach (var jsonObj in jsonObjs)
                list.Add(Deserialize<T>(jsonObj));
            return list;
        }

        public static List<JsonObj> Serialize<T>(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            var jsonObjs = new List<JsonObj>();
            foreach (var item in list)
                jsonObjs.Add(Serialize(item));
            return jsonObjs;
        }

        [Serializable]
        public struct JsonObj
        {
            public string typeFullName;
            public string jsonData;
        }


    }
}
