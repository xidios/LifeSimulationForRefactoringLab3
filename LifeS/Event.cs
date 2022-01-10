using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{
    public class Event
    {
        private bool exist = false;
        public int x;
        public int y;
        int distanse;
        Random rand;
        Direction direction;
        public bool EventIsExist() => exist;
        public Event(Random random, Cell[,] field)
        {
            exist = true;
            rand = random;
            x = rand.Next(field.GetLength(0));
            y = rand.Next(field.GetLength(1));
            distanse = rand.Next(100) + 5;
            direction = (Direction)rand.Next(4);
        }
        public void KillTheAll(Cell[,] field)
        {
            if (distanse == 0)
            {
                exist = false;
                return;
            }

            if (x < field.GetLength(0) && x >= 0 && y >= 0 && y < field.GetLength(1))
            {
                if (field[x, y].entity.Count > 0)               
                    KillAllEntities(field[x, y].entity);                              

                if (direction == Direction.Right)
                    x++;
                else if (direction == Direction.Left)
                    x--;
                else if (direction == Direction.Up)
                    y--;
                else if (direction == Direction.Down)
                    y++;

                distanse--;
            }
            else distanse = 0;


        }
        private void KillAllEntities(List<Entity> entity) 
        {
            foreach (Entity e in entity)           
                e.Dead();           

        }
        private void UpdateCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void MouseEvent(int x, int y, Cell[,] field)
        {
            UpdateCoordinates(x, y);
            KillTheAll(field);
        }
    }
}
