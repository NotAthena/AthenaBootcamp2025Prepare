using Newtonsoft.Json;
using System;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;


public class JsonHelper
{
    protected JsonHelper() { }


    public static T ReadData<T>(string filePath)
    {
        string jsonRead;
        try
        {
            jsonRead = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonRead);
        }
        catch (FileNotFoundException)
        {
            return default;
        }
    }

    public static void SaveData(Object saveObject, string filePath)
    {
        string json = JsonConvert.SerializeObject(saveObject, Formatting.Indented);

        string extension = Path.GetExtension(filePath);
        if (string.IsNullOrEmpty(extension))
        {
            filePath += ".txt";
        }

        string dirPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        else
        {
            File.WriteAllText(filePath, string.Empty);
        }

        using (StreamWriter sw = (File.Exists(filePath)) ? File.AppendText(filePath) : File.CreateText(filePath))
        {
            sw.WriteLine(json);
        }
    }
}
