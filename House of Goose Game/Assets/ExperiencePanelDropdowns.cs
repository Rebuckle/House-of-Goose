using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePanelDropdowns : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown _activitiesDropdown;
    [SerializeField]
    TMP_Dropdown _diningDropdown;
    [SerializeField]
    TMP_Dropdown _lodgingDropdown;
    [SerializeField]
    TMP_Dropdown _transitDropdown;

    public TMP_Dropdown ActivitiesDropdown { get => _activitiesDropdown; }
    public TMP_Dropdown DiningDropdown { get => _diningDropdown; }
    public TMP_Dropdown LodgingDropdown { get => _lodgingDropdown; }
    public TMP_Dropdown TransitDropdown { get => _transitDropdown; }
}
