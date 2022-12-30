using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UObject = UnityEngine.Object;

public class ResourcesManager : MonoBehaviour
{
    public class BundleInfo
    {
        public string AssetName;
        public string BundleName;
        public List<string> Dependences;
    }
    private Dictionary<string,BundleInfo> BundleInfos= new Dictionary<string,BundleInfo>();
    /// <summary>
    /// 解析版本文件
    /// </summary>
    private void ParseVersionFile()
    {
        //版本文件的路径
        string url = Path.Combine(PathUtil.BundleResourcesOutPath, APPConst.FILE_LIST_OUTPUT_NAME);
        string[] fileData = File.ReadAllLines(url);
        //解析文件信息
        foreach (var data in fileData)
        {
            BundleInfo bundleInfo= new BundleInfo();
            string[] info = data.Split('|');
            bundleInfo.AssetName= info[0];
            bundleInfo.BundleName= info[1];
            List<string> list= new List<string>(info.Length-2);
            for(int i = 2; i < info.Length; i++)
            {
                list.Add(info[i]);
            }
            bundleInfo.Dependences = list;
            BundleInfos.Add(bundleInfo.AssetName, bundleInfo);
        }
    }
    IEnumerator LoadBundleAsync(string assetName,Action<UObject> callback = null)
    {
        string bundleName = BundleInfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathUtil.BundleResourcesOutPath, bundleName);
        List<string> dependence = BundleInfos[assetName].Dependences;
        if (dependence!=null && dependence.Count > 0)
        {
            foreach (var dep in dependence)
            {
                yield return LoadBundleAsync(dep);
            }
        }

        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;
        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync(assetName);
        yield return bundleRequest;

        callback?.Invoke(bundleRequest?.asset);
    }
    private void EditorLoadAsset(string assetName, Action<UObject> callback = null)
    {
        UObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UObject>(assetName);
        if (obj == null) Debug.LogErrorFormat("asset:{0} do not found", assetName); 
        callback?.Invoke(obj);
    }
    #region Load Resources
    private void LoadAsset(string assetName, Action<UObject> callback = null)
    {
        if(APPConst.GameMode==GameMode.EditorMode)
            EditorLoadAsset(assetName, callback);
        else
            StartCoroutine(LoadBundleAsync(assetName, callback));
    }
    public void LoadUI(string assetName, Action<UObject> callback = null)
    {
        LoadAsset(PathUtil.GetUIPath(assetName), callback);
    }
    public void LoadMusic(string assetName, Action<UObject> callback = null)
    {
        LoadAsset(PathUtil.GetMusicPath(assetName), callback);
    }
    public void LoadSound(string assetName, Action<UObject> callback = null)
    {
        LoadAsset(PathUtil.GetSoundPath(assetName), callback);
    }
    public void LoadEffect(string assetName, Action<UObject> callback = null)
    {
        LoadAsset(PathUtil.GetEffectPath(assetName), callback);
    }
    public void LoadScene(string assetName, Action<UObject> callback = null)
    {
        LoadAsset(PathUtil.GetScenePath(assetName), callback);
    }
#endregion
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private void Start()
    {
        LoadUI("TestUI", OnComplete);
    }
    private void Awake()
    {
        this.ParseVersionFile();
    }
    private void OnComplete(UObject obj)
    {
        GameObject gameObject = obj as GameObject;
        Instantiate(gameObject, transform.position, Quaternion.identity, this.transform);
        gameObject.SetActive(true);
    }
}
