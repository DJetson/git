using GameObjects.Classes;
using GameObjects.Controls;
using GameObjects.Controls.DeveloperConsole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameObjects.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        #region MainWindow Properties
        private WorldObject _World;
        public WorldObject World
        {
            get { return _World; }
            set { _World = value; NotifyPropertyChanged("World"); }
        }

        private Boolean _IsFullscreen;
        public Boolean IsFullscreen
        {
            get { return _IsFullscreen; }
            set { _IsFullscreen = value; NotifyPropertyChanged("IsFullscreen"); }
        }

        private Boolean _IsDebugInfoPaneOpen = false;
        public Boolean IsDebugInfoPaneOpen
        {
            get { return _IsDebugInfoPaneOpen; }
            set { _IsDebugInfoPaneOpen = value; NotifyPropertyChanged("IsDebugInfoPaneOpen"); }
        }

        public static MainWindow Window = null;

        #endregion MainWindow Properties

        private static readonly double WORLD_HEIGHT = 5000;
        private static readonly double WORLD_WIDTH = 5000;

        public MainWindow()
        {
            if (Window != null)
                return;

            DataContext = this;
            InitializeComponent();

            if (Window == null)
                Window = this;

            World = WorldObject.CurrentWorld ?? new WorldObject("InputSample", true);

            World.Bounds = new Rect(0, 0, WORLD_WIDTH, WORLD_HEIGHT);

            //PlayerObject g = new PlayerObject();
            //PlayerControl p = new PlayerControl() { DataContext = g };

            //World.AddObject(p);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
//            World.Bounds = new Rect(0, 0, WorldControl.ActualWidth, WorldControl.ActualHeight);
            if (WorldObject.CurrentWorld.Camera == null)
                World.InitializeCamera();
            else
            {
                World.SetViewportSize(WorldControl.ActualWidth, WorldControl.ActualHeight);
            }
        }

        private void WorldControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
//            World.Bounds = new Rect(0, 0, WorldControl.ActualWidth, WorldControl.ActualHeight);
            if (WorldObject.CurrentWorld.Camera == null)
                World.InitializeCamera();
            else
            {
                World.SetViewportSize(WorldControl.ActualWidth, WorldControl.ActualHeight);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemTilde)
            {
                IsDebugInfoPaneOpen = !IsDebugInfoPaneOpen;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsDebugInfoPaneOpen && e.Key == Key.OemTilde)
            {
                DeveloperConsoleControl.FocusPrompt();
            }
            else if (e.Key == Key.OemTilde)
            {
                DeveloperConsoleControl.UnfocusPrompt();
            }
        }

        private void SetFullscreen()
        {
            WindowStyle = System.Windows.WindowStyle.None;
            ResizeMode = System.Windows.ResizeMode.NoResize;
            WindowState = System.Windows.WindowState.Maximized;
        }

        private void SetWindowed()
        {
            WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            ResizeMode = System.Windows.ResizeMode.CanResize;
            WindowState = System.Windows.WindowState.Normal;
        }

        public static void Exit()
        {
            Application.Current.Shutdown();
        }

        #region Command Methods
        public static void ToggleFullscreen()
        {
            Window.IsFullscreen = !Window.IsFullscreen;

            if (Window.IsFullscreen)
                Window.SetFullscreen();
            else
                Window.SetWindowed();
        }


        #endregion Command Methods
    }
}
