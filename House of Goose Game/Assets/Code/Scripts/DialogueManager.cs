using System.Data.Common;
using UnityEngine;
using System.Collections.Generic;
using System;

public class DialogueManager : HoG
{  
    public static DialogueManager instance { get; private set; }
    
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;

    bool advanceDialogueTriggered = false;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartDialogue()
    {

    }

    //IEnumerator RunDialogue(string[] dialogue, int startPosition)
    //{

    //}

    public void AdvanceDialogue()
    {
        advanceDialogueTriggered = true;
    }

    private string[][] LoadDialogueFromResource(string resource)
    {
        List<string[]> dialogue = new List<string[]>();
        TextAsset dialogueData = Resources.Load<TextAsset>(resource);
        if (dialogueData != null)
        {
            string[] col = dialogueData.text.Split('\n');

            for (int i = 0; i < col.Length; i++)
            {
                string[] row = col[i].Split(',');
                
            }
        }
        return dialogue.ToArray();
    }


}
