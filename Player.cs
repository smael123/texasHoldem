using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoldemHand;

namespace texasHoldem
{
    class Player
    {
        protected string name; //nombre means name ;name almost spells namespace and it constantly corrects
        protected Card[] hand;
        protected int funds;
        protected List<TurnInfo> turnInfoList;

        public Player()
        {
            this.name = "Jack";
            funds = Table.INITIAL_FUNDS;
            hand = new Card[2];
            turnInfoList = new List<TurnInfo>();
            
        }
        public Player(string nombre)
        {
            this.name = nombre;
            funds = Table.INITIAL_FUNDS;
            hand = new Card[2];
            turnInfoList = new List<TurnInfo>();
            
        }

        public int bet(int betAmount, int minBetAmount, string boardStr)
        {
            if (betAmount < funds)
            {
                int moneyToBet = funds;
                funds -= moneyToBet;
                addTurnInfo(moneyToBet, minBetAmount, boardStr);
                return moneyToBet;
            }
            else 
            {
                funds -= betAmount;
                addTurnInfo(betAmount, minBetAmount, boardStr);
                return betAmount;
            }  
        }
        public void setCard(int i, Card card) //check if it clears both
        {
            hand[i] = card;
        }
        public Card getCard(int i)
        {
            return hand[i];
        }
        public void increaseFunds(int winAmount)
        {
            funds += winAmount;
        }
        public void addTurnInfo(int bet, int minBet, string boardStr)
        {
            turnInfoList.Add(new TurnInfo(bet, minBet, funds + bet, new Hand(getHandStr(), boardStr)));
        }
        public void clearTurnInfo()
        {
            turnInfoList.Clear();
        }
        public String getHandStr()
        {
            StringBuilder handStr = new StringBuilder();

            int len = hand.Length;

            for (int i = 0; i < len; i++)
            {
                handStr.Append(hand[i].ToString());
                handStr.Append(" ");
            }

            return handStr.ToString();
        }

        public void setName(string nombre) { this.name = nombre; }
        public string getName() { return name; }

        public List<TurnInfo> getTurnInfoList()
        {
            return turnInfoList;
        }

        public int getFunds()
        {
            return funds;
        }
    }
}
