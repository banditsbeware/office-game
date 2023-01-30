using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "juicy.fruit";

    public FileDataHandler(string dirPath, string fileName)
    {
        dataDirPath = Application.persistentDataPath;
        dataFileName = fileName;
    }

    public meta Load()
    {
        string path = Path.Combine(dataDirPath, dataFileName);
        meta loadedData = null;
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
            loadedData = JsonUtility.FromJson<meta>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("oops: " + path + "\n" + e);
            }
        }
        return loadedData;
    }
    public void Save(meta data)
    {
        string path = Path.Combine(dataDirPath, dataFileName);
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
}
