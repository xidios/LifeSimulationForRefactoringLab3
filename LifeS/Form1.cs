using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeS
{
    public partial class Form1 : Form
    {

        private Graphics graphics;
        private int resolution;
        private Map gameEngine;
        int rows = 1000;
        int cols = 1000;
        int density = 600;
        private Animal observedHuman = null;
        System.Media.SoundPlayer sp = new System.Media.SoundPlayer();



        public Form1()
        {
            InitializeComponent();

        }
        private void StartGame()
        {

            buttonStart.Text = "RESTART";
            buttonPause.Text = "Pause";
            //sp.SoundLocation = "plants_vs_zombies.wav";

            sp.Play();//music start

            labelTimeChild.Text = null;
            humanSatiety.Text = null;
            status.Text = null;
            sex.Text = null;
            observedHuman = null;

            resolution = (int)Resolution.Value;//присваиваем значение в инт

            gameEngine = new Map(
                rows: rows,
                cols: cols,
                density: density
                );


            UpdateFormInformationAboutCurrentGeneration();

            BitmapChange();
            timer1.Start();
        }

        private void BitmapChange()
        {
            pictureBox1.Image = new Bitmap(cols * resolution, rows * resolution);//создаем битмап. Новую картинку // pictureBox1.Width, pictureBox1.Height
            graphics = Graphics.FromImage(pictureBox1.Image);//передали картинку из прошлой строчки
        }
        private void ColorChange(Animal a, int x, int y)
        {
            (Brush, Brush) colors = ColorsForType(a);
            if (a.gender == Gender.Female)
                graphics.FillRectangle(colors.Item1, x * resolution, y * resolution, resolution, resolution);
            else
                graphics.FillRectangle(colors.Item2, x * resolution, y * resolution, resolution, resolution);
        }
        private (Brush, Brush) ColorsForType(Animal a)
        {
            if (a is Herbivore)
            {
                return (Brushes.Coral, Brushes.Moccasin);
            }
            else if (a is Omnivore)
            {
                return (Brushes.Azure, Brushes.LightGray);
            }
            else if (a is Predator)
            {
                return (Brushes.DeepPink, Brushes.Plum);
            }
            return (null, null);
        }
        private void DrawGeneration()
        {
            UpdateBitmapIfResolutionChanged();
            graphics.Clear(Color.Black);//очищаем игровое поле
            var field = gameEngine.NextGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {

                    FillMap(ref field, x, y);

                }
            }
            DrawMapEvents();
            ObservedHuman();
            UpdateFormInformationAboutCurrentGeneration();
            pictureBox1.Refresh();
        }
        private void UpdateFormInformationAboutCurrentGeneration()
        {
            Text = $"Generation {gameEngine.CurrentGeneration}";
            totalOfAnimals.Text = $"Total of animals: {gameEngine.TotalOfAnimals}";
        }
        private void UpdateBitmapIfResolutionChanged()
        {
            if (resolution != (int)Resolution.Value)
            {
                resolution = (int)Resolution.Value;
                BitmapChange();
            }
        }

        private void DrawMapEvents()
        {
            if (gameEngine.mapEvents != null)
            {
                foreach (Event e in gameEngine.mapEvents)
                    if (e.exist)
                        graphics.FillRectangle(Brushes.Gold, e.x * resolution, e.y * resolution, resolution, resolution);
            }
        }
        private void FillMap(ref Cell[,] field, int x, int y)
        {
            if (field[x, y].plant != null && field[x, y].plant.alive)
            {
                graphics.FillRectangle(Brushes.Green, x * resolution, y * resolution, resolution, resolution);

            }

            if (field[x, y].animals.Count() > 0)
            {
                gameEngine.TotalOfAnimals += field[x, y].animals.Count();
                ColorChange(field[x, y].animals[0], x, y);
            }
            if (observedHuman != null)
            {
                graphics.FillRectangle(Brushes.Blue, observedHuman.x * resolution, observedHuman.y * resolution, resolution, resolution);
            }
        }
        private void ObservedHuman()
        {
            if (observedHuman != null && observedHuman.alive)
            {
                humanSatiety.Text = $"Satiety: {observedHuman.satiety}";
                status.Text = $"Status: {((observedHuman.satiety == 0) ? "Dead" : "Alive")}";
                labelTimeChild.Text = $"Time from last child: {observedHuman.timeLastChild}";
                sex.Text = $"Sex: {((observedHuman.gender == Gender.Male) ? "Male" : "Female")}";
            }
            else
            {
                observedHuman = null;
                status.Text = "Human not selected";
                labelTimeChild.Text = null;
                humanSatiety.Text = null;
                status.Text = null;
                sex.Text = null;
                animalType.Text = null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawGeneration();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }



        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                sp.Stop();
                buttonPause.Text = "Continue";
            }
            else if (!timer1.Enabled)
            {
                timer1.Start();
                sp.Play();
                buttonPause.Text = "Pause";
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseLeftClickObservedAnimal(e);
            }
            if (e.Button == MouseButtons.Right)
            {
                MouseRightClickCreateEvent(e);
            }
        }

        private void MouseLeftClickObservedAnimal(MouseEventArgs e)
        {
            int x = e.Location.X / resolution;
            int y = e.Location.Y / resolution;
            observedHuman = gameEngine.GetHuman(x, y);
            if (observedHuman != null)
            {
                humanSatiety.Text = $"Satiety: {observedHuman.satiety}";
                status.Text = $"Status: {((observedHuman.satiety == 0) ? "Dead" : "Alive")}";// $"{x} {y}"                 
                labelTimeChild.Text = $"Time from last child: {observedHuman.timeLastChild}";
                sex.Text = $"Sex: {((observedHuman.gender == Gender.Male) ? "Male" : "Female")}";
                animalType.Text = $"Type: {observedHuman.GetType().Name}";

            }
            else
            {
                animalType.Text = null;
                labelTimeChild.Text = null;
                humanSatiety.Text = null;
                status.Text = null;
                sex.Text = null;
            }
        }
        private void MouseRightClickCreateEvent(MouseEventArgs e)
        {
            int x = e.Location.X / resolution;
            int y = e.Location.Y / resolution;
            gameEngine.CreateEventByMouse(x, y);
        }
    }

}



