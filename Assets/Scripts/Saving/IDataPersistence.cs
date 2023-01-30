using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(meta meta);
    void SaveData(ref meta meta);
}
