using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SignApplication
{
    public partial class selectType : Form
    {
        private Form1 f1;
        
        public selectType(Form1 f1)
        {
            InitializeComponent();
            this.f1 = f1;
            f1.Hide();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string no = this.no.Text;
            string user_code = this.userCode.Text;
            string sign_type = this.selectName.Text;

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("no", no);
            parameters.Add("zqid", Config.ZQID);


            if (sign_type.Equals("自动签署"))
            {
                parameters.Add("signers", user_code);
                string context = RSAUtil.GetSignContent(parameters);
                string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
                sign_val = HttpUtility.UrlEncode(sign_val, Encoding.UTF8);
                parameters.Add("sign_val", sign_val);
                HTTPUtil.CreatePostHttpResponse(Config.URL + "signAuto", parameters);
                string res = HTTPUtil.CreatePostHttpResponse(Config.URL + "signAuto", parameters);
                MessageBox.Show(res);

            }
            else
            {

                parameters.Add("sign_type", sign_type);
                parameters.Add("user_code", user_code);
                parameters.Add("notify_url", "https://sign.zqsign.com");
                parameters.Add("return_url", "https://sign.zqsign.com");
                string context = RSAUtil.GetSignContent(parameters);
                string sign_val = RSAUtil.sign(context, Config.PRIVATE_KEY);
                parameters.Add("sign_val", sign_val);

                string html = HTTPUtil.formatFormatHtml(parameters, Config.URL + "signView");

                string htmls = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title></title></head><body>"+ html + "</body></html>";

                //实际开发中请将html放入页面中会自动跳转到众签签署页面

                //👇👇👇👇👇👇👇👇👇--------------------------------------👆👆👆👆👆👆👆👆
                //将拼装好的html文件写入本地  待用浏览器运行  
                //winform web浏览器是IE7   此页面不兼容IE7  所以放弃  将文件保存在本地用其他浏览器打开
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "文件保存|*.html";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;
                sfd.FileName = "signview.html";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(sfd.FileName, false);
                    sw.WriteLine(htmls);
                    sw.Close();//写入
                    MessageBox.Show("请用浏览器打开："+ sfd.FileName.ToString()+"此页面");
                }

            }
            
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            f1.Show();
        }
    }
}
