using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{   
    public class Predator : Animals<Predator,FoodForPredator>
    {
        public new int viewDistance = 70;
        public Predator(int _x, int _y, Random rand) : base(_x, _y, rand)
        {           
        }      
    }
}
