using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    public static ErrorPuzzle currentError;
    public static List<ErrorPuzzle> errors;
    public static int errorIndex;

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
        }
    };
    private ErrorPuzzle error_69 = new ErrorPuzzle
    {
        tasks = new List<Task>{
            new NumbersTask(696969),
            new WiresTask("orange", "red"),
            new MultiTask(new List<Task>{
                new NumbersTask(420),
                new WiresTask("red", "green"),
                new ButtonTask("green", true)
                })
        }
    };

    void Start()
    {
        ErrorPuzzle.OnComplete += Printer_OnErrorComplete;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("index: " + errorIndex + " err len: " + errors.Count);
        }
    }

    void OnEnable()
    {
        errors = new List<ErrorPuzzle>{
            error_1337,
            error_69
        };
        errorIndex = 0;
        currentError = errors[errorIndex];
        
    }

    private static void Printer_OnErrorComplete(object sender, EventArgs e)
    {
        if(errorIndex < errors.Count) 
        {
            errorIndex += 1;
            currentError = errors[errorIndex];
        }
        else
        {
            Debug.Log("errors complete!");
        }
    }
}



public class ErrorPuzzle
{
    public List<Task> tasks;
    public int currentTaskIndex = 0;
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
