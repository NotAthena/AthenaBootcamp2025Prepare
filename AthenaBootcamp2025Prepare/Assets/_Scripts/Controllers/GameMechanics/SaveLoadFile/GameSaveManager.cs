using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaveManager
{
    private static string saveDataPath = $"{Application.persistentDataPath}/Snake/";
    public static void Save(GameLog gameLog)
    {
        if (gameLog.GameTurns == null || gameLog.GameTurns.Count == 0)
        {
            Debug.Log("Nothing to save!");
            return;
        }

        string filePath = $"{saveDataPath}Snake_{gameLog.GameTurns[0].LevelInfo.Difficulty}_{DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss")}.txt";

        JsonHelper.SaveData(gameLog, filePath);
    }

    public static GameLog Load(string fileName)
    {
        string filePath = $"{saveDataPath}{fileName}";
        return JsonHelper.ReadData<GameLog>(filePath);
    }

    public static List<string> GetSaveNameList()
    {
        List<string> saveNameList = new();
        var info = new DirectoryInfo(saveDataPath);
        var fileInfo = info.GetFiles();

        foreach (var file in fileInfo)
        {
            saveNameList.Add(file.Name);
        }
        return saveNameList;
    }

    public static void DeleteSave()
    {

    }
}
