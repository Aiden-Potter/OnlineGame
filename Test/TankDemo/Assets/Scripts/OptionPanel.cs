using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionPanel : PanelBase
{
    private Button startBtn;
    private Dropdown camp1;
    private Dropdown camp2;
    private Button closeBtn;
    private int camp1Num;
    private int camp2Num;
    #region 生命周期

    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "OptionPanel";
        layer = PanelLayer.Panel;
    }
    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        startBtn = skinTrans.Find("StartBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(OnStartClick);
        closeBtn = skinTrans.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
        camp1 = skinTrans.Find("Dropdown1").GetComponent<Dropdown>();
        camp2 = skinTrans.Find("Dropdown2").GetComponent<Dropdown>();

    }
    #endregion
    

    public void OnCloseClick()
    {
        Close();
    }
    public void OnStartClick()
    {
        camp1Num = camp1.value + 1;//从0开始
        camp2Num = camp2.value + 1;

        Battle.Instance.StartTwoCampBattle(camp1Num, camp2Num);
        PanelMgr.instance.ClosePanel("TitlePanel");
        Close();
    }
}

