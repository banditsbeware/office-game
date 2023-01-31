using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    //for linking to outside stuff
    [SerializeField] private string permaURL;
    public static void openUrl(string url)
    {
        Application.OpenURL(url);
    }
    public void openUrl()
    {
        Application.OpenURL(permaURL);
    }
}
