using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public static class TaskManager
{
    public static TaskManagerInstance instance;
    
    public static List<TaskType> checklist;

    public static void TaskComplete(TaskType task)
    {
        if (checklist.Contains(task))
        {
            checklist.Remove(task);
            ChecklistCheck();
        }
    }

    public static void ChecklistCheck()
    {
        if (checklist.Count == 0)
        {
            Meta.SetValue("workComplete", true, Meta.Daily);
        }
    }
}

public enum TaskType
{
    Spreadsheet,
    Email,
    Document,
    Harold,
    Boss,
    Memo,
    Print,
    Dean,
    Marisol,
    Sacha

}
