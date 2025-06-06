using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JsonSaveLoadService : ISaveLoadService
{
    private readonly string filePath;

    public JsonSaveLoadService(string fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    public void Save(PlayerData data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public PlayerData Load()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
            Save(new PlayerData()
            {
                Name = "Unnamed",
                Balance = 100.0m,
                SelectedSkin = "bandit",
                AvailableSkins = new List<string>() {"bandit"}
            });
        }
    
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<PlayerData>(json);
    }
}