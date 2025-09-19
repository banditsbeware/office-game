using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

// various pre-programmed states for the world as it progresses
public class MetaStates : MonoBehaviour
{
    public static MetaStates instance;

    [HideInInspector]

    private void Awake()
    {
        CreateSingleton();
        SetQueues();
    }

    void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void SetQueues()
    {
        monDreams.Enqueue("genericDreamCutscene");
        tuesDreams.Enqueue("sheepDream");
        tuesDreams.Enqueue("terrence");
        tuesDreams.Enqueue("adventureDream");
        wedDreams.Enqueue("grassDream");
        wedDreams.Enqueue("eyesDream");
        wedDreams.Enqueue("reflectionBoxDream");
        thursDreams.Enqueue("adventureDreamPt2");
        thursDreams.Enqueue("adventureDreamPt2");
        thursDreams.Enqueue("adventureDreamPt2");
        friDreams.Enqueue("finalDream");
    }
    //******************************************************************************
    //  Office Task Lists
    //******************************************************************************
    public static List<TaskType> monRequiredTasks = new List<TaskType> { TaskType.Spreadsheet, TaskType.Email, TaskType.Boss, TaskType.Print, TaskType.Dean };
    public static List<TaskType> tuesRequiredTasks = new List<TaskType> { TaskType.Sacha, TaskType.Spreadsheet, TaskType.Document, TaskType.Memo, TaskType.Harold };
    public static List<TaskType> wedRequiredTasks = new List<TaskType> { TaskType.Harold, TaskType.Marisol, TaskType.Email, TaskType.Boss };
    public static List<TaskType> thursRequiredTasks = new List<TaskType> { TaskType.Bathroom, TaskType.Dean, TaskType.Print };
    public static List<TaskType> friRequiredTasks = new List<TaskType> { TaskType.Boss };

    //******************************************************************************
    //  Dreams
    //******************************************************************************
    public static Queue<string> monDreams;
    public static Queue<string> tuesDreams;
    public static Queue<string> wedDreams;
    public static Queue<string> thursDreams;
    public static Queue<string> friDreams;

}