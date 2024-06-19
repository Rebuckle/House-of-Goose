using System;
using UnityEngine;

public class EventsNotebook : ObjectEventSubscriber
{
    [SerializeField] private Transform nbAnchorCall;
    [SerializeField] private Transform nbAnchorDesk;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnGameStart(object gameManager, GameStartArgs gsa)
    {
        gsa.notebook = transform;
        gsa.player.notebook = transform;
        gsa.player.notebookAnchorCall = nbAnchorCall;
        gsa.player.notebookAnchorDesk = nbAnchorDesk;
    }
}
