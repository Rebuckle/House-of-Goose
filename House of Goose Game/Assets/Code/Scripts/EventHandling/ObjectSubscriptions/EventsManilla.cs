using System;
using System.Linq;
using UnityEngine;

public class EventsManilla : ObjectEventSubscriber
{
    protected override void OnGameStart(object gameManager, GameStartArgs gsa)
    {
        gsa.manillas.Add(transform);
    }
}
