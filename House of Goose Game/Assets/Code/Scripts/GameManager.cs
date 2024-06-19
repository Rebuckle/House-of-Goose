using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : HoG
{
    public static GameManager gm;

    public Client currentClient;

    //Feature Behaviors
    public NotebookBehavior notebookBehavior;
    public LocationBehavior locationBehavior;

    public DialogueManager dialogueManager;

    private void Awake()
    {
        if(gm != null && gm != this)
        {
            Destroy(this);
        } else
        {
            gm = this;
        }

        SceneManager.LoadScene("OfficeAssets", LoadSceneMode.Additive);


    }//end Awake


}
