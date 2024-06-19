using UnityEngine;

public class ObjectEventSubscriber : MonoBehaviour
{
    protected virtual void Awake()
    {
        GameManager gm = GameObject.Find("Game Controller").GetComponent<GameManager>();
        gm.OnGameStarted += OnGameStart;
    }

    protected virtual void OnGameStart(object gameManager, GameStartArgs gsa)
    {
        print($"Setting up {gameObject.name} from Event Delegate {gameManager}");
    }
}
