using System;
using XLua;

namespace UnityLib.GraphEditor
{
    public static class NodeGraphLuaEnv
    {
        private static LuaEnv m_env;

        public static LuaEnv ENV
        {
            get
            {
                if (m_env == null)
                {
                    m_env = new LuaEnv();
                }
                return m_env;
            }
        }

        public static LuaTable Parse(string text)
        {
            var objs = ENV.DoString(text);
            if(objs!=null && objs.Length>0)
            {
                return objs[0] as LuaTable;
            }
            throw new Exception("解析失败!");
        }
    }
}
