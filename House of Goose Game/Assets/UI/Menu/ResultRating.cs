using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ResultRating : MonoBehaviour
{
    
    public int maxRating;
    public int rating;

    public Sprite emptyStar;
    public Sprite halfStar;
    public Sprite fullStar;
    public Image[] ratingSprites;

    //int ratingTemp = 0;
    void Awake()
    {
        ratingSprites = new Image[5];
        for (int i = 0; i < ratingSprites.Length; i++)
        {
            Transform ratingImages = transform.Find("RatingImages");
            int val = i+1;
            string label = "Image " + val;
            Debug.Log(label + " || " + gameObject.name);
            Transform starImage = ratingImages.Find(label);
            ratingSprites[i] = starImage.GetComponent<Image>();
        }
    }


    public void UpdateStars()
    {
        //int ratingTemp = 0;

        if (rating == 10)
        {
            ratingSprites[4].sprite = fullStar;
            ratingSprites[3].sprite = fullStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 9)
        {
            ratingSprites[4].sprite = halfStar;
            ratingSprites[3].sprite = fullStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 8)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = fullStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 7)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = halfStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 6)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = fullStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 5)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = halfStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 4)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = fullStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 3)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = halfStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 2)
        {
            ratingSprites[4].sprite = emptyStar;
            ratingSprites[3].sprite = emptyStar;
            ratingSprites[2].sprite = emptyStar;
            ratingSprites[1].sprite = emptyStar;
            ratingSprites[0].sprite = fullStar;
        }
        else if (rating == 1)
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

    }
}
