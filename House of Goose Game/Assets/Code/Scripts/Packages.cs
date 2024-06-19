using UnityEngine;
using static HoG;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TripPackages", menuName = "Scriptable Objects/Trip Packages")]

public class TripPackages : ScriptableObject
{
    [SerializeField]
    string _packageName;

    [SerializeField]
    HoG.Locations _location;

    [SerializeField]
    double _averagePrice;

    [SerializeField]
    List<HoG.Attributes> _tags;

    [SerializeField]
    List<string> _activities;

    public string PackageName { get => _packageName; set => _packageName = value; }
    public Locations Location { get => _location; set => _location = value; }
    public double AveragePrice { get => _averagePrice; set => _averagePrice = value; }
    public List<Attributes> Tags { get => _tags; set => _tags = value; }
    public List<string> Activities { get => _activities; set => _activities = value; }
}
