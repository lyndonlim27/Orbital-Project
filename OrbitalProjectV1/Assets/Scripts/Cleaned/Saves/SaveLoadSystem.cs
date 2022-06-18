using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    public string SavePath => $"{Application.persistentDataPath}/save.txt";

    [ContextMenu("Save")]
    void Save()
    {
        var state = LoadFile();
        SaveState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    void Load()
    {
        var state = LoadFile();
        LoadState(state);
    }


    public void SaveFile(object state)
    {
        //using (var stream = File.Open(SavePath, FileMode.Create))
        //{
        //    var formatter = new BinaryFormatter();
        //    formatter.Serialize(stream, state);
        //}

        using (StreamWriter writer = new StreamWriter(SavePath))
        {
            writer.Write(JsonUtility.ToJson(state));
        }
    }

    public SerializableDictionary<string, object> LoadFile()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found");
            return new SerializableDictionary<string, object>();

        }

        //using (FileStream stream = File.Open(SavePath, FileMode.Open))
        //{
        //    var formatter = new BinaryFormatter();
        //    return (Dictionary<string, object>)formatter.Deserialize(stream);
        //}
        using (StreamReader reader = new StreamReader(SavePath))
        {
            return JsonUtility.FromJson<SerializableDictionary<string, object>>(reader.ReadToEnd());
        }
    }

    void SaveState(SerializableDictionary<string, object> state)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.SaveState();
        }
    }

    void LoadState(SerializableDictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if(state.TryGetValue(saveable.Id, out object savedState))
            {
                saveable.LoadState(savedState);
            }
        }
    }
}
