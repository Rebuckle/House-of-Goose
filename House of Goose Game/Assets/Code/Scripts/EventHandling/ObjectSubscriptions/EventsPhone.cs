using System;
using UnityEngine;

public class EventsPhone : ObjectEventSubscriber
{
    [SerializeField] private Transform pBase;
    [SerializeField] private Transform receiver;
    [SerializeField] private Transform pAnchorBase;
    [SerializeField] private Transform pAnchorCall;

    protected override void OnGameStart(object gameManager, GameStartArgs gsa)
    {
        gsa.phone = transform;
        gsa.player.phoneBase = pBase;
        gsa.player.phoneReciever = receiver;
        gsa.player.phoneAnchorBase = pAnchorBase;
        gsa.player.phoneAnchorCall = pAnchorCall;
    }
}
