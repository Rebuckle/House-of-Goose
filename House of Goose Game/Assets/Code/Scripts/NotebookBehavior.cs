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

        LoadAvailableLocations(GameManager.gm.currentClient);
        LoadAvailableSeasons(GameManager.gm.currentClient);
        LoadAvailableDining(GameManager.gm.currentClient);
        LoadAvailableLodging(GameManager.gm.currentClient);
        LoadAvailableTransit(GameManager.gm.currentClient);
        LoadAvailableActivities(GameManager.gm.currentClient);
        LoadAvailableAirplaneClass(GameManager.gm.currentClient);
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
                    _summaryPage.transform.parent = _pagePool.gameObject.transform;
                    _preferencePage.transform.parent = _pagePool.gameObject.transform;
                    _dislikesPage.transform.parent = _pagePool.gameObject.transform;
                    _likesPage.transform.parent = _pagePool.gameObject.transform;
                    break;
                }
            case NotebookStatus.PagesOneTwo:
                {
                    _summaryPage.transform.parent = _leftSideCanvas.gameObject.transform;
                    _preferencePage.transform.parent = _rightSideCanvas.gameObject.transform;
                    _dislikesPage.transform.parent = _pagePool.gameObject.transform;
                    _likesPage.transform.parent = _pagePool.gameObject.transform;
                    break;
                }
            case NotebookStatus.PagesThreeFour:
                {
                    _summaryPage.transform.parent = _pagePool.gameObject.transform;
                    _preferencePage.transform.parent = _pagePool.gameObject.transform;
                    _likesPage.transform.parent = _leftSideCanvas.gameObject.transform;
                    _dislikesPage.transform.parent = _rightSideCanvas.gameObject.transform;
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
        } else
        {
            _status = NotebookStatus.PagesThreeFour;
        }

        FlipPages();
        Debug.Log(_status);
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
    TextMeshProUGUI _budget;
    [SerializeField]
    TextMeshProUGUI _groupQuantity;
    [SerializeField]
    TextMeshProUGUI _startingRegion;
    [SerializeField]
    TextMeshProUGUI _tripDuration;
    [SerializeField]
    TextMeshProUGUI _diningAmount;
    [SerializeField]
    TextMeshProUGUI _activityAmount;

    public void LoadPreferencePage()
    {
        //need to format in proper accounting format
        _budget.text = GameManager.gm.currentClient.Budget.ToString();
        _groupQuantity.text = GameManager.gm.currentClient.GroupSize.ToString();
        _startingRegion.text = GameManager.gm.currentClient.StartingRegion.ToString();
        _tripDuration.text = GameManager.gm.currentClient.TripDuration.ToString();
        _diningAmount.text = GameManager.gm.currentClient.DiningAmount.ToString();
        _activityAmount.text = GameManager.gm.currentClient.ActivityAmount.ToString();
    }//end load preference page

    #endregion

    #region Likes and Dislikes Pages

    [Header("Prefabs")]
    [SerializeField]
    GameObject _activityDropdownPrefab;
    [SerializeField]
    GameObject _knownActivityTextPrefab;

    #region Locations

    [Header("Locations")]
    [SerializeField]
    List<string> _availableLocationList; //List of location attributes that can be selected
    [SerializeField]
    GameObject _likedLocationPanel;
    [SerializeField]
    GameObject _dislikedLocationPanel;
    [SerializeField]
    Dictionary<string, int> _likedLocationDictionary;
    [SerializeField]
    Dictionary<string, int> _dislikedLocationDictionary;
    [SerializeField]
    List<GameObject> _setLikedLocationDropdowns;
    [SerializeField]
    List<GameObject> _setDislikedLocationDropdowns;
    [SerializeField]
    TMP_Dropdown _removalLikedLocationDropdown;
    [SerializeField]
    TMP_Dropdown _removalDislikedLocationDropdown;


    /// <summary>
    /// Populate the Client Book with Available Locations, under both Liked and Disliked panels.
    /// </summary>
    /// <param name="currentClient"></param>
    public void LoadAvailableLocations(Client currentClient)
    {
        _likedLocationDictionary = new Dictionary<string, int>();
        _dislikedLocationDictionary = new Dictionary<string, int>();

        foreach(Attributes _location in System.Enum.GetValues(typeof(HoG.Attributes)))
        {
            if(currentClient.KnownPreferredLocationAttributes.Contains(_location)) {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedLocationPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _location.ToString();

                //Add to retrievable dictionary
                _likedLocationDictionary.Add(_location.ToString(), (int) _location);
            } 
            else if (currentClient.KnownUnpreferredLocationAttributes.Contains(_location))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedLocationPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _location.ToString();

                //Add to retrievable dictionary
                _dislikedLocationDictionary.Add(_location.ToString(), (int)_location);
            } 
            else
            {
                _availableLocationList.Add(_location.ToString());
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedLocationPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLocationList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedLocations(newDropdown.GetComponent<TMP_Dropdown>());  });
        _removalLikedLocationDropdown.ClearOptions();
        _setLikedLocationDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedLocationPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLocationList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedLocations(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalDislikedLocationDropdown.ClearOptions();
        _setDislikedLocationDropdowns.Add(newDropdown);
    }//end load available locations

    //Liked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection. 
    /// 1- For the selected value (string) in the _removalLikedLocationDropdown, find the game object in _setLikedLocationDropdowns whose dropdown value contains the selected value (string) in the _removalLikedLocationDropdown (GameObject _toRemove).
    /// 2- Remove the selected value from the liked dictionary list
    /// 3- Remove the selected value (string) from the _removalLikedLocationDropdown options list
    /// 4- Destroy(_toRemove);
    /// </summary>
    public void RemoveLikedLocationDropdown(TMP_Dropdown change)
    {        
        foreach(GameObject _dropdown in _setLikedLocationDropdowns)
        {
            if(_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _likedLocationDictionary.Remove(change.options[change.value].text);
                _removalLikedLocationDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveLikedLocationDropdown

    /// <summary>
    /// Add a new dropdown of location attributes to Liked Locations Panel.
    /// </summary>
    public void AddLikedLocationDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedLocationPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLocationList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedLocations(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setLikedLocationDropdowns.Add(newDropdown);
    }//end AddLikedLocationDropdown

    /// <summary>
    /// Update the dictionary of liked locations by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedLocations(TMP_Dropdown change)
    {

        switch (change.options[change.value].text)
        {
            case "old":
                {
                    if (!_likedLocationDictionary.ContainsKey("old"))
                    {
                        _likedLocationDictionary.Add(Attributes.old.ToString(), (int)Attributes.old);
                        _removalLikedLocationDropdown.AddOptions(_likedLocationDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "modern":
                {
                    if(!_likedLocationDictionary.ContainsKey("modern"))
                    {
                        _likedLocationDictionary.Add(Attributes.modern.ToString(), (int)Attributes.modern);
                        _removalLikedLocationDropdown.AddOptions(_likedLocationDictionary.Keys.ToList());
                    }
                    break;
                }
            case "open":
                {
                    if (!_likedLocationDictionary.ContainsKey("open"))
                    {
                        _likedLocationDictionary.Add(Attributes.open.ToString(), (int)Attributes.open);
                        _removalLikedLocationDropdown.AddOptions(_likedLocationDictionary.Keys.ToList());
                    }
                    break;
                }
            case "beach":
                {
                    if (!_likedLocationDictionary.ContainsKey("beach"))
                    {
                        _likedLocationDictionary.Add(Attributes.beach.ToString(), (int)Attributes.beach);
                        _removalLikedLocationDropdown.AddOptions(_likedLocationDictionary.Keys.ToList());
                    }
                    break;
                }
            case "dense":
                {
                    if (!_likedLocationDictionary.ContainsKey("dense"))
                    {
                        _likedLocationDictionary.Add(Attributes.dense.ToString(), (int)Attributes.dense);
                        _removalLikedLocationDropdown.AddOptions(_likedLocationDictionary.Keys.ToList());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Location Attribute!!");
                    break;
                }
        }//end switch

    }//end

    /// <summary>
    /// Provide a list of Liked Attributes for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<Attributes> ReturnLikedLocations()
    {
        List<Attributes> _returnList = new List<Attributes>();

        foreach (var _location in _likedLocationDictionary)
        {
            _returnList.Add((Attributes)_location.Value);
        }

        return _returnList;
    }//end ReturnLikedLocations

    //Disliked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveDislikedLocationDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setDislikedLocationDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _dislikedLocationDictionary.Remove(change.options[change.value].text);
                _removalDislikedLocationDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveDislikedLocationDropdown

    /// <summary>
    /// Add a new dropdown of location attributes to Disliked Locations Panel.
    /// </summary>
    public void AddDislikedLocationDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _dislikedLocationPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLocationList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedLocations(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setDislikedLocationDropdowns.Add(newDropdown);
    }//end AddLikedLocationDropdown

    /// <summary>
    /// Update the dictionary of disliked locations by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateDislikedLocations(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "old":
                {
                    if (!_likedLocationDictionary.ContainsKey("old"))
                    {
                        _dislikedLocationDictionary.Add(Attributes.old.ToString(), (int)Attributes.old);
                        _removalDislikedLocationDropdown.AddOptions(_dislikedLocationDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "modern":
                {
                    if (!_likedLocationDictionary.ContainsKey("modern"))
                    {
                        _dislikedLocationDictionary.Add(Attributes.modern.ToString(), (int)Attributes.modern);
                        _removalDislikedLocationDropdown.AddOptions(_dislikedLocationDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "open":
                {
                    if (!_likedLocationDictionary.ContainsKey("open"))
                    {
                        _dislikedLocationDictionary.Add(Attributes.open.ToString(), (int)Attributes.open);
                        _removalDislikedLocationDropdown.AddOptions(_dislikedLocationDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "beach":
                {
                    if (!_likedLocationDictionary.ContainsKey("beach"))
                    {
                        _dislikedLocationDictionary.Add(Attributes.beach.ToString(), (int)Attributes.beach);
                        _removalDislikedLocationDropdown.AddOptions(_dislikedLocationDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "dense":
                {
                    if (!_likedLocationDictionary.ContainsKey("dense"))
                    {
                        _dislikedLocationDictionary.Add(Attributes.dense.ToString(), (int)Attributes.dense);
                        _removalDislikedLocationDropdown.AddOptions(_dislikedLocationDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Location Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateDislikedLocations

    /// <summary>
    /// Provide a list of Disliked Attributes for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<Attributes> ReturnDislikedLocations()
    {
        List<Attributes> _returnList = new List<Attributes>();

        foreach (var _location in _dislikedLocationDictionary)
        {
            _returnList.Add((Attributes)_location.Value);
        }

        return _returnList;
    }//end ReturnDisikedLocations

    #endregion

    #region Seasons

    [Header("Seasons")]
    [SerializeField]
    List<string> _availableSeasonList;
    [SerializeField]
    GameObject _likedSeasonPanel;
    [SerializeField]
    GameObject _dislikedSeasonPanel;
    [SerializeField]
    Dictionary<string, int> _likedSeasonDictionary;
    [SerializeField]
    Dictionary<string, int> _dislikedSeasonDictionary;
    [SerializeField]
    List<GameObject> _setLikedSeasonDropdowns;
    [SerializeField]
    List<GameObject> _setDislikedSeasonDropdowns;
    [SerializeField]
    TMP_Dropdown _removalLikedSeasonDropdown;
    [SerializeField]
    TMP_Dropdown _removalDislikedSeasonDropdown;

    public void LoadAvailableSeasons(Client currentClient)
    {
        _likedSeasonDictionary = new Dictionary<string, int>();
        _dislikedSeasonDictionary = new Dictionary<string, int>();

        foreach (Seasons _season in System.Enum.GetValues(typeof(HoG.Seasons)))
        {
            if (currentClient.KnownPreferredSeasons.Contains(_season))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedSeasonPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _season.ToString();

                //Add to retrievable dictionary
                _likedSeasonDictionary.Add(_season.ToString(), (int)_season);
            }
            else if (currentClient.KnownUnpreferredSeasons.Contains(_season))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedSeasonPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _season.ToString();

                //Add to retrievable dictionary
                _dislikedSeasonDictionary.Add(_season.ToString(), (int)_season);
            }
            else
            {
                _availableSeasonList.Add(_season.ToString());
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedSeasonPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableSeasonList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedSeasons(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalLikedSeasonDropdown.ClearOptions();
        _setLikedSeasonDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedSeasonPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableSeasonList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedSeasons(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalDislikedSeasonDropdown.ClearOptions();
        _setDislikedSeasonDropdowns.Add(newDropdown);
    }//end load available seasons

    //Liked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveLikedSeasonDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setLikedSeasonDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _likedSeasonDictionary.Remove(change.options[change.value].text);
                _removalLikedSeasonDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveLikedSeasonDropdown

    /// <summary>
    /// Add a new dropdown of season attributes to Liked Seasons Panel.
    /// </summary>
    public void AddLikedSeasonDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedSeasonPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableSeasonList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedSeasons(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setLikedSeasonDropdowns.Add(newDropdown);
    }//end AddLikedSeasonDropdown

    /// <summary>
    /// Update the dictionary of liked season by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedSeasons(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Winter":
                if (!_likedSeasonDictionary.ContainsKey("Winter"))
                {
                    _likedSeasonDictionary.Add(Seasons.Winter.ToString(), (int)Seasons.Winter);
                    _removalLikedSeasonDropdown.AddOptions(_likedSeasonDictionary.Keys.ToList<string>());
                }
                break;
            case "Spring":
                {
                    if (!_likedSeasonDictionary.ContainsKey("Spring"))
                    {
                        _likedSeasonDictionary.Add(Seasons.Spring.ToString(), (int)Seasons.Spring);
                        _removalLikedSeasonDropdown.AddOptions(_likedSeasonDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Summer":
                {
                    if (!_likedSeasonDictionary.ContainsKey("Summer"))
                    {
                        _likedSeasonDictionary.Add(Seasons.Summer.ToString(), (int)Seasons.Summer);
                        _removalLikedSeasonDropdown.AddOptions(_likedSeasonDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Fall":
                {
                    if (!_likedSeasonDictionary.ContainsKey("Fall"))
                    {
                        _likedSeasonDictionary.Add(Seasons.Fall.ToString(), (int)Seasons.Fall);
                        _removalLikedSeasonDropdown.AddOptions(_likedSeasonDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Season Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateLikedSeasons

    /// <summary>
    /// Provide a list of Liked Seasons for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<Seasons> ReturnLikedSeasons()
    {
        List<Seasons> _returnList = new List<Seasons>();

        foreach (var _season in _likedSeasonDictionary)
        {
            _returnList.Add((Seasons)_season.Value);
        }

        return _returnList;
    }//end ReturnLikedSeasons

    //Disliked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveDislikedSeasonDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setDislikedSeasonDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _dislikedSeasonDictionary.Remove(change.options[change.value].text);
                _removalDislikedSeasonDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveDislikedSeasonDropdown

    /// <summary>
    /// Add a new dropdown of location attributes to Disliked Seasons Panel.
    /// </summary>
    public void AddDislikedSeasonDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _dislikedSeasonPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableSeasonList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedSeasons(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setDislikedSeasonDropdowns.Add(newDropdown);
    }//end AddDislikedSeasonDropdown

    /// <summary>
    /// Update the dictionary of disliked seasons by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateDislikedSeasons(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Winter":
                if (!_dislikedSeasonDictionary.ContainsKey("Winter"))
                {
                    _dislikedSeasonDictionary.Add(Seasons.Winter.ToString(), (int)Seasons.Winter);
                    _removalDislikedSeasonDropdown.AddOptions(_dislikedSeasonDictionary.Keys.ToList<string>());
                }
                break;
            case "Spring":
                {
                    if (!_dislikedSeasonDictionary.ContainsKey("Spring"))
                    {
                        _dislikedSeasonDictionary.Add(Seasons.Spring.ToString(), (int)Seasons.Spring);
                        _removalDislikedSeasonDropdown.AddOptions(_dislikedSeasonDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Summer":
                {
                    if (!_dislikedSeasonDictionary.ContainsKey("Summer"))
                    {
                        _dislikedSeasonDictionary.Add(Seasons.Summer.ToString(), (int)Seasons.Summer);
                        _removalDislikedSeasonDropdown.AddOptions(_dislikedSeasonDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Fall":
                {
                    if (!_dislikedSeasonDictionary.ContainsKey("Fall"))
                    {
                        _dislikedSeasonDictionary.Add(Seasons.Fall.ToString(), (int)Seasons.Fall);
                        _removalDislikedSeasonDropdown.AddOptions(_dislikedSeasonDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Season Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateDislikedSeasons

    /// <summary>
    /// Provide a list of Disliked Seasons for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<Seasons> ReturnDislikedSeasons()
    {
        List<Seasons> _returnList = new List<Seasons>();

        foreach (var _season in _dislikedSeasonDictionary)
        {
            _returnList.Add((Seasons)_season.Value);
        }

        return _returnList;
    }//end ReturnDislikedSeasons

    #endregion

    #region Dining

    [Header("Dining")]

    [SerializeField]
    List<string> _availableDiningList;
    [SerializeField]
    GameObject _likedDiningPanel;
    [SerializeField]
    GameObject _dislikedDiningPanel;
    [SerializeField]
    Dictionary<string, int> _likedDiningDictionary;
    [SerializeField]
    Dictionary<string, int> _dislikedDiningDictionary;
    [SerializeField]
    List<GameObject> _setLikedDiningDropdowns;
    [SerializeField]
    List<GameObject> _setDislikedDiningDropdowns;
    [SerializeField]
    TMP_Dropdown _removalLikedDiningDropdown;
    [SerializeField]
    TMP_Dropdown _removalDislikedDiningDropdown;

    public void LoadAvailableDining(Client currentClient)
    {
        _likedSeasonDictionary = new Dictionary<string, int>();
        _dislikedSeasonDictionary = new Dictionary<string, int>();

        for (int _qualityAmount = 0; _qualityAmount <= 3; _qualityAmount++)
        {
            if (currentClient.KnownDiningQualityPreference.Contains(_qualityAmount))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedDiningPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = TransformDiningQuality(_qualityAmount);

                //Add to retrievable dictionary
                _likedDiningDictionary.Add(_qualityAmount.ToString(), (int)_qualityAmount);
            }
            else
            {
                _availableDiningList.Add(TransformDiningQuality(_qualityAmount));
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedDiningPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableDiningList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedDining(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalLikedDiningDropdown.ClearOptions();
        _setLikedDiningDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedDiningPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableDiningList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedDining(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalDislikedDiningDropdown.ClearOptions();
        _setDislikedDiningDropdowns.Add(newDropdown);
    }//end load available dining

    string TransformDiningQuality(int _quality)
    {
        switch(_quality)
        {
            case 0:
                {
                    return "Free";
                }
            case 1:
                {
                    return "$";
                }
            case 2:
                {
                    return "$$";
                }
            case 3:
                {
                    return "$$$";
                }
            default:
                {
                    return "Error";
                }
        }
    }//end TransformDiningQuality

    //Liked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection. 
    /// </summary>
    public void RemoveLikedDiningDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setLikedDiningDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _likedDiningDictionary.Remove(change.options[change.value].text);
                _removalLikedDiningDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveLikedDiningDropdown

    /// <summary>
    /// Add a new dropdown of dining attributes to Liked Dining Panel.
    /// </summary>
    public void AddLikedDiningDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedDiningPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableDiningList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedDining(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setLikedDiningDropdowns.Add(newDropdown);
    }//end AddLikedDiningDropdown

    /// <summary>
    /// Update the dictionary of liked dining by comparing the text component of the options to valid values.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedDining(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Free":
                if (!_likedDiningDictionary.ContainsKey("Free"))
                {
                    _likedDiningDictionary.Add("Free", 0);
                    _removalLikedDiningDropdown.AddOptions(_likedDiningDictionary.Keys.ToList<string>());
                }
                break;
            case "$":
                {
                    if (!_likedDiningDictionary.ContainsKey("$"))
                    {
                        _likedDiningDictionary.Add("$", 1);
                        _removalLikedDiningDropdown.AddOptions(_likedDiningDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$":
                {
                    if (!_likedDiningDictionary.ContainsKey("$$"))
                    {
                        _likedDiningDictionary.Add("$$", 2);
                        _removalLikedDiningDropdown.AddOptions(_likedDiningDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$":
                {
                    if (!_likedDiningDictionary.ContainsKey("$$$"))
                    {
                        _likedDiningDictionary.Add("$$$", 3);
                        _removalLikedDiningDropdown.AddOptions(_likedDiningDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Dining Value!!");
                    break;
                }
        }//end switch

    }//end UpdateLikedDining

    /// <summary>
    /// Provide a list of Liked Dining values for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<int> ReturnLikedDining()
    {
        List<int> _returnList = new List<int>();

        foreach (var _dining in _likedDiningDictionary)
        {
            _returnList.Add(_dining.Value);
        }

        return _returnList;
    }//end ReturnLikedDining

    //Disliked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveDislikedDiningDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setDislikedDiningDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _dislikedDiningDictionary.Remove(change.options[change.value].text);
                _removalDislikedDiningDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveDislikedDiningDropdown

    /// <summary>
    /// Add a new dropdown of dining attributes to Disliked Dining Panel.
    /// </summary>
    public void AddDislikedDiningDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _dislikedDiningPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableDiningList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedDining(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setDislikedDiningDropdowns.Add(newDropdown);
    }//end AddDislikedDiningDropdown

    /// <summary>
    /// Update the dictionary of disliked dining by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateDislikedDining(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Free":
                if (!_dislikedDiningDictionary.ContainsKey("Free"))
                {
                    _dislikedDiningDictionary.Add("Free", 0);
                    _removalDislikedDiningDropdown.AddOptions(_dislikedDiningDictionary.Keys.ToList<string>());
                }
                break;
            case "$":
                {
                    if (!_dislikedDiningDictionary.ContainsKey("$"))
                    {
                        _dislikedDiningDictionary.Add("$", 1);
                        _removalDislikedDiningDropdown.AddOptions(_dislikedDiningDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$":
                {
                    if (!_dislikedDiningDictionary.ContainsKey("$$"))
                    {
                        _dislikedDiningDictionary.Add("$$", 2);
                        _removalDislikedDiningDropdown.AddOptions(_dislikedDiningDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$":
                {
                    if (!_dislikedDiningDictionary.ContainsKey("$$$"))
                    {
                        _dislikedDiningDictionary.Add("$$$", 3);
                        _removalDislikedDiningDropdown.AddOptions(_dislikedDiningDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Dining Value!!");
                    break;
                }
        }//end switch

    }//end UpdateDislikedDining

    /// <summary>
    /// Provide a list of Disliked Dining for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<int> ReturnDislikedDining()
    {
        List<int> _returnList = new List<int>();

        foreach (var _season in _dislikedSeasonDictionary)
        {
            _returnList.Add(_season.Value);
        }

        return _returnList;
    }//end ReturnDislikedDining

    #endregion

    #region Lodging

    [Header("Lodging")]

    [SerializeField]
    List<string> _availableLodgingList;
    [SerializeField]
    GameObject _likedLodgingPanel;
    [SerializeField]
    GameObject _dislikedLodgingPanel;
    [SerializeField]
    Dictionary<string, int> _likedLodgingDictionary;
    [SerializeField]
    Dictionary<string, int> _dislikedLodgingDictionary;
    [SerializeField]
    List<GameObject> _setLikedLodgingDropdowns;
    [SerializeField]
    List<GameObject> _setDislikedLodgingDropdowns;
    [SerializeField]
    TMP_Dropdown _removalLikedLodgingDropdown;
    [SerializeField]
    TMP_Dropdown _removalDislikedLodgingDropdown;

    public void LoadAvailableLodging(Client currentClient)
    {
        _likedLodgingDictionary = new Dictionary<string, int>();
        _dislikedLodgingDictionary = new Dictionary<string, int>();

        for (int _qualityAmount = 0; _qualityAmount <= 3; _qualityAmount++)
        {
            if (currentClient.KnownLodgingQuality.Contains(_qualityAmount))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedLodgingPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = TransformLodgingQuality(_qualityAmount);

                //Add to retrievable dictionary
                _likedLodgingDictionary.Add(_qualityAmount.ToString(), (int)_qualityAmount);
            }
            else
            {
                _availableLodgingList.Add(TransformLodgingQuality(_qualityAmount));
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedLodgingPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLodgingList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedLodging(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalLikedLodgingDropdown.ClearOptions();
        _setLikedLodgingDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedLodgingPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLodgingList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedLodging(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalDislikedLodgingDropdown.ClearOptions();
        _setDislikedLodgingDropdowns.Add(newDropdown);
    }//end load available lodging

    string TransformLodgingQuality(int _quality)
    {
        switch (_quality)
        {
            case 0:
                {
                    return "Free";
                }
            case 1:
                {
                    return "$";
                }
            case 2:
                {
                    return "$$";
                }
            case 3:
                {
                    return "$$$";
                }
            case 4:
                {
                    return "$$$$";
                }
            case 5:
                {
                    return "$$$$$";
                }
            default:
                {
                    return "Error";
                }
        }
    }//end TransformLodgingQuality

    //Liked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection. 
    /// </summary>
    public void RemoveLikedLodgingDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setLikedLodgingDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _likedLodgingDictionary.Remove(change.options[change.value].text);
                _removalLikedLodgingDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveLikedLodgingDropdown

    /// <summary>
    /// Add a new dropdown of lodging attributes to Liked Lodging Panel.
    /// </summary>
    public void AddLikedLodgingDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedLodgingPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLodgingList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedLodging(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setLikedLodgingDropdowns.Add(newDropdown);
    }//end AddLikedLodgingDropdown

    /// <summary>
    /// Update the dictionary of liked lodging by comparing the text component of the options to valid values.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedLodging(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Free":
                if (!_likedLodgingDictionary.ContainsKey("Free"))
                {
                    _likedLodgingDictionary.Add("Free", 0);
                    _removalLikedLodgingDropdown.AddOptions(_likedLodgingDictionary.Keys.ToList<string>());
                }
                break;
            case "$":
                {
                    if (!_likedLodgingDictionary.ContainsKey("$"))
                    {
                        _likedLodgingDictionary.Add("$", 1);
                        _removalLikedLodgingDropdown.AddOptions(_likedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$":
                {
                    if (!_likedLodgingDictionary.ContainsKey("$$"))
                    {
                        _likedLodgingDictionary.Add("$$", 2);
                        _removalLikedLodgingDropdown.AddOptions(_likedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$":
                {
                    if (!_likedLodgingDictionary.ContainsKey("$$$"))
                    {
                        _likedLodgingDictionary.Add("$$$", 3);
                        _removalLikedLodgingDropdown.AddOptions(_likedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$$":
                {
                    if (!_likedLodgingDictionary.ContainsKey("$$$$"))
                    {
                        _likedLodgingDictionary.Add("$$$$", 4);
                        _removalLikedLodgingDropdown.AddOptions(_likedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$$$":
                {
                    if (!_likedLodgingDictionary.ContainsKey("$$$$$"))
                    {
                        _likedDiningDictionary.Add("$$$$$", 5);
                        _removalLikedLodgingDropdown.AddOptions(_likedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Lodging Value!!");
                    break;
                }
        }//end switch

    }//end UpdateLikedLodging

    /// <summary>
    /// Provide a list of Liked Lodging values for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<int> ReturnLikedLodging()
    {
        List<int> _returnList = new List<int>();

        foreach (var _lodging in _likedLodgingDictionary)
        {
            _returnList.Add(_lodging.Value);
        }

        return _returnList;
    }//end ReturnLikedLodging

    //Disliked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveDislikedLodgingDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setDislikedLodgingDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _dislikedLodgingDictionary.Remove(change.options[change.value].text);
                _removalDislikedLodgingDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveDislikedLodgingDropdown

    /// <summary>
    /// Add a new dropdown of lodging attributes to Disliked Lodging Panel.
    /// </summary>
    public void AddDislikedLodgingDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _dislikedLodgingPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLodgingList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedLodging(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setDislikedLodgingDropdowns.Add(newDropdown);
    }//end AddDislikedLodgingDropdown

    /// <summary>
    /// Update the dictionary of disliked lodging by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateDislikedLodging(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Free":
                if (!_dislikedLodgingDictionary.ContainsKey("Free"))
                {
                    _dislikedLodgingDictionary.Add("Free", 0);
                    _removalDislikedLodgingDropdown.AddOptions(_dislikedLodgingDictionary.Keys.ToList<string>());
                }
                break;
            case "$":
                {
                    if (!_dislikedLodgingDictionary.ContainsKey("$"))
                    {
                        _dislikedLodgingDictionary.Add("$", 1);
                        _removalDislikedLodgingDropdown.AddOptions(_dislikedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$":
                {
                    if (!_dislikedLodgingDictionary.ContainsKey("$$"))
                    {
                        _dislikedLodgingDictionary.Add("$$", 2);
                        _removalDislikedLodgingDropdown.AddOptions(_dislikedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$":
                {
                    if (!_dislikedLodgingDictionary.ContainsKey("$$$"))
                    {
                        _dislikedLodgingDictionary.Add("$$$", 3);
                        _removalDislikedLodgingDropdown.AddOptions(_dislikedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$$":
                {
                    if (!_dislikedLodgingDictionary.ContainsKey("$$$$"))
                    {
                        _dislikedLodgingDictionary.Add("$$$$", 4);
                        _removalDislikedLodgingDropdown.AddOptions(_dislikedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "$$$$$":
                {
                    if (!_dislikedLodgingDictionary.ContainsKey("$$$$$"))
                    {
                        _dislikedDiningDictionary.Add("$$$$$", 5);
                        _removalDislikedLodgingDropdown.AddOptions(_dislikedLodgingDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Lodging Value!!");
                    break;
                }
        }//end switch

    }//end UpdateDislikedLodging

    /// <summary>
    /// Provide a list of Disliked Lodging for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<int> ReturnDislikedLodging()
    {
        List<int> _returnList = new List<int>();

        foreach (var _lodging in _dislikedLodgingDictionary)
        {
            _returnList.Add(_lodging.Value);
        }

        return _returnList;
    }//end ReturnDislikedLodging

    #endregion

    #region Activities

    [Header("Activities")]

    [SerializeField]
    List<string> _availableActivityList;
    [SerializeField]
    GameObject _likedActivityPanel;
    [SerializeField]
    GameObject _dislikedActivityPanel;
    [SerializeField]
    Dictionary<string, int> _likedActivityDictionary;
    [SerializeField]
    Dictionary<string, int> _dislikedActivityDictionary;
    [SerializeField]
    List<GameObject> _setLikedActivityDropdowns;
    [SerializeField]
    List<GameObject> _setDislikedActivityDropdowns;
    [SerializeField]
    TMP_Dropdown _removalLikedActivityDropdown;
    [SerializeField]
    TMP_Dropdown _removalDislikedActivityDropdown;

    public void LoadAvailableActivities(Client currentClient)
    {
        _likedActivityDictionary = new Dictionary<string, int>();
        _dislikedActivityDictionary = new Dictionary<string, int>();

        foreach (ActivityTags _activities in System.Enum.GetValues(typeof(HoG.ActivityTags)))
        {
            if (currentClient.KnownLikedActivityTags.Contains(_activities))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedActivityPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _activities.ToString();

                //Add to retrievable dictionary
                _likedLocationDictionary.Add(_activities.ToString(), (int)_activities);
            }
            else if (currentClient.KnownLikedActivityTags.Contains(_activities))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedActivityPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _activities.ToString();

                //Add to retrievable dictionary
                _dislikedLocationDictionary.Add(_activities.ToString(), (int)_activities);
            }
            else
            {
                _availableActivityList.Add(_activities.ToString());
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedActivityPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableActivityList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedActivity(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalLikedActivityDropdown.ClearOptions();
        _setLikedActivityDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedActivityPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableActivityList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedActivity(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalDislikedActivityDropdown.ClearOptions();
        _setDislikedActivityDropdowns.Add(newDropdown);
    }//end load available locations

    //Liked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveLikedActivityDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setLikedActivityDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _likedActivityDictionary.Remove(change.options[change.value].text);
                _removalLikedActivityDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveLikedActivityDropdown

    /// <summary>
    /// Add a new dropdown of activities attributes to Liked Activities Panel.
    /// </summary>
    public void AddLikedActivityDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedActivityPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableActivityList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedActivity(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setLikedActivityDropdowns.Add(newDropdown);
    }//end AddLikedActivityDropdown

    /// <summary>
    /// Update the dictionary of liked activities by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedActivity(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Null":
                if (!_likedActivityDictionary.ContainsKey("Null"))
                {
                    _likedActivityDictionary.Add(ActivityTags.Null.ToString(), (int)ActivityTags.Null);
                    _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                }
                break;
            case "Food":
                {
                    if (!_likedActivityDictionary.ContainsKey("Food"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Food.ToString(), (int)ActivityTags.Food);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Thrills":
                {
                    if (!_likedActivityDictionary.ContainsKey("Thrills"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Thrills.ToString(), (int)ActivityTags.Thrills);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Active":
                {
                    if (!_likedActivityDictionary.ContainsKey("Active"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Active.ToString(), (int)ActivityTags.Active);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Scary":
                {
                    if (!_likedActivityDictionary.ContainsKey("Scary"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Scary.ToString(), (int)ActivityTags.Scary);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Experience":
                {
                    if (!_likedActivityDictionary.ContainsKey("Experience"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Experience.ToString(), (int)ActivityTags.Experience);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Adventure":
                {
                    if (!_likedActivityDictionary.ContainsKey("Adventure"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Adventure.ToString(), (int)ActivityTags.Adventure);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Mature":
                {
                    if (!_likedActivityDictionary.ContainsKey("Mature"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Mature.ToString(), (int)ActivityTags.Mature);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Quiet":
                {
                    if (!_likedActivityDictionary.ContainsKey("Quiet"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Quiet.ToString(), (int)ActivityTags.Quiet);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Luxury":
                {
                    if (!_likedActivityDictionary.ContainsKey("Luxury"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Luxury.ToString(), (int)ActivityTags.Luxury);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Relax":
                {
                    if (!_likedActivityDictionary.ContainsKey("Relax"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Relax.ToString(), (int)ActivityTags.Relax);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Lively":
                {
                    if (!_likedActivityDictionary.ContainsKey("Lively"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Lively.ToString(), (int)ActivityTags.Lively);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Sightseeing":
                {
                    if (!_likedActivityDictionary.ContainsKey("Sightseeing"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Sightseeing.ToString(), (int)ActivityTags.Sightseeing);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Drinks":
                {
                    if (!_likedActivityDictionary.ContainsKey("Drinks"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Drinks.ToString(), (int)ActivityTags.Drinks);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Children":
                {
                    if (!_likedActivityDictionary.ContainsKey("Children"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Children.ToString(), (int)ActivityTags.Children);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Nature":
                {
                    if (!_likedActivityDictionary.ContainsKey("Nature"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Nature.ToString(), (int)ActivityTags.Nature);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Static":
                {
                    if (!_likedActivityDictionary.ContainsKey("Static"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Static.ToString(), (int)ActivityTags.Static);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Romantic":
                {
                    if (!_likedActivityDictionary.ContainsKey("Romantic"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Romantic.ToString(), (int)ActivityTags.Romantic);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Animals":
                {
                    if (!_likedActivityDictionary.ContainsKey("Animals"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Animals.ToString(), (int)ActivityTags.Animals);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Messy":
                {
                    if (!_likedActivityDictionary.ContainsKey("Messy"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Messy.ToString(), (int)ActivityTags.Messy);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Water":
                {
                    if (!_likedActivityDictionary.ContainsKey("Water"))
                    {
                        _likedActivityDictionary.Add(ActivityTags.Water.ToString(), (int)ActivityTags.Water);
                        _removalDislikedActivityDropdown.AddOptions(_likedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Activity Attribute!!");
                    break;
                }
        }//end switch

    }//end

    /// <summary>
    /// Provide a list of Liked Activities for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<ActivityTags> ReturnLikedActivities()
    {
        List<ActivityTags> _returnList = new List<ActivityTags>();

        foreach (var _activity in _likedLocationDictionary)
        {
            _returnList.Add((ActivityTags)_activity.Value);
        }

        return _returnList;
    }//end ReturnLikedActivities

    //Disliked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveDislikedActivityDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setDislikedActivityDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _dislikedActivityDictionary.Remove(change.options[change.value].text);
                _removalDislikedActivityDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveDislikedActivityDropdown

    /// <summary>
    /// Add a new dropdown of activity attributes to Disliked Locations Panel.
    /// </summary>
    public void AddDislikedActivityDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _dislikedActivityPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableActivityList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedActivity(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setDislikedActivityDropdowns.Add(newDropdown);
    }//end AddDislikedActivityDropdown

    /// <summary>
    /// Update the dictionary of disliked activities by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateDislikedActivity(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "Null":
                if (!_dislikedActivityDictionary.ContainsKey("Null"))
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Null.ToString(), (int)ActivityTags.Null);
                    _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                }
                break;
            case "Food":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Food"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Food.ToString(), (int)ActivityTags.Food);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Thrills":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Thrills"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Thrills.ToString(), (int)ActivityTags.Thrills);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Active":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Active"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Active.ToString(), (int)ActivityTags.Active);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Scary":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Scary"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Scary.ToString(), (int)ActivityTags.Scary);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Experience":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Experience"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Experience.ToString(), (int)ActivityTags.Experience);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Adventure":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Adventure"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Adventure.ToString(), (int)ActivityTags.Adventure);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Mature":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Mature"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Mature.ToString(), (int)ActivityTags.Mature);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Quiet":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Quiet"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Quiet.ToString(), (int)ActivityTags.Quiet);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Luxury":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Luxury"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Luxury.ToString(), (int)ActivityTags.Luxury);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Relax":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Relax"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Relax.ToString(), (int)ActivityTags.Relax);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Lively":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Lively"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Lively.ToString(), (int)ActivityTags.Lively);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Sightseeing":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Sightseeing"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Sightseeing.ToString(), (int)ActivityTags.Sightseeing);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Drinks":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Drinks"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Drinks.ToString(), (int)ActivityTags.Drinks);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Children":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Children"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Children.ToString(), (int)ActivityTags.Children);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Nature":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Nature"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Nature.ToString(), (int)ActivityTags.Nature);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Static":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Static"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Static.ToString(), (int)ActivityTags.Static);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Romantic":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Romantic"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Romantic.ToString(), (int)ActivityTags.Romantic);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Animals":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Animals"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Animals.ToString(), (int)ActivityTags.Animals);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Messy":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Messy"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Messy.ToString(), (int)ActivityTags.Messy);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Water":
                {
                    if (!_dislikedActivityDictionary.ContainsKey("Water"))
                    {
                        _dislikedActivityDictionary.Add(ActivityTags.Water.ToString(), (int)ActivityTags.Water);
                        _removalDislikedActivityDropdown.AddOptions(_dislikedActivityDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Activity Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateDislikedActivity

    /// <summary>
    /// Provide a list of Disliked Activities for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<ActivityTags> ReturnDislikedActivity()
    {
        List<ActivityTags> _returnList = new List<ActivityTags>();

        foreach (var _activity in _dislikedActivityDictionary)
        {
            _returnList.Add((ActivityTags)_activity.Value);
        }

        return _returnList;
    }//end ReturnDislikedActivity

    #endregion

    #region Transit

    [Header("Transit")]
    [SerializeField]
    List<string> _availableTransitList;
    [SerializeField]
    GameObject _likedTransitPanel;
    [SerializeField]
    GameObject _dislikedTransitPanel;
    [SerializeField]
    Dictionary<string, int> _likedTransitDictionary;
    [SerializeField]
    Dictionary<string, int> _dislikedTransitDictionary;
    [SerializeField]
    List<GameObject> _setLikedTransitDropdowns;
    [SerializeField]
    List<GameObject> _setDislikedTransitDropdowns;
    [SerializeField]
    TMP_Dropdown _removalLikedTransitDropdown;
    [SerializeField]
    TMP_Dropdown _removalDislikedTransitDropdown;

    public void LoadAvailableTransit(Client currentClient)
    {
        _likedTransitDictionary = new Dictionary<string, int>();
        _dislikedTransitDictionary = new Dictionary<string, int>();

        foreach (Transit _transit in System.Enum.GetValues(typeof(HoG.Transit)))
        {
            if (currentClient.KnownPreferredTransit.Contains(_transit))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedTransitPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _transit.ToString();

                //Add to retrievable dictionary
                _likedTransitDictionary.Add(_transit.ToString(), (int)_transit);
            }
            else if (currentClient.KnownUnpreferredTransit.Contains(_transit))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedTransitPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _transit.ToString();

                //Add to retrievable dictionary
                _dislikedTransitDictionary.Add(_transit.ToString(), (int)_transit);
            }
            else
            {
                _availableTransitList.Add(_transit.ToString());
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedTransitPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableTransitList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedTransit(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalLikedTransitDropdown.ClearOptions();
        _setLikedTransitDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedTransitPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableTransitList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedTransit(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalDislikedTransitDropdown.ClearOptions();
        _setDislikedTransitDropdowns.Add(newDropdown);
    }//end load available transit

    //Liked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveLikedTransitDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setLikedTransitDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _likedTransitDictionary.Remove(change.options[change.value].text);
                _removalLikedTransitDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveLikedTransitDropdown

    /// <summary>
    /// Add a new dropdown of Transit attributes to Liked Seasons Panel.
    /// </summary>
    public void AddLikedTransitDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedTransitPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableTransitList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedTransit(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setLikedTransitDropdowns.Add(newDropdown);
    }//end AddLikedTransitDropdown

    /// <summary>
    /// Update the dictionary of liked Transit by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedTransit(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "None":
                {
                    if (!_likedTransitDictionary.ContainsKey("None"))
                    {
                        _likedTransitDictionary.Add(Transit.None.ToString(), (int)Transit.None);
                        _removalLikedTransitDropdown.AddOptions(_likedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "PrivateDriver":
                {
                    if (!_likedTransitDictionary.ContainsKey("PrivateDriver"))
                    {
                        _likedTransitDictionary.Add(Transit.PrivateDriver.ToString(), (int)Transit.PrivateDriver);
                        _removalLikedTransitDropdown.AddOptions(_likedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "CarRental":
                {
                    if (!_likedTransitDictionary.ContainsKey("CarRental"))
                    {
                        _likedTransitDictionary.Add(Transit.CarRental.ToString(), (int)Transit.CarRental);
                        _removalLikedTransitDropdown.AddOptions(_likedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "TransitPass":
                {
                    if (!_likedTransitDictionary.ContainsKey("TransitPass"))
                    {
                        _likedTransitDictionary.Add(Transit.TransitPass.ToString(), (int)Transit.TransitPass);
                        _removalLikedTransitDropdown.AddOptions(_likedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Transit Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateLikedTransit

    /// <summary>
    /// Provide a list of Liked Transit for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<Transit> ReturnLikedTransit()
    {
        List<Transit> _returnList = new List<Transit>();

        foreach (var _transit in _likedTransitDictionary)
        {
            _returnList.Add((Transit)_transit.Value);
        }

        return _returnList;
    }//end ReturnLikedTransit

    //Disliked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveDislikedTransitDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setDislikedTransitDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _dislikedTransitDictionary.Remove(change.options[change.value].text);
                _removalDislikedTransitDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveDislikedTransitDropdown

    /// <summary>
    /// Add a new dropdown of location attributes to Disliked Seasons Panel.
    /// </summary>
    public void AddDislikedTransitDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _dislikedTransitPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableTransitList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedTransit(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setDislikedTransitDropdowns.Add(newDropdown);
    }//end AddDislikedTransitDropdown

    /// <summary>
    /// Update the dictionary of disliked Transit by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateDislikedTransit(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "None":
                {
                    if (!_dislikedTransitDictionary.ContainsKey("None"))
                    {
                        _dislikedTransitDictionary.Add(Transit.None.ToString(), (int)Transit.None);
                        _removalDislikedTransitDropdown.AddOptions(_dislikedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "PrivateDriver":
                {
                    if (!_dislikedTransitDictionary.ContainsKey("PrivateDriver"))
                    {
                        _dislikedTransitDictionary.Add(Transit.PrivateDriver.ToString(), (int)Transit.PrivateDriver);
                        _removalDislikedTransitDropdown.AddOptions(_dislikedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "CarRental":
                {
                    if (!_dislikedTransitDictionary.ContainsKey("CarRental"))
                    {
                        _dislikedTransitDictionary.Add(Transit.CarRental.ToString(), (int)Transit.CarRental);
                        _removalDislikedTransitDropdown.AddOptions(_dislikedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "TransitPass":
                {
                    if (!_dislikedTransitDictionary.ContainsKey("TransitPass"))
                    {
                        _dislikedTransitDictionary.Add(Transit.TransitPass.ToString(), (int)Transit.TransitPass);
                        _removalDislikedTransitDropdown.AddOptions(_dislikedTransitDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Transit Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateDislikedTransit

    /// <summary>
    /// Provide a list of Disliked Seasons for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<Transit> ReturnDislikedTransit()
    {
        List<Transit> _returnList = new List<Transit>();

        foreach (var _transit in _dislikedTransitDictionary)
        {
            _returnList.Add((Transit)_transit.Value);
        }

        return _returnList;
    }//end ReturnDislikedTransit


    #endregion

    #region Airplane Class


    [Header("Airplane Class")]
    [SerializeField]
    List<string> _availableAirplaneClassList;
    [SerializeField]
    GameObject _likedAirplaneClassPanel;
    [SerializeField]
    GameObject _dislikedAirplaneClassPanel;
    [SerializeField]
    Dictionary<string, int> _likedAirplaneClassDictionary;
    [SerializeField]
    Dictionary<string, int> _dislikedAirplaneClassDictionary;
    [SerializeField]
    List<GameObject> _setLikedAirplaneClassDropdowns;
    [SerializeField]
    List<GameObject> _setDislikedAirplaneClassDropdowns;
    [SerializeField]
    TMP_Dropdown _removalLikedAirplaneClassDropdown;
    [SerializeField]
    TMP_Dropdown _removalDislikedAirplaneClassDropdown;

    public void LoadAvailableAirplaneClass(Client currentClient)
    {
        _likedAirplaneClassDictionary = new Dictionary<string, int>();
        _dislikedAirplaneClassDictionary = new Dictionary<string, int>();

        foreach (AirplaneClass _airplaneClass in System.Enum.GetValues(typeof(HoG.AirplaneClass)))
        {
            if (currentClient.KnownPreferredAirplaneClass.Contains(_airplaneClass))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedAirplaneClassPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _airplaneClass.ToString();

                //Add to retrievable dictionary
                _likedAirplaneClassDictionary.Add(_airplaneClass.ToString(), (int)_airplaneClass);
            }
            else if (currentClient.KnownUnpreferredAirplaneClass.Contains(_airplaneClass))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedAirplaneClassPanel.transform);
                addAttribute.GetComponentInChildren<TextMeshProUGUI>().text = _airplaneClass.ToString();

                //Add to retrievable dictionary
                _dislikedAirplaneClassDictionary.Add(_airplaneClass.ToString(), (int)_airplaneClass);
            }
            else
            {
                _availableAirplaneClassList.Add(_airplaneClass.ToString());
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedAirplaneClassPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableAirplaneClassList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedAirplaneClass(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalLikedAirplaneClassDropdown.ClearOptions();
        _setLikedAirplaneClassDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedAirplaneClassPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableAirplaneClassList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedAirplaneClass(newDropdown.GetComponent<TMP_Dropdown>()); });
        _removalDislikedAirplaneClassDropdown.ClearOptions();
        _setDislikedAirplaneClassDropdowns.Add(newDropdown);
    }//end load available Airplane Class

    //Liked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveLikedAirplaneClassDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setLikedAirplaneClassDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _likedAirplaneClassDictionary.Remove(change.options[change.value].text);
                _removalLikedAirplaneClassDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveLikedAirplaneClassDropdown

    /// <summary>
    /// Add a new dropdown of AirplaneClass attributes to Liked Seasons Panel.
    /// </summary>
    public void AddLikedAirplaneClassDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedAirplaneClassPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableAirplaneClassList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateLikedAirplaneClass(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setLikedAirplaneClassDropdowns.Add(newDropdown);
    }//end AddLikedAirplaneClassDropdown

    /// <summary>
    /// Update the dictionary of liked AirplaneClass by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedAirplaneClass(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "None":
                {
                    if (!_likedAirplaneClassDictionary.ContainsKey("None"))
                    {
                        _likedAirplaneClassDictionary.Add(AirplaneClass.None.ToString(), (int)AirplaneClass.None);
                        _removalLikedAirplaneClassDropdown.AddOptions(_likedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "First":
                {
                    if (!_likedAirplaneClassDictionary.ContainsKey("First"))
                    {
                        _likedAirplaneClassDictionary.Add(AirplaneClass.First.ToString(), (int)AirplaneClass.First);
                        _removalLikedAirplaneClassDropdown.AddOptions(_likedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Business":
                {
                    if (!_likedAirplaneClassDictionary.ContainsKey("Business"))
                    {
                        _likedAirplaneClassDictionary.Add(AirplaneClass.Business.ToString(), (int)AirplaneClass.Business);
                        _removalLikedAirplaneClassDropdown.AddOptions(_likedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Economy":
                {
                    if (!_likedAirplaneClassDictionary.ContainsKey("Economy"))
                    {
                        _likedAirplaneClassDictionary.Add(AirplaneClass.Economy.ToString(), (int)AirplaneClass.Economy);
                        _removalLikedAirplaneClassDropdown.AddOptions(_likedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Airplane Class Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateLikedAirplaneClass

    /// <summary>
    /// Provide a list of Liked AirplaneClass for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<AirplaneClass> ReturnLikedAirplaneClass()
    {
        List<AirplaneClass> _returnList = new List<AirplaneClass>();

        foreach (var _airplaneClass in _likedAirplaneClassDictionary)
        {
            _returnList.Add((AirplaneClass)_airplaneClass.Value);
        }

        return _returnList;
    }//end ReturnLikedAirplaneClass

    //Disliked

    /// <summary>
    /// Remove a dropdown from the list of added dropdowns based on selection.
    /// </summary>
    public void RemoveDislikedAirplaneClassDropdown(TMP_Dropdown change)
    {
        foreach (GameObject _dropdown in _setDislikedAirplaneClassDropdowns)
        {
            if (_dropdown.GetComponent<TMP_Dropdown>().options.Contains(change.options[change.value]))
            {
                _dislikedAirplaneClassDictionary.Remove(change.options[change.value].text);
                _removalDislikedAirplaneClassDropdown.options.RemoveAt(change.value);
                Destroy(_dropdown);
                return;
            }
        }
    }//end RemoveDislikedAirplaneClassDropdown

    /// <summary>
    /// Add a new dropdown of AirplaneClass attributes to Disliked Seasons Panel.
    /// </summary>
    public void AddDislikedAirplaneClassDropdown()
    {
        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _dislikedAirplaneClassPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableAirplaneClassList);
        newDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { UpdateDislikedAirplaneClass(newDropdown.GetComponent<TMP_Dropdown>()); });
        _setDislikedAirplaneClassDropdowns.Add(newDropdown);
    }//end AddDislikedAirplaneClassDropdown

    /// <summary>
    /// Update the dictionary of disliked AirplaneClass by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateDislikedAirplaneClass(TMP_Dropdown change)
    {
        switch (change.options[change.value].text)
        {
            case "None":
                {
                    if (!_dislikedAirplaneClassDictionary.ContainsKey("None"))
                    {
                        _dislikedAirplaneClassDictionary.Add(AirplaneClass.None.ToString(), (int)AirplaneClass.None);
                        _removalDislikedAirplaneClassDropdown.AddOptions(_dislikedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "First":
                {
                    if (!_dislikedAirplaneClassDictionary.ContainsKey("First"))
                    {
                        _dislikedAirplaneClassDictionary.Add(AirplaneClass.First.ToString(), (int)AirplaneClass.First);
                        _removalDislikedAirplaneClassDropdown.AddOptions(_dislikedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Business":
                {
                    if (!_dislikedAirplaneClassDictionary.ContainsKey("Business"))
                    {
                        _dislikedAirplaneClassDictionary.Add(AirplaneClass.Business.ToString(), (int)AirplaneClass.Business);
                        _removalDislikedAirplaneClassDropdown.AddOptions(_dislikedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            case "Economy":
                {
                    if (!_dislikedAirplaneClassDictionary.ContainsKey("Economy"))
                    {
                        _dislikedAirplaneClassDictionary.Add(AirplaneClass.Economy.ToString(), (int)AirplaneClass.Economy);
                        _removalDislikedAirplaneClassDropdown.AddOptions(_dislikedAirplaneClassDictionary.Keys.ToList<string>());
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Invalid Airplane Class Attribute!!");
                    break;
                }
        }//end switch

    }//end UpdateDislikedAirplaneClass

    /// <summary>
    /// Provide a list of Disliked AirplaneClass for the Itinerary Budget Calculator.
    /// </summary>
    /// <returns></returns>
    public List<AirplaneClass> ReturnDislikedAirplaneClass()
    {
        List<AirplaneClass> _returnList = new List<AirplaneClass>();

        foreach (var _airplaneClass in _dislikedAirplaneClassDictionary)
        {
            _returnList.Add((AirplaneClass)_airplaneClass.Value);
        }

        return _returnList;
    }//end ReturnDislikedAirplaneClass


    #endregion

    #endregion
}
