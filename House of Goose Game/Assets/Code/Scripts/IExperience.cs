/// <summary>
/// The base interface for all Experience type game objects, which groups the common and most necessary elements of each.
/// </summary>
public interface IExperience
{
    /// <summary>
    /// The types of experiences that can be entered into the Itinerary.
    /// </summary>
    public enum ExperienceEnum { Dining, Transit, Lodging, Activity };

    public ExperienceEnum ExperienceType {
        get;
    }

    public string Name
    {
        get;
        set;
    }

    public int Quality
    {
        get;
        set;
    }

    public double Cost
    {
        get;
        set;
    }
}