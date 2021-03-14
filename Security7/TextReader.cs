using GemBox.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace Security7
{
    static class TextReader
    {
        public static string directory = "";
        public static string getText(string file)
        {
            var info = new FileInfo(file);
            string text = "";
            if (info.Extension.Contains("txt"))
            {
                text = File.ReadAllText(file);
            }
            else
            {
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                var document = DocumentModel.Load(file);
                text = document.Content.ToString();
            }
            return text;
        }

        public static string WriteToFile(CustomFile r)
        {
            string endFile = "";
            if (r.Type == TextType.Encrypted)
            {
                r.Name = r.Name.Replace("_decrypted", "");
                endFile = $"{directory}/{r.Name}_encrypted{r.Extension}";
                toFile(r.Extension, endFile, r.Text);
            }
            else
            {
                r.Name = r.Name.Replace("_encrypted", "");
                endFile = $"{directory}/{r.Name}_decrypted{r.Extension}";
                toFile(r.Extension, endFile, r.Text);
            }
            return endFile;
        }
        static void toFile(string ext ,string endFile,string text)
        {
            if(ext.Contains("txt"))
            {
                File.WriteAllText(endFile, text);
            }
            else
            {
                Word.Application application = new Word.Application();
                Word.Document document = application.Documents.Add();
                Word.Paragraph paragraph = document.Paragraphs.Add();
                paragraph.Range.Text = text;
                document.SaveAs2(endFile);
                document.Close();
                application.Quit();
            }
        }
    }
}
