#define ZOMBIE_GAME
using GPOne.Controls;
using GPOne.Objects;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace GPOne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region MainWindow Properties
        private WorldObject _World;
        public WorldObject World
        {
            get { return _World; }
            set { _World = value; NotifyPropertyChanged("World"); }
        }
        #endregion MainWindow Properties

        //private PlaceHolderObject TestObject;

#if ZOMBIE_GAME
        public MainWindow()
        {
            DataContext = this;
            World = new WorldObject("ZombieWorld1", true);
            InitializeComponent();

            World.Bounds = new Rect(0, 0, Width, Height);

            ZombieObject z;
            ZombieControl ZCtrl;
            for (int i = 0; i < 25; i++)
            {
                z = new ZombieObject();
                ZCtrl = new ZombieControl() { DataContext = z };
                World.AddObject(ZCtrl);
            }

        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            //WorldObject.CurrentWorld.Bounds = new Rect(-UFO.Width / 2, -UFO.Height / 2, ActualWidth - (UFO.Width), ActualHeight - (UFO.Height));

            //UFO.Position = new Vector(ActualWidth, ActualHeight);
        }

#else
        private CharacterObject UFO;

        public MainWindow()
        {
            DataContext = this;
            World = new WorldObject("NewWorld1", true);
            InitializeComponent();

            World.Bounds = new Rect(0, 0, Width, Height - 10);

            UFO = new CharacterObject();
            UFO.Velocity = new BoundedVector(0, 0, -5.0e-3, -5.0e-3, 5.0e-3, 5.0e-3);
            UFO.Acceleration = new BoundedVector(0, 0, -5.0e-12, -5.0e-12, 5.0e-12, 5.0e-12);
            UFO.AngularVelocity = 1.0e-5;
            UFO.Size = new Vector(50, 50);
            UFO.Scale = new Vector(1, 1);
            UFO.Position = new BoundedVector(Width / 2, Height / 2);
            UFOControl u = new UFOControl() { DataContext = UFO };
            World.AddObject(u);

            //            TestObject = new PlaceHolderObject();

            //TestObject.Position = new BoundedVector(150, 150, 0, 0, ActualWidth, ActualHeight);
            //TestObject.Fill = new SolidColorBrush(Colors.Blue);
            //TestObject.Stroke = new SolidColorBrush(Colors.Black);
            //TestObject.Size = new Vector(50, 50);
            //TestObject.Scale = new Vector(0.5, 0.5);
            //TestObject.Velocity = new BoundedVector(0, 0, -5.0e-3, -5.0e-3, 5.0e-3, 5.0e-3);
            //TestObject.Acceleration = new BoundedVector(0, 0, -1.0e-11, -1.0e-11, 1.0e-11, 1.0e-11);
            //TestObject.AngularAcceleration = 0.000000000000000001;
            //PlaceHolder p = new PlaceHolder() { DataContext = TestObject };

            //World.AddObject(p);
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            WorldObject.CurrentWorld.Bounds = new Rect(-UFO.Width / 2, -UFO.Height / 2, ActualWidth - (UFO.Width), ActualHeight-(UFO.Height));

            //UFO.Position = new Vector(ActualWidth, ActualHeight);
        }

#endif


    }
}
