using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackOpening
{
    public abstract class Card
    {
        public int cost;
        protected int defending;
        protected int passing;
        protected int shooting;
        
        public Player? player;
        public CardType? cardType;

        public Card(int cost, int defending, int passing, int shooting)
        {
            this.cost = cost;
            this.defending = defending;
            this.passing = passing;
            this.shooting = shooting;
            this.player = null;
            this.cardType = null;
        }

        public void LinkToPlayer(Player player)
        {
            this.player = player;
        }

        public abstract int Rating();

        public virtual bool isDefender() { return false; }
        public virtual bool isMidfielder() { return false; }
        public virtual bool isAttacker() { return false; }

        public void setRarity(CardType cardType)
        {
            this.cardType = cardType;
        }
        public abstract void BonusStats();
    }

    public class Defender : Card
    {
        public Defender(int cost, int defending, int passing, int shooting) : base(cost, defending, passing, shooting) { }
        public override int Rating()
        {
            return defending;
        }
        public override bool isDefender()
        {
            return true;
        }
        public override void BonusStats()
        {
            defending += cardType!.BootStats(this);
            cost += cardType.BootStats(this);
        }
    }

    public class Midfielder : Card
    {
        public Midfielder(int cost, int defending, int passing, int shooting) : base(cost, defending, passing, shooting) { }
        public override int Rating()
        {
            return passing;
        }
        public override bool isMidfielder()
        {
            return true;
        }
        public override void BonusStats()
        {
            defending += cardType!.BootStats(this);
            cost += cardType.BootStats(this);
        }
    }

    public class Attacker : Card
    {
        public Attacker(int cost, int defending, int passing, int shooting) : base(cost, defending, passing, shooting) { }
        public override int Rating()
        {
            return shooting;
        }
        public override bool isAttacker()
        {
            return true;
        }
        public override void BonusStats()
        {
            defending += cardType!.BootStats(this);
            cost += cardType.BootStats(this);
        }
    }
}
