using GameObjects.Controls;
using GameObjects.Controls.DeveloperConsole;
using GameObjects.Interfaces;
using SlimDX.XInput;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameObjects.Classes
{
    public class WorldObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region WorldObject Properties
        private String _Name;
        public String Name
        {
            get { return _Name; }
            set { _Name = value; NotifyPropertyChanged("Name"); }
        }

        private Boolean _IsPaused;
        public Boolean IsPaused
        {
            get { return _IsPaused; }
            set { _IsPaused = value; NotifyPropertyChanged("IsPaused"); }
        }

        private Rect _Bounds;
        public Rect Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; NotifyPropertyChanged("Bounds"); }
        }

        private Visibility _IsTitleVisible;
        public Visibility IsTitleVisible
        {
            get { return _IsTitleVisible; }
            set { _IsTitleVisible = value; NotifyPropertyChanged("IsTitleVisible"); }
        }

        private Boolean _IsDebugModeEnabled;
        public Boolean IsDebugModeEnabled
        {
            get { return _IsDebugModeEnabled; }
            set { _IsDebugModeEnabled = value; NotifyPropertyChanged("IsDebugModeEnabled"); }
        }

        private Boolean _IsGravityEnabled;
        public Boolean IsGravityEnabled
        {
            get { return _IsGravityEnabled; }
            set { _IsGravityEnabled = value; NotifyPropertyChanged("IsGravityEnabled"); }
        }

        private CameraObject _Camera;
        public CameraObject Camera
        {
            get { return _Camera; }
            set { _Camera = value; NotifyPropertyChanged("Camera"); }
        }

        public Vector DrawPosition
        {
            get { return new Vector(-Camera.Position.X, -Camera.Position.Y); }
        }


        public WorldControl Control = null;

        #region Management Lists
        ///Management lists are used for all objects. An object may belong to multiple lists, and the lists to
        ///which it belongs determine how the world treats the object. For example, all objects in the RenderList are
        ///drawn to the screen. All objects in the UpdateList undergo positioning calculations at regular intervals.
        ///Objects belonging to the CollisionList check for collisions. This is done in lieu of ObjectManager classes.

        /// <summary>
        /// Management list for renderable objects
        /// </summary>
        private ObservableCollection<GameControl> _RenderList = new ObservableCollection<GameControl>();
        public ObservableCollection<GameControl> RenderList
        {
            get { return _RenderList; }
            set { _RenderList = value; NotifyPropertyChanged("RenderList"); }
        }

        /// <summary>
        /// Management list for moveable objects
        /// </summary>
        private ObservableCollection<IMovableObject> _UpdateList = new ObservableCollection<IMovableObject>();
        public ObservableCollection<IMovableObject> UpdateList
        {
            get { return _UpdateList; }
            set { _UpdateList = value; NotifyPropertyChanged("UpdateList"); }
        }

        //private List<IReceivesInput> _InputList = new List<IReceivesInput>();
        //public List<IReceivesInput> InputList
        //{
        //    get { return _InputList; }
        //    set { _InputList = value; NotifyPropertyChanged("InputList"); }
        //}

        /// <summary>
        /// This management list is intended to accommodate the foreach loops that are
        /// used to iterate through the world objects. Since the collections cannot be altered
        /// while the foreach is in progress, any item in the collection which create new objects
        /// or triggers the removal of existing objects (example: the player object creates a 
        /// new projectile object, or the player picks up a power up) will add the
        /// new objects to one of the following two collections where they will be stored until 
        /// the foreach is completed. Upon completion of the foreach loop, objects in this list 
        /// will be migrated to or removed from the main object list.
        /// </summary>
        private List<GameControl> _NewObjectStaging = new List<GameControl>();
        public List<GameControl> NewObjectStaging
        {
            get { return _NewObjectStaging; }
            set { _NewObjectStaging = value; NotifyPropertyChanged("NewObjectStaging"); }
        }

        private List<GameControl> _DeadObjectStaging = new List<GameControl>();
        public List<GameControl> DeadObjectStaging
        {
            get { return _DeadObjectStaging; }
            set { _DeadObjectStaging = value; NotifyPropertyChanged("DeadObjectStaging"); }
        }
        #endregion Management Lists

        #endregion WorldObject Properties

        #region WorldObject Members

        private DispatcherTimer MainTimer;
        private long LastTime;

