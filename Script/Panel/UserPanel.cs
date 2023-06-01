using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : BasePanel
{
    public Button btnOk;
    public Button btnCancel;
    protected override void Awake() {
        base.Awake();
        btnOk.onClick.AddListener(OnBtnOk);
        btnCancel.onClick.AddListener(OnBtnCancel);
    }

    private void OnBtnOk()
    {
        Debug.Log(">>>>>>>>> on btn ok");
        ClosePanel();
    }

    private void OnBtnCancel()
    {
        Debug.Log(">>>>>>>>> on btn cancel");
        ClosePanel();
    }

}
