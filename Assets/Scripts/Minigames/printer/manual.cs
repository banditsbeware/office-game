using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class manual : MonoBehaviour
{
    private Animator manualAnimator;
    private int pageNumber;
    private List<string> contents = new List<string>(){
        "",
        "Error 69:\n\n- Input technician code\n- connect red and orange ports\n- activate red and orange ports\n- connect blue and green ports\n- de-activate red and orange ports\n- activate blue and green ports\n- Input 'port seed reset' code\n- de-activate blue and green ports",
        "Error 1337:\n\nAttempt turning printer on and back off again\nIf this does not fix the problem:\n- Ensure proper connections between red and purple ports, as well as the blue and orange.\n- activate any unused ports.\n- input your personalized printer access code\n- de-activate unused ports\n- try printing again",
    };
    [SerializeField] private TMP_Text currentPage;
    [SerializeField] private TMP_Text turningPage;
    void OnEnable()
    {
        manualAnimator = GetComponent<Animator>();
        pageNumber = 0;
    }

    //called from arrows on manual
    public void Forward()
    {
        if(pageNumber < contents.Count - 1) 
        {
            pageNumber += 1;

            manualAnimator.SetInteger("page_number", pageNumber);
            manualAnimator.SetTrigger("page_open");
            turningPage.text = currentPage.text;
            currentPage.text = contents[pageNumber];
        }
    }

    public void Back()
    {
        if(pageNumber > 0) 
        {
            pageNumber -= 1;
            manualAnimator.SetInteger("page_number", pageNumber);
            manualAnimator.SetTrigger("page_close");
            turningPage.text = contents[pageNumber];
        }
    }

    public void changeContents()
    {
        currentPage.text = contents[pageNumber];
    }

}
