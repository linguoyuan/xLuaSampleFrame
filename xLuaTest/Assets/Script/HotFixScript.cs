using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
using UnityEngine.Networking;

public class HotFixScript : MonoBehaviour {

    private LuaEnv LuaEnv;

    private static HotFixScript hotFix;

    public static HotFixScript HotFix
    {
        get
        {
            return hotFix;
        }
    }
    private HotFixScript()
    {
        hotFix = this;
    }

    public void DoLuaStart()
    {
        LuaEnv = new LuaEnv();
        LuaEnv.AddLoader(MyLoader);
        LuaEnv.DoString("require'mylua'");
	}
	
	private byte[] MyLoader(ref string filePath)
    {
        string absPath = @"D:\MyProject\1_Game\14_xlua\xLuaSampleFrame\PlayerGamePackage\" + filePath + ".lua.txt";
        return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(absPath));
    }

    private void OnDisable()
    {
        LuaEnv.DoString("require'myDispose'");
    }

    private void OnDestroy()
    {
        LuaEnv.Dispose();
    }



}
