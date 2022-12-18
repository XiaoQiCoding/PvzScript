using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager instance;
    public Text sunNumText;
    public ProgressPanel progressPanel;
    public AllCardPanel allCardPanel;
    
    private void Awake() {
        instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
        progressPanel.gameObject.SetActive(false);
        // 根据配置的数据生成可选的卡片
        allCardPanel.InitCards();
    }

    public void UpdateUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
    }

    public void InitProgressPanel()
    {
        LevelInfoItem levelInfo = GameManager.instance.levelInfo.LevelInfoList[GameManager.instance.curLevelId];
        for (int i = 0; i < levelInfo.progressPercent.Length; i++)
        {
            // 拿到配置的数据，并且在指定位置生成旗子
            float percent = levelInfo.progressPercent[i];
            if(percent == 1)
            {
                continue;
            }
            progressPanel.SetFlagPercent(percent);
        }
        // 初始化进度为0
        progressPanel.SetPercent(0);
        progressPanel.gameObject.SetActive(true);
    }

    public void UpdateProgressPanel()
    {
        // todo: 拿到当前波次的僵尸总数
        int progressNum = 0;
        for (int i = 0; i < GameManager.instance.levelData.LevelDataList.Count; i++)
        {
            LevelItem levelItem = GameManager.instance.levelData.LevelDataList[i];
            if(levelItem.levelId == GameManager.instance.curLevelId && levelItem.progressId == GameManager.instance.curProgressId)
            {
                progressNum += 1;
            }
        }

        // 当前波次剩余的僵尸数量
        int remainNum = GameManager.instance.curProgressZombie.Count;
        // 当前波次进行到多少百分比
        float percent = (float)(progressNum - remainNum) / progressNum;
        // 当前波次比例，前一波次比例
        LevelInfoItem levelInfoItem = GameManager.instance.levelInfo.LevelInfoList[GameManager.instance.curLevelId];
        float progressPercent = levelInfoItem.progressPercent[GameManager.instance.curProgressId - 1];
        float lastProgressPercent = 0;
        if(GameManager.instance.curProgressId > 1)
        {
            lastProgressPercent = levelInfoItem.progressPercent[GameManager.instance.curProgressId - 2];
        }
        // 最终比例 = 当前波次百分比 + 前一波次百分比
        float finalPercent = percent * (progressPercent - lastProgressPercent) + lastProgressPercent;
        progressPanel.SetPercent(finalPercent);
    }

}
