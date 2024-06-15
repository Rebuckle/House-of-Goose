/// <summary>
/// The base interface for all Experience type game objects, which groups the common and most necessary elements of each.
/// </summary>
public interface IExperience
{
    /// <summary>
    /// The types of experiences that can be entered into the Itinerary.
    /// </summary>
    enum ExperienceType { Dining, Transit, Lodging, Activity };

    /// <summary>
    /// Returns key information about the particular IExperience, which may include Name, Quality, and Cost.
    /// </summary>
    /// <returns>Returns an IExperience object type.</returns>
    public IExperience ReturnExperience();
    /// <summary>
    /// Function to create a new Experience Game Object using standard types.
    /// </summary>
    /// <param name="_name">Describes the name of the Experience.</param>
    /// <param name="_quality">Quantifies the sentimental value of the Experience.</param>
    /// <param name="_cost">Quantifies the monetary value of the Experience.</param>
    /// <param name="experienceType">Qualifies the type of Experience.</param>
    public void AddExperience(string _name, int _quality, double _cost, ExperienceType experienceType);
    /// <summary>
    /// The name of the Experience.
    /// </summary>
    /// <returns>String value of Experience.</returns>
    public string GetName();
    /// <summary>
    /// Assigns a name to the Experience.
    /// </summary>
    /// <param name="_name"></param>
    public void SetName(string _name);
    /// <summary>
    /// The name of the Experience.
    /// </summary>
    /// <returns>String value of Experience.</returns>
    public int GetQuality();
    /// <summary>
    /// Assigns an integer value for the quality of the Experience.
    /// </summary>
    /// <param name="_quality"></param>
    public void SetQuality(int _quality);
    /// <summary>
    /// The quality of the Experience.
    /// </summary>
    /// <returns>Integer value of Experience.</returns>
    public double GetCost();
    /// <summary>
    /// Assigns a double value for the cost of the Experience.
    /// </summary>
    /// <param name="_cost"></param>
    public void SetCost(double _cost);
    /// <summary>
    /// The type of the Experience.
    /// </summary>
    /// <returns>ExperienceType value of Experience.</returns>
    public ExperienceType GetExperience();
    /// <summary>
    /// Assigns a type for the Experience.
    /// </summary>
    /// <param name="_experienceType"></param>
    public void SetExperienceType(ExperienceType _experienceType);
}