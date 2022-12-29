using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil
{
    /// <summary>
    /// Unity��Դ��Ŀ¼
    /// </summary>
    public static readonly string AssetsPath = Application.dataPath;
    /// <summary>
    /// ��Ҫ��Bundle��Ŀ¼
    /// </summary>
    public static readonly string BuildResourcesPath = AssetsPath + "/BuildResources/";
    /// <summary>
    /// Bundle���Ŀ¼
    /// </summary>
    public static readonly string BundleOutPath = Application.streamingAssetsPath;
    /// <summary>
    /// AssetBundle����·��
    /// </summary>
    public static string BundleResourcesOutPath{ get => Application.streamingAssetsPath; }
    /// <summary>
    /// ��ȡUnity���·��
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
    /// ��ȡ��׼·��: \->/
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        return path.Trim().Replace("\\", "/");
    }
}
