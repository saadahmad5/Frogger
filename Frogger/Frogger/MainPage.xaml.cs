using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using System.Threading.Tasks;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI;
using Windows.UI.Core;
using Windows.UI;
using Windows.Media.Playback;
using Windows.Media.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Frogger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        FroggerGame Frogger;
        //AddSprites Sprites;
        CanvasBitmap FrogU;
        CanvasBitmap FrogD;

        CanvasBitmap FrogL;

        CanvasBitmap FrogR;

        CanvasBitmap Car;
        CanvasBitmap RaceCar;
        CanvasBitmap Log;
        CanvasBitmap Turtle;

        MediaPlayer froggerHop;
        //MediaPlayer LevelMusic;

        public MainPage()
        {
            this.InitializeComponent();
            Frogger = new FroggerGame();
            Window.Current.CoreWindow.KeyDown += Canvas_KeyDown;
            Window.Current.CoreWindow.KeyUp += Canvas_KeyUp;
        }

        private void playSound()
        {
            froggerHop = new MediaPlayer();
            string path = "ms-appx:///Assets/frogger-hop.wav";
            var file = new Uri(path);
            froggerHop.Source = MediaSource.CreateFromUri(file);
           froggerHop.Play();
        }

        //private void playLevelMusic()
        //{
        //    LevelMusic = new MediaPlayer();
        //    string path = "ms-appx:///Assets/Frogger_Level_Music.wav";
        //    var file = new Uri(path);
        //    LevelMusic.Source = MediaSource.CreateFromUri(file);
        //    LevelMusic.Play();
        //}

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            //Frogger.DrawGame(args.DrawingSession);
            using (var sb = args.DrawingSession.CreateSpriteBatch(CanvasSpriteSortMode.None, CanvasImageInterpolation.Linear, CanvasSpriteOptions.None))
            {
                sb.Draw(Car, new Vector2((float)Frogger.car_row_10.GetCarColumn(), (float)Frogger.car_row_10.GetCarRow()));
                sb.Draw(RaceCar, new Vector2((float)Frogger.car_row_9.GetCarColumn(), (float)Frogger.car_row_9.GetCarRow()));
                sb.Draw(Car, new Vector2((float)Frogger.car_row_8.GetCarColumn(), (float)Frogger.car_row_8.GetCarRow()));
                sb.Draw(RaceCar, new Vector2((float)Frogger.car_row_7.GetCarColumn(), (float)Frogger.car_row_7.GetCarRow()));
                sb.Draw(Log, new Vector2((float)Frogger.Logs_1.GetWaterColumn(), (float)Frogger.Logs_1.GetWaterRow()));
                sb.Draw(Turtle, new Vector2((float)Frogger.Turtles_1.GetWaterColumn(), (float)Frogger.Turtles_1.GetWaterRow()));
                sb.Draw(Log, new Vector2((float)Frogger.Logs_2.GetWaterColumn(), (float)Frogger.Logs_2.GetWaterRow()));
                sb.Draw(Turtle, new Vector2((float)Frogger.Turtles_2.GetWaterColumn(), (float)Frogger.Turtles_2.GetWaterRow()));
               
                if(Frogger.frogger.getFroggerDirection() == direction.UP)
                    sb.Draw(FrogU, new Vector2((float)Frogger.frogger.getFroggerClm(), (float)Frogger.frogger.getFroggerRow()));
                else if (Frogger.frogger.getFroggerDirection() == direction.DOWN)
                    sb.Draw(FrogD, new Vector2((float)Frogger.frogger.getFroggerClm(), (float)Frogger.frogger.getFroggerRow()));
                else if (Frogger.frogger.getFroggerDirection() == direction.LEFT)
                    sb.Draw(FrogL, new Vector2((float)Frogger.frogger.getFroggerClm(), (float)Frogger.frogger.getFroggerRow()));
                else if (Frogger.frogger.getFroggerDirection() == direction.RIGHT)
                    sb.Draw(FrogR, new Vector2((float)Frogger.frogger.getFroggerClm(), (float)Frogger.frogger.getFroggerRow()));
               
            }

            args.DrawingSession.DrawText(Frogger.frogger.score.Score.ToString(), new Vector2(0, 0), new Color { A = 255, B = 0, G = 0, R = 0 });


        }

        private async void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            Frogger.returnToStart();
            if (Frogger.Update() == false)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.Frame.Navigate(typeof(GameOver), Frogger.frogger.score.Score);

                });

            };


           
        }

        async Task LoadImages(CanvasDevice device)
        {
            FrogU = await CanvasBitmap.LoadAsync(device, "Assets/Frogger_Up.png");
            FrogD = await CanvasBitmap.LoadAsync(device, "Assets/Frogger_Down.png");
            FrogL = await CanvasBitmap.LoadAsync(device, "Assets/Frogger_Left.png");
            FrogR = await CanvasBitmap.LoadAsync(device, "Assets/Frogger_Right.png");
            Car = await CanvasBitmap.LoadAsync(device, "Assets/Car.png");
           RaceCar = await CanvasBitmap.LoadAsync(device, "Assets/Race_Car.png");
            Log  = await CanvasBitmap.LoadAsync(device, "Assets/Log.png");
            Turtle = await CanvasBitmap.LoadAsync(device, "Assets/Turtle.png");
        }

        void OnCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(LoadImages(sender.Device).AsAsyncAction());

        }

        
        private void Canvas_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {           

            if (e.VirtualKey == Windows.System.VirtualKey.Left || e.VirtualKey == Windows.System.VirtualKey.GamepadDPadLeft)
            {
                Frogger.frogger.FroggerLeft();
                playSound();
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right || e.VirtualKey == Windows.System.VirtualKey.GamepadDPadRight)
            {
                Frogger.frogger.FroggerRight();
                playSound();
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Up || e.VirtualKey == Windows.System.VirtualKey.GamepadDPadUp)
            {
                Frogger.frogger.FroggerUp();
                playSound();
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Down || e.VirtualKey == Windows.System.VirtualKey.GamepadDPadDown)
            {
                Frogger.frogger.FroggerDown();
                playSound();
            }
            
            else if(e.VirtualKey == Windows.System.VirtualKey.U)
            {
                Frogger.car_row_10.EnableCheats();
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.I)
            {
                Frogger.car_row_9.EnableCheats();
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.O)
            {
                Frogger.car_row_8.EnableCheats();
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.P)
            {
                Frogger.car_row_7.EnableCheats();
            }

        }

        private void Canvas_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Left)
            {

            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
            {

            }
        }

        public int getFroggerRow()
        {
            return Frogger.frogger.getFroggerRow();
        }

        public int getFroggerClm()
        {
            return Frogger.frogger.getFroggerClm();
        }

       
        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
