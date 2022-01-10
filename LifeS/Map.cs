using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LifeS
{
    class Map
    {
        private Cell[,] field;
        public List<Event> mapEvents=new List<Event>();
        public readonly int rows=1000;
        public readonly int cols=1000;
        Random random = new Random();

        public int CurrentGeneration { get; private set; }
        public int TotalOfAnimals { get;  set; }


        public Map(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new Cell[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {                   
                    field[x, y] = new Cell(cols, rows);
                    CreateAnimal<Herbivore>(x, y, density);
                    CreateAnimal<Omnivore>(x, y, density*4);
                    CreateAnimal<Predator>(x, y, density*5);
                    CreatePlant(x, y, density);                                       
                }
            }
            CurrentGeneration++;
        }
        private void CreateAnimal<T>(int x, int y, int density)
            where T : Animal
        {
            if (random.Next(density) == 0)
            {
                Animal animal = (Animal)Activator.CreateInstance(typeof(T), x, y, random);
                field[x, y].entity.Add(animal);
                field[x, y].animals.Add(animal);
            }
        }       
        private void CreatePlant(int x, int y, int density) {
            if (random.Next(density/2) == 0)
            {
                field[x, y].plant = new Plant(x, y);
                field[x, y].entity.Add(field[x, y].plant);
            }
        }
        public Cell[,] NextGeneration()
        {
            StartEvent();
            CurrentGeneration++;
            TotalOfAnimals = 0;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    CreateRandomPlant(x, y, random);
                    UpdatePlantsInfo(x, y);

                    if (field[x, y].entity.Count > 0)
                    {
                        UpdateCellInformation(x, y);
                    }
                }
            }
            
            CheckedToFalse();
            return field;
        }
        private void UpdateCellInformation(int x,int y)
        {
            UpdateAnimalsInfo(x, y);
            
            for (int i = 0; i < field[x, y].entity.Count(); i++)
            {
                if (field[x, y].entity.Count() > 0 && field[x, y].entity[i] != null && field[x, y].entity[i] is Animal)
                {
                    
                    Animal a = null;
                    a = (Animal)field[x, y].entity[i];
                    if (!a.changed)
                    {
                        a.DoSomething(field.GetLength(0), field.GetLength(1), field);
                        field[a.x, a.y].entity.Add(a);
                        field[x, y].entity.Remove(a);
                        field[a.x, a.y].animals.Add(a);
                        field[x, y].animals.Remove(a);
                    }

                }
            }


        }
        public Animal GetHuman(int _x, int _y)
        {
            if (field[_x, _y].animals.Count > 0)
                return field[_x, _y].animals[0];          
            return null;
        }
        private void CheckedToFalse()
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (field[x, y].animals.Count() > 0)
                    {
                        foreach (Animal b in field[x, y].animals)
                        {
                            b.changed = false;
                        }
                    }

                }
            }
        }

        private void CreateRandomPlant(int x, int y, Random _random)//возможно неуместная близость
        {

            if (field[x, y].plant == null)
            {
                
                int countPlantsAround = 0;
                CheckNeighborPlants(x - 1, y, ref countPlantsAround);
                CheckNeighborPlants(x, y - 1, ref countPlantsAround);
                CheckNeighborPlants(x + 1, y, ref countPlantsAround);
                CheckNeighborPlants(x, y + 1, ref countPlantsAround);
                if (countPlantsAround > 0 && _random.Next(300 / countPlantsAround) == 0)
                {
                    field[x, y].plant = new Plant(x, y);
                    field[x, y].entity.Add(field[x, y].plant);
                }
            }
        }
        private void CheckNeighborPlants(int x,int y, ref int count)
        {
            if (x>= 0 && x<field.GetLength(0) && y >= 0 && y < field.GetLength(1) && field[x, y].plant != null)
            {
                count++;
            }
        }
        private void UpdatePlantsInfo(int x, int y)
        {
            if (field[x, y].plant != null && !field[x, y].plant.alive)
            {
                field[x, y].entity.Remove(field[x, y].plant);
                field[x, y].plant = null;
                
            }
        }

        private void UpdateAnimalsInfo(int x, int y)
        {


            for (int i = 0; i < field[x, y].entity.Count(); i++)
            {
                if (field[x, y].entity.Count() > 0 && field[x, y].entity[i] != null && field[x, y].entity[i] is Animal)
                {
                    //тут проверить
                    Animal a = null;
                    a = (Animal)field[x, y].entity[i];
                    if (a.satiety <= 0 || !a.alive)
                    {
                        a.Dead();
                        field[x, y].entity.Remove(a);
                        field[x, y].animals.Remove(a);
                    }
                   
                }
            }
           
        }

        private void StartEvent()
        {
            List<Event> _mapEvents = new List<Event>();
            if (random.Next(50) == 0)
            {
                Event _event = new Event(random, field);
                mapEvents.Add(_event);
            }
            foreach (Event e in mapEvents)
            {
                if (e.EventIsExist())
                {
                    e.KillTheAll(field);
                    _mapEvents.Add(e);
                }

            }
            mapEvents = _mapEvents;
        }

        public void CreateEventByMouse(int x, int y)
        {
            Event ev = new Event(random,field);
            mapEvents.Add(ev);
            ev.MouseEvent(x, y, field);
        }

    }
}

