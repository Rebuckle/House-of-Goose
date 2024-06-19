using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ItineraryBehavior : HoG
{
    [SerializeField]
    public Client TheClient;

    [SerializeField]
    TMP_Dropdown _destinationDropdown;
    [SerializeField]
    TMP_Dropdown _seasonDropdown;
    [SerializeField]
    TMP_Dropdown _airplaneClassDropdown;
    [SerializeField]
    TMP_Dropdown _packagesDropdown;
    [SerializeField]
    List<DayPlan> _allPlans;
    [SerializeField]
    TextMeshProUGUI _budgetText;
    [SerializeField]
    TextMeshProUGUI _tripCostText;

    [SerializeField] List<Location> _allLocations;

    [System.Serializable]
    public class DayPlan
    {
        public List<TMP_Dropdown> _activities;
        public TMP_Dropdown _transit;
        public TMP_Dropdown _lodging;
        public TMP_Dropdown _dining;
    }

    string _destination;
    string _season;
    string _airplaneClass;
    string _packages;
    List<string> _activities;
    List<string> _packageActivities;

    public void Start()
    {
        _budgetText.text = TheClient.Budget.ToString();
        _tripCostText.text = "$0000";
    }

    public void UpdateDestination(TMP_Dropdown change)
    {
        _destination = change.itemText.ToString();
    }

    public void UpdateSeason(TMP_Dropdown change)
    {
        _season = change.itemText.ToString();
    }

    public void UpdateAirplaneClass(TMP_Dropdown change)
    {
        _airplaneClass = change.itemText.ToString();
    }

    public void UpdatePackages(TMP_Dropdown change)
    {
        _packages = change.itemText.ToString();
    }

    public void UpdatePackageActivities(TMP_Dropdown change)
    {
        foreach(string packAct in _packageActivities)
        {
            _activities.Remove(packAct);
        }
        _packageActivities.Clear();

        foreach(TripPackages _tripPackage in GetLocation().Packages)
        {
            if(change.itemText.ToString() == _tripPackage.PackageName)
            {
                foreach(string _activities in _tripPackage.Activities)
                {

                    _packageActivities.Add(_activities);
                    UpdateActivities(change);
                }
            }
        }
    }

    public void UpdateActivities(TMP_Dropdown change)
    {
        _activities.Clear();
        foreach(DayPlan dayPlan in _allPlans)
        {
            foreach(TMP_Dropdown _dropdown in dayPlan._activities)
            {
                _activities.Add(_dropdown.value.ToString());
            }
        }
    }

    //Getters

    public Location GetLocation()
    {
        foreach(Location _location in _allLocations)
        {
            if(_location.Name.ToString() == _destination)
            {
                return _location;
            }
        }
        return null;
    }

    public string GetSeason()
    {
        return _season;
    }

    public string GetAirplaneClass()
    {
        return _airplaneClass;
    }

    public string GetDestination()
    {
        return _destination;
    }

    public string GetPackage()
    {
        return _packages;
    }

    public string[] GetActivities()
    {
        return _activities.ToArray();
    }

    public string[] GetPackageActivities()
    {
        return _packageActivities.ToArray();
    }

}
