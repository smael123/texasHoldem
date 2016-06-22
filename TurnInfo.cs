using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoldemHand;


namespace texasHoldem
{
    class TurnInfo
    {
        private double percentageOfFunds; //amount they betted: bet/total money they had before bet
        private Hand handConfidence;
        private int bet;
        private bool betHigherThanMinimum;
        

        public TurnInfo(int bet, int minBet, int total, Hand handConfidence)
        {
            this.handConfidence = handConfidence;
            this.bet = bet;
            percentageOfFunds = bet / total;

            if (bet > minBet)
            {
                betHigherThanMinimum = true;
            }
            else
            {
                betHigherThanMinimum = false;
            }
        }

        public double PercentageOfFunds
        {
            get { return percentageOfFunds; }
        }
        public void setPercentageOfFunds(int bet, int total)
        {
            percentageOfFunds = bet / total;
        }
        public Hand HandConfidence
        {
            get { return handConfidence; }
            set { handConfidence = value; }
        }
        public bool BetHigherThanMinimum
        {
            get { return betHigherThanMinimum; }
            set { betHigherThanMinimum = value; }
        }
        public int Bet
        {
            get { return bet; }
            set { bet = value; }
        }
    }
}
