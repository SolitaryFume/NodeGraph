using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using LuaStringBuilder = System.Text.StringBuilder;

public interface IGeneratesBodyCode
{
    void GenerateNodeCode(LuaStringBuilder luaStringBuilder, bool getNext = false);
}
