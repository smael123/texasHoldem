using HoldemHand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace texasHoldem
{
    public partial class Table : Form
    {
        public const int INITIAL_FUNDS = 2000;
        public const int INITIAL_BLINDS = 200;
        
        Player p1;
        ComputerPlayer com;
        int turn;
        int blinds;
        private Stack<Card> deck;
        private int communityPot;
        private List<Card> communityHand;
        private Card[] unshuffledDeck;
        private int minBetAmount;
        


        public Table()
        {
            InitializeComponent(); //this is usually in the base constructor do not remove it
            initializeValues();
            initializeCards();
            shuffleDeck();
            updateFormValues();
            dealCards();     
        }
        public Table(string p1Name)
        {
            InitializeComponent(); //this is usually in the base constructor do not remove it
            initializeValues(p1Name);
            initializeCards();
            shuffleDeck();
            updateFormValues();
            dealCards();     
        }

        private void dealCards()
        {
            if (turn == 1) //pre flop
            {
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        p1.setCard(i, deck.Pop());
                    }
                    catch (System.InvalidOperationException e) //if stack is empty
                    {
                        shuffleDeck();
                        p1.setCard(i, deck.Pop());           
                    }  
                }
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        com.setCard(i, deck.Pop());
                        
                    }
                    catch (System.InvalidOperationException e)
                    {
                        shuffleDeck();
                        com.setCard(i, deck.Pop());
                        
                    }
                }

                picBxP1C1.Image = p1.getCard(0).CardPic;
                picBxP1C2.Image = p1.getCard(1).CardPic;
                picBxCOMC1.Image = texasHoldem.Properties.Resources.Card_back_06;
                picBxCOMC2.Image = texasHoldem.Properties.Resources.Card_back_06;
            }
            else if (turn == 2) //flop
            {
                
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        communityHand.Add(deck.Pop());
                        
                    }
                    catch (System.InvalidOperationException e)
                    {
                        shuffleDeck();
                        communityHand.Add(deck.Pop());
                    }
                }

                
                picBxShareC1.Image = communityHand[0].CardPic;
                picBxShareC2.Image = communityHand[1].CardPic;
                picBxShareC3.Image = communityHand[2].CardPic;
            }
            else if (turn == 3) //turn
            {
                try
                {
                    communityHand.Add(deck.Pop());
                }
                catch (System.InvalidOperationException e)
                {
                    shuffleDeck();
                    communityHand.Add(deck.Pop());
                }
                
                picBxShareC4.Image = communityHand[3].CardPic;
            }
            else if (turn == 4) //river
            {
                try
                {
                    communityHand.Add(deck.Pop());
                }
                catch (System.InvalidOperationException e)
                {
                    shuffleDeck();
                    communityHand.Add(deck.Pop());
                }
                
                picBxShareC1.Image = communityHand[4].CardPic; //this is the only diff between 3 and 4 maybe put the picBxes in a collection
            }
            else
            {
                beginShowdown();
            }
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            lblMinBet.Text = p1.bet(Int32.Parse(lblMinBet.Text), minBetAmount, getBoardStr()).ToString(); //will this work?
            communityPot += Int32.Parse(lblMinBet.Text);
            lblMinBet.Text = com.bet(Int32.Parse(lblMinBet.Text)).ToString();
            communityPot += Int32.Parse(lblMinBet.Text);
            if (lblMinBet.Text.Equals("-1")) //if com folded
            {
                p1.increaseFunds(communityPot);
                com.performEndOfRoundCalculations(false);
                beginNewRound();
            }
            ++turn;
            dealCards();
        }

        private void btnFold_Click(object sender, EventArgs e)
        {
            com.increaseFunds(communityPot);
            com.performEndOfRoundCalculations(true);
            beginNewRound();
        }
        public void beginShowdown()
        {
            picBxCOMC1.Image = com.getCard(0).CardPic;
            picBxCOMC1.Image = com.getCard(1).CardPic;
            determineWinner();
            beginNewRound();
        }
        public void beginNewRound()
        {
            turn = 1;
            lblMinBet.Text = blinds.ToString();
            communityPot = 0;
            updateFormValues();
            communityHand.Clear();
            dealCards();
        }

        
        public void determineWinner()
        {
            WinnerQueryBox w = new WinnerQueryBox();
            ShowDialog(w);
            if (w.Answer == 0)
            {
                p1.increaseFunds(communityPot);
                com.performEndOfRoundCalculations(p1.getTurnInfoList(), false);
            }
            else if (w.Answer == 1)
            {
                com.increaseFunds(communityPot);
                com.performEndOfRoundCalculations(p1.getTurnInfoList(), true);
            }
            else
            {
                if (communityPot % 2 == 1)
                {
                    p1.increaseFunds((communityPot - 1) * 2);
                    com.increaseFunds((communityPot - 1) * 2);
                    p1.increaseFunds(1); // let the human get it
                }
            }
            w.Close();
        }

        public void shuffleDeck()
        {
            Random rand = new Random();
            int r;

            for (int i = 0; i < 52; i++)
            {
                r = rand.Next(0, i);
                Card temp = unshuffledDeck[i];
                unshuffledDeck[i] = unshuffledDeck[r];
                unshuffledDeck[r] = temp;
            }

            for (int i = 0; i < 52; i++)
            {
                deck.Push(unshuffledDeck[i]);
            }
        }

        private void btnBet_Click(object sender, EventArgs e)
        {
            int betAm = Int32.Parse(txtBxBetAmount.Text);

            if (betAm < Int32.Parse(lblMinBet.Text))
            {
                MessageBox.Show("You need to bet at least: " + lblMinBet.Text);
            }
            else 
            {
                minBetAmount = p1.bet(betAm, minBetAmount, getBoardStr());
                communityPot += betAm;
                
                minBetAmount = com.bet(minBetAmount);
                updateFormValues();
            }
            
        }

        public String getBoardStr()
        {
            StringBuilder boardStr = new StringBuilder();

            int len = communityHand.Count;

            for (int i = 0; i < len; i++)
            {
                boardStr.Append(communityHand[i].ToString());
                boardStr.Append(" ");
            }

            return boardStr.ToString(); //will return "" if nothing which is fine
        }

        public void updateFormValues()
        {
            lblMinBet.Text = minBetAmount.ToString();
            lblCommPot.Text = communityPot.ToString();
            lblBlinds.Text = blinds.ToString();
            lblCom.Text = communityPot.ToString();
            lblP1Fund.Text = p1.getFunds().ToString();
            lblComFund.Text = p1.getFunds().ToString();
        }

        private void initializeValues()
        {
            p1 = new Player();
            com = new ComputerPlayer();

            unshuffledDeck = new Card[52];
            deck = new Stack<Card>();
            communityHand = new List<Card>();

            turn = 1;
            blinds = INITIAL_BLINDS;
            minBetAmount = 0;

            lblP1.Text = p1.getName();
            lblCom.Text = com.getName();
        }

        private void initializeValues(string p1Name)
        {
            p1 = new Player(p1Name);
            
            com = new ComputerPlayer();

            unshuffledDeck = new Card[52];
            deck = new Stack<Card>();
            communityHand = new List<Card>();

            turn = 1;
            blinds = INITIAL_BLINDS;
            minBetAmount = 0;

            lblP1.Text = p1.getName();
            lblCom.Text = com.getName();
        }

        private void initializeCards()
        {
            unshuffledDeck[0] = new Card(0, 2, texasHoldem.Properties.Resources.Playing_card_heart_2);
            unshuffledDeck[1] = new Card(0, 3, texasHoldem.Properties.Resources.Playing_card_heart_3);
            unshuffledDeck[2] = new Card(0, 4, texasHoldem.Properties.Resources.Playing_card_heart_4);
            unshuffledDeck[3] = new Card(0, 5, texasHoldem.Properties.Resources.Playing_card_heart_5);
            unshuffledDeck[4] = new Card(0, 6, texasHoldem.Properties.Resources.Playing_card_heart_6);
            unshuffledDeck[5] = new Card(0, 7, texasHoldem.Properties.Resources.Playing_card_heart_7);
            unshuffledDeck[6] = new Card(0, 8, texasHoldem.Properties.Resources.Playing_card_heart_8);
            unshuffledDeck[7] = new Card(0, 9, texasHoldem.Properties.Resources.Playing_card_heart_9);
            unshuffledDeck[8] = new Card(0, 10, texasHoldem.Properties.Resources.Playing_card_heart_10);
            unshuffledDeck[9] = new Card(0, 11, texasHoldem.Properties.Resources.Playing_card_heart_J);
            unshuffledDeck[10] = new Card(0, 12, texasHoldem.Properties.Resources.Playing_card_heart_Q);
            unshuffledDeck[11] = new Card(0, 13, texasHoldem.Properties.Resources.Playing_card_heart_K);
            unshuffledDeck[12] = new Card(0, 14, texasHoldem.Properties.Resources.Playing_card_heart_A);

            unshuffledDeck[13] = new Card(1, 2, texasHoldem.Properties.Resources.Playing_card_diamond_2);
            unshuffledDeck[14] = new Card(1, 3, texasHoldem.Properties.Resources.Playing_card_diamond_3);
            unshuffledDeck[15] = new Card(1, 4, texasHoldem.Properties.Resources.Playing_card_diamond_4);
            unshuffledDeck[16] = new Card(1, 5, texasHoldem.Properties.Resources.Playing_card_diamond_5);
            unshuffledDeck[17] = new Card(1, 6, texasHoldem.Properties.Resources.Playing_card_diamond_6);
            unshuffledDeck[18] = new Card(1, 7, texasHoldem.Properties.Resources.Playing_card_diamond_7);
            unshuffledDeck[19] = new Card(1, 8, texasHoldem.Properties.Resources.Playing_card_diamond_8);
            unshuffledDeck[20] = new Card(1, 9, texasHoldem.Properties.Resources.Playing_card_diamond_9);
            unshuffledDeck[21] = new Card(1, 10, texasHoldem.Properties.Resources.Playing_card_diamond_10);
            unshuffledDeck[22] = new Card(1, 11, texasHoldem.Properties.Resources.Playing_card_diamond_J);
            unshuffledDeck[23] = new Card(1, 12, texasHoldem.Properties.Resources.Playing_card_diamond_Q);
            unshuffledDeck[24] = new Card(1, 13, texasHoldem.Properties.Resources.Playing_card_diamond_K);
            unshuffledDeck[25] = new Card(1, 14, texasHoldem.Properties.Resources.Playing_card_diamond_A);

            unshuffledDeck[26] = new Card(2, 2, texasHoldem.Properties.Resources.Playing_card_club_2);
            unshuffledDeck[27] = new Card(2, 3, texasHoldem.Properties.Resources.Playing_card_club_3);
            unshuffledDeck[28] = new Card(2, 4, texasHoldem.Properties.Resources.Playing_card_club_4);
            unshuffledDeck[29] = new Card(2, 5, texasHoldem.Properties.Resources.Playing_card_club_5);
            unshuffledDeck[30] = new Card(2, 6, texasHoldem.Properties.Resources.Playing_card_club_6);
            unshuffledDeck[31] = new Card(2, 7, texasHoldem.Properties.Resources.Playing_card_club_7);
            unshuffledDeck[32] = new Card(2, 8, texasHoldem.Properties.Resources.Playing_card_club_8);
            unshuffledDeck[33] = new Card(2, 9, texasHoldem.Properties.Resources.Playing_card_club_9);
            unshuffledDeck[34] = new Card(2, 10, texasHoldem.Properties.Resources.Playing_card_club_10);
            unshuffledDeck[35] = new Card(2, 11, texasHoldem.Properties.Resources.Playing_card_club_J);
            unshuffledDeck[36] = new Card(2, 12, texasHoldem.Properties.Resources.Playing_card_club_Q);
            unshuffledDeck[37] = new Card(2, 13, texasHoldem.Properties.Resources.Playing_card_club_K);
            unshuffledDeck[38] = new Card(2, 14, texasHoldem.Properties.Resources.Playing_card_club_A);

            unshuffledDeck[39] = new Card(3, 2, texasHoldem.Properties.Resources.Playing_card_spade_2);
            unshuffledDeck[40] = new Card(3, 3, texasHoldem.Properties.Resources.Playing_card_spade_3);
            unshuffledDeck[41] = new Card(3, 4, texasHoldem.Properties.Resources.Playing_card_spade_4);
            unshuffledDeck[42] = new Card(3, 5, texasHoldem.Properties.Resources.Playing_card_spade_5);
            unshuffledDeck[43] = new Card(3, 6, texasHoldem.Properties.Resources.Playing_card_spade_6);
            unshuffledDeck[44] = new Card(3, 7, texasHoldem.Properties.Resources.Playing_card_spade_7);
            unshuffledDeck[45] = new Card(3, 8, texasHoldem.Properties.Resources.Playing_card_spade_8);
            unshuffledDeck[46] = new Card(3, 9, texasHoldem.Properties.Resources.Playing_card_spade_9);
            unshuffledDeck[47] = new Card(3, 10, texasHoldem.Properties.Resources.Playing_card_spade_10);
            unshuffledDeck[48] = new Card(3, 11, texasHoldem.Properties.Resources.Playing_card_spade_J);
            unshuffledDeck[49] = new Card(3, 12, texasHoldem.Properties.Resources.Playing_card_spade_Q);
            unshuffledDeck[50] = new Card(3, 13, texasHoldem.Properties.Resources.Playing_card_spade_K);
            unshuffledDeck[51] = new Card(3, 14, texasHoldem.Properties.Resources.Playing_card_spade_A);
        }
        
    }
}
