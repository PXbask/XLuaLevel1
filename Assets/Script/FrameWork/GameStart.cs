using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Tooltip("ѡ�񿪷�ģʽ")]
    public GameMode GameMode;
    private void Awake()
    {
        APPConst.GameMode = GameMode;
    }
}
