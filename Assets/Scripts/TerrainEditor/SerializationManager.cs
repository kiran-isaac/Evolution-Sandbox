using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializationManager
{
    public static bool Save(int saveSlot, object saveData)
    {
        saveData.prep();

        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/Terrain Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Terrain Saves");
        }

        string path = Application.persistentDataPath + "/Terrain Saves/" + "saveslot" + saveSlot.ToString() + ".trn";

        FileStream file = File.Create(path);

        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }

    public static object Load(int saveSlot)
    {
        string path = Application.persistentDataPath + "/Terrain Saves/" + "saveslot" + saveSlot.ToString() + ".trn";

        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        return formatter;
    }
}
