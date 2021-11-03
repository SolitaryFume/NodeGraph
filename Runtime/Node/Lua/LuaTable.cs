namespace UnityLib.Graph
{
    using UnityEngine;

    public interface ICompile 
    {
        string Compile(params string[] args);
    }

    public abstract class LuaValue<T> : NodeData
    {
        [Input] [Output] public T value;
    }

    [NodeMenu("Value")]
    public class LuaVariable:NodeData
    {
        [HideInInspector] public string Name;
        [HideInInspector] public LUATYPE LuaType;
        [HideInInspector] public LuaValueType Value;
        [HideInInspector] public PROPERTY Property;
    }

    public abstract class LuaNodeData : NodeData
    {
        public abstract string ToLuaCode(GraphData graphData);
    }

    public enum LUATYPE
    {
        NIL,
        BOOLEAN,
        STRING,
        NUMBER,
        TABLE,
        FUNCTION,
        THERAD,
        USERDATA
    }

    public enum PROPERTY
    { 
        GET,
        SET,
    }

    public enum FLOWDIRECTION
    { 
        FROM,
        TO
    }

    public interface LuaValueType { }
    public struct LUATYPE_NIL : LuaValueType { }
    public struct LUATYPE_BOOLEAN : LuaValueType { }
    public struct LUATYPE_STRING : LuaValueType { }
    public struct LUATYPE_FUNCTION : LuaValueType { }
    public struct LUATYPE_USERDATA : LuaValueType { }
    public struct LUATYPE_THREAD : LuaValueType { }
    public struct LUATYPE_TABLE : LuaValueType { }

    [System.Serializable]
    public struct FLOW 
    {
        public bool IsConnect;
        public FLOWDIRECTION Direction;
        public string Node;
        public string Port;
    }//流向

    public abstract class Function : NodeData
    {
        [Input] [HideInInspector] public FLOW From;
        [Output] [HideInInspector] public FLOW To;
    }

    [NodeMenu("Flow/IF_ELSE")]
    public class Flow_IF_ELSE : LuaNodeData
    {
        [Input] [HideInInspector] public FLOW From;
        [Input] [HideInInspector] public bool Condition;
        [Output] [HideInInspector] public FLOW True;
        [Output] [HideInInspector] public FLOW Flse;

        public override string ToLuaCode(GraphData graphData)
        {
            string conditionStr = string.Empty;
            string trueStr = string.Empty;
            string falseStr = string.Empty;
            return string.Format("if {0} then\n {1} else\n {2} end\n", conditionStr, trueStr, falseStr);
        }
    }

    [NodeMenu("Flow/While")]
    public class Flow_While_Do : NodeData
    {
        [Input] [HideInInspector] public bool Condition;
        [Output] [HideInInspector] public FLOW Leep;
        [Output] [HideInInspector] public FLOW Flse;
    }

    [NodeMenu("Flow/Repeat")]
    public class Flow_Repeat_Until : NodeData
    {
        [Input] [HideInInspector] public FLOW Condition;
        [Output] [HideInInspector] public FLOW Leep;
        [Output] [HideInInspector] public FLOW Flse;
    }

    [NodeMenu("Flow/For")]
    public class Flow_For : Function
    {
        [Input] [HideInInspector] public long StartValue;
        [Input] [HideInInspector] public long EndValue;
        [Input] [HideInInspector] public long StepValue;
        [Output] [HideInInspector] public FLOW Leep;
    }

    [NodeMenu("Head")]
    public class Flow_Head: NodeData
    {
        [Output] [HideInInspector] public FLOW To;
    }
}