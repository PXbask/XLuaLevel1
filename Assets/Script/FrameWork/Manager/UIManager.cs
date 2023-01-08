using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Dictionary<string,GameObject> UI = new Dictionary<string,GameObject>();
    /// <summary>
    /// UI·Ö×é
    /// </summary>
    Dictionary<string,Transform> UIGroups = new Dictionary<string,Transform>();
    private Transform UIParent;
    private void Awake()
    {
        UIParent = this.transform.parent.Find("UI");
    }
    public void SetUIGroup(List<string> group)
    {
        foreach (var item in group)
        {
            GameObject go = new GameObject("group_" + item);
            go.transform.SetParent(UIParent,false);
            UIGroups.Add(item, go.transform);
        }
    }
    Transform GetUIGroup(string group)
    {
        if (!UIGroups.TryGetValue(group, out var go))
        {
            Debug.LogErrorFormat("Group: {0} is not found", group);
            return null;
        }
        else
        {
            return go;
        }
    }
    public void OpenUI(string uiName,string group,string luaName)
    {
        GameObject ui = null;
        string uipath = PathUtil.GetUIPath(uiName);
        Object uiObj = Manager.Pool.Spawn("UI", uipath);
        Transform parent = GetUIGroup(group);
        if (uiObj!=null)
        {
            ui = uiObj as GameObject;
            UILogic uILogic = ui.GetComponent<UILogic>();
            ui.transform.SetParent(parent, false);
            uILogic.OnOpen();
            return;
        }
        Manager.Resources.LoadUI(uiName, (UnityEngine.Object obj) =>
        {
            ui = Instantiate(obj) as GameObject;
            ui.transform.SetParent(parent,false);

            UILogic uiLogic = ui.AddComponent<UILogic>();
            uiLogic.assetName = uipath;
            uiLogic.Init(luaName);
            uiLogic.OnOpen();
        });
    }
}
