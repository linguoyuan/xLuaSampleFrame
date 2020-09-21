using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using UnityEngine.Networking;

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
        StartCoroutine(LoadResourceCorotine("BoxImage", "1.ab"));
        yield return new WaitForSeconds(3);
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
        return prefabDict[goName];
    }

    IEnumerator LoadResourceCorotine(string resName, string filePath)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(@"http://localhost/AssetBundles/" + filePath);
        //UnityWebRequest request = UnityWebRequest.Get(@"C:\05_Unity\01_GameProject\06_xLuaFrameSample\xLuaSampleFrame\SimpleTest\SimpleTest\AssetBundles\" + filePath);
        yield return request.SendWebRequest();
        while (!request.isDone)
        {
            yield return null;
        }
        AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        //AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        if (ab == null)
        {
            Debug.Log("ab == null");
        }
        GameObject gameObject = ab.LoadAsset<GameObject>(resName);
        prefabDict.Add(resName, gameObject);
    }
}
