using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeS
{
    abstract public class Animals<T, TFood> : Animal
        where T : Animal
    {
        public Animals(int _x, int _y, Random rand) : base(_x, _y, rand)
        {

        }

        public Entity CheckTarget<Target>(int _x, int _y, ref Cell[,] field, TypeOfTarget type)
        {
            int tempX = x + _x;
            int tempY = y + _y;

            if (!(tempX < 0 || tempX >= field.GetLength(0) || tempY < 0 || tempY >= field.GetLength(1)))
            {
                if (field[tempX, tempY].entity.Count > 0)
                {
                    if (type == TypeOfTarget.ForReproduction)
                        return ForeachForReproduction<Target>(tempX, tempY, ref field);
                    if (type == TypeOfTarget.ForFood)
                        return ForeachForFood<Target>(tempX, tempY, ref field);
                }
            }

            return null;
        }
        private Entity ForeachForReproduction<Target>(int _x, int _y, ref Cell[,] field)
        {
            foreach (Entity e in field[_x, _y].entity)
            {
                if (e is Animal && e is T)
                {
                    Animal a = (Animal)e;
                    if (gender != a.gender && a.satiety >= 50 && a.timeLastChild == 0)
                    {
                        return a;
                    }
                }
            }
            return null;
        }
        private Entity ForeachForFood<Target>(int _x, int _y, ref Cell[,] field)
        {
            foreach (Entity e in field[_x, _y].entity)
            {
                if (e is Target)
                    return e;
            }
            return null;
        }

        private Entity FindTarget<Target>(ref Cell[,] field, TypeOfTarget type)
        {
            int visibility = viewDistance;
            Entity target = null;
            for (int i = 0; i <= visibility; i++)
            {
                //свверхе погоризнтали
                for (int x = -i, y = x; x <= i; x++)
                {
                    target = CheckTarget<Target>(x, y, ref field, type);
                    if (target != null)
                        return target;
                }
                //свнизу по горизонтали
                for (int x = -i, y = -x; x <= i; x++)
                {
                    target = CheckTarget<Target>(x, y, ref field, type);
                    if (target != null)
                        return target;

                }

                //слева вертикально
                for (int y = -i + 1, x = -i; y < i; y++)
                {
                    target = CheckTarget<Target>(x, y, ref field, type);
                    if (target != null)
                        return target;

                }

                //справа вертикально
                for (int y = -i + 1, x = i; y < i; y++)
                {
                    target = CheckTarget<Target>(x, y, ref field, type);
                    if (target != null)
                        return target;

                }
            }
            return target;
        }
        private void MoveRandom(int xSize, int ySize)
        {
            Direction direction = (Direction)random.Next(5);
            Move(xSize, ySize, direction);
        }
        private void PanicMove(int xSize, int ySize)
        {
            Direction direction = (Direction)random.Next(4);
            Move(xSize, ySize, direction);
        }
        private void Move(int xSize, int ySize, Direction direction)
        {

            switch (direction)
            {
                case Direction.Left:
                    if (x - 1 >= 0)
                    {
                        x--;
                        changed = true;
                    }

                    break;
                case Direction.Up:
                    if (y - 1 >= 0)
                    {
                        y--;
                        changed = true;
                    }
                    break;
                case Direction.Right:
                    if (x + 1 < xSize)
                    {
                        x++;
                        changed = true;
                    }
                    break;
                case Direction.Down:
                    if (y + 1 < ySize)
                    {
                        y++;
                        changed = true;
                    }
                    break;
                case Direction.None:
                    break;
            }
            satiety--;
        }

        private Direction MoveToTarget(Entity a, int _x, int _y)
        {
            if (a.x - _x < 0)
            {
                return Direction.Left;
            }
            else if (a.x - _x > 0)
            {
                return Direction.Right;
            }
            else if (a.y - _y < 0)
            {
                return Direction.Up;
            }
            else if (a.y - _y > 0)
            {
                return Direction.Down;
            }
            else if (a.y == _y && a.x == _x)
                return Direction.SamePosition;
            return Direction.None;
        }

        private void EatSmth(int _x, int _y, Entity en)
        {
            if (en is Plant)
            {
                satiety += 50;
                en.Dead();
            }
            else
            {
                satiety += 70;
                en.Dead();
            }
        }

        private void SearchTarget<Target>(Cell[,] field, TypeOfTarget type)
        {
            Entity en = FindTarget<Target>(ref field, type);

            Direction direction = Direction.None;
            if (en != null)
            {
                direction = MoveToTarget(en, x, y);
            }

            if (direction == Direction.SamePosition && type == TypeOfTarget.ForFood)
            {
                EatSmth(x, y, en);
                return;
            }

            if (direction == Direction.SamePosition && type == TypeOfTarget.ForReproduction)
            {
                DoChild(x, y, field, en);

            }

            if (direction == Direction.None)
            {
                PanicMove(field.GetLength(0), field.GetLength(1));
                return;
            }

            Move(field.GetLength(0), field.GetLength(1), direction);


        }
        public override void DoSomething(int _x, int _y, Cell[,] field)
        {

            if (satiety <= 50)
            {
                SearchTarget<TFood>(field, TypeOfTarget.ForFood);

            }
            else if (satiety >= 70 && timeLastChild == 0)
            {
                SearchTarget<T>(field, TypeOfTarget.ForReproduction);

            }
            else
            {
                MoveRandom(_x, _y);
            }

            if (timeLastChild > 0)
                timeLastChild--;
        }





        private void DoChild(int _x, int _y, Cell[,] field, Entity h)
        {

            Animal child = (Animal)Activator.CreateInstance(typeof(T), _x, _y, random);
            child.changed = true;
            Animal parent = (Animal)h;
            field[_x, _y].animals.Add(child);
            field[_x, _y].entity.Add(child);
            parent.timeLastChild = 150;
            timeLastChild = 200;
        }


    }
}
