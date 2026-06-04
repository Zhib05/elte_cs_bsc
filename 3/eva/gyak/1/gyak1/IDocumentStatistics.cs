using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gyak1
{
    public interface IDocumentStatistics
    {
        public string FileContent { get; set; }
        void Load();
    }
}
