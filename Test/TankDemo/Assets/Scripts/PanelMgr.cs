using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
public enum PanelLayer
{
    Panel,
    Tips,
}
public class PanelMgr : MonoBehaviour
{
    public static PanelMgr instance;
    private GameObject canvas;
    public Dictionary<string, PanelBase> dict;
    private Dictionary<PanelLayer, Transform> layerDict;

    public void Awake()
    {
        instance = this;
        InitLayer();
        dict = new Dictionary<string, PanelBase>();
    }

    void InitLayer()
    {
        canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("panelMgr.InitLayer fail, canvas is null");
        }
        layerDict = new Dictionary<PanelLayer, Transform>();
        foreach (PanelLayer pl in Enum.GetValues(typeof(PanelLayer)))
        {
            string name = pl.ToString();
            Transform transform = canvas.transform.Find(name);
            layerDict.Add(pl, transform);
        }
    }
    /// <summary>
    /// 打开一个面板，这个面板类必须继承PanelBase，运用了设计模式Templete Method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="skinPath"></param>
    /// <param name="args"></param>
    public void OpenPanel<T>(string skinPath,params object[] args) where T:PanelBase
    {
        string name = typeof(T).ToString();
        if (dict.ContainsKey(name))//已经打开则不必打开
        {
            return;
        }
        PanelBase panel = canvas.AddComponent<T>();//运行时多态
        panel.Init(args);
        dict.Add(name, panel);
        //加载皮肤
        skinPath = (skinPath != "" ? skinPath : panel.skinPath);
        GameObject skin = Resources.Load<GameObject>(skinPath);
        if (skin == null)
        {
            Debug.LogError("Panel Open error,Resources load fail,skinpath is :"+skinPath);
        }
        panel.skin = Instantiate(skin);
        
        //坐标层级
        Transform skinTrans = panel.skin.transform;
       
        PanelLayer layer = panel.layer;
        Transform parent = layerDict[layer];
        skinTrans.SetParent(parent);
        skinTrans.localPosition = Vector3.zero;
        //Panel生命周期
        panel.OnShowing();
        //animtion
        panel.OnShowed();
    }

    public void ClosePanel(string name)
    {
        PanelBase panel = (PanelBase)dict[name];
        if (panel == null)
        {
            //没了就不用关了
            return;
        }
        panel.OnClosing();
        dict.Remove(name);
        panel.OnClosed();
        GameObject.Destroy(panel.skin);
        Component.Destroy(panel);
    }
}
