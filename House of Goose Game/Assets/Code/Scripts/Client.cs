using UnityEngine;
using static HoG;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Client", menuName = "Scriptable Objects/Client")]
public class Client : ScriptableObject
{
    [SerializeField]
    string _clientName;
    [SerializeField]
    [TextArea (3,10)]
    string _clientStory;
    [SerializeField]
    Sprite _clientImage;
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
    HoG.Attributes[] _preferredLocationAttributes;
    [SerializeField]
    HoG.Attributes[] _unpreferredLocationAttributes;

    [SerializeField]
    List<Seasons> _knownPreferredSeasons;
    [SerializeField]
    List<Seasons> _knownUnpreferredSeasons;
    [SerializeField]
    List<HoG.Attributes> _knownPreferredLocationAttributes;
    [SerializeField]
    List<Attributes> _knownUnpreferredLocationAttributes;

    [Header("Dining")]
    [SerializeField]
    bool _optOutDining;
    //[SerializeField]
    //bool _fullDining;
    [SerializeField]
    [Range(1, 60)]
    int _diningAmount;
    [SerializeField]
    [Range(0, 3)]
    int[] _diningQuality; //How many $ is the dining worth from 0 (free) to 3 ($$$)

    [SerializeField]
    [Range(0, 3)]
    List<int> _knownDiningQualityPreference; //How many $ is the dining worth from 0 (free) to 3 ($$$)

    [Header("Transit")]
    [SerializeField]
    bool _optOutTransit;
    [SerializeField]
    AirplaneClass _airplaneClass;
    [SerializeField]
    HoG.Transit[] _preferredTransit;
    [SerializeField]
    HoG.Transit[] _unpreferredTransit;

    [SerializeField]
    List <HoG.Transit> _knownPreferredTransit;
    [SerializeField]
    List <HoG.Transit> _knownUnpreferredTransit;
    [SerializeField]
    List<AirplaneClass> _knownPreferredAirplaneClass;
    [SerializeField]
    List<AirplaneClass> _knownUnpreferredAirplaneClass;

    [Header("Lodging")]
    [SerializeField]
    [Range(0, 5)]
    int[] _lodgingQuality; //How many $ is the lodging worth from 0 (free) to 5 ($$$$$)

    [SerializeField]
    [Range(0, 5)]
    List<int> _knownLodgingQuality; //How many $ is the lodging worth from 0 (free) to 5 ($$$$$)

    [Header("Activity")]
    [SerializeField]
    bool _optOutActivity;
    [SerializeField]
    [Range(1,60)]
    int _activityAmount;
    [SerializeField]
    ActivityTags[] _likedActivityTags;
    [SerializeField]
    ActivityTags[] _dislikedActivityTags;

    [SerializeField]
    List<ActivityTags> _knownLikedActivityTags;
    [SerializeField]
    List<ActivityTags> _knownDislikedActivityTags;

    [Header("Dialogue")]
    [SerializeField]
    DialogueTree _mainDialogue;
    [SerializeField]
    DialogueTree _repeatDialogue;

    public string ClientName { get => _clientName; set => _clientName = value; }
    public string ClientStory { get => _clientStory; set => _clientStory = value; }
    public double Budget { get => _budget; set => _budget = value; }
    public int TripDuration { get => _tripDuration; set => _tripDuration = value; }
    public int GroupSize { get => _groupSize; set => _groupSize = value; }
    public int CallLimit { get => _callLimit; set => _callLimit = value; }
    public Regions StartingRegion { get => _startingRegion; set => _startingRegion = value; }
    public Seasons[] PreferredSeasons { get => _preferredSeasons; set => _preferredSeasons = value; }
    public bool OptOutDining { get => _optOutDining; set => _optOutDining = value; }
    //public bool FullDining { get => _fullDining; set => _fullDining = value; }
    public int[] DiningQuality { get => _diningQuality; set => _diningQuality = value; }
    public bool OptOutTransit { get => _optOutTransit; set => _optOutTransit = value; }
    public HoG.Transit[] PreferredTransit { get => _preferredTransit; set => _preferredTransit = value; }
    public int[] LodgingQuality { get => _lodgingQuality; set => _lodgingQuality = value; }
    public bool OptOutActivity { get => _optOutActivity; set => _optOutActivity = value; }
    public ActivityTags[] LikedActivityTags { get => _likedActivityTags; set => _likedActivityTags = value; }
    public ActivityTags[] DislikedActivityTags { get => _dislikedActivityTags; set => _dislikedActivityTags = value; }
    public int ActivityAmount { get => _activityAmount; set => _activityAmount = value; }
    public Seasons[] UnpreferredSeasons { get => _unpreferredSeasons; set => _unpreferredSeasons = value; }
    public Attributes[] PreferredLocations { get => _preferredLocationAttributes; set => _preferredLocationAttributes = value; }
    public Attributes[] UnpreferredLocations { get => _unpreferredLocationAttributes; set => _unpreferredLocationAttributes = value; }
    public HoG.Transit[] UnpreferredTransit { get => _unpreferredTransit; set => _unpreferredTransit = value; }
    public int DiningAmount { get => _diningAmount; set => _diningAmount = value; }
    public Sprite ClientImage { get => _clientImage; set => _clientImage = value; }

    public List<Seasons> KnownPreferredSeasons { get => _knownPreferredSeasons; set => _knownPreferredSeasons = value; }
    public List<Seasons> KnownUnpreferredSeasons { get => _knownUnpreferredSeasons; set => _knownUnpreferredSeasons = value; }
    public List<Attributes> KnownPreferredLocationAttributes { get => _knownPreferredLocationAttributes; set => _knownPreferredLocationAttributes = value; }
    public List<Attributes> KnownUnpreferredLocationAttributes { get => _knownUnpreferredLocationAttributes; set => _knownUnpreferredLocationAttributes = value; }
    public List<HoG.Transit> KnownPreferredTransit { get => _knownPreferredTransit; set => _knownPreferredTransit = value; }
    public List<HoG.Transit> KnownUnpreferredTransit { get => _knownUnpreferredTransit; set => _knownUnpreferredTransit = value; }
    public List<ActivityTags> KnownLikedActivityTags { get => _knownLikedActivityTags; set => _knownLikedActivityTags = value; }
    public List<ActivityTags> KnownDislikedActivityTags { get => _knownDislikedActivityTags; set => _knownDislikedActivityTags = value; }
    public List<int> KnownDiningQualityPreference { get => _knownDiningQualityPreference; set => _knownDiningQualityPreference = value; }
    public List<int> KnownLodgingQuality { get => _knownLodgingQuality; set => _knownLodgingQuality = value; }
    public List<AirplaneClass> KnownPreferredAirplaneClass { get => _knownPreferredAirplaneClass; set => _knownPreferredAirplaneClass = value; }
    public List<AirplaneClass> KnownUnpreferredAirplaneClass { get => _knownUnpreferredAirplaneClass; set => _knownUnpreferredAirplaneClass = value; }
}
