using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// frog is collidable, cars are not same for logs and turtles
// race car right 2 in a lane
// shit car left 5 in a lane

namespace Frogger
{
    
    public interface IDrawable
    {
        void Draw(CanvasDrawingSession canvas);
    }

    public interface ICollidable
    {
        bool CollidesLeftEdge(int x, int y);
        bool ColllidesRightEdge(int x, int y);
        bool CollidesTopEdge(int x, int y);
        bool CoolidesBottomEdge(int x, int y);
        bool CollidesCar(int x, int y);
    }

    // game master class
    public class FroggerGame
    {
        public Frogger frogger { get; private set; }
        
        public Car car_row_10 { get; private set; }
        public Car car_row_9 { get; private set; }
        public Car car_row_8 { get; private set; }
        public Car car_row_7 { get; private set; }

        public WaterObjects Turtles_1 { get; private set; }
        public WaterObjects Logs_1 { get; private set; }
        public WaterObjects Turtles_2 { get; private set; }
        public WaterObjects Logs_2 { get; private set; }

        List<IDrawable> drawables;
        Random rand1;
        public int pos1 { get; set; }
        Random rand2;
        public int pos2 { get; set; }
        //public score score;

        public BoundaryCollision Wall_1 { get; private set; }
        public BoundaryCollision Wall_2 { get; private set; }

        public Gamepad controller;
        public bool death;
        public FroggerGame()
        {
            rand1 = new Random();

            //score = new score();

            drawables = new List<IDrawable>();

            frogger = new Frogger();
            drawables.Add(frogger);
            rand2 = new Random();
            

            viewCars();
            viewWater();
            

            drawables.Add(car_row_10);
            drawables.Add(car_row_9);
            drawables.Add(car_row_8);
            drawables.Add(car_row_7);

            drawables.Add(Turtles_1);
            drawables.Add(Logs_1);
            drawables.Add(Turtles_2);
            drawables.Add(Logs_2);

            Color color = Color.FromArgb(255, 0, 255, 0);
            Wall_1 = new BoundaryCollision(-128, 64, 704, color);
            Wall_2 = new BoundaryCollision(512, 64, 704, color);
            drawables.Add(Wall_1);
            drawables.Add(Wall_2);
        }

        public void viewWater()
        {
            pos1 = rand1.Next(10, 14);
            pos2 = rand2.Next(-5, -1);
            Turtles_1 = new WaterObjects(direction.LEFT, 5, pos1, 0, 0.02, true);
            Logs_1 = new WaterObjects(direction.RIGHT, 4, 0, pos2, 0.04, false);
            pos1 = rand1.Next(10, 14);
            Turtles_2 = new WaterObjects(direction.LEFT, 3, pos1, 0, 0.03, true);
            pos2 = rand2.Next(-5, -1);
            Logs_2 = new WaterObjects(direction.RIGHT, 2, 0, pos2, 0.05, false);
        }

        public void viewCars()
        {
            pos1 = rand1.Next(10, 14);
            pos2 = rand2.Next(-5, -1);
            car_row_10 = new Car(direction.LEFT, 10, pos1, 0, 0.02, true);
            car_row_9 = new Car(direction.RIGHT, 9, pos2, 0, 0.04, false);
            pos1 = rand1.Next(10, 14);
            car_row_8 = new Car(direction.LEFT, 8, pos1, 0, 0.03, true);
            pos2 = rand2.Next(-5, -1);
            car_row_7 = new Car(direction.RIGHT, 7, pos2, 0, 0.05, false);
        }

