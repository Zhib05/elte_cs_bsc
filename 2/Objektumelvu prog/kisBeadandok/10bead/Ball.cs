using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF10
{
    public class Ball : Gift
    {
        public Ball(Size s)
        {
            _size = s;
        }
        public override int Point()
        {
            return 1;
        }
    }
}
