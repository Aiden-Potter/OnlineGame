using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinPanel : PanelBase
{
    private Image winImage;
    private Image failImage;
    private TextMeshProUGUI text;
    private Button closeBtn;
    private bool isWin;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "WinPanel";
        layer = PanelLayer.Panel;
        if (args.Length ==1)
        {//玩家默认阵营1
            int camp = (int)args[0];
            isWin = (camp == 1);
        }
    }
    public override void OnShowing()
    {
        base.OnShowing();
        Cursor.lockState = CursorLockMode.None;
        Transform skinTrans = skin.transform;
        closeBtn = skinTrans.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
        failImage = skinTrans.Find("FailImage").GetComponent<Image>();
        winImage = skinTrans.Find("WinImage").GetComponent<Image>();
        text = skinTrans.Find("Text").GetComponent<TextMeshProUGUI>();
        if (isWin)
        {
            failImage.enabled = false;
            text.text = "祖国和人民感谢你!";
        }
        else
        {
            winImage.enabled = false;
            text.text = "祖国和人民对你很失望!";
        }
    }
    #endregion

    public void OnCloseClick()
    {
        Battle.Instance.ClearBattle();
        PanelMgr.instance.OpenPanel<TitlePanel>("");
        Close();
    }
}
