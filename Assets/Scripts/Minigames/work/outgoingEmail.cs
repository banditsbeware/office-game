using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outgoingEmail : MonoBehaviour
{
    public void Send()
    {
        StartCoroutine(transform.parent.parent.parent.GetComponent<workEmail>().SendEmail());
    }
}
