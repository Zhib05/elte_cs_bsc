using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.DocuStat.Persistence
{
    internal class TextFileManager : IFileManager
    {
        private readonly string _filePath;
        public TextFileManager(string filePath)
        {
            _filePath = filePath;
        }
        public string Load()
        {
            try
            {
                var content = File.ReadAllText(_filePath);
                return content;
            } catch (Exception e)
            {
                throw new FileException($"Error occured: {e.Message}", e);
            }
        }
    }
}
