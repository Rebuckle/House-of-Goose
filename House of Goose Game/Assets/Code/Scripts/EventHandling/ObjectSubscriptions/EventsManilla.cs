public class EventsManilla : ObjectEventSubscriber
{
    protected override void OnGameStart(object gameManager, GameStartArgs gsa)
    {
        base.OnGameStart(gameManager, gsa);
        if (gsa.manillas == null) {
            gsa.manillas = new();
        }

        if(!gsa.manillas.Contains(transform))
        {
            gsa.manillas.Add(transform);
        }
    }
}