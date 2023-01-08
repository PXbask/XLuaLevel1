using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Tooltip("选择开发模式")]
    public GameMode GameMode;
    public bool OpenLog;
    private void Start()
    {
        Manager.Event.Subscribe(10000, OnLuaInit);

        APPConst.GameMode = GameMode;
        APPConst.OpenLog = OpenLog;
        DontDestroyOnLoad(this);

        Manager.Resources.ParseVersionFile();
        Manager.Lua.Init();
    }
    private void OnLuaInit(object args)
    {
        Manager.Lua.StartLua("main");

        Manager.Pool.CreateGameObjectPool("UI", 10);
        Manager.Pool.CreateGameObjectPool("Monster", 120);
        Manager.Pool.CreateGameObjectPool("Effect", 120);

        Manager.Pool.CreateAssetPool("AssetBundle", 10);
    }
    private void OnApplicationQuit()
    {
        Manager.Event.UnSubscribe(10000, OnLuaInit);
    }
}
