using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

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
    private int newWindowCounter = 100;
    private int windowCounterMinimum = 4000;
    private int delayPerWindow = 2000;


    private void OnEnable() 
    {
        windows.Clear();

        referenceWindows = new List<GameObject>(){docReference, emailReference, slackReference, sheetReference};

        screenSpace = GameObject.Find("computerScreen");
        CreateWindow(sheetReference);
    }

    private void OnDisable() 
    {    
        foreach (workWindow window in windows)
        {
            window.DestroyWindow();
        }
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

    void FixedUpdate()
    {
        if (newWindowCounter > 0)
        {
            newWindowCounter--;
            return;
        }

        CreateRandomWindow();
        newWindowCounter = windowCounterMinimum + delayPerWindow * windows.Count;
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
