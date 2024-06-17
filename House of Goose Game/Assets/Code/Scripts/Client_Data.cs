using UnityEngine;

public class Client_Data : HoG
{
    public Client clients;

    public string ClientName { get => ClientName; set => ClientName = value; }
    public string ClientStory { get => ClientStory; set => ClientStory = value; }
    public double Budget { get => Budget; set => Budget = value; }
    public int TripDuration { get => TripDuration; set => TripDuration = value; }
    public int GroupSize { get => GroupSize; set => GroupSize = value; }
    public int CallLimit { get => CallLimit; set => CallLimit = value; }
    public Regions StartingRegion { get => StartingRegion; set => StartingRegion = value; }
    public Seasons[] PreferredSeasons { get => PreferredSeasons; set => PreferredSeasons = value; }
    public bool OptOutDining { get => OptOutDining; set => OptOutDining = value; }
    public int[] DiningQuality { get => DiningQuality; set => DiningQuality = value; }
    public bool OptOutTransit { get => OptOutTransit; set => OptOutTransit = value; }
    public HoG.Transit[] PreferredTransit { get => PreferredTransit; set => PreferredTransit = value; }
    public int[] LodgingQuality { get => LodgingQuality; set => LodgingQuality = value; }
    public bool OptOutActivity { get => OptOutActivity; set => OptOutActivity = value; }
    public ActivityTags[] LikedActivityTags { get => LikedActivityTags; set => LikedActivityTags = value; }
    public ActivityTags[] DislikedActivityTags { get => DislikedActivityTags; set => DislikedActivityTags = value; }
    public int ActivityAmount { get => ActivityAmount; set => ActivityAmount = value; }
    public Seasons[] UnpreferredSeasons { get => UnpreferredSeasons; set => UnpreferredSeasons = value; }
    public Attributes[] PreferredLocations { get => PreferredLocations; set => PreferredLocations = value; }
    public Attributes[] UnpreferredLocations { get => UnpreferredLocations; set => UnpreferredLocations = value; }
    public HoG.Transit[] UnpreferredTransit { get => UnpreferredTransit; set => UnpreferredTransit = value; }
    public int DiningAmount { get => DiningAmount; set => DiningAmount = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string ClientName = clients.ClientName;
        string ClientStory = clients.ClientStory;
        double Budget = clients.Budget;
        int TripDuration = clients.TripDuration;
        int GroupSize = clients.GroupSize;
        int CallLimit = clients.CallLimit;
        int DiningAmount = clients.DiningAmount;
        int ActivityAmount = clients.ActivityAmount;
        bool OptOutDining = clients.OptOutDining;
        bool OptOutTransit = clients.OptOutTransit;
        bool OptOutActivity = clients.OptOutActivity;
        int[] DiningQuality = clients.DiningQuality;
        int[] LodgingQuality = clients.LodgingQuality;

        HoG.Transit[] PreferredTransit = clients.PreferredTransit;
        HoG.Transit[] UnpreferredTransit = clients.UnpreferredTransit;
        Regions StartingRegion = clients.StartingRegion;
        Seasons[] PreferredSeasons = clients.PreferredSeasons;
        Seasons[] UnpreferredSeasons = clients.UnpreferredSeasons;
        ActivityTags[] LikedActivityTags = clients.LikedActivityTags;
        ActivityTags[] DislikedActivityTags = clients.DislikedActivityTags;
        Attributes[] PreferredLocations = clients.PreferredLocations;
        Attributes[] UnpreferredLocations = clients.UnpreferredLocations;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
