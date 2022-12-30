using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    None,
    EditorMode,
    PackageBundle,
    UpdateMode,
}
public class APPConst
{
    public const string ASSET_BUNDLE_EXTENSION = ".ab";
    public const string FILE_LIST_OUTPUT_NAME = "fileList.txt";
    public static GameMode GameMode = GameMode.EditorMode;
}
