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
        if(TheClient == null)
        {
            TheClient = GameObject.Find("Game Controller").GetComponent<GameManager>().currentClient;
        }

        _budgetText.text = TheClient.Budget.ToString();
        _tripCostText.text = "$0000";

        SetDestinationList();

        _destination = HoG.Locations.Edinburgh.ToString();

        SetSeasonList();
        SetAirplaneClassList();
        SetPackagesList();
    }

    // Setters
    
    public void SetDestinationList()
    {
        List<string> _allLocs = new List<string>();

        foreach (HoG.Locations _location in System.Enum.GetValues(typeof(HoG.Locations)))
        {
            _allLocs.Add(_location.ToString());
        }

        _destinationDropdown.AddOptions(_allLocs);
    }

    public void SetSeasonList()
    {
        List<string> _allSeas = new List<string>();

        foreach (HoG.Seasons _sea in System.Enum.GetValues(typeof(HoG.Seasons)))
        {
            _allSeas.Add(_sea.ToString());
        }

        _seasonDropdown.AddOptions(_allSeas);
    }

    public void SetAirplaneClassList()
    {
        List<string> _allAirplaneClasses = new List<string>();

        foreach (HoG.AirplaneClass _class in System.Enum.GetValues(typeof(HoG.AirplaneClass)))
        {
            _allAirplaneClasses.Add(_class.ToString());
        }

        _airplaneClassDropdown.AddOptions(_allAirplaneClasses);
    }

    public void SetPackagesList()
    {
        List<string> _allPacks = new List<string>();

        foreach (TripPackages _package in GetLocation().Packages)
        {
            foreach(string _packageName in _package.Activities)
            {
                _allPacks.Add(_packageName);
            }
        }

        _packagesDropdown.AddOptions(_allPacks);
    }



    // Updaters

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

    #region Day Plans

    [SerializeField]
    GameObject _dayPlanButtonsParent;
    [SerializeField]
    GameObject _numberedDayPrefab;
    [SerializeField]
    GameObject _newDayPlusButton;

    [SerializeField]
    GameObject _experiencePanelParent;
    [SerializeField]
    GameObject _experiencePanelPrefab;
    [SerializeField]
    List<GameObject> _dayPlans;

    [SerializeField]
    GameObject _activitiesButtonsParent;
    [SerializeField]
    GameObject _numberedActivityPrefab;
    [SerializeField]
    GameObject _newActivityPlusButton;

    [SerializeField]
    GameObject _activitiesParent;
    [SerializeField]
    GameObject _activitiesDropdownPrefab;

    public void AddNewDayPlan()
    {
        GameObject _newDayPlan = Instantiate(_experiencePanelPrefab, _experiencePanelParent.transform);
        GameObject _newNumberedButton = Instantiate(_numberedDayPrefab, _dayPlanButtonsParent.transform);

        _newDayPlusButton.gameObject.transform.SetAsLastSibling();
        _newNumberedButton.GetComponentInChildren<TextMeshProUGUI>().text = _newDayPlusButton.gameObject.transform.GetSiblingIndex() + "";
        _dayPlans.Add(_newDayPlan);
        _newDayPlan.SetActive(false);

        _newDayPlan.gameObject.transform.Find("NewActivityButton-Itinerary");
    }

    public void GoToDay(int index)
    {
        foreach(GameObject _dayPlan in _dayPlans)
        {
            _dayPlan.SetActive(false);
        }
        _dayPlans[index].SetActive(true);
    }

    public void AddNewActivityDropdown()
    {
        GameObject _newActivity = Instantiate(_activitiesDropdownPrefab, _activitiesParent.transform);
        GameObject _newNumberedButton = Instantiate(_numberedActivityPrefab, _activitiesButtonsParent.transform);

        _newActivityPlusButton.gameObject.transform.SetAsLastSibling();
        _newNumberedButton.GetComponentInChildren<TextMeshProUGUI>().text = _newActivityPlusButton.gameObject.transform.GetSiblingIndex() + 1 + "";
    }

    #endregion

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
