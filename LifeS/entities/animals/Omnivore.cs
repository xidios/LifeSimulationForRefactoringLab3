using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{
    public class Omnivore : Animals<Omnivore, FoodForOmnivore>, FoodForPredator
    {
        public new int viewDistance = 30;
        public Omnivore(int _x, int _y, Random rand) : base(_x, _y, rand)
        {
        }       
    } 
}

