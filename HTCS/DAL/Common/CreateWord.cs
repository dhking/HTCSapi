
using Aspose.Words;
using NPOI.XWPF.UserModel;


using System.Configuration;
using System.IO;

using System.Text;
using System.Text.RegularExpressions;

namespace DAL.Common
{
    public class CreateWord
    {
        private DocumentBuilder oWordApplic; //   a   reference   to   Word   application 
     
        private int _docversion;
        public int Docversion
        {
            get { return _docversion; }
            set { _docversion = value; }
        }
        public bool Create(string html, string filename)
        {
            string path = ConfigurationManager.AppSettings["signpath"];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + "/" + filename;

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
            file = savepath + "//" + filename;
            string demo = ConfigurationManager.AppSettings["signpath"];
            file2 = demo + "//" + filename;
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

        public void SaveAsWord(string content,string file)
        {
            WriteText(file, content);
        }
        public void SaveAs(string strFileName, Aspose.Words.Document oDoc)
        {

            if (this.Docversion == 2007)
            {
                oDoc.Save(strFileName, SaveFormat.Docx);
            }
            else
            {
                oDoc.Save(strFileName, SaveFormat.Doc);
            }
           
        }
        public void WriteText(string filename, string content)
        {
            string path = ConfigurationManager.AppSettings["signpath"];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + "/" + filename;
            Aspose.Words.Document oDoc = new Aspose.Words.Document();
            oWordApplic = new DocumentBuilder(oDoc);
            oWordApplic.Font.Size = 14;
            oWordApplic.PageSetup.PaperSize = PaperSize.A4;
            string html = "<html>" + content + "</html>";
            oWordApplic.InsertHtml(html);
            SaveAs(path, oDoc);
        }
        private Run SplitRun(Run run, int position)
        {
            Run beforeRun = (Run)run.Clone(true);
            beforeRun.Text ="一定要成功吧";
            beforeRun.Font.Underline = Underline.Single;
            run.Text = run.Text.Substring(position);

            run.ParentNode.InsertBefore(beforeRun, run);

            return run;
        }

        public void ReplaceWithEvaluator()
        {

            Aspose.Words.Document doc = new Aspose.Words.Document("");

            doc.Range.Replace(new Regex("[s|m]ad"), new MyReplaceEvaluator(), true);

            doc.Save("");

        }
    }

    public class MyReplaceEvaluator : IReplacingCallback

    {

        /// <summary>

        /// This is called during a replace operation each time a match is found.

        /// This method appends a number to the match string and returns it as a replacement string.

        /// </summary>

        ReplaceAction IReplacingCallback.Replacing(ReplacingArgs e)

        {

            e.Replacement = e.Match.ToString() + mMatchNumber.ToString();

            mMatchNumber++;

            return ReplaceAction.Replace;

        }

        private int mMatchNumber;

    }
}
