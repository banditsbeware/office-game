using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//instance attached to static class
public class TaskManagerInstance : MonoBehaviour
{
    void Awake()
	{
		TaskManager.instance = this;
        TaskManager.checklist = Meta.Daily["todaysTasks"];
    }
    void Update()
    {
        
    }

    //TaskComplete function - each task calls once finished / started

}
