using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SignApplication
{
    public partial class download : Form
    {
        private Form1 f1;
        public download(Form1 f1)
        {
            InitializeComponent();
            this.f1 = f1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string no = this.no.Text;
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("no", no);
            parameters.Add("zqid", Config.ZQID);

            string context = RSAUtil.GetSignContent(parameters);
            string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
            sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);
            parameters.Add("sign_val", sign_val);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文件保存|*.pdf";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.FileName = "download.pdf";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bool d = HTTPUtil.Download(Config.URL + "getPdf", parameters, sfd.FileName.ToString());
                if (d) {
                    MessageBox.Show("下载成功");
                }
               
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f1.Show();
            this.Close();
        }
    }
}
