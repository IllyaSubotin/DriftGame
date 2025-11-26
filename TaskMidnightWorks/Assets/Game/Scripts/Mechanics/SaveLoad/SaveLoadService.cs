using UnityEngine;
using System.IO;

public class SaveLoadService
{
    private readonly string _path;

    public SaveLoadService()
    {
        _path = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_path, json);
    }

    public SaveData Load()
    {
        if (!File.Exists(_path))
            return new SaveData();

        string json = File.ReadAllText(_path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void DeleteSave()
    {
        if (File.Exists(_path))
            File.Delete(_path);
    }
}

[System.Serializable]
public class SaveData
{
    public CarSaveData[] carSaveDatas;
    public int currentCarId;

    public float Volume;
    public float Sensitivity;

    public int cash;
    public int gems;
}
