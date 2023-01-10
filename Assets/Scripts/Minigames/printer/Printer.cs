using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Printer : MonoBehaviour
{
    public static ErrorPuzzle currentError;
    public static List<ErrorPuzzle> errors;
    public static int errorIndex;
    [SerializeField] private TMP_Text popup;
    

    private ErrorPuzzle error_1337 = new ErrorPuzzle
    {
        tasks = new List<Task>{
            new MultiTask(new List<Task>{
                new WiresTask("red", "purple"),
                new WiresTask("blue", "orange")
                }),
            new ButtonTask("green", true),
            new NumbersTask(12345),
            new ButtonTask("green", false)
        },
        errorPopup = "A printer accessory has failed, please refer to manual or contact a certified printer engineer for repairs.\nerror#1337"
    };
    private ErrorPuzzle error_69 = new ErrorPuzzle
    {
        tasks = new List<Task>{
            new NumbersTask(696969),
            new WiresTask("orange", "red"),
            new MultiTask(new List<Task>{
                new ButtonTask("orange", true),
                new ButtonTask("red", true)
                }),
            new WiresTask("blue", "green"),
            new MultiTask(new List<Task>{
                new ButtonTask("orange", false),
                new ButtonTask("red", false),
                new ButtonTask("blue", true),
                new ButtonTask("green", true),
                }),
            new NumbersTask(420),
            new MultiTask(new List<Task>{
                new ButtonTask("green", false),
                new ButtonTask("blue", false),
                }),

        },
        errorPopup = "The printer's port seed has come undone. Please contact a certified HB technician.\nerror#69"
    };

    void Start()
    {
        ErrorPuzzle.OnComplete += Printer_OnErrorComplete;
    }

    void OnEnable()
    {
        errors = new List<ErrorPuzzle>{
            error_69,
            error_1337,
        };
        errorIndex = 0;
        currentError = errors[errorIndex];
        popup.text = currentError.errorPopup;
        
        if(transform.parent.GetComponent<interact_minigame>().isGame) 
        {
            AkSoundEngine.PostEvent("Play_printer_jam", gameObject);
        }
    }

    private void Printer_OnErrorComplete(object sender, EventArgs e)
    {
        if(errorIndex < errors.Count - 1) 
        {
            errorIndex += 1;
            currentError = errors[errorIndex];
            popup.text = currentError.errorPopup;
        }
        else
        {
            Debug.Log("errors complete!");
        }
        transform.Find("paper").GetComponent<Animator>().SetTrigger("print");
    }
}



public class ErrorPuzzle
{
    public List<Task> tasks;
    public int currentTaskIndex = 0;
    public string errorPopup;
    public static event EventHandler OnComplete;
    public static event EventHandler OnFailed;
    

    //called from task scripts, test is the bool from checking the task's correctness
    public void Completed(bool isCorrect)
    {
        if (isCorrect){
            if (currentTask() is MultiTask)
            {
                if(currentTask().CheckMulti())
                {
                    Debug.Log("woah! multi-task complete!");
                    currentTaskIndex += 1;
                    
                    CheckErrorCompletion();
                }
            }
            else
            {
                Debug.Log("woah! task complete!");
                currentTaskIndex += 1;
                CheckErrorCompletion();
            }
        }
        else
        {
            currentTaskIndex = 0;
            AkSoundEngine.PostEvent("Play_printer_error", GameObject.Find("printerStation"));
            OnFailed(this, EventArgs.Empty);
        }
    }

    public void CheckErrorCompletion()
    {
        if (currentTaskIndex == tasks.Count)
            {
                OnComplete?.Invoke(this, EventArgs.Empty);
                Debug.Log("Error completed!");
            }
    }

    public Task currentTask()
    {
        return tasks[currentTaskIndex];
    }
}
