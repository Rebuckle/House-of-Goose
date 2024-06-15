using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class Dining : ScriptableObject, IExperience
{
    [SerializeField]
    string _name;
    [SerializeField]
    int _quality;
    [SerializeField]
    double _cost;

    public IExperience.ExperienceEnum ExperienceType { get => IExperience.ExperienceEnum.Dining; }
    public string Name { get => _name; set => _name = value; }
    public int Quality { get => _quality; set => _quality = value; }
    public double Cost { get => _cost; set => _cost = value; }
}
