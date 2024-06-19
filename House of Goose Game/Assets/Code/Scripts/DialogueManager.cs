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
    private void Start()
    {
        dialogueBox.SetActive(false);
        answerBox.SetActive(false);
    }
    public void StartDialogue(DialogueTree dialogueTree)
    {
        StartCoroutine(RunDialogue(dialogueTree, 0));
    }
    
    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        Debug.Log("Starting section " + section);   
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {
            if (dialogueTree.sections[section].dialogue[i].line == "") break;
            dialogueBox.SetActive(true);

            string line = dialogueTree.sections[section].dialogue[i].line;




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
                if (skipLineTriggered)
                {
                    skipLineTriggered = false;
                    dialogueText.text = line;
                    break;
                }
                yield return new WaitForSeconds(1 / charactersPerSecond);
            }

            while (skipLineTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
        }
        
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            Debug.Log("Ending dialogue");
            OnDialogueEnded?.Invoke();
            dialogueBox.SetActive(false);
            answerBox.SetActive(false);
            yield break;
        }

        //dialogueText.text = dialogueTree.sections[section].branchPoint.question;
        StartCoroutine(ShowAnswers(dialogueTree.sections[section].branchPoint));

        while (answerTriggered == false)
        {
            yield return null;
        }
        //answerBox.SetActive(false);
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

    IEnumerator ShowAnswers(BranchPoint branchPoint)
    {       
            skipLineTriggered = false;
            dialogueBox.SetActive(true);
            answerBox.SetActive(true);
            foreach (Button button in answerButtons)
            {
                button.gameObject.SetActive(false);
            }
            string line = branchPoint.question;
            //Typewriter effect
            string textBuffer = null;
            foreach(char c in line)
            {
                textBuffer += c;
                dialogueText.text = textBuffer;
                if (skipLineTriggered)
                {
                    skipLineTriggered = false;
                    dialogueText.text = line;
                    break;
                }
                yield return new WaitForSeconds(1 / charactersPerSecond);
            }

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