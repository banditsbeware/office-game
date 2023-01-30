using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistence : MonoBehaviour
{
    [Header("File Storage")]
    [SerializeField] private string fileName;
    private meta meta;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistence instance {get; private set;}

    private void Awake() 
    {
        if (instance != null)
        {
            Debug.Log("uh oh stinky");
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start() 
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    public void NewGame()
    {
        meta = new meta();
    }
    public void LoadGame()
    {
        meta = dataHandler.Load();
        foreach (IDataPersistence dpo in dataPersistenceObjects)
        {
            dpo.LoadData(meta);
        }
    }
    public void SaveGame()
    {
        foreach (IDataPersistence dpo in dataPersistenceObjects)
        {
            dpo.SaveData(ref meta);
        }
        dataHandler.Save(meta);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
