using System.Collections;
using System.Collections.Generic;
using SpeakEasy;
using UnityEngine;

public class DeanActions : MonoBehaviour
{
    private SEDialogue dialogueRef;

    private void Start() {
        dialogueRef = transform.GetComponentInChildren<SEDialogue>();
    }
    public void DeanTakesAction()
    {
        dialogueRef.BeginNode();
    }
}
