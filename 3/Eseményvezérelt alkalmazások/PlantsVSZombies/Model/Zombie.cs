using System;
using System.Collections.Generic;
using System.Linq; // Fontos a listák kezeléséhez (pl. lövés) 
using System.Timers; // A logikai időzítőhöz 

namespace PlantsVsZombies
{
    // Segédosztály: Egy zombi adatai 
    public class Zombie
    {
        public int Row { get; set; }
        public int Col { get; set; }
        // Ha true, épp eszik, tehát ebben a körben nem lépett 
        public bool JustAte { get; set; }

        public Zombie(int r, int c)
        {
            Row = r;
            Col = c;
            JustAte = false;
        }
    }
}
