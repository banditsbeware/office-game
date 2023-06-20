using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSwitcher : MonoBehaviour
{
    public GameObject characterA;
    public GameObject characterB;

    void Start()
    {
        if (Meta.Daily["afterWork"])
        {
            characterA.SetActive(false);
            return;
        }

        characterB.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
