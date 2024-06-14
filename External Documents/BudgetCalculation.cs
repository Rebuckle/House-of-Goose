using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BudgetCalculation : MonoBehaviour
{

    void Start()
    {
        //general cost variable
        double tripCost = 0;

        //general client provided information variables
        double tripDuration = 0;
        bool peakSeason = false; //element multiplier of 1.5
        bool lowSeason = false; //element multiplier of 0.7
        bool regularSeason = false; //element multiplier of 1
        double groupSize = 0; //how many are in the group

        //lodging variable (affected by tripDuration, groupSize, and inputSeason)
        double lodgingCost = 0;

        double lodgingRate = 0;

        //dining variables (affected by tripDuration and groupSize)
        double diningCost = 0;

        bool fullDining = false; //reservation meal daily
        double diningRate = 0;
        double[] collectDining = new double[5] { 0, 0, 0, 0, 0}; //specified amount of dining
        bool optOutDining = false; //client opted out of dining

        //activity variables (affected by groupSize)
        double activityCost = 0;

        double[] collectActivity = new double[5] {0, 0, 0, 0, 0}; //agent adds activities based off of client prompts
        bool optOutActivity = false; //client opted out of activities

        //transit variables (affected by tripDuration and groupSize)
        double transitCost = 0;

        double toFromAirportCost = 0; //transit cost to and from airport
        double carRentalCost = 0; //transit cost to rent a car
        double personalDriverCost = 0;//transit cost to employ a personal driver
        double transitPassCost = 0; //transit cost to buy daily pass
        bool optOutTransit = false; //client opted out of transit

        // ----------------------------------------------------------------------------------------------------------------------------


        //setting test values

        tripDuration = 7;
        groupSize = 2;
        peakSeason = true;
        
        lodgingRate = 200;

        optOutDining = false;
        fullDining = true;
        diningRate = 70;

        optOutActivity = false;
        collectActivity[0] = 100; collectActivity[1] = 50; collectActivity[2] = 60; 

        optOutTransit = false;
        toFromAirportCost = 40;
        carRentalCost = 0; 
        personalDriverCost = 0;
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
