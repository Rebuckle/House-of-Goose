public class EventsComputer : ObjectEventSubscriber
{
    protected override void OnGameStart(object gameManager, GameStartArgs gsa)
    {
        base.OnGameStart(gameManager, gsa);
        gsa.computer = transform;
        gsa.player.computer = transform;
    }
}
