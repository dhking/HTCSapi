using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace SignApplication
{
    public partial class createContract : Form
    {
        private Form1 f1;
        private DialogResult dis;
        public createContract(Form1 f1)
        {
            InitializeComponent();
            this.f1 = f1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.folderBrowserDialog1
            //DialogResult dires = this.folderBrowserDialog1.ShowDialog();
            dis = this.openFileDialog1.ShowDialog();
            /*if (dis == System.Windows.Forms.DialogResult.OK) {
                FileStream fs = File.OpenRead(openFileDialog1.FileName);//传文件的路径即可
                BinaryReader br = new BinaryReader(fs);
                byte[] bt = br.ReadBytes(Convert.ToInt32(fs.Length));
                string contract_file = Convert.ToBase64String(bt);
            }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dis == System.Windows.Forms.DialogResult.OK)
            {
                FileStream fs = File.OpenRead(openFileDialog1.FileName);//传文件的路径即可
                BinaryReader br = new BinaryReader(fs);
                byte[] bt = br.ReadBytes(Convert.ToInt32(fs.Length));
                string contract_file = Convert.ToBase64String(bt);

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                
                parameters.Add("no", this.no.Text);
                parameters.Add("name", this.name.Text);
                parameters.Add("contract", contract_file);
                parameters.Add("zqid", Config.ZQID);
                //计算签名值
                string context = RSAUtil.GetSignContent(parameters);
                string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
                //将合同文件和签名值进行url编码 放入parameters中
                contract_file = HttpUtility.UrlEncode(contract_file, Encoding.UTF8);
                parameters.Remove("contract");
                parameters.Add("contract", contract_file);
                
                sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);
               
                parameters.Add("sign_val", sign_val);
                string result = HTTPUtil.CreatePostHttpResponse(Config.URL + "uploadPdf", parameters);
                MessageBox.Show(result);
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            f1.Show();
            this.Close();
        }
    }
}
