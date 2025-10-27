using System.IO;
using UnityEngine;
using System.Text;

public static class SaveManager
{
    private static readonly string filePath = Path.Combine(Application.persistentDataPath, "save.dat");
    private static readonly string bkpFilePath = Path.Combine(Application.persistentDataPath, "save_bkp.dat");

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        string encoded = Encode(json);
        File.WriteAllText(filePath, encoded);
    }

    public static SaveData Load()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("Nessun file di salvataggio trovato, creo nuovi dati.");
            return NewData();
        }

        try
        {
            string encoded = File.ReadAllText(filePath);
            string json = Decode(encoded);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HandleVersion(data);

            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Errore nel caricamento: {e.Message}");
            return NewData();
        }
    }

    public static SaveData NewData()
    {
        SaveData newData = new SaveData();
        newData.StageCompleteStatus = new int[120];
        return newData;
    }

    public static void Restart()
    {
        SaveBackup();
        SaveData newData = NewData();
        Save(newData);
    }

    static void SaveBackup()
    {
        SaveData currentData = Load();
        string json = JsonUtility.ToJson(currentData);
        string encoded = Encode(json);
        File.WriteAllText(bkpFilePath, encoded);
    }

    static string Encode(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        return System.Convert.ToBase64String(bytes);
    }

    static string Decode(string encoded)
    {
        byte[] bytes = System.Convert.FromBase64String(encoded);
        return Encoding.UTF8.GetString(bytes);
    }

    static void HandleVersion(SaveData data)
    {
        // 🔹 Qui puoi gestire differenze tra versioni
        if (data.version != "0.8")
        {
            Debug.Log($"Aggiornamento dati da versione {data.version} a 0.8");
            data.version = "0.8";
            PlayerPrefs.SetInt("ResetExpFlag", 1);
            Save(data);
            // Qui puoi aggiungere eventuali adattamenti
        }
    }
}


[System.Serializable]
public class SaveData
{
    public string version = "0.8"; 
    public int[] StageCompleteStatus;
}

