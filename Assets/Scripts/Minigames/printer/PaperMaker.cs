using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMaker : MonoBehaviour
{
    [SerializeField] private GameObject printedPaper;

    void MakePaper()
    {
        if(!printedPaper.activeSelf) 
        {
            printedPaper.SetActive(true);
        }
    }
}
