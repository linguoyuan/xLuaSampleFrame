using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
using UnityEngine.Networking;

public class HotFixScript : MonoBehaviour {

    private LuaEnv LuaEnv;

    

	void Awake()
    {
        LuaEnv = new LuaEnv();
        LuaEnv.AddLoader(MyLoader);
        LuaEnv.DoString("require'mylua'");
	}
	
	private byte[] MyLoader(ref string filePath)
    {
        string absPath = @"C:\05_Unity\01_GameProject\06_xLuaFrameSample\xLuaSampleFrame\SimpleTest\SimpleTest\PlayerGamePackage\" + filePath + ".lua.txt";
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
