using UnityEngine;

public class EventsManilla : ObjectEventSubscriber
{
    [SerializeField] private Transform manillaOpenRootAnchor;
    [SerializeField] private Transform manillaOpenCoverAnchor;
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

        if (gsa.player.manillaOpenRootAnchor == null && manillaOpenRootAnchor != null)
            gsa.player.manillaOpenRootAnchor = manillaOpenRootAnchor;
        if (gsa.player.manillaOpenCoverAnchor == null && manillaOpenCoverAnchor != null)
            gsa.player.manillaOpenCoverAnchor = manillaOpenCoverAnchor;
    }
}