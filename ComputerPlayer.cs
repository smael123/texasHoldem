using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldem
{
    class ComputerPlayer : Player
    {
        private const double INITIAL_CONFIDENCE = 0.7;
        private const double WEAK_CONFIDENCE = 0.5;

        private double aiConfidence;

        private int[] enemyLyingBetMin;
        private int[] enemyLyingBetMax;
        private int[] enemyTruthingBetMin;
        private int[] enemyTruthingBetMax;

        //private int enemyLyingBetPreFlopMin; //maybe percentage of cash instead
        //private int enemyLyingBetPreFlopMax;
        //private int enemyTruthingBetPreFlopMin; //I AIN'T CALLING YOU A TRUTHER!
        //private int enemyTruthingBetPreFlopMax;

        //private int enemyLyingBetFlopMin;
        //private int enemyLyingBetFloptMax;
        //private int enemyTruthingBetFlopMin; 
        //private int enemyTruthingBetFlopMax;

        //private int enemyLyingBetTurnMin; 
        //private int enemyLyingBetTurnMax;
        //private int enemyTruthingBetTurnMin; 
        //private int enemyTruthingBetTurnMax;

        //private int enemyLyingBetRiverMin;
        //private int enemyLyingBetRiverMax;
        //private int enemyTruthingBetRiverMin;
        //private int enemyTruthingBetRiverMax;


        public int bet(int minBetAmount)
        {
            if (aiConfidence >= 0.6 && aiConfidence <= 0.75) //call
            {
                if (minBetAmount < funds)
                {
                    int moneyToBet = funds;
                    funds -= moneyToBet;
                    return moneyToBet;
                }
                else
                {
                    funds -= minBetAmount;
                    return minBetAmount;
                }  
                
            }
            else if (aiConfidence > 0.75) //raise
            {
                if (minBetAmount * 2 < funds)
                {
                    int moneyToBet = funds;
                    funds -= moneyToBet;
                    return moneyToBet;
                }
                else
                {
                    funds -= minBetAmount * 2;
                    return minBetAmount * 2;
                }  
            }
            else //fold
            {
                return -1;
            }
        }
        
        public void performEndOfRoundCalculations(List<TurnInfo> p1Info, bool win)
        {
            int size = p1Info.Count;
            for (int i = 0; i < size; i++)
            {
                if (p1Info[i].HandConfidence.HandValue < WEAK_CONFIDENCE && p1Info[i].BetHigherThanMinimum == true) //if he was lying
                {
                    if (p1Info[i].Bet < enemyLyingBetMin[i])
                    {
                        enemyLyingBetMin[i] = p1Info[i].Bet;
                    }
                    else if (p1Info[i].Bet > enemyLyingBetMax[i])
                    {
                        enemyLyingBetMax[i] = p1Info[i].Bet;
                    }
                }
                else
                {
                    if (p1Info[i].Bet < enemyTruthingBetMin[i])
                    {
                        enemyLyingBetMin[i] = p1Info[i].Bet;
                    }
                    else if (p1Info[i].Bet > enemyTruthingBetMax[i])
                    {
                        enemyLyingBetMax[i] = p1Info[i].Bet;
                    }
                }
            }

            if (win)
            {
                aiConfidence += 0.1;
                if (aiConfidence > 1.0)
                {
                    aiConfidence = 1.0;
                }
            }
            else
            {
                aiConfidence -= 0.1;
                if (aiConfidence < 0.0)
                {
                    aiConfidence = 0.0;
                }
            }

        }
        public void performEndOfRoundCalculations(bool win)
        {
            if (win)
            {
                aiConfidence += 0.1;
                if (aiConfidence > 1.0)
                {
                    aiConfidence = 1.0;
                }
            }
            else
            {
                aiConfidence -= 0.1;
                if (aiConfidence < 0.0)
                {
                    aiConfidence = 0.0;
                }
            }
        }

        public ComputerPlayer() : base()
        {
            aiConfidence = INITIAL_CONFIDENCE;
            enemyLyingBetMin = new int[4];
            enemyLyingBetMax = new int[4];
            enemyTruthingBetMin = new int[4];
            enemyTruthingBetMax = new int[4];

            for (int i = 0; i < 4; i++)
            {
                enemyTruthingBetMin[i] = int.MaxValue;
                enemyLyingBetMin[i] = int.MaxValue;
                enemyTruthingBetMax[i] = 0;
                enemyTruthingBetMax[i] = 0;
           }
        }
        public ComputerPlayer(string name) : base(name)
        {
            aiConfidence = INITIAL_CONFIDENCE;
            enemyLyingBetMin = new int[4];
            enemyLyingBetMax = new int[4];
            enemyTruthingBetMin = new int[4];
            enemyTruthingBetMax = new int[4];

            for (int i = 0; i < 4; i++)
            {
                enemyTruthingBetMin[i] = int.MaxValue;
                enemyLyingBetMin[i] = int.MaxValue;
                enemyTruthingBetMax[i] = 0;
                enemyTruthingBetMax[i] = 0;
            }
        }

        


    }
}
