using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            user user = new user(this);
            this.Hide();
            user.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            createContract createContract = new createContract(this);
            this.Hide();
            createContract.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new selectType(this);
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            download d = new download(this);
            d.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            configWin configWin = new configWin(this);
            configWin.Show();
            this.Hide();
        }
    }
}
