using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF10
{
    public class Plush : Gift
    {
        public Plush(Size s)
        {
            _size = s;
        }
        public override int Point()
        {
            return 3;
        }
    }
}
