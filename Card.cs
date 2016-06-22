using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace texasHoldem
{
    class Card
    {
        private int suit; //0 hearts (♥), 1 diamonds (♦), 2 clubs (♣), 3 spades (♠)
        private int number;
        private Bitmap cardPic;

        public int Suit
        {
            get { return suit; }
            set { suit = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public Bitmap CardPic
        {
            get { return cardPic; }
        }

        public Card(int suit, int number, Bitmap cardPic)
        {
            this.suit = suit;
            this.number = number;
            this.cardPic = cardPic;
        }

        public void setCardPic(Bitmap obj)
        {
            cardPic = new Bitmap(obj);
 
        }
        public override string ToString()
        {
            string suitStr;
            string numStr;

            switch (suit)
            {
                case 0:
                    suitStr = "h";
                    break;
                case 1:
                    suitStr = "d";
                    break;
                case 2:
                    suitStr = "c";
                    break;
                default:
                    suitStr = "s";
                    break;
            }
            switch (number)
            {
                case 11:
                    numStr = "j";
                    break;
                case 12:
                    numStr = "q";
                    break;
                case 13:
                    numStr = "k";
                    break;
                case 14:
                    numStr = "a";
                    break;
                default:
                    numStr = number.ToString();
                    break;
            }

            return numStr + suitStr;
        }
    }
}
