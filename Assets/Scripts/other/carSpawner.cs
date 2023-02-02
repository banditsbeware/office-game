using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawner : MonoBehaviour
{
    [SerializeField] private GameObject carObject;
    private static List<GameObject> cars;
    
    //

    void Start()
    {
        StartCoroutine(StartCar());
    }

        void Update()
    {
        
    }

    IEnumerator StartCar()
    {
        while (true)
        {
            Instantiate(carObject);
            yield return new WaitForSeconds(Random.Range(4f, 10f));
        }
    }
}
