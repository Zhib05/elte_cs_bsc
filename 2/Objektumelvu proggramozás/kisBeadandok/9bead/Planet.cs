namespace HF9
{
    public class Planet
    {
        public string name;
        private List<Starship> ships;

        public Planet(string name)
        {
            this.name = name;
            ships = new List<Starship>();
        }

        public void Defends(Starship s)
        {
            if (ships.Contains(s))
            {
                throw new Exception();
            }
            ships.Add(s);
        }

        public void Leaves(Starship s)
        {
            if (!ships.Contains(s))
            {
                throw new Exception();
            }
            ships.Remove(s);
        }

        public int ShipCount()
        {
            return ships.Count;
        }

        public int ShieldSum()
        {
            int sum = 0;
            foreach (Starship e in ships)
            {
                sum += e.GetShield();
            }

            return sum;
        }

        public (bool, double, Starship) MaxFireP()
        {
            if (ships.Count == 0)
            {
                return (false, 0, null);
            }

            double maxValue = ships[0].FireP();
            Starship maxShip = ships[0];
            bool l = true;

            foreach (Starship e in ships)
            {
                if (e.FireP() > maxValue)
                {
                    maxValue = e.FireP();
                    maxShip = e;
                }
            }

            return (l, maxValue, maxShip);
        }
    }
}