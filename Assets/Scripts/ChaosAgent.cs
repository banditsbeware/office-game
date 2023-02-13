using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChaosAgent
{
    public int threshold; //point of chaos at which agent begins appearing

    public int Weight() //returns the likelihood of agent ocurring based on its threshold
    {
        int weight = 0;
        if(meta.chaos >= threshold) 
        {
            weight = (threshold * meta.chaos) / (threshold * threshold);
        }
        return weight;
    }
}
