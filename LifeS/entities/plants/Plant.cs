using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{
    public class Plant : Entity, FoodForOmnivore, FoodForHerbovire
    {

        public Plant(int _x, int _y) : base ( _x, _y)
        {
            alive = true;
        }     
    }
}
