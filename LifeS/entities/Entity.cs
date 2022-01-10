using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{
    public abstract class Entity
    {
        public int x;
        public int y;
        public bool alive;        

        public Entity(int _x, int _y) {
            x = _x;
            y = _y;
            alive = true;
        }

        public void Dead()
        {
            alive = false;
        }
    }
}
