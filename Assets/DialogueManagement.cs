using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy
{
    public sealed class DialogueManagement : MonoBehaviour
    {
        private static DialogueManagement instance = null;
        private static readonly object padlock = new object();

        DialogueManagement()
        {
        }

        public static DialogueManagement Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DialogueManagement();
                    }
                    return instance;
                }
            }
        }
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
