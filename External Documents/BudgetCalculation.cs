using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//--Itinerary
public class BudgetCalculation : MonoBehaviour
{

    void Start()
    {
        //general cost variable
        //--Itinerary
        double tripCost = 0;

        //general client provided information variables
        //--Client Book
        double tripDuration = 0;
        //--Itinerary
        bool peakSeason = false; //element multiplier of 1.5
        //--Itinerary
        bool lowSeason = false; //element multiplier of 0.7
        //--Itinerary
        bool regularSeason = false; //element multiplier of 1
        //--Client Book
        double groupSize = 0; //how many are in the group

        //lodging variable (affected by tripDuration, groupSize, and inputSeason)

        //--Itinerary
        double lodgingCost = 0;

        //--Organizer
        double lodgingRate = 0;

        //dining variables (affected by tripDuration and groupSize)
        //--Itinerary
        double diningCost = 0;

        //--Client Book
        bool fullDining = false; //reservation meal daily
        //--Organizer
        double diningRate = 0;
        //--Itinerary
        double[] collectDining = new double[5] { 0, 0, 0, 0, 0}; //specified amount of dining
        //--Client Book
        bool optOutDining = false; //client opted out of dining

        //activity variables (affected by groupSize)
        //--Itinerary
        double activityCost = 0;

        //--Itinerary
        double[] collectActivity = new double[5] {0, 0, 0, 0, 0}; //agent adds activities based off of client prompts
        //--Client Book
        bool optOutActivity = false; //client opted out of activities

        //transit variables (affected by tripDuration and groupSize)
        //--Itinerary
        double transitCost = 0;

        //--Itinerary
        double toFromAirportCost = 0; //transit cost to and from airport
        //--Organizer
        double carRentalCost = 0; //transit cost to rent a car
        //--Organizer
        double personalDriverCost = 0;//transit cost to employ a personal driver
        //--Organizer
        double transitPassCost = 0; //transit cost to buy daily pass
        //--Client Book
        bool optOutTransit = false; //client opted out of transit

        // ----------------------------------------------------------------------------------------------------------------------------


        //setting test values

        //--Client Book
        tripDuration = 7;
        //--Client Book
        groupSize = 2;
        //--Itinerary
        peakSeason = true;

        //--Organizer
        lodgingRate = 200;

        //--Client Book
        optOutDining = false;
        //--Client Book
        fullDining = true;
        //--Organizer
        diningRate = 70;

        //--Client Book
        optOutActivity = false;
        //--Itinerary
        collectActivity[0] = 100; collectActivity[1] = 50; collectActivity[2] = 60;

        //--Client Book
        optOutTransit = false;
        //--Itinerary
        toFromAirportCost = 40;
        //--Organizer
        carRentalCost = 0;
        //--Organizer
        personalDriverCost = 0;
        //--Organizer
        transitPassCost = 16;


        // ----------------------------------------------------------------------------------------------------------------------------

        //lodging calculation
        //assuming these are Queen beds or Doubles for splitting, if 2 peeps don't want to sleep in the same bed, we'll just say they downgraded to smaller beds
        
        if (groupSize == 1 || groupSize == 2) //solo traveller covers full bed
        {
            lodgingRate = lodgingRate;
        }
        else if (groupSize == 3 || groupSize == 4) 
        {
            lodgingRate = lodgingRate * 2;
        }

        lodgingCost = lodgingRate * tripDuration;

        // ----------------------------------------------------------------------------------------------------------------------------

        //dining calculation
        if (optOutDining == false) //if client is opting in for dining coverage
        {
            if (fullDining == true) //if client is opting in for full daily dining
            {
                diningCost = diningRate * groupSize * tripDuration;
            }
            else if (fullDining == false) //if opting for limited / specific dining coverage
            {
                double diningSum = 0;
                for (int i = 0; i < collectDining.Length; i++) //add the array to a total
                {
                    diningSum = collectDining[i];
                }
                
                diningCost = diningSum * groupSize;
            }
        }
        else if (optOutDining == true) //if client is opting out of dining coverage
        {
            diningCost = 0;
        }

        // ----------------------------------------------------------------------------------------------------------------------------

        //activity calculation
        if (optOutActivity == false)
        {
            double activitySum = 0;
            for (int i = 0; i < collectActivity.Length; i++) //add the array to a total
            {
                activitySum += collectActivity[i];
            }

            activityCost = activitySum * groupSize;
            
        }
        else if (optOutActivity == true)
        {
            activityCost = 0;
        }

        // ----------------------------------------------------------------------------------------------------------------------------

        //transit calculation
        if (optOutTransit == false) //if client is opting in for transit coverage
        {
            transitCost = toFromAirportCost + (tripDuration * (carRentalCost + personalDriverCost + (transitPassCost * groupSize)));
        }
        else if (optOutTransit == true) //if client is opting out of transit coverage
        {
            transitCost = 0;
        }

        // ----------------------------------------------------------------------------------------------------------------------------

        //season variables
        double valueSeason = 1; //base number

        if (peakSeason == true)
        {
            valueSeason = valueSeason * 1.4; //40% more
        }
        else if (lowSeason == true)
        {
            valueSeason = valueSeason * 0.7; //30% less
        }
        else if (regularSeason == true)
        {
            valueSeason = valueSeason;
        }

        //adjust costs impacted by season variables
        lodgingCost = lodgingCost * valueSeason;

        // ----------------------------------------------------------------------------------------------------------------------------

        //calculate woop woop
        tripCost = lodgingCost + diningCost + activityCost + transitCost;

        Debug.Log(tripCost);
    }

    
}
