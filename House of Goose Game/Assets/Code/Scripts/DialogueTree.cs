using UnityEngine;

[CreateAssetMenu(fileName = "DialogueTree", menuName = "Scriptable Objects/DialogueTree")]
public class DialogueTree : ScriptableObject
{
    
    [SerializeField] public DialogueSection[] sections;

}
[System.Serializable]
public struct DialogueSection
{
    
    public DialogueLine[] dialogue;
    public bool endAfterDialogue;
    public BranchPoint branchPoint;
}

[System.Serializable]
public struct DialogueLine
{
    [TextArea]
    public string line;
    public bool client;
}

[System.Serializable]
public struct BranchPoint
{
    [TextArea]
    public string question;
    public Answer[] answers;
}

[System.Serializable]
public struct Answer
{
    public string answerLabel;
    public int nextElement;
}