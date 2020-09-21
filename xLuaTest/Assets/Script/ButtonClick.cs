using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using UnityEngine.Networking;
using System.IO;

[Hotfix]
public class ButtonClick : MonoBehaviour {

    public Text textObj;
    GameObject Cav;

    public static Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();
    private void Awake()
    {
        
    }

    IEnumerator Start ()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(OnButtonClick);
        Cav = GameObject.Find("Canvas");
        StartCoroutine(LoadResourceCorotine("BoxImage1", "2.ab"));
        StartCoroutine(LoadLuaCorotine());
        yield return new WaitForSeconds(3);//等待ab资源加载完成和脚本加载完成
        HotFixScript.HotFix.DoLuaStart();
        MyLoadResource();
    }
	
    [LuaCallCSharp]
    void OnButtonClick()
    {
        textObj.text = "you use OnButtonClick()";
    }

    [LuaCallCSharp]
    void MyLoadResource()
    {
        
        //注意这里的名字是预支体的名字BoxImage，而不是ab文件的名字1
        //LoadResource("BoxImage", "1.ab");
        GameObject go = GetGameObjcet("BoxImage");

        //LoadResource("BoxImage1", "2.ab");
        //GameObject go1 = GetGameObjcet("BoxImage1");
        if (go != null)
        {
            Debug.Log(go.name);
        }
        GameObject.Instantiate(go, Cav.transform);
    }

    public GameObject GetGameObjcet(string goName)
    {
        if (!prefabDict.ContainsKey(goName))
        {
            Debug.Log("不存在 key：" + goName);
        }
        return prefabDict[goName];
    }

    //加载网络ab资源到本地
    IEnumerator LoadResourceCorotine(string resName, string filePath)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(@"http://localhost/box/" + filePath);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Get downloaded asset bundle
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
            if (ab == null)
            {
                Debug.Log("ab == null");
            }
            GameObject gameObject = ab.LoadAsset<GameObject>(resName);
            prefabDict.Add(resName, gameObject);
        }
       
    }

    //加载网络lua脚本到本地
    IEnumerator LoadLuaCorotine()
    {
        UnityWebRequest request = UnityWebRequest.Get(@"http://localhost/mylua.lua.txt");
        yield return request.SendWebRequest();
        string str = request.downloadHandler.text;
        if (request.isNetworkError)
        {
            Debug.Log(" Error: " + request.error);
        }
        else
        {
            Debug.Log("Received: " + request.downloadHandler.text);
        }

        Debug.Log(str);
        File.WriteAllText(@"D:\MyProject\1_Game\14_xlua\xLuaSampleFrame\PlayerGamePackage\mylua.lua.txt", str);
        UnityWebRequest request1 = UnityWebRequest.Get(@"http://localhost/myDispose.lua.txt");
        yield return request1.SendWebRequest();
        string str1 = request1.downloadHandler.text;
        Debug.Log(str1);
        File.WriteAllText(@"D:\MyProject\1_Game\14_xlua\xLuaSampleFrame\PlayerGamePackage\myDispose.lua.txt", str1);
    }
}
