using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    public TMPro.TMP_Text text;
    // Update is called once per frame
    void Update()
    {
        text.text = Input.mousePosition.ToString();
    }
}
