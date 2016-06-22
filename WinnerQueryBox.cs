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
    public partial class WinnerQueryBox : Form
    {
        private int answer; // 0 win 1 lose 2 tie

        public int Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        public WinnerQueryBox()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            answer = 0;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            answer = 1;
        }

        private void btnTie_Click(object sender, EventArgs e)
        {
            answer = 2;
        }
        
    }
}
