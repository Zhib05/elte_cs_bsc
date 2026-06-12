using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ELTE.DocuStat.Persistence
{
    internal class FileException : IOException
    {
        public FileException() : base() { }
        public FileException(string message) : base(message) { }
        public FileException(string message, Exception e) : base(message, e) { }
    }
}
