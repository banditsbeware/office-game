using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class work : MonoBehaviour
{
    //references
    private GameObject screenSpace;
    [SerializeField] private GameObject docReference;
    [SerializeField] private GameObject emailReference;
    [SerializeField] private GameObject slackReference;
    [SerializeField] private GameObject sheetReference;

    private List<GameObject> referenceWindows;

    //windows
    public static List<workWindow> windows = new List<workWindow>();
    public static workWindow activeWindow;


    private void OnEnable() 
    {
        referenceWindows = new List<GameObject>(){docReference, emailReference, slackReference, sheetReference};

        screenSpace = GameObject.Find("computerScreen");
        CreateWindow(emailReference);
    }

    private void OnDisable() {
        foreach (workWindow window in windows)
        {
            window.Destroy();
        }
        windows.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            activeWindow.Back();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            activeWindow.Enter();
            return;
        }
        if (Input.inputString != "")
        {
            activeWindow.InputRecieved(Input.inputString);
        }
    }

    private void CreateWindow(GameObject reference)
    {
        workWindow window = Instantiate(reference, screenSpace.transform).GetComponent<workWindow>();
        windows.Add(window);
    }

    private void CreateRandomWindow()
    {
        int windowIndex = Random.Range(0, referenceWindows.Count);
        CreateWindow(referenceWindows[windowIndex]);
    }
}
