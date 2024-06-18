using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        LoadSummaryPage();
        LoadPreferencePage();
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
    Dropdown _removalLikedLocationDropdown;
    [SerializeField]
    Dropdown _removalDislikedLocationDropdown;

    /// <summary>
    /// Populate the Client Book with Available Locations, under both Liked and Disliked panels.
    /// </summary>
    /// <param name="currentClient"></param>
    public void LoadAvailableLocations(Client currentClient)
    {
        foreach(Attributes _location in System.Enum.GetValues(typeof(HoG.Attributes)))
        {
            if(currentClient.KnownPreferredLocationAttributes.Contains(_location)) {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedLocationPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = _location.ToString();

                //Add to retrievable dictionary
                _likedLocationDictionary.Add(_location.ToString(), (int) _location);
            } 
            else if (currentClient.KnownUnpreferredLocationAttributes.Contains(_location))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedLocationPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = _location.ToString();

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
        _setLikedLocationDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedLocationPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLocationList);
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
        _setLikedLocationDropdowns.Add(newDropdown);
    }//end AddLikedLocationDropdown

    /// <summary>
    /// Update the dictionary of liked locations by comparing the text component of the options to valid Attributes.
    /// </summary>
    /// <param name="change"></param>
    public void UpdateLikedLocations(TMP_Dropdown change)
    {
        //_likedLocationDictionary.Add(change.options[change.value], );

        switch (change.options[change.value].text)
        {
            case "old":
                {
                    _likedLocationDictionary.Add(Attributes.old.ToString(), (int)Attributes.old);
                    break;
                }
            case "modern":
                {
                    _likedLocationDictionary.Add(Attributes.modern.ToString(), (int)Attributes.modern);
                    break;
                }
            case "open":
                {
                    _likedLocationDictionary.Add(Attributes.open.ToString(), (int)Attributes.open);
                    break;
                }
            case "beach":
                {
                    _likedLocationDictionary.Add(Attributes.beach.ToString(), (int)Attributes.beach);
                    break;
                }
            case "dense":
                {
                    _likedLocationDictionary.Add(Attributes.dense.ToString(), (int)Attributes.dense);
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
                    _dislikedLocationDictionary.Add(Attributes.old.ToString(), (int)Attributes.old);
                    break;
                }
            case "modern":
                {
                    _dislikedLocationDictionary.Add(Attributes.modern.ToString(), (int)Attributes.modern);
                    break;
                }
            case "open":
                {
                    _dislikedLocationDictionary.Add(Attributes.open.ToString(), (int)Attributes.open);
                    break;
                }
            case "beach":
                {
                    _dislikedLocationDictionary.Add(Attributes.beach.ToString(), (int)Attributes.beach);
                    break;
                }
            case "dense":
                {
                    _dislikedLocationDictionary.Add(Attributes.dense.ToString(), (int)Attributes.dense);
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
    Dropdown _removalLikedSeasonDropdown;
    [SerializeField]
    Dropdown _removalDislikedSeasonDropdown;

    public void LoadAvailableSeasons(Client currentClient)
    {
        foreach (Seasons _season in System.Enum.GetValues(typeof(HoG.Seasons)))
        {
            if (currentClient.KnownPreferredSeasons.Contains(_season))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedSeasonPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = _season.ToString();
            }
            else if (currentClient.KnownUnpreferredSeasons.Contains(_season))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedSeasonPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = _season.ToString();
            }
            else
            {
                _availableSeasonList.Add(_season.ToString());
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedSeasonPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableSeasonList);
        _setLikedSeasonDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedSeasonPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableSeasonList);
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
                {
                    _likedSeasonDictionary.Add(Seasons.Winter.ToString(), (int)Seasons.Winter);
                    break;
                }
            case "Spring":
                {
                    _likedSeasonDictionary.Add(Seasons.Spring.ToString(), (int)Seasons.Spring);
                    break;
                }
            case "Summer":
                {
                    _likedSeasonDictionary.Add(Seasons.Summer.ToString(), (int)Seasons.Summer);
                    break;
                }
            case "Fall":
                {
                    _likedSeasonDictionary.Add(Seasons.Fall.ToString(), (int)Seasons.Fall);
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
            case "Spring":
                {
                    _dislikedLocationDictionary.Add(Seasons.Spring.ToString(), (int)Seasons.Spring);
                    break;
                }
            case "Winter":
                {
                    _dislikedLocationDictionary.Add(Seasons.Winter.ToString(), (int)Seasons.Winter);
                    break;
                }
            case "Summer":
                {
                    _dislikedLocationDictionary.Add(Seasons.Summer.ToString(), (int)Seasons.Summer);
                    break;
                }
            case "Fall":
                {
                    _dislikedLocationDictionary.Add(Seasons.Fall.ToString(), (int)Seasons.Fall);
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
    Dropdown _removalLikedDiningDropdown;
    [SerializeField]
    Dropdown _removalDislikedDiningDropdown;

    public void LoadAvailableDining(Client currentClient)
    {
        for(int _qualityAmount = 0; _qualityAmount <= 3; _qualityAmount++)
        {
            if (currentClient.KnownDiningQualityPreference.Contains(_qualityAmount))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedDiningPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = TransformDiningQuality(_qualityAmount);
            }
            else
            {
                _availableDiningList.Add(TransformDiningQuality(_qualityAmount));
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedDiningPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableDiningList);
        _setLikedDiningDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedDiningPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableDiningList);
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
                {
                    _likedDiningDictionary.Add("Free", 0);
                    break;
                }
            case "$":
                {
                    _likedDiningDictionary.Add("$", 1);
                    break;
                }
            case "$$":
                {
                    _likedDiningDictionary.Add("$$", 2);
                    break;
                }
            case "$$$":
                {
                    _likedDiningDictionary.Add("$$$", 3);
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
                {
                    _dislikedDiningDictionary.Add("Free", 0);
                    break;
                }
            case "$":
                {
                    _dislikedDiningDictionary.Add("$", 1);
                    break;
                }
            case "$$":
                {
                    _dislikedDiningDictionary.Add("$$", 2);
                    break;
                }
            case "$$$":
                {
                    _dislikedDiningDictionary.Add("$$$", 3);
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
    Dropdown _removalLikedLodgingDropdown;
    [SerializeField]
    Dropdown _removalDislikedLodgingDropdown;

    public void LoadAvailableLodging(Client currentClient)
    {
        for (int _qualityAmount = 0; _qualityAmount <= 3; _qualityAmount++)
        {
            if (currentClient.KnownLodgingQuality.Contains(_qualityAmount))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedLodgingPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = TransformLodgingQuality(_qualityAmount);
            }
            else
            {
                _availableLodgingList.Add(TransformLodgingQuality(_qualityAmount));
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedLodgingPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLodgingList);
        _setLikedLodgingDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedLodgingPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableLodgingList);
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
                {
                    _likedLodgingDictionary.Add("Free", 0);
                    break;
                }
            case "$":
                {
                    _likedLodgingDictionary.Add("$", 1);
                    break;
                }
            case "$$":
                {
                    _likedLodgingDictionary.Add("$$", 2);
                    break;
                }
            case "$$$":
                {
                    _likedLodgingDictionary.Add("$$$", 3);
                    break;
                }
            case "$$$$":
                {
                    _likedLodgingDictionary.Add("$$$$", 4);
                    break;
                }
            case "$$$$$":
                {
                    _likedLodgingDictionary.Add("$$$$$", 5);
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
                {
                    _dislikedDiningDictionary.Add("Free", 0);
                    break;
                }
            case "$":
                {
                    _dislikedDiningDictionary.Add("$", 1);
                    break;
                }
            case "$$":
                {
                    _dislikedDiningDictionary.Add("$$", 2);
                    break;
                }
            case "$$$":
                {
                    _dislikedDiningDictionary.Add("$$$", 3);
                    break;
                }
            case "$$$$":
                {
                    _dislikedDiningDictionary.Add("$$$$", 4);
                    break;
                }
            case "$$$$$":
                {
                    _dislikedDiningDictionary.Add("$$$$$", 5);
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
    Dropdown _removalLikedActivityDropdown;
    [SerializeField]
    Dropdown _removalDislikedActivityDropdown;

    public void LoadAvailableActivities(Client currentClient)
    {
        foreach (ActivityTags _activities in System.Enum.GetValues(typeof(HoG.ActivityTags)))
        {
            if (currentClient.KnownLikedActivityTags.Contains(_activities))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _likedActivityPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = _activities.ToString();
            }
            else if (currentClient.KnownLikedActivityTags.Contains(_activities))
            {
                GameObject addAttribute = Instantiate(_knownActivityTextPrefab, _dislikedActivityPanel.transform);
                addAttribute.GetComponent<TextMeshProUGUI>().text = _activities.ToString();
            }
            else
            {
                _availableActivityList.Add(_activities.ToString());
            }
        }

        GameObject newDropdown = Instantiate(_activityDropdownPrefab, _likedActivityPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableActivityList);

        _setLikedActivityDropdowns.Add(newDropdown);

        newDropdown = Instantiate(_activityDropdownPrefab, _dislikedActivityPanel.transform);
        newDropdown.GetComponent<TMP_Dropdown>().AddOptions(_availableActivityList);

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
                {
                    _likedActivityDictionary.Add(ActivityTags.Null.ToString(), (int)ActivityTags.Null);
                    break;
                }
            case "Food":
                {
                    _likedActivityDictionary.Add(ActivityTags.Food.ToString(), (int)ActivityTags.Food);
                    break;
                }
            case "Thrills":
                {
                    _likedActivityDictionary.Add(ActivityTags.Thrills.ToString(), (int)ActivityTags.Thrills);
                    break;
                }
            case "Active":
                {
                    _likedActivityDictionary.Add(ActivityTags.Active.ToString(), (int)ActivityTags.Active);
                    break;
                }
            case "Scary":
                {
                    _likedActivityDictionary.Add(ActivityTags.Scary.ToString(), (int)ActivityTags.Scary);
                    break;
                }
            case "Experience":
                {
                    _likedActivityDictionary.Add(ActivityTags.Experience.ToString(), (int)ActivityTags.Experience);
                    break;
                }
            case "Adventure":
                {
                    _likedActivityDictionary.Add(ActivityTags.Adventure.ToString(), (int)ActivityTags.Adventure);
                    break;
                }
            case "Mature":
                {
                    _likedActivityDictionary.Add(ActivityTags.Mature.ToString(), (int)ActivityTags.Mature);
                    break;
                }
            case "Quiet":
                {
                    _likedActivityDictionary.Add(ActivityTags.Quiet.ToString(), (int)ActivityTags.Quiet);
                    break;
                }
            case "Luxury":
                {
                    _likedActivityDictionary.Add(ActivityTags.Luxury.ToString(), (int)ActivityTags.Luxury);
                    break;
                }
            case "Relax":
                {
                    _likedActivityDictionary.Add(ActivityTags.Relax.ToString(), (int)ActivityTags.Relax);
                    break;
                }
            case "Lively":
                {
                    _likedActivityDictionary.Add(ActivityTags.Lively.ToString(), (int)ActivityTags.Lively);
                    break;
                }
            case "Sightseeing":
                {
                    _likedActivityDictionary.Add(ActivityTags.Sightseeing.ToString(), (int)ActivityTags.Sightseeing);
                    break;
                }
            case "Drinks":
                {
                    _likedActivityDictionary.Add(ActivityTags.Drinks.ToString(), (int)ActivityTags.Drinks);
                    break;
                }
            case "Children":
                {
                    _likedActivityDictionary.Add(ActivityTags.Children.ToString(), (int)ActivityTags.Children);
                    break;
                }
            case "Nature":
                {
                    _likedActivityDictionary.Add(ActivityTags.Nature.ToString(), (int)ActivityTags.Nature);
                    break;
                }
            case "Static":
                {
                    _likedActivityDictionary.Add(ActivityTags.Static.ToString(), (int)ActivityTags.Static);
                    break;
                }
            case "Romantic":
                {
                    _likedActivityDictionary.Add(ActivityTags.Romantic.ToString(), (int)ActivityTags.Romantic);
                    break;
                }
            case "Animals":
                {
                    _likedActivityDictionary.Add(ActivityTags.Animals.ToString(), (int)ActivityTags.Animals);
                    break;
                }
            case "Messy":
                {
                    _likedActivityDictionary.Add(ActivityTags.Messy.ToString(), (int)ActivityTags.Messy);
                    break;
                }
            case "Water":
                {
                    _likedActivityDictionary.Add(ActivityTags.Water.ToString(), (int)ActivityTags.Water);
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
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Null.ToString(), (int)ActivityTags.Null);
                    break;
                }
            case "Food":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Food.ToString(), (int)ActivityTags.Food);
                    break;
                }
            case "Thrills":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Thrills.ToString(), (int)ActivityTags.Thrills);
                    break;
                }
            case "Active":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Active.ToString(), (int)ActivityTags.Active);
                    break;
                }
            case "Scary":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Scary.ToString(), (int)ActivityTags.Scary);
                    break;
                }
            case "Experience":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Experience.ToString(), (int)ActivityTags.Experience);
                    break;
                }
            case "Adventure":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Adventure.ToString(), (int)ActivityTags.Adventure);
                    break;
                }
            case "Mature":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Mature.ToString(), (int)ActivityTags.Mature);
                    break;
                }
            case "Quiet":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Quiet.ToString(), (int)ActivityTags.Quiet);
                    break;
                }
            case "Luxury":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Luxury.ToString(), (int)ActivityTags.Luxury);
                    break;
                }
            case "Relax":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Relax.ToString(), (int)ActivityTags.Relax);
                    break;
                }
            case "Lively":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Lively.ToString(), (int)ActivityTags.Lively);
                    break;
                }
            case "Sightseeing":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Sightseeing.ToString(), (int)ActivityTags.Sightseeing);
                    break;
                }
            case "Drinks":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Drinks.ToString(), (int)ActivityTags.Drinks);
                    break;
                }
            case "Children":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Children.ToString(), (int)ActivityTags.Children);
                    break;
                }
            case "Nature":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Nature.ToString(), (int)ActivityTags.Nature);
                    break;
                }
            case "Static":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Static.ToString(), (int)ActivityTags.Static);
                    break;
                }
            case "Romantic":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Romantic.ToString(), (int)ActivityTags.Romantic);
                    break;
                }
            case "Animals":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Animals.ToString(), (int)ActivityTags.Animals);
                    break;
                }
            case "Messy":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Messy.ToString(), (int)ActivityTags.Messy);
                    break;
                }
            case "Water":
                {
                    _dislikedActivityDictionary.Add(ActivityTags.Water.ToString(), (int)ActivityTags.Water);
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

    #endregion
}
