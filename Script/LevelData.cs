using System;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "XiaoQi/LevelData", fileName = "LevelData", order = 3)]
public class LevelData : ScriptableObject
{
    public List<LevelItem> LevelDataList = new List<LevelItem>();
    // public Dictionary<int, LevelItem> LevelDataDict = new Dictionary<int, LevelItem>();
}

[System.Serializable]
public class LevelItem
{
    public int id;
    public int levelId;
    public int progressId;
    public int createTime;
    public int zombieType;
    public int bornPos;

    override
    public string ToString()
    {
        return "[id]: " + id.ToString();
    }
}
