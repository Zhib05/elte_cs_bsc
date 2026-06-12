using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Map_Skeleton.Map;

namespace Map_Skeleton
{
    public class Map
    {
        public class ExistingKeyException : Exception { }
        public class NotExistingKeyException : Exception { }
        public class NonExistingKeyException : Exception { }

        public List<Element> seq;

        public Map()
        {
            seq = new List<Element>();
        }

        public void SetEmpty()
        {
            seq = new List<Element> ();
            //seq.Clear();
        }

        public int Count()
        {
            return seq.Count;
        }

        public void Insert(Element element)
        {
            (bool l, int ind) = LogSearch(element.key);

            if (l)
            {
                throw new ExistingKeyException();
            }
            //seq.Add(element);
            seq.Insert(ind, element);
        }
        public void Remove(int key)
        {
            (bool l, int ind) = LogSearch(key);

            if (!l)
            {
                throw new NotExistingKeyException();
            }
            
            seq.RemoveAt(ind);
        }

        public bool In(int key)
        {
            (bool l, int ind) = LogSearch(key);
            return l;
        }

        private (bool, int) LogSearch(int key)
        {
            bool l = false;
            int ah = 0;
            int fh = seq.Count - 1;

            int ind = 0;

            while (!l && ah <= fh)
            {
                ind = (ah + fh) / 2;
                if (seq[ind].key == key)
                {
                    l = true;
                }
                else if (seq[ind].key > key)
                {
                    fh = ind - 1;
                }
                else if (seq[ind].key < key)
                {
                    ah = ind + 1;
                }
            }
            if (!l)
            {
                ind = ah;
            }
            return (l, ind);
        }

        public string Select(int key)

        {
            (bool l, int ind) = LogSearch(key);
            if (!l)
            {

                throw new NonExistingKeyException();

            }

            return seq[ind].data;

        }


        public override string ToString()
        {
            string returnString = "[";
            foreach(Element e in seq)
            {
                returnString += e.ToString();
            }
            return returnString + "]";
        }
    }
}
