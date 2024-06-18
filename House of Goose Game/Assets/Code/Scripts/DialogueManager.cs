using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;
using System.Collections;

public class DialogueManager : HoG
{  
    public static DialogueManager instance { get; private set; }
    
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject answerBox;
    [SerializeField] Button[] answerButtons;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;

    public float charactersPerSecond = 15;

    bool skipLineTriggered;
    bool answerTriggered;
    int answerIndex;

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

    public void StartDialogue(DialogueTree dialogueTree)
    {
        StartCoroutine(RunDialogue(dialogueTree, 0));
    }
    
    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {


            string line = dialogueTree.sections[section].dialogue[i].line;




            if (dialogueTree.sections[section].dialogue[i].client)
            {
                
                for (int k = 0; k < answerButtons.Length; k++)
                {
                    answerButtons[k].gameObject.SetActive(false);
                }

                //Typewriter effect
                string textBuffer = null;
                foreach(char c in line)
                {
                    textBuffer += c;
                    dialogueText.text = textBuffer;
                    yield return new WaitForSeconds(1 / charactersPerSecond);
                }

                if (dialogueTree.sections[section].dialogue.Length >= i-1 && dialogueTree.sections[section].dialogue[i+1].client == false)
                {
                    skipLineTriggered = true;
                }

            }
            else
            {
                answerButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = line;
                answerButtons[0].gameObject.SetActive(true);
                for (int k = 1; k < answerButtons.Length; k++)
                {
                    answerButtons[k].gameObject.SetActive(false);
                }
            }
            while (skipLineTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
        }

        if (dialogueTree.sections[section].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke();
            dialogueBox.SetActive(false);
            yield break;
        }

        dialogueText.text = dialogueTree.sections[section].branchPoint.question;
        ShowAnswers(dialogueTree.sections[section].branchPoint);

        while (answerTriggered == false)
        {
            yield return null;
        }
        answerBox.SetActive(false);
        answerTriggered = false;

        StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement));
    }

    void ResetBox()
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        answerBox.SetActive(false);
        skipLineTriggered = false;
        answerTriggered = false;
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        // Reveals the aselectable answers and sets their text values
        answerBox.SetActive(true);
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < branchPoint.answers.Length)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = branchPoint.answers[i].answerLabel;
                answerButtons[i].gameObject.SetActive(true);
            }
            else {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        answerIndex = answer;
        answerTriggered = true;
    }
}