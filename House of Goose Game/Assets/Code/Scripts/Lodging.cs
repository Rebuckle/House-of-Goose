using UnityEngine;

[CreateAssetMenu(fileName = "Lodging", menuName = "Scriptable Objects/Lodging")]
public class Lodging : ScriptableObject, IExperience
{
    [SerializeField]
    string _name;
    [SerializeField]
    int _quality;
    [SerializeField]
    double _cost;

    public IExperience.ExperienceEnum ExperienceType { get => IExperience.ExperienceEnum.Lodging; }
    public string Name { get => _name; set => _name = value; }
    public int Quality { get => _quality; set => _quality = value; }
    public double Cost { get => _cost; set => _cost = value; }
}
