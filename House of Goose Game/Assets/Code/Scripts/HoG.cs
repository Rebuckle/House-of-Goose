using UnityEngine;

/// <summary>
/// Base Monobehavior class for the House of Goose game to define common or static variables. Important Game Objects can inheret from this class to inheret Monobhevaior and associated object types.
/// </summary>
public class HoG : MonoBehaviour
{    
    /// <summary>
    /// The primary seasons that a trip can be scheduled for. Also used to define Peak and Down season for each location.
    /// </summary>
    enum Seasons { Winter, Spring, Summer, Fall };

    /// <summary>
    /// Defines the primary regions that locations and clients can be based in for the purpose of determining flight needs.
    /// </summary>
    enum Regions { NorthAmerica, SouthAmerica, Caribbean, Europe, Asia, Africa, Oceania };
}
