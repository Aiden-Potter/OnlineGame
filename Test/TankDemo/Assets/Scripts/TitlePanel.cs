﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : PanelBase
{
    private Button startBtn;
    private Button infoBtn;
    private Button exitBtn;
    private RectTransform rect;
    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "TitlePanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        
        startBtn = skinTrans.Find("StartBtn").GetComponent<Button>();
        infoBtn = skinTrans.Find("InfoBtn").GetComponent<Button>();
        exitBtn = skinTrans.Find("ExitBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(OnStartClick);
        infoBtn.onClick.AddListener(OnInfoClick);
        exitBtn.onClick.AddListener(OnExitClick);
    }
    #endregion

    public void OnStartClick()
    {
        //Battle.Instance.StartTwoCampBattle(2, 2);
        PanelMgr.instance.OpenPanel<OptionPanel>("");
        
    }
    public void OnInfoClick()
    {
        PanelMgr.instance.OpenPanel<InfoPanel>("");
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}
