using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    public ErrorPuzzle currentError;

    private ErrorPuzzle error_1337 = new ErrorPuzzle
    {
        tasks = new List<Task>{
            new WiresTask("red", "purple"),
            new WiresTask("blue", "orange")
        }
    };

    void OnEnable()
    {
        currentError = error_1337;
    }





}



public class ErrorPuzzle
{
    public List<Task> tasks;
    public int currentTaskIndex = 0;

    //called from nodeManager when a wire is completed
    public void wiresCompleted(string node1, string node2)
    {
        //specific to WiresTask class, checks if two selected nodes match the current task's nodes
        if (tasks[currentTaskIndex].Check(node1, node2))
        {
            Debug.Log("woah! wires complete!");
            currentTaskIndex += 1;
            
            CheckCompletion();
        }
        else
        {
            Debug.Log("Uh Oh! go back to the start of the errors!");
            currentTaskIndex = 0;
        }
    }

    public void keyPadCompleted()
    {

    }

    public void buttonsCompleted()
    {

    }

    public void CheckCompletion()
    {
        if (currentTaskIndex == tasks.Count)
            {
                //Error completed
            }
    }
}
