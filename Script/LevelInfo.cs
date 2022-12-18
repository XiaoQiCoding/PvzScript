using System;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "XiaoQi/LevelInfo", fileName = "LevelInfo", order = 2)]
public class LevelInfo : ScriptableObject
{
    public List<LevelInfoItem> LevelInfoList = new List<LevelInfoItem>();
    // public Dictionary<int, LevelItem> LevelDataDict = new Dictionary<int, LevelItem>();
}

[System.Serializable]
public class LevelInfoItem
{
    public int levelId;
    public string levelName;
    public float [] progressPercent;


    override
    public string ToString()
    {
        return "[id]: " + levelId.ToString();
    }
}