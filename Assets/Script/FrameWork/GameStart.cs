using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Tooltip("选择开发模式")]
    public GameMode GameMode;
    private void Awake()
    {
        APPConst.GameMode = GameMode;
    }
}
