using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{
    public abstract class Animal : Entity
        
    {
        public int satiety;
        public bool changed = false;
        public int timeLastChild;        
        public Gender gender;
        public int viewDistance = 40;
        public Random random;
        public Animal(int _x,int _y,Random rand):  base(_x,_y)
        {
            x = _x;
            y = _y;
            alive = true;
            random = rand;
            timeLastChild = random.Next(70) + 50;
            satiety = random.Next(30) + 70;


            if (random.Next(2) == 0)
                gender = Gender.male;
            else
                gender = Gender.female;
        }

        public abstract void DoSomething(int _x, int _y, Cell[,] field);
    }
}
