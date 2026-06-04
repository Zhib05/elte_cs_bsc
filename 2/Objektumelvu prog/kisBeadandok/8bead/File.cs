using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF8
{
     public class File: Registration
    {
        private int size;
        public File(int size)
        { 
            this.size = size;
        }
        public override int GetSize()
        {
            return size;
        }
    }
}
