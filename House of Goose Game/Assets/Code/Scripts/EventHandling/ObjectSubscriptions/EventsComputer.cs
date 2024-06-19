using System;
using UnityEngine;

public class EventsComputer : ObjectEventSubscriber
{
    protected override void OnGameStart(object gameManager, GameStartArgs gsa)
    {
        gsa.computer = transform;
        gsa.player.computer = transform;
    }
}
