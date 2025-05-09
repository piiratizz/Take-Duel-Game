using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JsonSaveLoadService<T> : ISaveLoadService<T>
{
    private readonly string filePath;

    public JsonSaveLoadService(string fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    public void Save(T data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public T Load()
    {
        if (!File.Exists(filePath))
            return default;
    
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(json);
    }
}