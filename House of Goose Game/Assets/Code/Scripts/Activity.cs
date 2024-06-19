using UnityEngine;

[CreateAssetMenu(fileName = "Activity", menuName = "Scriptable Objects/Activity")]
public class Activity : ScriptableObject, IExperience
{
    [SerializeField]
    string _name;
    [SerializeField]
    int _quality;
    [SerializeField]
    double _cost;
    [SerializeField]
    HoG.ActivityTags[] _attributes;

    public IExperience.ExperienceEnum ExperienceType { get => IExperience.ExperienceEnum.Activity; }
    public string Name { get => _name; set => _name = value; }
    public int Quality { get => _quality; set => _quality = value; }
    public double Cost { get => _cost; set => _cost = value; }
    public HoG.ActivityTags[] Attributes { get => _attributes; set => _attributes = value; }
}
