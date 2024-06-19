using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : HoG
{
    public static GameManager gm;

    public Client currentClient;

    public Player playerObj;

    public event EventHandler<GameStartArgs> OnGameStarted;

    private GameStartArgs gameStartArgs;

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
        gameStartArgs = new GameStartArgs() { player = playerObj };
        StartCoroutine(LoadOffice());
    }//end Awake

    private IEnumerator LoadOffice()
    {
        var asyncOffice = SceneManager.LoadSceneAsync("OfficeAssets", LoadSceneMode.Additive);
        while (!asyncOffice.isDone)
        {
            yield return null;
        }
        OnGameStarted?.Invoke(this, gameStartArgs);
    }

}
