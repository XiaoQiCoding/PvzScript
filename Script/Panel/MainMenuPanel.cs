using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public Button btnChangeUser;

    protected override void Awake() {
        base.Awake();
        btnChangeUser.onClick.AddListener(OnBtnChangeUser);
    }

    private void OnBtnChangeUser()
    {
        // todo: 打开用户列表界面
        Debug.Log("OnBtnChangeUser");
        BaseUIManager.Instance.OpenPanel(UIConst.UserPanel);
    }
 
}
