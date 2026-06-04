using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF10
{
    public class Figure : Gift
    {
        public Figure(Size s)
        {
            _size = s;
        }
        public override int Point()
        {
            return 2;
        }
    }
}
