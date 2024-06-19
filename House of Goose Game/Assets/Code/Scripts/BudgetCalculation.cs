using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BudgetCalculation : MonoBehaviour
{
    [SerializeField]
    ItineraryBehavior TheItinerary;

    [SerializeField]
    Client TheClient;

    //general cost variable
    double tripCost = 0;

    //lodging variable (affected by tripDuration, TheClient.GroupSize, and inputSeason)
    double lodgingCost = 0;

    double lodgingRate = 0;

    //dining variables (affected by tripDuration and TheClient.GroupSize)
    double diningCost = 0;

    double diningRate = 0;
    List<double> collectDining; //specified amount of dining
    bool optOutDining = false; //client opted out of dining

    //activity variables (affected by TheClient.GroupSize)
    double activityCost = 0;

    double[] collectActivity = new double[5] { 0, 0, 0, 0, 0 }; //agent adds activities based off of client prompts
    bool optOutActivity = false; //client opted out of activities

    //transit variables (affected by tripDuration and TheClient.GroupSize)
    double transitCost = 0;

    double toFromAirportCost = 0; //transit cost to and from airport
    double carRentalCost = 0; //transit cost to rent a car
    double personalDriverCost = 0;//transit cost to employ a personal driver
    double transitPassCost = 0; //transit cost to buy daily pass
    bool optOutTransit = false; //client opted out of transit

    //airfare variables (affected by TheClient.GroupSize and inputSeason)
    string clientStartRegion = ""; //start continent 
    string clientDestinationCity = ""; //destination city
    string clientDestination = ""; //destination continent

    string NorthAmerica = "North America";
    string Africa = "Africa";
    string Europe = "Europe";
    string Asia = "Asia";
    string Oceania = "Oceania";

    bool economyClass = false;
    bool businessClass = false;
    bool firstClass = false;

    double classType = 0;
    double airfareCost = 0;

    //season variables
    double valueSeason = 1; //base number

    public void Start()
    {
                
    }

    public void CalculateBudget()
    {
        CalculateLodging();
        CalculateDining();
        CalculateActivity();
        CalculateTransit();
        CalculateSeason();
        CalculateAirfare();

        CalculateTripCost();
        CalculateRating(tripCost);
    }

    void CalculateLodging()
    {
        //lodging calculation
        //assuming these are Queen beds or Doubles for splitting, if 2 peeps don't want to sleep in the same bed, we'll just say they downgraded to smaller beds

        if (TheClient.GroupSize == 1 || TheClient.GroupSize == 2) //solo traveller covers full bed
        {
            lodgingRate = lodgingRate;
        }
        else if (TheClient.GroupSize == 3 || TheClient.GroupSize == 4)
        {
            lodgingRate = lodgingRate * 2;
        }

        lodgingCost = lodgingRate * TheClient.TripDuration;
    }

    void CalculateDining()
    {
        //dining calculation
        if (TheClient.OptOutDining == false) //if client is opting in for dining coverage
        {
            //collectDining = itinerary.collectDining 

            double diningSum = 0;
            for (int i = 0; i < collectDining.Count; i++) //add the array to a total
            {
                diningSum = collectDining[i];
            }

            diningCost = diningSum * TheClient.GroupSize;            
        }
        else if (optOutDining == true) //if client is opting out of dining coverage
        {
            diningCost = 0;
        }
    }

    void CalculateActivity()
    {
        //activity calculation
        if (TheClient.OptOutActivity == false)
        {
            double activitySum = 0;
            for (int i = 0; i < collectActivity.Length; i++) //add the array to a total
            {
                activitySum += collectActivity[i];
            }

            activityCost = activitySum * TheClient.GroupSize;

        }
        else if (TheClient.OptOutActivity == true)
        {
            activityCost = 0;
        }
    }

    void CalculateTransit()
    {
        //transit calculation
        if (TheClient.OptOutTransit == false) //if client is opting in for transit coverage
        {
            transitCost = toFromAirportCost + (TheClient.TripDuration * (carRentalCost + personalDriverCost + (transitPassCost * TheClient.GroupSize)));
        }
        else if (optOutTransit == true) //if client is opting out of transit coverage
        {
            transitCost = 0;
        }
    }

    void CalculateSeason()
    {
        if (TheItinerary.GetLocation().PeakSeason.ToString() == TheItinerary.GetSeason())
        {
            valueSeason = valueSeason * 1.4; //40% more
        }
        else if (TheItinerary.GetLocation().DownSeason.ToString() == TheItinerary.GetSeason())
        {
            valueSeason = valueSeason * 0.7; //30% less
        }
        else
        {
            valueSeason = valueSeason;
        }

        //adjust costs impacted by season variables
        lodgingCost = lodgingCost * valueSeason;
    }

    void CalculateAirfare()
    {
        //airfare calculation
        //clients from NA, AFR, EUR, ASIA, OCN
        //destinations only NA, EUR, ASIA

        //set values
        //clientStartRegionCountry = "";
        //clientDestinationCity = "";
        //businessClass = true;

        //turn client and location input into continents to condense calculations

        clientStartRegion = TheClient.StartingRegion.ToString();

        if (clientDestinationCity == "Portland")
            clientDestination = "North America";

        else if (clientDestinationCity == "Edinburgh" || clientDestinationCity == "Paris" || clientDestinationCity == "Santorini")
            clientDestination = "Europe";

        else if (clientDestinationCity == "Busan" || clientDestinationCity == "Tokyo" || clientDestinationCity == "Delhi" || clientDestinationCity == "Bali")
            clientDestination = "Asia";


        //set class type multiple
        if (TheItinerary.GetAirplaneClass() == "Economy")
            classType = 1;
        else if (TheItinerary.GetAirplaneClass() == "Business")
            classType = 1.6;
        else if (TheItinerary.GetAirplaneClass() == "FirstClass")
            classType = 2.2;

        if (TheClient.StartingRegion.ToString() == NorthAmerica)
        {
            if (TheItinerary.GetLocation().Region.ToString() == NorthAmerica)
            {
                airfareCost = 300 * TheClient.GroupSize * valueSeason * classType; //group size, season type, and class type adjusts ticket
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Europe)
            {
                airfareCost = 700 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Asia)
            {
                airfareCost = 1600 * TheClient.GroupSize * valueSeason * classType;
            }
        }
        else if (TheClient.StartingRegion.ToString() == Africa)
        {
            if (TheItinerary.GetLocation().Region.ToString() == NorthAmerica)
            {
                airfareCost = 1200 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Europe)
            {
                airfareCost = 1000 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Asia)
            {
                airfareCost = 1400 * TheClient.GroupSize * valueSeason * classType;
            }
        }
        else if (TheClient.StartingRegion.ToString() == Europe)
        {
            if (TheItinerary.GetLocation().Region.ToString() == NorthAmerica)
            {
                airfareCost = 700 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Europe)
            {
                airfareCost = 250 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Asia)
            {
                airfareCost = 900 * TheClient.GroupSize * valueSeason * classType;
            }
        }
        else if (TheClient.StartingRegion.ToString() == Asia)
        {
            if (TheItinerary.GetLocation().Region.ToString() == NorthAmerica)
            {
                airfareCost = 1600 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Europe)
            {
                airfareCost = 900 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Asia)
            {
                airfareCost = 300 * TheClient.GroupSize * valueSeason * classType;
            }
        }
        else if (TheClient.StartingRegion.ToString() == Oceania)
        {
            if (TheItinerary.GetLocation().Region.ToString() == NorthAmerica)
            {
                airfareCost = 1300 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Europe)
            {
                airfareCost = 1100 * TheClient.GroupSize * valueSeason * classType;
            }
            else if (TheItinerary.GetLocation().Region.ToString() == Asia)
            {
                airfareCost = 600 * TheClient.GroupSize * valueSeason * classType;
            }
        }
    }

    void CalculateTripCost()
    {
        //calculate woop woop
        tripCost = lodgingCost + diningCost + activityCost + transitCost + airfareCost;

        Debug.Log(tripCost);
    }


    void CalculateRating(double tripCost)
    {

        //variables list

        //budget variables ----------------------------------------------
        double tripBudget = 0;
        double budgetRating = 0;

        //season variables ----------------------------------------------
        string summer = "summer";
        string winter = "winter";
        string spring = "spring";
        string autumn = "autumn";

        string[] clientTopSeasons = new string[2] { "", "" };
        string tripSeason = "";
        double seasonRating = 0;

        //transit variables ----------------------------------------------
        string clientTransit = "";
        string tripTransit = "";
        double transitRating = 0;

        //location variables ----------------------------------------------
        string[] clientLocation = new string[2] { "", "" };
        string[] tripLocation = new string[2] { "", "" };
        double locationRating = 0;

        //airplane class type variables ----------------------------------------------
        string clientPlaneClassType = "";
        string tripPlaneClassType = "";
        double airTypeRating = 0;

        //activities variables ----------------------------------------------
        //single activities
        bool locationActivity1 = false;
        string[] locationActivity1Tags = new string[3] { "", "", "" }; //tags
        double locationActivity1Cost = 0;

        bool locationActivity2 = false;
        string[] locationActivity2Tags = new string[3] { "", "", "" };
        double locationActivity2Cost = 0;

        bool locationActivity3 = false;
        string[] locationActivity3Tags = new string[3] { "", "", "" };
        double locationActivity3Cost = 0;

        bool locationActivity4 = false;
        string[] locationActivity4Tags = new string[3] { "", "", "" };
        double locationActivity4Cost = 0;

        bool locationActivity5 = false;
        string[] locationActivity5Tags = new string[3] { "", "", "" };
        double locationActivity5Cost = 0;

        //bundle activities
        bool locationActivityBundle1 = false;
        string[] locationActivityBundle1Tags = new string[2] { "", "" };
        double locationActivityBundle1Cost = 0;
        double locationActivityBundle1Quantity = 0;


        bool locationActivityBundle2 = false;
        string[] locationActivityBundle2Tags = new string[2] { "", "" };
        double locationActivityBundle2Cost = 0;
        double locationActivityBundle2Quantity = 0;

        //client
        string[] clientActivityLikes = new string[6] { "", "", "", "", "", "" }; //put in keywords to determine whether they like selected activities, max 6
        string[] clientActivityHates = new string[6] { "", "", "", "", "", "" }; //put in keywords to determine whether they dislike selected activities, max 6

        double clientActivityAmount = 3; //client ideal activity amount
        double tripActivityAmount = 0;
        double tripActivityCost = 0;

        double activityRating = 0;

        //all hot & cold keywords
        string[] hotKeywordsTags = new string[14] { "Static", "Active", "Quiet", "Lively", "Scary", "Romantic", "Children", "Mature", "Luxury", "Water", "Thrills", "Animals", "Food", "Messy" };
        string[] coldKeywordsTags = new string[6] { "Nature", "Sightseeing", "Drinks", "Adventure", "Experience", "Relax" };

        //customer satisfaction variables ----------------------------------------------
        double customerCallLimit = 0; //client has a specific call maximum limit
        double customerCallAmount = 0; //going over call limit

        bool customerContent = false; //mostly good dialogue choices
        bool customerIrritated = false; //mixed good bad choices
        bool customerAngry = false; //mostly bad choices

        double customerServiceRating = 0;

        //star rating variables ----------------------------------------------
        double clientRating = 0;
        double starRatingTemp = 0; //for calculations
        double starRating = 0;

        // ----------------------------------------------------------------------------------------------------------------------------

        //budgetRating calculation

        Debug.Log(tripBudget + " & " + tripCost);

        if (tripCost <= tripBudget)
        {
            budgetRating = 0.1; //satisfied
            Debug.Log("Green");
        }
        else if (tripCost > (tripBudget + 1) && tripCost < (tripBudget * 1.2))
        {
            budgetRating = 0.05; //okay with
            Debug.Log("Yellow");
        }
        else
        {
            budgetRating = -0.2; //pissed off
            Debug.Log("Red");
        }

        Debug.Log("budget rating: " + budgetRating);

        // ----------------------------------------------------------------------------------------------------------------------------
        
        //seasonRating calculation

        for (int i = 0; i < clientTopSeasons.Length; i++) //add the array to a total
        {
            if (clientTopSeasons[i] == tripSeason)
                seasonRating = 0.2;
        }

        if (seasonRating == 0)
        {
            for (int i = 0; i < clientTopSeasons.Length; i++) //add the array to a total
            {
                if (clientTopSeasons[i] != spring && tripSeason == spring || clientTopSeasons[i] != autumn && tripSeason == autumn) //seasons generally tolerated
                {
                    seasonRating = 0.1;
                }
                else if (clientTopSeasons[i] != summer && tripSeason == summer || clientTopSeasons[i] != winter && tripSeason == winter) //seasons with generally strongest opposition
                {
                    seasonRating = 0;
                    break;
                }
            }
        }

        Debug.Log("season rating: " + seasonRating);

        // ----------------------------------------------------------------------------------------------------------------------------

        //transitRating calculation
        //4 types: private driver, car rental, transit pass, no transit
        
        if (clientTransit == tripTransit)
            transitRating = 0.1;
        else 
            transitRating = 0;

        Debug.Log("transit rating: " + transitRating);

        // ----------------------------------------------------------------------------------------------------------------------------

        //locationRating  calculation
        //each location has 2 attributes to track matching / similar locations

        for (int i = 0; i < clientLocation.Length; i++ )
        {
            if (clientLocation[i] == tripLocation[0] || clientLocation[i] == tripLocation[1])
            {
                locationRating = 0.1; //doubles if both tags match for good score
            }
        }

        Debug.Log("location rating: " + locationRating);

        // ----------------------------------------------------------------------------------------------------------------------------

        //airTypeRating calculation

        if (clientPlaneClassType == tripPlaneClassType)
            airTypeRating = 0.05;
        else
            airTypeRating = -0.05;

        Debug.Log("location rating: " + airTypeRating);

        // ----------------------------------------------------------------------------------------------------------------------------

        //activityRating calculation

        //calculate tripActivityAmount
        if (locationActivity1 == true)
            tripActivityAmount += 1;

        if (locationActivity2 == true)
            tripActivityAmount += 1;

        if (locationActivity3 == true)
            tripActivityAmount += 1;

        if (locationActivity4 == true)
            tripActivityAmount += 1;

        if (locationActivity5 == true)
            tripActivityAmount += 1;

        if (locationActivityBundle1 == true)
            tripActivityAmount += locationActivityBundle1Quantity;

        if (locationActivityBundle2 == true)
            tripActivityAmount += locationActivityBundle2Quantity;

        //calculate activity amount
        if (clientActivityAmount == tripActivityAmount)
        {
            activityRating += 0.05; //ideal amount of activities
        }
        else if (clientActivityAmount > (tripActivityAmount * 0.6) && clientActivityAmount < tripActivityAmount || clientActivityAmount < (tripActivityAmount * 1.4) && clientActivityAmount > tripActivityAmount)
        {
            activityRating += 0.025; //close to ideal
        }
        else if (clientActivityAmount < (tripActivityAmount * 0.6) && clientActivityAmount > (tripActivityAmount * 0.2) || clientActivityAmount > (tripActivityAmount * 1.4) && clientActivityAmount < (tripActivityAmount * 1.8))
        {
            activityRating += -0.025; //a little far from ideal
        }
        else if (clientActivityAmount <= (tripActivityAmount * 0.2) || clientActivityAmount >= (tripActivityAmount * 1.8))
        {
            activityRating += -0.05; //completely off from ideal
        }

        //set opposites for hot hate tags
        for (int i = 0; i < clientActivityLikes.Length; i++)
        {
            if (clientActivityLikes[i] == "Static")
            {
                clientActivityHates[i] = "Active";
            }
            else if (clientActivityLikes[i] == "Active")
            {
                clientActivityHates[i] = "Static";
            }
            else if (clientActivityLikes[i] == "Quiet")
            {
                clientActivityHates[i] = "Lively";
            }
            else if (clientActivityLikes[i] == "Lively")
            {
                clientActivityHates[i] = "Quiet";
            }
            else if (clientActivityLikes[i] == "Scary")
            {
                clientActivityHates[i] = "Romantic";
            }
            else if (clientActivityLikes[i] == "Romantic")
            {
                clientActivityHates[i] = "Scary";
            }
            else if (clientActivityLikes[i] == "Children")
            {
                clientActivityHates[i] = "Mature";
            }
            else if (clientActivityLikes[i] == "Mature" || clientActivityLikes[i] == "Luxury") //Dual opposite tags for Children, doesn't need to set luxury as hate but luxury would hate children y'know
            {
                clientActivityHates[i] = "Children";
            }
        }

        //calculate rating with comparison of likes, hates, and activity tags
        if (locationActivity1 == true)
        {
            tripActivityAmount += locationActivity1Cost;

            for (int i = 0; i < clientActivityLikes.Length; i++)
            {
                if (clientActivityLikes[i] == locationActivity1Tags[0] || clientActivityLikes[i] == locationActivity1Tags[1] || clientActivityLikes[i] == locationActivity1Tags[2]) //liked activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += 0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += 0.025;
                    }
                }
                if (clientActivityHates[i] == locationActivity1Tags[0] || clientActivityHates[i] == locationActivity1Tags[1] || clientActivityHates[i] == locationActivity1Tags[2]) //hated activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += -0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += -0.025;
                    }
                }
            }
        }

        if (locationActivity2 == true)
        {
            tripActivityAmount += locationActivity2Cost;

            for (int i = 0; i < clientActivityLikes.Length; i++)
            {
                if (clientActivityLikes[i] == locationActivity2Tags[0] || clientActivityLikes[i] == locationActivity2Tags[1] || clientActivityLikes[i] == locationActivity2Tags[2]) //liked activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += 0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += 0.025;
                    }
                }
                if (clientActivityHates[i] == locationActivity2Tags[0] || clientActivityHates[i] == locationActivity2Tags[1] || clientActivityHates[i] == locationActivity2Tags[2]) //hated activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += -0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += -0.025;
                    }
                }
            }
        }

        if (locationActivity3 == true)
        {
            tripActivityAmount += locationActivity3Cost;

            for (int i = 0; i < clientActivityLikes.Length; i++)
            {
                if (clientActivityLikes[i] == locationActivity3Tags[0] || clientActivityLikes[i] == locationActivity3Tags[1] || clientActivityLikes[i] == locationActivity3Tags[2]) //liked activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += 0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += 0.025;
                    }
                }
                if (clientActivityHates[i] == locationActivity3Tags[0] || clientActivityHates[i] == locationActivity3Tags[1] || clientActivityHates[i] == locationActivity3Tags[2]) //hated activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += -0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += -0.025;
                    }
                }
            }
        }

        if (locationActivity4 == true)
        {
            tripActivityAmount += locationActivity4Cost;

            for (int i = 0; i < clientActivityLikes.Length; i++)
            {
                if (clientActivityLikes[i] == locationActivity4Tags[0] || clientActivityLikes[i] == locationActivity4Tags[1] || clientActivityLikes[i] == locationActivity4Tags[2]) //liked activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += 0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += 0.025;
                    }
                }
                if (clientActivityHates[i] == locationActivity4Tags[0] || clientActivityHates[i] == locationActivity4Tags[1] || clientActivityHates[i] == locationActivity4Tags[2]) //hated activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += -0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += -0.025;
                    }
                }
            }
        }

        if (locationActivity5 == true)
        {
            tripActivityAmount += locationActivity5Cost;

            for (int i = 0; i < clientActivityLikes.Length; i++)
            {
                if (clientActivityLikes[i] == locationActivity5Tags[0] || clientActivityLikes[i] == locationActivity5Tags[1] || clientActivityLikes[i] == locationActivity5Tags[2]) //liked activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += 0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += 0.025;
                    }
                }
                if (clientActivityHates[i] == locationActivity5Tags[0] || clientActivityHates[i] == locationActivity5Tags[1] || clientActivityHates[i] == locationActivity5Tags[2]) //hated activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += -0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += -0.025;
                    }
                }
            }
        }

        if (locationActivityBundle1 == true)
        {
            tripActivityCost += locationActivityBundle1Cost * locationActivityBundle1Quantity;

            for (int i = 0; i < clientActivityLikes.Length; i++)
            {
                if (clientActivityLikes[i] == locationActivityBundle1Tags[0] || clientActivityLikes[i] == locationActivityBundle1Tags[1]) //liked activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += 0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += 0.025;
                    }
                }
                if (clientActivityHates[i] == locationActivityBundle1Tags[0] || clientActivityHates[i] == locationActivityBundle1Tags[1]) //hated activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += -0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += -0.025;
                    }
                }
            }
        }

        if (locationActivityBundle2 == true)
        {
            tripActivityCost += locationActivityBundle2Cost * locationActivityBundle2Quantity;

            for (int i = 0; i < clientActivityLikes.Length; i++)
            {
                if (clientActivityLikes[i] == locationActivityBundle2Tags[0] || clientActivityLikes[i] == locationActivityBundle2Tags[1]) //liked activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += 0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += 0.025;
                    }
                }
                if (clientActivityHates[i] == locationActivityBundle2Tags[0] || clientActivityHates[i] == locationActivityBundle2Tags[1]) //hated activity tags
                {
                    for (int j = 0; j < hotKeywordsTags.Length; j++)
                    {
                        if (clientActivityLikes[i] == hotKeywordsTags[j]) //hot keywords
                            activityRating += -0.05;
                    }
                    for (int k = 0; k < coldKeywordsTags.Length; k++)
                    {
                        if (clientActivityLikes[i] == coldKeywordsTags[k]) //cold
                            activityRating += -0.025;
                    }
                }
            }
        }

        Debug.Log("activity rating: " + activityRating);

        // ----------------------------------------------------------------------------------------------------------------------------

        //customerServiceRating calculation

        if (customerCallLimit >= customerCallAmount)
            customerServiceRating += 0.05;
        else
            customerServiceRating += -0.05;

        if (customerContent == true)
            customerServiceRating += 0.05;
        else if (customerIrritated == true) //sensitive
            customerServiceRating += -0.1;
        else if (customerAngry == true) //highly sensitive
            customerServiceRating += -0.2;

        Debug.Log("customer service rating: " + customerServiceRating);

        // ----------------------------------------------------------------------------------------------------------------------------

        //calculate final rating and star rating from client

        clientRating = budgetRating + locationRating + seasonRating + transitRating + airTypeRating + activityRating + customerServiceRating;

        starRatingTemp = clientRating * 100;

        if (starRatingTemp > 95) //hard to get perfect score
            starRating = 5;
        else if (starRatingTemp > 90)
            starRating = 4.5;
        else if (starRatingTemp > 80)
            starRating = 4;
        else if (starRatingTemp > 70)
            starRating = 3.5;
        else if (starRatingTemp > 60)
            starRating = 3;
        else if (starRatingTemp > 50)
            starRating = 2.5;
        else if (starRatingTemp > 40)
            starRating = 2;
        else if (starRatingTemp > 30)
            starRating = 1.5;
        else if (starRatingTemp > 20)
            starRating = 1;
        else if (starRatingTemp > 10)
            starRating = 0.5;
        else if (starRatingTemp >= 0)
            starRating = 0;

        Debug.Log("client rating: " + clientRating);
        Debug.Log("final star rating: " + starRating + " / 5");

        // ----------------------------------------------------------------------------------------------------------------------------

        //output success message
        if (starRating == 5)
            Debug.Log("EXCELLENT WORK!");
        else if (starRating >= 4)
            Debug.Log("GOOD JOB!");
        else if (starRating >= 3)
            Debug.Log("YOU'RE GETTING THERE!");
        else if (starRating >= 2)
            Debug.Log("TRY HARDER.");
        else if (starRating >= 1)
            Debug.Log("THAT WAS TERRIBLE.");
        else if (starRating >= 0)
            Debug.Log("YOUR JOB IS AT RISK.");

        // ----------------------------------------------------------------------------------------------------------------------------

    }
}
