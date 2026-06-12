using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF8
{
     class Folder:Registration
     {
        private List<Registration> items;
        public Folder()
        {
            items = new List<Registration>();
        }
        public override int GetSize()
        {
            int sum = 0;
            foreach(Registration e in items) 
            {
                sum += e.GetSize();
            }
            return sum;
        }
        public void Add(Registration r) 
        {
            items.Add(r);
        }
        public void Remove(Registration r) 
        {
            items.Remove(r);
        }
     }
}
