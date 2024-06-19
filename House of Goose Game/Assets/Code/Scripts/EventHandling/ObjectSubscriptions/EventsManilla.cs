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
        Renderer imgmat = locationImageGO.GetComponent<Renderer>();
        imgmat.material.SetTexture("_MainTex", locationImageImage);
        Renderer datamat = locationDataGO.GetComponent<Renderer>();
        datamat.materials[1].SetTexture("_MainTex", locationDataImage);

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