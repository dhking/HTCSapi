using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
namespace SignApplication
{
    public partial class user : Form
    {
        private Form1 f1;
        public user(Form1 f1)
        {
           
            InitializeComponent();
            this.f1 = f1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.Show();
            this.Close();
        }

        private void tas_Click(object sender, EventArgs e)
        {
            string name = this.name.Text;
            string id_card_no = this.id_card_no.Text;
            string moblie = this.mobile.Text;
            string user_code = this.user_code.Text;
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("name", name);
            parameters.Add("id_card_no", id_card_no);
            parameters.Add("mobile", moblie);
            parameters.Add("user_code", user_code);
            parameters.Add("zqid", Config.ZQID);
            //MessageBox.Show(moblie);
            string context = RSAUtil.GetSignContent(parameters);
            string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
            sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);

            parameters.Add("sign_val", sign_val);

            string result = HTTPUtil.CreatePostHttpResponse(Config.URL+ "personReg", parameters);
            MessageBox.Show(result);

        }
    }
}
