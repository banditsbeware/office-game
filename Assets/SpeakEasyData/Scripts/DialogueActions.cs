using System.Collections;
using System.Collections.Generic;
using SpeakEasy;
using UnityEngine;

public class DialogueActions : MonoBehaviour
{
    private List<GameObject> defaultButtonsObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> tipButtons = new List<GameObject>();
    private SEDialogue dialogueController;

    private void Awake() {
        //stash default button objects for SetChoiceButtonsToPreset()
        dialogueController = GetComponent<SEDialogue>();
        foreach (GameObject button in dialogueController.choiceButtons)
        {
            defaultButtonsObjects.Add(button);
        }
    }

    //Button Preset meta variable must be set in the callbacks of the Action node
    //replaces the buttons being shown/hidden with a different set existing in the player
    public void SetChoiceButtonsToPreset()
    {
        switch (Meta.Global["dialogueChoiceButtonMode"])
        {
            case "default":
                dialogueController.choiceButtons.Clear();
                foreach (GameObject button in defaultButtonsObjects)
                {
                    dialogueController.choiceButtons.Add(button);
                }
                dialogueController.BeginNode();
                break;
            case "tip":
                dialogueController.choiceButtons.Clear();
                foreach (GameObject button in tipButtons)
                {
                    dialogueController.choiceButtons.Add(button);
                }
                dialogueController.BeginNode();
                break;
        }
    }
}
