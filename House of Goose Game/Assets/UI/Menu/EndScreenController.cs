using TMPro;
using UnityEngine;

public class EndScreenController : MonoBehaviour
{

    public Comments[] comments;
    public TextMeshProUGUI[] reviewTextBoxes;

    int[] ratingCollection = new int[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadRatings();

        for (int i = 0; i < 5; i++)
        {
            reviewTextBoxes[i].text = comments[i].StarRatings[ratingCollection[(i-1)/2]];
        }
    }

    void LoadRatings()
    {
        ratingCollection[0] = PlayerPrefs.GetInt("Lily Mitchell", 2);
        ratingCollection[1] = PlayerPrefs.GetInt("Octavio Fallas", 4);
        ratingCollection[2] = PlayerPrefs.GetInt("Andrew Charron", 6);
        ratingCollection[3] = PlayerPrefs.GetInt("Suyin Teo", 8);
        ratingCollection[4] = PlayerPrefs.GetInt("Inês Madeira", 10);
    }

}
