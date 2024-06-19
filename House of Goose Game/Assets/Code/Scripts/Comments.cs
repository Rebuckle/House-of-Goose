using UnityEngine;

[CreateAssetMenu(fileName = "Comments", menuName = "Scriptable Objects/Comments")]
public class Comments : ScriptableObject
{
    [SerializeField]
    string[] _starRatings;

    public string[] StarRatings { get { return _starRatings; } set { _starRatings = value;}}

}
