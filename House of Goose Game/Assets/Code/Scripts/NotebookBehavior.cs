using System.Collections.Generic;
using UnityEngine;

public class NotebookBehavior : HoG
{
    [SerializeField]
    Canvas _leftSideCanvas;
    [SerializeField]
    Canvas _rightSideCanvas;
    [SerializeField]
    Canvas _pagePool;

    //Pages
    [SerializeField]
    RectTransform _summaryPage; //page one
    [SerializeField]
    RectTransform _preferencePage; //page two
    [SerializeField]
    RectTransform _dislikesPage; //page three
    [SerializeField]
    RectTransform _likesPage; //page four

    //Locations
    public LocationBehavior locationBehavior;
    List<Location> knownLikedLocations;
    List<Location> knownDislikedLocations;
    List<Location> unknownLikedLocations;
    List<Location> unknownDislikedLocations;

    public enum NotebookStatus { Disabled, PagesOneTwo, PagesThreeFour };
    NotebookStatus _status;

    private void Awake()
    {
        _status = NotebookStatus.Disabled;
        FlipPages();
    }//end awake

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
                    _summaryPage.SetParent(_pagePool.gameObject.transform);
                    _preferencePage.SetParent(_pagePool.gameObject.transform);
                    _dislikesPage.SetParent(_pagePool.gameObject.transform);
                    _likesPage.SetParent(_pagePool.gameObject.transform);
                    break;
                }
            case NotebookStatus.PagesOneTwo:
                {
                    _summaryPage.SetParent(_leftSideCanvas.gameObject.transform);
                    _preferencePage.SetParent(_rightSideCanvas.gameObject.transform);
                    _dislikesPage.SetParent(_pagePool.gameObject.transform);
                    _likesPage.SetParent(_pagePool.gameObject.transform);
                    break;
                }
            case NotebookStatus.PagesThreeFour:
                {
                    _summaryPage.SetParent(_pagePool.gameObject.transform);
                    _preferencePage.SetParent(_pagePool.gameObject.transform);
                    _likesPage.SetParent(_leftSideCanvas.gameObject.transform);
                    _dislikesPage.SetParent(_rightSideCanvas.gameObject.transform);
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
}