#if NO_GRAVITY
        public static Vector Gravity = new Vector(0, 0);
#else
        public static Vector Gravity = new Vector(0, 9.8e-14);
#endif
        public static WorldObject CurrentWorld = null;

        public static GamepadState PlayerOneInput = new GamepadState(UserIndex.One);
        public static KeyboardInput PlayerOneKeyInput = new KeyboardInput();
        public Boolean IsObjectStagingRequired = false;
        #endregion WorldObject Members

        public WorldObject()
        {
            if (CurrentWorld != null)
                return;

            CurrentWorld = this;
        }

        public WorldObject(String WorldName = "NewWorld", Boolean DebugMode = false)
        {

            if (CurrentWorld != null)
                return;

            if (CurrentWorld == null)
                CurrentWorld = this;

            Name = WorldName;
            IsDebugModeEnabled = DebugMode;

            Initialize();
        }

        public void Initialize()
        {
            InitializeTimer();
            InitializeCamera();
            InitializePlayer();
        }

        public void InitializePlayer()
        {
            PlayerObject p = new PlayerObject()
            {
                Position = new BoundedVector(CurrentWorld.Bounds.Width / 2,
                                             CurrentWorld.Bounds.Height / 2)
            };
            PlayerControl ctrl = new PlayerControl() { DataContext = p };
            AddObject(ctrl);
        }

        public static CameraObject GetCamera()
        {
            return CurrentWorld.Camera;
        }

        public void InitializeCamera()
        {
            if (Control != null)
            {
                Camera = new CameraObject()
                {
                    Position = new Vector(Control.ActualWidth, Control.ActualHeight),
                    ViewportSize = new Size(Control.ActualWidth, Control.ActualHeight),
                    Zoom = new Vector(1, 1)
                };

                ///Add World Bounds
                BoundsControl WorldBounds = new BoundsControl() { DataContext = this };
                RenderList.Add(WorldBounds);
            }
        }

        public void SetViewportSize(double x, double y)
        {
            Camera.ViewportSize = new Size(x, y);
        }

        public void SetZoomLevel(double x, double y)
        {
            Camera.Zoom = new Vector(x, y);
        }

        public void SetCameraPosition(double x, double y)
        {
            Camera.Position = new Vector(x, y);
        }

        public void InitializeTimer()
        {
            MainTimer = new DispatcherTimer();
            MainTimer.Interval = new TimeSpan((long)15000);
            MainTimer.Tick += MainTimer_Tick;
            MainTimer.Start();
            LastTime = DateTime.Now.Ticks;
        }

        void MainTimer_Tick(object sender, EventArgs e)
        {
            long ElapsedTime = DateTime.Now.Ticks - LastTime;
            NotifyPropertyChanged("DrawPosition");
            PlayerOneInput.Update();
            PlayerOneKeyInput.Update();

            if (IsPaused == true)
                return;

            ///Remove any objects in Dead Object Staging
            foreach (GameControl DeadObject in DeadObjectStaging)
                RemoveObject(DeadObject);
            DeadObjectStaging.Clear();

            ///Lock the object manager so that new objects are staged
            IsObjectStagingRequired = true;

            ///Update all managed objects
            foreach (IMovableObject Item in UpdateList)
                Item.BeginUpdate(ElapsedTime);
            LastTime = DateTime.Now.Ticks;

            ///Unlock the object manager so new objects are migrated into the manager
            IsObjectStagingRequired = false;

            ///Migrate all staged objects
            foreach (GameControl NewObject in NewObjectStaging)
                AddObject(NewObject);
            NewObjectStaging.Clear();
        }

        /// <summary>
        /// Adds a new object to the necessary management lists
        /// </summary>
        /// <param name="NewObject">The object to add</param>
        public void AddObject(GameControl NewObject)
        {
            ///If the list being added to is currently being iterated by a foreach loop
            ///then the object will be added to a staging collection until the loop is 
            ///complete.
            ///
            ///
            if (IsObjectStagingRequired == true)
            {
                NewObjectStaging = NewObjectStaging ?? new List<GameControl>();
                if (NewObjectStaging.Contains(NewObject) == false)
                    NewObjectStaging.Add(NewObject);
            }
            else
            {
                RenderList = RenderList ?? new ObservableCollection<GameControl>();
                if (NewObject is GameControl && RenderList.Contains(NewObject as GameControl) == false)
                    RenderList.Add(NewObject as GameControl);

                UpdateList = UpdateList ?? new ObservableCollection<IMovableObject>();
                if (NewObject.DataContext is IMovableObject && UpdateList.Contains(NewObject.DataContext as IMovableObject) == false)
                    UpdateList.Add(NewObject.DataContext as IMovableObject);

                //InputList = InputList ?? new List<IReceivesInput>();
                //if (NewObject.DataContext is IReceivesInput && InputList.Contains(NewObject.DataContext as IReceivesInput) == false)
                //    InputList.Add(NewObject.DataContext as IReceivesInput);
            }
        }

        /// <summary>
        /// Removes an object from all management lists
        /// </summary>
        /// <param name="NewObject"></param>
        public void RemoveObject(GameControl NewObject)
        {
            if (IsObjectStagingRequired == true)
            {
                DeadObjectStaging = DeadObjectStaging ?? new List<GameControl>();
                if (DeadObjectStaging.Contains(NewObject) == false)
                    DeadObjectStaging.Add(NewObject);
            }
            else
            {
                RenderList = RenderList ?? new ObservableCollection<GameControl>();
                if (NewObject is GameControl)
                    RenderList.Remove(NewObject as GameControl);

                UpdateList = UpdateList ?? new ObservableCollection<IMovableObject>();
                if (NewObject.DataContext is IMovableObject)
                    UpdateList.Remove(NewObject.DataContext as IMovableObject);

                //InputList = InputList ?? new List<IReceivesInput>();
                //if (NewObject.DataContext is IReceivesInput)
                //    InputList.Remove(NewObject.DataContext as IReceivesInput);
            }
        }

        internal void ProcessInput(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                IsPaused = !IsPaused;

                if (IsPaused == false)
                {
                    LastTime = DateTime.Now.Ticks;
                    foreach (IMovableObject Item in UpdateList)
                    {
                        Item.LastTime = LastTime;
                    }
                }
            }
        }

        #region Command Methods

        public static void ListAllObjects()
        {
            List<String> SendToConsole = new List<string>();

            int i = 0;
            foreach (GameControl g in CurrentWorld.RenderList)
            {
                SendToConsole.Add(String.Format("Object {0}: {1}", i, g.DataContext.ToString()));
                i++;
            }

            DeveloperConsoleControl.PublishToConsole(SendToConsole);
        }

        public static void AddEnemy()
        {
            Random Generator = new Random((int)DateTime.Now.Ticks);

            double StartX = Generator.NextDouble() * CurrentWorld.Bounds.Width;
            double StartY = Generator.NextDouble() * CurrentWorld.Bounds.Height;

            EnemyObject e = new BasicEnemy(new Vector(StartX, StartY));
            EnemyControl ctrl = new EnemyControl() { DataContext = e };

            CurrentWorld.AddObject(ctrl);
        }

        public static void AddPlayer()
        {
            foreach (GameControl g in CurrentWorld.RenderList)
            {
                if (g is PlayerControl)
                    return;
            }

            PlayerObject p = new PlayerObject();
            PlayerControl ctrl = new PlayerControl() { DataContext = p };
            CurrentWorld.AddObject(ctrl);
        }

        public static void ToggleIsPaused()
        {
            WorldObject.CurrentWorld.IsPaused = !WorldObject.CurrentWorld.IsPaused;
        }

        public static void MoveCameraRight()
        {
            WorldObject.CurrentWorld.Camera.Position += new Vector(100, 0);
        }

        public static void ZoomIn()
        {
            WorldObject.CurrentWorld.Camera.Zoom += new Vector(0.15, 0.15);
        }

        public static void ZoomOut()
        {
            if (WorldObject.CurrentWorld.Camera.Zoom.X > 0.35 &&
                WorldObject.CurrentWorld.Camera.Zoom.Y > 0.35)
                WorldObject.CurrentWorld.Camera.Zoom += new Vector(-0.15, -0.15);
        }

        public static void GetCameraInfo()
        {
            String CamInfo = String.Format("CameraPosition - [X:{0},Y:{1}], ZoomLevel - [X:{2},Y:{3}]", GetCamera().Position.X, GetCamera().Position.Y, GetCamera().Zoom.X, GetCamera().Zoom.Y);

            DeveloperConsoleControl.PublishToConsole(CamInfo);
        }

        #endregion Command Methods
    }
}
