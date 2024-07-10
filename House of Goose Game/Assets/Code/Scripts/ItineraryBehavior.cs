using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItineraryBehavior : HoG
{
    [SerializeField]
    public Client TheClient;

    [SerializeField]
    BudgetCalculation BudgetCalculator;

    [SerializeField]
    TMP_Dropdown _destinationDropdown;
    [SerializeField]
    TMP_Dropdown _seasonDropdown;
    [SerializeField]
    TMP_Dropdown _airplaneClassDropdown;
    [SerializeField]
    TMP_Dropdown _packagesDropdown;
    [SerializeField]
    TextMeshProUGUI _budgetText;
    [SerializeField]
    TextMeshProUGUI _tripCostText;

    [SerializeField] List<Location> _allLocations;

    string _destination;
    string _season;
    string _airplaneClass;
    string _packages;
    List<string> _activities;
    List<string> _packageActivities;
    List<string> _dinings;
    List<string> _transits;
    List<string> _lodgings;

    public void Start()
    {
        if(TheClient == null)
        {
            TheClient = FindFirstObjectByType<GameManager>().currentClient;
        }

        _budgetText.text = TheClient.Budget.ToString();
        _tripCostText.text = "$00.00";

        SetDestinationList();

        _destination = HoG.Locations.Edinburgh.ToString();

        SetSeasonList();
        SetAirplaneClassList();
        SetPackagesList();


        _activities = new List<string>();
        _packageActivities = new List<string>();
        _dinings = new List<string>();
        _transits = new List<string>();
        _lodgings = new List<string>();


        _dayPlans = new List<GameObject>();
        _numberedButtons = new List<GameObject>();

        LoadActivities();
        LoadDining();
        LoadTransit();
        LoadLodging();

        AddNewDayPlan();
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

    public void UpdateTripCost()
    {
        _tripCostText.text = "$"+ BudgetCalculator.CalculateTripCost();
    }//end update trip cost

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
        foreach(GameObject _dayPlan in _dayPlans)
        {
            _dayPlan.GetComponent<ExperiencePanelDropdowns>().ActivitiesDropdown.ClearOptions();
            _dayPlan.GetComponent<ExperiencePanelDropdowns>().ActivitiesDropdown.AddOptions(_activities);
            _dayPlan.GetComponent<ExperiencePanelDropdowns>().ActivitiesDropdown.AddOptions(_packageActivities);

            _dayPlan.GetComponent<ExperiencePanelDropdowns>().DiningDropdown.ClearOptions();
            _dayPlan.GetComponent<ExperiencePanelDropdowns>().DiningDropdown.AddOptions(_dinings);

            _dayPlan.GetComponent<ExperiencePanelDropdowns>().LodgingDropdown.ClearOptions();
            _dayPlan.GetComponent<ExperiencePanelDropdowns>().LodgingDropdown.AddOptions(_lodgings);

            _dayPlan.GetComponent<ExperiencePanelDropdowns>().TransitDropdown.ClearOptions();
            _dayPlan.GetComponent<ExperiencePanelDropdowns>().TransitDropdown.AddOptions(_transits);
        }
    }//end update activities

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
    List<GameObject> _dayPlans;
    List<GameObject> _numberedButtons;

    public void AddNewDayPlan()
    {
        GameObject _newDayPlan = Instantiate(_experiencePanelPrefab, _experiencePanelParent.transform);
        GameObject _newNumberedButton = Instantiate(_numberedDayPrefab, _dayPlanButtonsParent.transform);

        //Set up Numbered Buttons

        _newDayPlusButton.gameObject.transform.SetAsLastSibling();
        _numberedButtons.Add(_newNumberedButton);
        _newNumberedButton.GetComponentInChildren<TextMeshProUGUI>().text = (_numberedButtons.IndexOf(_newNumberedButton) + 1) + "";
        
        _newNumberedButton.name = "Button " + (_numberedButtons.IndexOf(_newNumberedButton) + 1);
        
        _newNumberedButton.GetComponent<Button>().onClick.AddListener(delegate { GoToDay(_numberedButtons.IndexOf(_newNumberedButton)); } );
        
        _dayPlans.Add(_newDayPlan);
        if(_dayPlans.Count > 1 == true)
        {
            _newDayPlan.SetActive(false);
        }
        _newDayPlan.name = "Day Plan " + _dayPlans.IndexOf(_newDayPlan);

        //Set up Day Plan
        _newDayPlan.GetComponent<ExperiencePanelDropdowns>().ActivitiesDropdown.AddOptions(_activities);
        _newDayPlan.GetComponent<ExperiencePanelDropdowns>().DiningDropdown.AddOptions(_dinings);
        _newDayPlan.GetComponent<ExperiencePanelDropdowns>().LodgingDropdown.AddOptions(_lodgings);
        _newDayPlan.GetComponent<ExperiencePanelDropdowns>().TransitDropdown.AddOptions(_transits);
    }//end add new day plan

    public void GoToDay(int index)
    {
        foreach (GameObject _dayPlan in _dayPlans)
        {
            _dayPlan.SetActive(false);
        }
        _dayPlans[index].SetActive(true);
    }

    //Setters

    void LoadActivities()
    {
        foreach (ActivityTags _activity in System.Enum.GetValues(typeof(HoG.ActivityTags)))
        {
            _activities.Add(_activity.ToString());
        }
    }//end load activities

    void LoadTransit()
    {
        foreach (Transit _transit in System.Enum.GetValues(typeof(HoG.Transit)))
        {
            _transits.Add(_transit.ToString());
        }
    }//end load transit

    void LoadDining()
    {
        foreach (DiningQuality _dining in System.Enum.GetValues(typeof(HoG.DiningQuality)))
        {
            _dinings.Add(_dining.ToString());
        }
    }//end load dining

    void LoadLodging()
    {
        foreach (LodgingQuality _activity in System.Enum.GetValues(typeof(HoG.LodgingQuality)))
        {
            _lodgings.Add(_activity.ToString());
        }
    }//end load activities

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
