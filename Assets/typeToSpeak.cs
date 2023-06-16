using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class typeToSpeak : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputBox;
    
    void Start() 
    {
        
    }

    public void ReselectTextBox()
    {
        Debug.Log("e");
        inputBox.Select();
    }
}