        public bool Update()
        {
            foreach (var drawable in drawables)
            {
                ICollidable colidiable = drawable as ICollidable;
                if (colidiable != null)
                {
                    if (colidiable.CollidesLeftEdge(frogger.getFroggerClm(), frogger.getFroggerRow()) ||
                        colidiable.ColllidesRightEdge(frogger.getFroggerClm(), frogger.getFroggerRow()) ||
                         colidiable.CollidesCar(frogger.getFroggerClm(), frogger.getFroggerRow()))
                    {
                        death = false;
                        return death;
                    }
                }
            }
            car_row_10.Update(car_row_10);
            car_row_9.Update(car_row_9);
            car_row_8.Update(car_row_8);
            car_row_7.Update(car_row_7);

            Turtles_1.Update(Turtles_1);
            Turtles_2.Update(Turtles_2);
            Logs_1.Update(Logs_1);
            Logs_2.Update(Logs_2);

            return true;
        }


        public void DrawGame(CanvasDrawingSession canvas)
        {
            foreach (var drawable in drawables)
            {
                drawable.Draw(canvas);
            }
        }
    }

    public enum direction { UP, DOWN, LEFT, RIGHT };

    public class Frogger : IDrawable
    {
        
        direction froggerDirection;
        int froggerRow;
        int froggerClm;

        public Frogger()
        {
            froggerDirection = direction.UP;
            froggerRow = 11;
            froggerClm = 5;

        }

        public void Draw(CanvasDrawingSession canvas)
        {
            Color color = new Color();
            color.R = 0;
            color.B = 0;
            color.G = 204;
            color.A = 255;
            Rect rect = new Rect(getFroggerClm(), getFroggerRow(), 64, 64);
            canvas.FillRectangle(rect, color);
            canvas.DrawRectangle(rect, color);
            //canvas.DrawRectangle(8*64, 10*64, 64, 64, Colors.Red);
            //canvas.DrawRectangle((float)getFroggerClm(), (float)getFroggerRow(), 64, 64, Colors.Red);
        }

        public direction getFroggerDirection()
        {
            return froggerDirection;
        }

        public int getFroggerRow()
        {
            return (froggerRow - 1) * 64;
        }

        public int getFroggerClm()
        {
            return (froggerClm - 1) * 64;
        }

        public int FroggerLeft()
        {
            froggerDirection = direction.LEFT;
            return froggerClm--;
        }
        public int FroggerRight()
        {
            froggerDirection = direction.RIGHT;
            return froggerClm++;
        }
        public int FroggerUp()
        {
            froggerDirection = direction.UP;
            return froggerRow--;
        }
        public int FroggerDown()
        {
            froggerDirection = direction.DOWN;
            return froggerRow++;
        }

    }

    public class Car : IDrawable, ICollidable
    {
        public static int CAR_WIDTH = 64;
        public static int CAR_HEIGTH = 64;
        public bool cheat = false;
        direction CarDirection;
        public double CarRow { get; set; }
        public double CarColumn { get; set; }
        public int SpawnPosition { get; set; }
        public double Speed { get; set; }
        public bool TravellingLeftward { get; set; }

        public bool EnableCheats()
        {
            cheat = true;
            return true;
        }

        public Car(direction direction, double CarRow, double CarColumn, int SpawnPosition, double speed, bool TravellingLeftward)
        {
            CarDirection = direction;
            this.CarRow = CarRow;
            this.CarColumn = CarColumn;
            this.SpawnPosition = SpawnPosition;
            this.Speed = speed;
            this.TravellingLeftward = TravellingLeftward;
        }

        public void Draw(CanvasDrawingSession canvas)
        {
            Color color = new Color();

            if (TravellingLeftward)
            {                
                color.R = 51;
                color.B = 255;
                color.G = 51;
                color.A = 255;
            }
            else
            {
                color.R = 204;
                color.B = 0;
                color.G = 0;
                color.A = 255;
            }
            
            Rect rect = new Rect(GetCarColumn(), GetCarRow(), CAR_WIDTH, CAR_HEIGTH);
            canvas.FillRectangle(rect, color);
            canvas.DrawRectangle(rect, color);
            //canvas.DrawRectangle(8*64, 10*64, 64, 64, Colors.Red);
            canvas.DrawRectangle((int)GetCarColumn(), (int)GetCarRow(), CAR_WIDTH, CAR_HEIGTH, Colors.Blue);
        }

