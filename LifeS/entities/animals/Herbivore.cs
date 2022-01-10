using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{
      
    public class Herbivore : Animals<Herbivore, FoodForHerbovire>, FoodForOmnivore, FoodForPredator
    {
        public new int viewDistance = 40;
        public Herbivore(int _x, int _y, Random rand) : base (_x,_y,rand)
        {            
        }
       
    }
}

