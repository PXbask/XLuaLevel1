using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil
{
    /// <summary>
    /// Unity资源根目录
    /// </summary>
    public static readonly string AssetsPath = Application.dataPath;
    /// <summary>
    /// 需要打Bundle的目录
    /// </summary>
    public static readonly string BuildResourcesPath = AssetsPath + "/BuildResources/";
    /// <summary>
    /// Bundle输出目录
    /// </summary>
    public static readonly string BundleOutPath = Application.streamingAssetsPath;
    /// <summary>
    /// AssetBundle下载路径
    /// </summary>
    public static string BundleResourcesOutPath{ get => Application.streamingAssetsPath; }
    /// <summary>
    /// 获取Unity相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }
        return path.Substring(path.IndexOf("Assets"));
    }
    /// <summary>
    /// 获取标准路径: \->/
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        return path.Trim().Replace("\\", "/");
    }
    public static string GetLuaPath(string name)
    {
        return string.Format("Assets/BuildResources/LuaScript/{0}.bytes", name);
    }
    public static string GetUIPath(string name)
    {
        return string.Format("Assets/BuildResources/UI/Prefab/{0}.prefab", name);
    }
    public static string GetMusicPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Music/{0}", name);
    }
    public static string GetSoundPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Sound/{0}", name);
    }
    public static string GetEffectPath(string name)
    {
        return string.Format("Assets/BuildResources/Effect/Prefabs/{0}.prefab", name);
    }
    public static string GetSpritePath(string name)
    {
        return string.Format("Assets/BuildResources/Sprite/{0}", name);
    }
    public static string GetScenePath(string name)
    {
        return string.Format("Assets/BuildResources/Scene/{0}.unity", name);
    }
}