        public void Update(Car car)
        {
            if (TravellingLeftward)
            {
                CarColumn -= Speed;
            }
           
            if (!TravellingLeftward)
            {
                CarColumn += Speed;
            }
            
            if ((CarColumn <= 0) && TravellingLeftward && car.CarRow==10)
            {
                Random rand = new Random();
                int pos = rand.Next(10, 14);
                car.CarColumn = pos;
                if (cheat)
                    car.Speed = (rand.Next(40, 100)) / 100.0;
                else
                    car.Speed = (rand.Next(2, 5)) / 100.0;

            }
            if ((CarColumn <= 0) && TravellingLeftward && car.CarRow == 8)
            {
                Random rand = new Random();
                int pos = rand.Next(10, 14);
                car.CarColumn = pos;
                car.Speed = (rand.Next(1, 4)) / 100.0;
                //FOR INSANE MODE CHEAT
                if(cheat)
                    car.Speed = (rand.Next(40, 100)) / 100.0;
                else
                    car.Speed = (rand.Next(1, 4)) / 100.0;

            }
            if ((CarColumn >= 10) && !TravellingLeftward && car.CarRow==9)
            {
                Random rand = new Random();
                int pos = rand.Next(-5, -1);
                car.CarColumn = pos;
                if (cheat)
                    car.Speed = (rand.Next(40, 100)) / 100.0;
                else
                    car.Speed = (rand.Next(5, 9)) / 100.0;
            }
            if ((CarColumn >= 10) && !TravellingLeftward && car.CarRow == 7)
            {
                Random rand = new Random();
                int pos = rand.Next(-5, -1);
                car.CarColumn = pos;
                if (cheat)
                    car.Speed = (rand.Next(40, 100)) / 100.0;
                else
                    car.Speed = (rand.Next(4, 17)) / 100.0;
                //FOR INSANE MODE CHEAT
                //car.Speed = (rand.Next(40, 100)) / 100.0;


            }

        }

        public direction GetCarDirection()
        {
            return CarDirection;
        }

        public double GetCarRow()
        {
            return (CarRow - 1) * 64;
        }

        public double GetCarColumn()
        {
            return (CarColumn - 1) * 64;
        }

        public double CarLeft()
        {
            return CarColumn--;
        }
        public double CarRight()
        {
            return CarColumn++;
        }
        public bool CollidesLeftEdge(int x, int y)
        {
            return false;
        }

        public bool ColllidesRightEdge(int x, int y)
        {
            return false;
        }
        public bool CollidesTopEdge(int x, int y)
        {
            return false;
        }

        public bool CoolidesBottomEdge(int x, int y)
        {
            return false;
        }
        public bool CollidesCar(int x, int y)
        {
            return ((x >= GetCarColumn() - 57) && (x <= GetCarColumn() + 57)) && y == GetCarRow();
        }
    }

    public class WaterObjects : IDrawable, ICollidable
    {
        public static int CAR_WIDTH = 64;
        public static int CAR_HEIGTH = 64;

        direction WaterDirection;
        public double WaterRow { get; set; }
        public double WaterColumn { get; set; }
        public int SpawnPosition { get; set; }
        public double Speed { get; set; }
        public bool TravellingLeftward { get; set; }

        public WaterObjects(direction WaterDirection, double WaterRow, double WaterColumn, int SpawnPosition, double speed, bool TravellingLeftward)
        {
            this.WaterDirection = WaterDirection;
            this.WaterRow = WaterRow;
            this.WaterColumn = WaterColumn;
            this.SpawnPosition = SpawnPosition;
            this.Speed = speed;
            this.TravellingLeftward = TravellingLeftward;
        }

