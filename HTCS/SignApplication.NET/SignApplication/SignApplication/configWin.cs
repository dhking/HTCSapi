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
    public partial class configWin : Form
    {
        private Form1 f1;
        public configWin(Form1 f1)
        {
            InitializeComponent();
            this.f1 = f1;
        }

        private void configWin_Load(object sender, EventArgs e)
        {
            this.zqid.Text = Config.ZQID;
            this.privateKey.Text = Config.PRIVATE_KEY;
            this.publicKey.Text = Config.PUBLIC_KEY;
            this.service_url.Text = Config.URL;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config.ZQID = this.zqid.Text;
            Config.PRIVATE_KEY = this.privateKey.Text;
            Config.PUBLIC_KEY = this.publicKey.Text;
            Config.URL = this.service_url.Text;
            MessageBox.Show("修改成功");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            f1.Show();
            this.Close();
        }
    }
}
