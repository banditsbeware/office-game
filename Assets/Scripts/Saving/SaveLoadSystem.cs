using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadSystem : MonoBehaviour
{
    private static string dataDirPath;
    private static string dataFileName = "juicy.fruit";

    //finds permanent data path for storing files
    private void Awake() 
    {
        dataDirPath = Application.persistentDataPath;
        
        if (Meta.Global.Count == 0)
        {
            Meta.ResetData();
        }
    }

    //imports data from JSON file and applies to meta's static variables
    public static void Load()
    {
        string path = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(path))
        {
            try
            {
                //load serialized data
                string dataToLoad = "";

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
            //convert back to c# object
            SerializableMeta loadedData = JsonUtility.FromJson<SerializableMeta>(dataToLoad);
            Meta.Import(loadedData);

            }
            catch (Exception e)
            {
                Debug.LogError("oops: " + path + "\n" + e);
            }
        }
        
    }

    //saves current static meta variables to a JSON file
    public static void Save()
    {
        string path = Path.Combine(dataDirPath, dataFileName);

        SerializableMeta data = new SerializableMeta();
        data = Meta.Serialize();

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("oops: " + path + "\n" + e);
        }
    }

    private void OnApplicationQuit() {
        Save();
    }
}
