using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : HoG
{
    public static GameManager gm;

    public Client currentClient;

    [SerializeField]
    List<Client> _allClients = new List<Client>();

    public Player playerObj;

    public event EventHandler<GameStartArgs> OnGameStarted;

    private GameStartArgs gameStartArgs;

    //Feature Behaviors
    public NotebookBehavior notebookBehavior;
    public LocationBehavior locationBehavior;
    public DialogueManager dialogueManager;
    public ItineraryBehavior itineraryBehavior;

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

        currentClient = _allClients[0];

        
    }//end Awake

    private IEnumerator LoadOffice()
    {
        var asyncOffice = SceneManager.LoadSceneAsync("OfficeAssets", LoadSceneMode.Additive);
        while (!asyncOffice.isDone)
        {
            Scene officeScene = SceneManager.GetSceneByName("OfficeAssets");

            GameObject[] officeAssets = officeScene.GetRootGameObjects();

            foreach (GameObject go in officeAssets)
            {
                if (go.GetComponent<ItineraryBehavior>())
                {
                    itineraryBehavior = go.GetComponent<ItineraryBehavior>();
                    itineraryBehavior.TheClient = currentClient;
                }

                if (go.GetComponent<NotebookBehavior>())
                {
                    notebookBehavior = go.GetComponent<NotebookBehavior>();
                }

                if (itineraryBehavior != null && notebookBehavior != null)
                {
                    break;
                }
            }

            yield return null;
        }
        OnGameStarted?.Invoke(this, gameStartArgs);
    }

    public void SwitchClient()
    {
        int currentIndex = _allClients.IndexOf(currentClient);
        if(_allClients.Count > currentIndex + 1)
        {
            currentClient = _allClients[currentIndex + 1];
        }
    }

}
