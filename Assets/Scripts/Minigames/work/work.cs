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
    [SerializeField] private Vector2 windowXYMin = new Vector2(0, -250);
    [SerializeField] private Vector2 windowXYMax = new Vector2(450, 0);
    private int initalWindowCounter = 150;
    [SerializeField] private int newWindowCounter;
    [SerializeField] private int windowCounterMinimum;
    [SerializeField] private int delayPerWindow;


    private void OnEnable() 
    {
        windows.Clear();

        newWindowCounter = initalWindowCounter;

        referenceWindows = new List<GameObject>(){docReference, emailReference, slackReference, sheetReference};

        screenSpace = GameObject.Find("computerScreen");
        CreateWindow(sheetReference);
    }

    private void OnDisable() 
    {    
        List<workWindow> windowsToDestroy = new List<workWindow>();
        windowsToDestroy.AddRange(windows);

        foreach (workWindow window in windowsToDestroy)
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

        Vector2 randomScreenPosition = new Vector2(Random.Range(windowXYMin.x, windowXYMax.x), Random.Range(windowXYMin.y, windowXYMax.y));
        window.GetComponent<RectTransform>().localPosition = randomScreenPosition;

        windows.Add(window);
        activeWindow = window;
        window.transform.SetAsLastSibling();
    }

    private void CreateRandomWindow()
    {
        int windowIndex = Random.Range(0, referenceWindows.Count);
        CreateWindow(referenceWindows[windowIndex]);
    }
}
