using UnityEngine;
using UnityEditor;
using OfficeOpenXml;
using System.IO;
using System;
using System.Reflection;

[InitializeOnLoad]
public class Startup
{
    public static bool needRead = true;
    static Startup()
    {
        string path = Application.dataPath + "/Editor/关卡管理.xlsx";
        string assetName = "Level";
        Debug.Log("path, " + path);
        FileInfo fileInfo = new FileInfo(path);
        // LevelData levelData = new LevelData();
        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["僵尸"];
            for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
            {
                LevelItem levelItem = new LevelItem();
                Type type = typeof(LevelItem);
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    FieldInfo variable = type.GetField(worksheet.GetValue(2, j).ToString());
                    string tableValue = worksheet.GetValue(i, j).ToString();
                    variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
                }
                levelData.LevelDataList.Add(levelItem);
            }
        }

        if (Startup.needRead)
        {
            AssetDatabase.CreateAsset(levelData, "Assets/Resources/" + assetName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    static int getInt(string input)
    {
        string value = input.Trim();
        int num = -1;
        int.TryParse(value, out num);
        return num;
    }

}
