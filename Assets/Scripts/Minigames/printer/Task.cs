using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public bool isCompleted;

    public Task()
    {
        isCompleted = false;
    }
    
    public virtual bool Check()
    {
        return false;
    }

    public virtual bool Check(string n1, string n2)
    {
        return false;
    }

}

public class WiresTask : Task {
    public string startNode;
    public string endNode;

    public WiresTask(string n1, string n2)
    {
        isCompleted = false;
        startNode = n1;
        endNode = n2;
    }

    public override bool Check(string n1, string n2)
    {
        if ((n1 == startNode && n2 == endNode) || (n2 == startNode && n1 == endNode))
        {
            return true;
        }

        return false;
    }
        
}

