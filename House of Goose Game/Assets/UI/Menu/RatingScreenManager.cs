using UnityEngine;

public class RatingScreenManager : MonoBehaviour
{
    public ResultRating[] resultRatings;
    public ResultRating finalRating;

    int[] ratingCollection = new int[5];
    int finalScore = 0;

    void LoadRatings()
    {
        ratingCollection[0] = PlayerPrefs.GetInt("Lily Mitchell", 2);
        ratingCollection[1] = PlayerPrefs.GetInt("Octavio Fallas", 4);
        ratingCollection[2] = PlayerPrefs.GetInt("Andrew Charron", 6);
        ratingCollection[3] = PlayerPrefs.GetInt("Suyin Teo", 8);
        ratingCollection[4] = PlayerPrefs.GetInt("Inês Madeira", 10);
    }

    void CalculateFinalScore()
    {
        int total = 0;
        for (int i = 0; i < ratingCollection.Length; i++)
        {
            total += ratingCollection[i];
        }
        finalScore = total / ratingCollection.Length;
    }


    void Start()
    {
        LoadRatings();
        CalculateFinalScore();

        for (int i = 0; i < resultRatings.Length; i++)
        {
            resultRatings[i].rating = ratingCollection[i];
            resultRatings[i].UpdateStars();
        }
        finalRating.rating = finalScore;
        finalRating.UpdateStars();
    }

}
