using System;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "XiaoQi/PlantInfo", fileName = "PlantInfo", order = 1)]
public class PlantInfo : ScriptableObject
{
    public List<PlantInfoItem> plantInfoList = new List<PlantInfoItem>();
    // public Dictionary<int, LevelItem> LevelDataDict = new Dictionary<int, LevelItem>();
}

[System.Serializable]
public class PlantInfoItem
{
    public int plantId;
    public string plantName;
    public string description;
    public GameObject cardPrefab;
    // 这些信息已经存储在卡片中
    // public int useNum;
    // public int cdTime;
    // public GameObject prefab;


    override
    public string ToString()
    {
        return "[id]: " + plantId.ToString();
    }
}