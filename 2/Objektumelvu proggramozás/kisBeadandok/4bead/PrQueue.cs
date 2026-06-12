using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF4
{
    class PrQueue
    {
        public class EmptyQueueException : Exception { }

        private List<Element> seq;

        public PrQueue()
        {
            seq = new List<Element>();
        }

        public void SetEmpty()
        {
            seq.Clear();
        }

        public bool isEmpty()
        {
            return seq.Count == 0;
        }

        public void Add(Element e)
        {
            seq.Add(e);
        }

        public Element GetMax()
        {
            if (seq.Count == 0)
            {
                throw new EmptyQueueException();
            }

            (int max, int ind) = MaxSelect();
            return seq[ind];
        }

        public Element RemMax()
        {
            if (seq.Count == 0)
            {
                throw new EmptyQueueException();
            }
            (int max, int ind) = MaxSelect();
            Element e = seq[ind];
            seq.Remove(e);
            return e;
        }

        private (int, int) MaxSelect()
        {
            if (seq.Count == 0)
            {
                throw new EmptyQueueException();
            }

            int max = seq[0].pr;
            int ind = 0;
            for (int i = 1; i < seq.Count; ++i)
            {
                if (seq[i].pr > max)
                {
                    max = seq[i].pr;
                    ind = i;
                }
            }

            return (max, ind);
        }
    }
}
