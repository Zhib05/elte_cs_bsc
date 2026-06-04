using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF10
{
    public class Gift
    {
        public TargetShot targetShot;
        protected Size _size;

        public Size Size() => _size;
        public int Value()
        {
            return Point() * _size.Multi();
        }

        public virtual int Point()
        {
            return 0;
        }
    }
}
