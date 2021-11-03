namespace UnityEditor.NodeGraph
{
    using System;
    
    [Flags]
    public enum SlotValueLuaType
    { 
        Nil = 1<<0,
        Boolean = 1<<1,
        Function = 1<<2,
        Nmuber = 1 << 3,
        String = 1 << 4,
        Table = 1 << 5,
        UserData = 1<<6,
        Thread = 1 << 7,
        Flow = 1 << 8,
        All = Nil| Boolean| Function| Nmuber| String| Table| UserData| Thread,
        NotNil = Boolean| Function| Nmuber| String| Table| UserData| Thread
    }
}