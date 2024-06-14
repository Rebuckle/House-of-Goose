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
        bool groupCoverage = false; //is one person covering entire cost of the group or is it split
        double groupSize = 0; //how many are in the group

        //lodging variable (affected by tripDuration, groupSize, and inputSeason)
        double lodgingCost = 0;

        double lodgingRate = 0;
        double groupLodgingTotal = 0;
        //rates set to 1/2 bed with assumption of couple. maximum of 2 beds coverage, 3-4 people possible, makes coverage for split beds group later easier

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
        groupSize = 1;
        groupCoverage = false;
        lowSeason = true;
        
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
        if (groupSize == 1) //solo traveller covers both 1/2s of bed, original lodgingRate is set to 1/2 bed
        {
            lodgingRate = lodgingRate * 2;
        }
        else if (groupSize == 2) //partner travellers split the bed, value stays the same unless groupCoverage
        {
            if (groupCoverage == false)
            {
                lodgingRate = lodgingRate;
            }
            else if (groupCoverage == true)
            {
                lodgingRate = lodgingRate * 2;
            }
        }
        else if (groupSize == 3 || groupSize == 4) //group travellers add an extra bed, so 4 halves are now there
        {
            if (groupCoverage == false)
            {
                lodgingRate = lodgingRate;
            }
            else if (groupCoverage == true)
            {
                lodgingRate = lodgingRate * 4;
            }
        }

        lodgingCost = lodgingRate * tripDuration;

        // ----------------------------------------------------------------------------------------------------------------------------

        //dining calculation
        if (optOutDining == false) //if client is opting in for dining coverage
        {
            if (fullDining == true) //if client is opting in for full daily dining
            {
                if (groupCoverage == false) //if the client isn't covering for an entire group
                {
                    diningCost = diningRate * tripDuration;
                }
                else if (groupCoverage == true) //if the client is covering for an entire group
                {
                    diningCost = (diningRate * groupSize) * tripDuration;
                }
            }
            else if (fullDining == false) //if opting for limited / specific dining coverage
            {
                double diningSum = 0;
                for (int i = 0; i < collectDining.Length; i++) //add the array to a total
                {
                    diningSum = collectDining[i];
                }

                if (groupCoverage == false) //if the client isn't covering for an entire group
                {
                    diningCost = diningSum;
                }
                else if (groupCoverage == true) //if the client is covering for an entire group
                {
                    diningCost = diningSum * groupSize;
                }
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
            if (groupCoverage == false) //if the client isn't covering for an entire group
            {
                activityCost = activitySum;
            }
            else if (groupCoverage == true) //if the client is covering for an entire group
            {
                activityCost = activitySum * groupSize;
            }
        }
        else if (optOutActivity == true)
        {
            activityCost = 0;
        }

        // ----------------------------------------------------------------------------------------------------------------------------

        //transit calculation
        if (optOutTransit == false) //if client is opting in for transit coverage
        {
            if (groupCoverage == false) //if the client isn't covering for an entire group
            {
                transitCost = (toFromAirportCost / groupSize) + (tripDuration * (((carRentalCost + personalDriverCost) / groupSize) + transitPassCost)); //split between group members
            }
            else if (groupCoverage == true) //if the client is covering for an entire group
            {
                transitCost = toFromAirportCost + (tripDuration * (carRentalCost + personalDriverCost + (groupSize * transitPassCost))); //cover group members
            }
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
