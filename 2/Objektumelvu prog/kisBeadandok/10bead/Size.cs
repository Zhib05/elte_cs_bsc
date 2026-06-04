using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF10
{
    public interface Size
    {
        public int Multi();
    }

    public class S : Size
    {
        private static S? _instance = null;
        public static S Instance() => _instance ??= new S();
        private S() { }

        public int Multi()
        {
            return 1;
        }
    }

    public class M : Size
    {
        private static M? _instance = null;
        public static M Instance() => _instance ??= new M();
        private M() { }

        public int Multi()
        {
            return 2;
        }
    }

    public class L : Size
    {
        private static L? _instance = null;
        public static L Instance() => _instance ??= new L();
        private L() { }
        public int Multi()
        {
            return 3;
        }
    }

    public class XL : Size
    {
        private static XL? _instance = null;
        public static XL Instance() => _instance ??= new XL();
        private XL() { }
        public int Multi()
        {
            return 4;
        }
    }
}
