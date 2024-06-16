using UnityEngine;

[CreateAssetMenu(fileName = "Transit", menuName = "Scriptable Objects/Transit")]
public class Transit : ScriptableObject, IExperience
{
    [SerializeField]
    HoG.Transit _transitType;
    [SerializeField]
    int _quality;
    [SerializeField]
    double _cost;

    public IExperience.ExperienceEnum ExperienceType { get => IExperience.ExperienceEnum.Transit; }
    public string Name { get => _transitType.ToString(); set => Name = _transitType.ToString(); }
    public int Quality { get => _quality; set => _quality = value; }
    public double Cost { get => _cost; set => _cost = value; }
}
