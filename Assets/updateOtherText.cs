using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpeakEasy
{
    public class updateOtherText : MonoBehaviour
    {
        TMP_Text thisText;
        TMP_Text otherText;
        private void Awake()
        {
            thisText = gameObject.GetComponent<TMP_Text>();
            otherText = gameObject.GetComponentInChildren<TMP_Text>();

            Debug.Log(gameObject.GetComponentInChildren<TMP_InputField>() == null);

            gameObject.GetComponentInChildren<TMP_InputField>().onValueChanged.AddListener(OnTextChanged);
        }

        private void OnTextChanged(string newText)
        {
            otherText.text = newText;
        }
    }
}
