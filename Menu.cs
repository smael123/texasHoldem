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
    public partial class Menu : Form
    {
        string p1Name;
        Table table;

        public Menu()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            p1Name = txtbxName.Text;
            table = new Table(p1Name);
            table.ShowDialog();
        }
    }
}
