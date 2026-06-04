using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackOpening
{
    public abstract class CardType
    {
        public virtual int BootStats(Defender defender) { return 0; }
        public virtual int BootStats(Midfielder midfielder) { return 0; }
        public virtual int BootStats(Attacker attacker) { return 0; }
    }

    public class TOTY : CardType
    {
        private static TOTY? instance;
        private TOTY() { }
        public static TOTY Instance()
        {
            if (instance == null)
            {
                instance = new TOTY();
            }
            return instance;
        }
        public override int BootStats(Defender defender) { return 4; }
        public override int BootStats(Midfielder midfielder) { return 6; }
        public override int BootStats(Attacker attacker) { return 7; }
    }

    public class Rare : CardType
    {
        private static Rare? instance;
        private Rare() { }
        public static Rare Instance()
        {
            if (instance == null)
            {
                instance = new Rare();
            }
            return instance;
        }
        public override int BootStats(Defender defender) { return 1; }
        public override int BootStats(Midfielder midfielder) { return 2; }
        public override int BootStats(Attacker attacker) { return 1; }
    }

    public class Inform : CardType
    {
        private static Inform? instance;
        private Inform() { }
        public static Inform Instance()
        {
            if (instance == null)
            {
                instance = new Inform();
            }
            return instance;
        }
        public override int BootStats(Defender defender) { return 2; }
        public override int BootStats(Midfielder midfielder) { return 3; }
        public override int BootStats(Attacker attacker) { return 4; }
    }
}
