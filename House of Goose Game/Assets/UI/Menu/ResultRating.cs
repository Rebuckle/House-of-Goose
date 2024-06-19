using UnityEngine;
using UnityEngine.UI;

public class ResultRating : MonoBehaviour
{
    
    public double maxRating;
    public double rating;

    public Sprite emptyStar;
    public Sprite halfStar;
    public Sprite fullStar;
    public Image[] ratingSprites;

    double[] ratingCollection = new double[5];
    //int ratingTemp = 0;

    void Start()
    {
        int ratingTemp = 0;

        if (rating == 5)
        {
            ratingSprites[4].sprite = fullStar;
            ratingSprites[3].sprite = fullStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 4.5)
        {
            ratingSprites[4].sprite = halfStar;
            ratingSprites[3].sprite = fullStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 4)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = fullStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 3.5)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = halfStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 3)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 2.5)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = halfStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 2)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 1.5)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = halfStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 1)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = emptyStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 0.5)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = emptyStar;
            ratingSprites[0].sprite = halfStar;
        }
        else if (rating == 0)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = emptyStar;
            ratingSprites[0].sprite = emptyStar;
        }

        ratingCollection[ratingTemp] = rating;
        ratingTemp += 1;
        Debug.Log(ratingCollection[ratingTemp]);
    }

    void Update()
    {
        

    }


}
