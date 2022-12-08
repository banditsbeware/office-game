using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public bool isCompleted = false;

    public void SubscribeEvents()
    {
        ErrorPuzzle.OnComplete += ResetTaskCompletion;
        ErrorPuzzle.OnFailed += ResetTaskCompletion;
    }
    private void ResetTaskCompletion(object sender, EventArgs e)
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

    public virtual bool Check(string button, bool press)
    {
        return false;
    }

    public virtual bool Check(int n)
    {
        return false;
    }

    public virtual bool CheckMulti()
    {
        return false;
    }

}

public class WiresTask : Task {
    public string startNode;
    public string endNode;

    public WiresTask(string n1, string n2)
    {
        startNode = n1;
        endNode = n2;
        SubscribeEvents();
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

public class ButtonTask : Task {
    public string buttonColor;
    public bool state;  //true = press, false = release

    public ButtonTask(string col, bool down)
    {
        buttonColor = col;
        state = down;
        SubscribeEvents();
    }

    public override bool Check(string button, bool press)
    {
        if (button == buttonColor && press == state)
        {
            return true;
        }

        return false;
    }
}

public class NumbersTask : Task {
    public int password;

    public NumbersTask(int pass)
    {
        password = pass;
        SubscribeEvents();
    }

    public override bool Check(int attempt)
    {
        if (attempt == password)
        {
            return true;
        }

        return false;
    }
}

public class MultiTask : Task {
    public List<Task> multiple;

    public MultiTask(List<Task> list)
    {
        multiple = list;
    }

    public override bool Check()
    {
        foreach (Task task in multiple)
        {
            if (task.Check())
            {
                task.isCompleted = true;
                return true;
            }
        }

        return false;
    }

    public override bool Check(int n)
    {
        foreach (Task task in multiple)
        {
            if (task.Check(n))
            {
                task.isCompleted = true;
                return true;
            }
        }

        return false;
    }

    public override bool Check(string n1, string n2)
    {
        foreach (Task task in multiple)
        {
            if (task.Check(n1, n2))
            {
                task.isCompleted = true;
                return true;
            }
        }

        return false;
    }

    public override bool Check(string button, bool press)
    {
        foreach (Task task in multiple)
        {
            if (task.Check(button, press))
            {
                task.isCompleted = true;
                return true;
            }
        }

        return false;
    }

    public override bool CheckMulti()
    {
        bool complete = true;
        foreach (Task task in multiple)
        {
            if (!task.isCompleted)
            {
                complete = false;
            }
        }

        return complete;
    }
}

