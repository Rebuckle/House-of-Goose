using UnityEngine;
using static HoG;

[CreateAssetMenu(fileName = "Client", menuName = "Scriptable Objects/Client")]
public class Client : ScriptableObject
{
    [SerializeField]
    string _clientName;
    [SerializeField]
    [TextArea (3,10)]
    string _clientStory;
    [SerializeField]
    double _budget;
    [SerializeField]
    int _tripDuration;
    [SerializeField]
    int _groupSize;
    [SerializeField]
    int _callLimit;

    [Header("Location")]
    [SerializeField]
    Regions _startingRegion;
    [SerializeField]
    Seasons[] _preferredSeasons;
    [SerializeField]
    Seasons[] _unpreferredSeasons;
    [SerializeField]
    HoG.Locations[] _preferredLocations;
    [SerializeField]
    HoG.Locations[] _unpreferredLocations;

    [Header("Dining")]
    [SerializeField]
    bool _optOutDining;
    [SerializeField]
    bool _fullDining;
    [SerializeField]
    [Range(0, 3)]
    int[] _diningQuality; //How many $ is the dining worth from 0 (free) to 3 ($$$)

    [Header("Transit")]
    [SerializeField]
    bool _optOutTransit;
    [SerializeField]
    HoG.Transit[] _preferredTransit;
    [SerializeField]
    HoG.Transit[] _unpreferredTransit;

    [Header("Lodging")]
    [SerializeField]
    [Range(0, 5)]
    int[] _lodgingQuality; //How many $ is the lodging worth from 0 (free) to 5 ($$$$$)

    [Header("Activity")]
    [SerializeField]
    bool _optOutActivity;
    [SerializeField]
    [Range(1,10)]
    int _activityAmount;
    [SerializeField]
    ActivityTags[] _likedActivityTags;
    [SerializeField]
    ActivityTags[] _dislikedActivityTags;

    [Header("Dialogue")]
    //DialogueTree _dialogueTree;

    public string ClientName { get => _clientName; set => _clientName = value; }
    public string ClientStory { get => _clientStory; set => _clientStory = value; }
    public double Budget { get => _budget; set => _budget = value; }
    public int TripDuration { get => _tripDuration; set => _tripDuration = value; }
    public int GroupSize { get => _groupSize; set => _groupSize = value; }
    public int CallLimit { get => _callLimit; set => _callLimit = value; }
    public Regions StartingRegion { get => _startingRegion; set => _startingRegion = value; }
    public Seasons[] PreferredSeasons { get => _preferredSeasons; set => _preferredSeasons = value; }
    public bool OptOutDining { get => _optOutDining; set => _optOutDining = value; }
    public bool FullDining { get => _fullDining; set => _fullDining = value; }
    public int[] DiningQuality { get => _diningQuality; set => _diningQuality = value; }
    public bool OptOutTransit { get => _optOutTransit; set => _optOutTransit = value; }
    public HoG.Transit[] PreferredTransit { get => _preferredTransit; set => _preferredTransit = value; }
    public int[] LodgingQuality { get => _lodgingQuality; set => _lodgingQuality = value; }
    public bool OptOutActivity { get => _optOutActivity; set => _optOutActivity = value; }
    public ActivityTags[] LikedActivityTags { get => _likedActivityTags; set => _likedActivityTags = value; }
    public ActivityTags[] DislikedActivityTags { get => _dislikedActivityTags; set => _dislikedActivityTags = value; }
    public int ActivityAmount { get => _activityAmount; set => _activityAmount = value; }
    public Seasons[] UnpreferredSeasons { get => _unpreferredSeasons; set => _unpreferredSeasons = value; }
    public Locations[] PreferredLocations { get => _preferredLocations; set => _preferredLocations = value; }
    public Locations[] UnpreferredLocations { get => _unpreferredLocations; set => _unpreferredLocations = value; }
    public HoG.Transit[] UnpreferredTransit { get => _unpreferredTransit; set => _unpreferredTransit = value; }
}
