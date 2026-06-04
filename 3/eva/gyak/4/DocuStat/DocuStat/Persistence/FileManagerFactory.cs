using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.DocuStat.Persistence
{
    public static class FileManagerFactory
    {
        public static IFileManager? CreateForPath(string path)
        {
            if (!File.Exists(path))
                return null;

            string extension = System.IO.Path.GetExtension(path);
            if (extension == ".txt")
            {
                return new TextFileManager(path);
            }
            else if (extension == ".pdf")
            {
                return new PdfFileManager(path);
            }
            else
            {
                return null;
            }
        }
    }
}