        public void Draw(CanvasDrawingSession canvas)
        {
            Color color = new Color();
            color.R = 102;
            color.B = 0;
            color.G = 51;
            color.A = 255;
            Rect rect = new Rect(GetWaterColumn(), GetWaterRow(), CAR_WIDTH, CAR_HEIGTH);            
            canvas.FillRectangle(rect, color);            
            canvas.DrawRectangle(rect, color);            

            //canvas.DrawRectangle(8*64, 10*64, 64, 64, Colors.Red);        
            
        }

        public void Update(WaterObjects wo)
        {
            if (TravellingLeftward)
            {
                WaterColumn -= Speed;
            }

            if (!TravellingLeftward)
            {
                WaterColumn += Speed;
            }

            if ((wo.WaterRow == 4) && !TravellingLeftward && (wo.WaterColumn > 10))
            {
                Random rand = new Random();
                int pos = rand.Next(-5, -1);
                wo.WaterColumn = pos;
                wo.Speed = (rand.Next(2, 5)) / 100.0;
                ;
            }

            if ((wo.WaterRow == 2) && !TravellingLeftward && (wo.WaterColumn > 10))
            {
                Random rand = new Random();
                int pos = rand.Next(-5, -1);
                wo.WaterColumn = pos;
                wo.Speed = (rand.Next(2, 5)) / 100.0;
                ;
            }

            if ((wo.WaterRow == 5) && TravellingLeftward && (wo.WaterColumn <= 0))
            {
                Random rand = new Random();
                int pos = rand.Next(10, 14);
                wo.WaterColumn = pos;
                wo.Speed = (rand.Next(2, 5)) / 100.0;
                ;
            }

            if ((wo.WaterRow == 3) && TravellingLeftward && (wo.WaterColumn <= 0))
            {
                Random rand = new Random();
                int pos = rand.Next(10, 14);
                wo.WaterColumn = pos;
                wo.Speed = (rand.Next(2, 5)) / 100.0;
                ;
            }


        }

        public direction GetCarDirection()
        {
            return WaterDirection;
        }

        public double GetWaterRow()
        {
            return (WaterRow - 1) * 64;
        }

        public double GetWaterColumn()
        {
            return (WaterColumn - 1) * 64;
        }

        public double WaterLeft()
        {
            return WaterColumn--;
        }
        public double WaterRight()
        {
            return WaterColumn++;
        }
        public bool CollidesCar(int x, int y)
        {
            return ((x >= GetWaterColumn() - 57) && (x <= GetWaterColumn() + 57)) && y == GetWaterRow();
        }

        public bool CollidesLeftEdge(int x, int y)
        {
            return false;
        }

        public bool ColllidesRightEdge(int x, int y)
        {
            return false;
        }

        public bool CollidesTopEdge(int x, int y)
        {
            return false;
        }

        public bool CoolidesBottomEdge(int x, int y)
        {
            return false;
        }
    }

    public class BoundaryCollision : IDrawable, ICollidable
    {
        public static int HEIGHT = 64;
        public static int WIDTH = 704;
        public int Y0 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public Color Color { get; set; }
        public BoundaryCollision(int x1, int y0, int y1, Color color)
        {
            X1 = x1;
            Y0 = y1;
            Y1 = y1;

        }

        public bool CollidesLeftEdge(int x, int y)
        {
            return x == X1 + HEIGHT && y <= Y0 + WIDTH;
        }

        public bool ColllidesRightEdge(int x, int y)
        {
            return x == X1 + HEIGHT && y <= Y0 + WIDTH;
        }
        public bool CollidesTopEdge(int x, int y)
        {
            return false;
        }

        public bool CoolidesBottomEdge(int x, int y)
        {
            return false;
        }

        public void Draw(CanvasDrawingSession canvas)
        {
            canvas.DrawRectangle(X1, Y0, Y1, HEIGHT, Color);
        }

        public bool CollidesCar(int x, int y)
        {
            return false;
        }
    }

}
