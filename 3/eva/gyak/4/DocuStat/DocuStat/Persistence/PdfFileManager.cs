using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace ELTE.DocuStat.Persistence
{
    public class PdfFileManager : IFileManager
    {
        private readonly string _path;
        public PdfFileManager(string filePath)
        {
            _path = filePath;
        }
        public string Load()
        {
            using PdfReader reader = new PdfReader(_path);
            using PdfDocument document = new PdfDocument(reader);
            try
            {
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= document.GetNumberOfPages(); i++)
                {
                    PdfPage page = document.GetPage(i);
                    text.Append(PdfTextExtractor.GetTextFromPage(page));
                }
                return text.ToString();
            }
            catch (Exception ex)
            {
                throw new FileException($"Error occured: {ex.Message}", ex);
            }
        }
    }
}
