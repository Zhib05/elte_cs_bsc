using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF9
{
    public class Solarsystem
    {
        public List<Planet> planets;

        public Solarsystem()
        {
            planets = new List<Planet>();
        }

        public (bool, Starship) MaxFireP()
        {
            if (planets.Count == 0 || planets.All(p => p.ShipCount() == 0))
            {
                return (false, null);
            }

            //(bool l, double max, Starship ship) = planets[0].MaxFireP();
            double max = double.MinValue;
            Starship ship = null;


            foreach (Planet e in planets)
            {
                (bool l1, double max2, Starship maxs2) = e.MaxFireP();
                if (max2 > max)
                {
                    max = max2;
                    ship = maxs2;
                }
            }

            return (true, ship);
        }

        public List<Planet> Defenseless()
        {
            List<Planet> p = new List<Planet>();

            foreach (Planet e in planets)
            {
                if (e.ShipCount() == 0)
                {
                    p.Add(e);
                }
            }
            return p;
        }
    }
}
