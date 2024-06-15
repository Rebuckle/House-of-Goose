using System.Collections.Generic;
using UnityEngine;
using static HoG;

[CreateAssetMenu(fileName = "Location", menuName = "Scriptable Objects/Location")]
public class Location : ScriptableObject
{
    [SerializeField]
    Locations _name;
    [SerializeField]
    bool _unlocked;
    [SerializeField]
    Seasons _peakSeason;
    [SerializeField]
    Seasons _downSeason;
    [SerializeField]
    Regions _region;
    [SerializeField]
    Attributes[] _attributes;

    [SerializeField]
    List<Lodging> _lodging;

    [SerializeField]
    List<Dining> _dining;

    [SerializeField]
    List<Transit> _transit;

    [SerializeField]
    List<Activity> _activities;

    public Locations Name { get => _name; set => _name = value; }
    public bool Unlocked { get => _unlocked; set => _unlocked = value; }
    public Seasons PeakSeason { get => _peakSeason; set => _peakSeason = value; }
    public Seasons DownSeason { get => _downSeason; set => _downSeason = value; }
    public Regions Region { get => _region; set => _region = value; }
    public Attributes[] Attributes { get => _attributes; set => _attributes = value; }

    public Location(Locations _newName, Attributes[] _attributes)
    {
        Name = _newName;
        Attributes = _attributes;
    }

}
