using Microsoft.CSharp;

public class ChaosAgent
{
    public int threshold; //point of chaos at which agent begins appearing

    public int Weight() //returns the likelihood of agent ocurring based on its threshold
    {
        int weight = 0;
        if(Meta.Variables["chaos"] >= threshold) 
        {
            weight = (threshold * Meta.Variables["chaos"]) / (threshold * threshold);
        }
        return weight;
    }
}
