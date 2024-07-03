using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotebookBehavior : HoG
{
    [Header("Canvases")]
    [SerializeField]
    Canvas _leftSideCanvas;
    [SerializeField]
    Canvas _rightSideCanvas;
    [SerializeField]
    Canvas _pagePool;
    [SerializeField]
    Button _flipPageButton;

    [Header("Pages")]
    [SerializeField]
    GameObject _summaryPage; //page one
    [SerializeField]
    GameObject _preferencePage; //page two
    [SerializeField]
    GameObject _dislikesPage; //page three
    [SerializeField]
    GameObject _likesPage; //page four

    public enum NotebookStatus { Disabled, PagesOneTwo, PagesThreeFour };
    NotebookStatus _status;

    private void Awake()
    {
        _status = NotebookStatus.Disabled;
        _flipPageButton.gameObject.SetActive(false);
        FlipPages();
    }//end awake

    private void Start()
    {
        if (GameManager.gm.notebookBehavior == null)
        {
            GameManager.gm.notebookBehavior = this;
        }
        LoadSummaryPage();
        LoadPreferencePage();

        LoadAvailableLocations();
        LoadAvailableSeasons();
        LoadAvailableDining();
        LoadAvailableLodging();
        LoadAvailableTransit();
        LoadAvailableActivities();
        LoadAvailableAirplaneClass();
    }//end start

    public void FlipPages()
    {
        if(_status == NotebookStatus.PagesOneTwo)
        {
            _status = NotebookStatus.PagesThreeFour;
        } 
        else if (_status == NotebookStatus.PagesThreeFour)
        {
            _status = NotebookStatus.PagesOneTwo;
        }

        switch (_status)
        {
            case NotebookStatus.Disabled:
                {
                    _summaryPage.transform.SetParent(_pagePool.gameObject.transform, false);
                    _preferencePage.transform.SetParent(_pagePool.gameObject.transform, false);
                    _dislikesPage.transform.SetParent(_pagePool.gameObject.transform, false);
                    _likesPage.transform.SetParent(_pagePool.gameObject.transform, false);
                    break;
                }
            case NotebookStatus.PagesOneTwo:
                {
                    _summaryPage.transform.SetParent(_leftSideCanvas.gameObject.transform, false);
                    _preferencePage.transform.SetParent(_rightSideCanvas.gameObject.transform, false);
                    _dislikesPage.transform.SetParent(_pagePool.gameObject.transform, false);
                    _likesPage.transform.SetParent(_pagePool.gameObject.transform, false);
                    break;
                }
            case NotebookStatus.PagesThreeFour:
                {
                    _summaryPage.transform.SetParent(_pagePool.gameObject.transform, false);
                    _preferencePage.transform.SetParent(_pagePool.gameObject.transform, false);
                    _likesPage.transform.SetParent(_leftSideCanvas.gameObject.transform, false);
                    _dislikesPage.transform.SetParent(_rightSideCanvas.gameObject.transform, false);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }//end flip pages

    public void ToggleNotebook()
    {
        if(_status != NotebookStatus.Disabled)
        {
            _status = NotebookStatus.Disabled;
            _flipPageButton.gameObject.SetActive(false);
        } else
        {
            _status = NotebookStatus.PagesThreeFour;
            _flipPageButton.gameObject.SetActive(true);
        }

        FlipPages();
    }//end Toggle Notebook

    #region Summary Page

    [Header("Summary Page")]
    [SerializeField]
    Image _clientImage;
    [SerializeField]
    TextMeshProUGUI _clientName;
    [SerializeField]
    TextMeshProUGUI _clientStory;

    public void LoadSummaryPage()
    {
        _clientName.text = GameManager.gm.currentClient.ClientName;
        _clientImage.sprite = GameManager.gm.currentClient.ClientImage;
        _clientStory.text = GameManager.gm.currentClient.ClientStory;
    }//end load summary page

    #endregion

    #region Preferences Page

    [Header("Preferences Page")]
    [SerializeField]
    TMP_Dropdown _startingRegion;

    public void LoadPreferencePage()
    {
        List<string> _regions = new List<string>();

        foreach(Regions _region in System.Enum.GetValues(typeof(HoG.Regions)))
        {
            _regions.Add(_region.ToString());
        }

        _startingRegion.AddOptions(_regions);
    }//end load preference page

    #endregion

    #region Likes and Dislikes Pages

    #region Locations

    [Header("Locations")]
    [SerializeField]
    TMP_Dropdown _likedLocationDropdown;
    [SerializeField]
    TMP_Dropdown _dislikedLocationDropdown;

    /// <summary>
    /// Populate the Client Book with Available Locations, under both Liked and Disliked panels.
    /// </summary>
    /// <param name="currentClient"></param>
    public void LoadAvailableLocations()
    {
        List<string> likedLocations = new List<string>();
        List<string> dislikedLocations = new List<string>();

        foreach (Attributes _location in System.Enum.GetValues(typeof(HoG.Attributes)))
        {
            likedLocations.Add(_location.ToString());
            dislikedLocations.Add(_location.ToString());
        }

        _likedLocationDropdown.AddOptions(likedLocations);
        _dislikedLocationDropdown.AddOptions(dislikedLocations);
    }//end load available locations


    #endregion

    #region Seasons

    [Header("Seasons")]
    [SerializeField]
    TMP_Dropdown _likedSeasonDropdown;
    [SerializeField]
    TMP_Dropdown _dislikedSeasonDropdown;

    public void LoadAvailableSeasons()
    {
        List<string> likedSeasons = new List<string>();
        List<string> dislikedSeasons = new List<string>();

        foreach (Seasons _season in System.Enum.GetValues(typeof(HoG.Seasons)))
        {
            likedSeasons.Add(_season.ToString());
            dislikedSeasons.Add(_season.ToString());
        }

        _likedSeasonDropdown.AddOptions(likedSeasons);
        _dislikedSeasonDropdown.AddOptions(dislikedSeasons);
    }//end load available seasons

    #endregion

    #region Dining

    [Header("Dining")]
    [SerializeField]
    TMP_Dropdown _likedDiningDropdown;
    [SerializeField]
    TMP_Dropdown _dislikedDiningDropdown;

    public void LoadAvailableDining()
    {
        List<string> likedDining = new List<string>();
        List<string> dislikedDining = new List<string>();

        foreach (DiningQuality _dining in System.Enum.GetValues(typeof(HoG.DiningQuality)))
        {
            likedDining.Add(_dining.ToString());
            dislikedDining.Add(_dining.ToString());
        }

        _likedDiningDropdown.AddOptions(likedDining);
        _dislikedDiningDropdown.AddOptions(dislikedDining);
    }//end load available dining

    #endregion

    #region Lodging

    [Header("Lodging")]
    [SerializeField]
    TMP_Dropdown _likedLodgingDropdown;
    [SerializeField]
    TMP_Dropdown _dislikedLodgingDropdown;

    public void LoadAvailableLodging()
    {
        List<string> likedLodging = new List<string>();
        List<string> dislikedLodging = new List<string>();

        foreach (LodgingQuality _lodging in System.Enum.GetValues(typeof(HoG.LodgingQuality)))
        {
            likedLodging.Add(_lodging.ToString());
            dislikedLodging.Add(_lodging.ToString());
        }

        _likedLodgingDropdown.AddOptions(likedLodging);
        _dislikedLodgingDropdown.AddOptions(dislikedLodging);
    }//end load available lodging

    #endregion

    #region Activities

    [Header("Activities")]
    [SerializeField]
    TMP_Dropdown _likedActivityDropdown;
    [SerializeField]
    TMP_Dropdown _dislikedActivityDropdown;

    public void LoadAvailableActivities()
    {
        List<string> likedActivity = new List<string>();
        List<string> dislikedActivity = new List<string>();

        foreach (ActivityTags _activity in System.Enum.GetValues(typeof(HoG.ActivityTags)))
        {
            likedActivity.Add(_activity.ToString());
            dislikedActivity.Add(_activity.ToString());
        }

        _likedActivityDropdown.AddOptions(likedActivity);
        _dislikedActivityDropdown.AddOptions(dislikedActivity);
    }//end load available locations

    #endregion

    #region Transit

    [Header("Transit")]
    [SerializeField]
    TMP_Dropdown _likedTransitDropdown;
    [SerializeField]
    TMP_Dropdown _dislikedTransitDropdown;

    public void LoadAvailableTransit()
    {
        List<string> likedTransit = new List<string>();
        List<string> dislikedTransit = new List<string>();

        foreach (Transit _transit in System.Enum.GetValues(typeof(HoG.Transit)))
        {
            likedTransit.Add(_transit.ToString());
            dislikedTransit.Add(_transit.ToString());
        }

        _likedTransitDropdown.AddOptions(likedTransit);
        _dislikedTransitDropdown.AddOptions(dislikedTransit);
    }//end load available transit

    #endregion

    #region Airplane Class


    [Header("Airplane Class")]
    [SerializeField]
    TMP_Dropdown _likedAirplaneClassDropdown;
    [SerializeField]
    TMP_Dropdown _dislikedAirplaneClassDropdown;

    public void LoadAvailableAirplaneClass()
    {
        List<string> likedAirplaneClass = new List<string>();
        List<string> dislikedAirplaneClass = new List<string>();

        foreach (AirplaneClass _airplaneClass in System.Enum.GetValues(typeof(HoG.AirplaneClass)))
        {
            likedAirplaneClass.Add(_airplaneClass.ToString());
            dislikedAirplaneClass.Add(_airplaneClass.ToString());
        }

        _likedAirplaneClassDropdown.AddOptions(likedAirplaneClass);
        _dislikedAirplaneClassDropdown.AddOptions(dislikedAirplaneClass);
    }//end load available Airplane Class

    #endregion

    #endregion
}
