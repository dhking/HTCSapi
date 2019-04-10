using Microsoft.Office.Interop.Word;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
   public   class CreateWord
    {
       
        public bool Create(string html,string filename)
        {
            string path = ConfigurationManager.AppSettings["signpath"];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + "/"+filename;
          
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite,
             FileShare.ReadWrite))
            {
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                {
                    writer.Write(html);
                    writer.Flush();
                    writer.Dispose();
                }
                fs.Dispose();
            }
            return true;
        }
        private string createWord(string filename, string savepath)
        {
            string file = ""; //路径1（我们之前生成的文件路径）
            string file2 = "";//路径2
            file = savepath+"//" + filename ;
            string demo = ConfigurationManager.AppSettings["signpath"];
            file2 = demo + "//"+ filename;
            if (File.Exists(file2))
            {
                File.Delete(file2);
            }
           
            using (FileStream stream = new FileStream(@"E:\html\contract_474.docx", FileMode.Open))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                FileStream sw = File.Create(file2);
                doc.Write(sw);
                sw.Close();
            }
            //删除原本生成的文件
            File.Delete(file);
            return file2;
        }
    }
}
