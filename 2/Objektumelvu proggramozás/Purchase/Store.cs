using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase
{
    class Store
    {
        public Department Foods;
        public Department Technical;

        public Store(Department foods, Department technical)
        {
            Foods = foods;
            Technical = technical;
        }
    }
}
