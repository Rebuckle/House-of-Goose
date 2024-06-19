using UnityEngine;

public class EventsManilla : ObjectEventSubscriber
{
    [SerializeField] private Texture locationImageImage;
    [SerializeField] private Texture locationDataImage;
    [SerializeField] private GameObject locationImageGO;
    [SerializeField] private GameObject locationDataGO;

    protected override void Awake()
    {
        base.Awake();
        Material imgmat = locationImageGO.GetComponent<Material>();
        imgmat.SetTexture("Base", locationImageImage);
        Material datamat = locationDataGO.GetComponent<Material>();
        datamat.SetTexture("Base", locationDataImage);

    }

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