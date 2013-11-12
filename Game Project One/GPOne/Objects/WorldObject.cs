#define NO_GRAVITY

using GPOne.BaseClasses;
using GPOne.Controls;
using GPOne.Input;
using GPOne.Interfaces;
using SlimDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GPOne.Objects
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

        #region Management Lists
        ///Management lists are used for all objects. An object may belong to multiple lists, and the lists to
        ///which it belongs determine how the world treats the object. For example, all objects in the RenderList are
        ///drawn to the screen. All objects in the UpdateList undergo positioning calculations at regular intervals.
        ///Objects belonging to the CollisionList check for collisions. This is done in lieu of ObjectManager classes.

        /// <summary>
        /// Management list for renderable objects
        /// </summary>
        private List<GameObjectBase> _RenderList;
        public List<GameObjectBase> RenderList
        {
            get { return _RenderList; }
            set { _RenderList = value; NotifyPropertyChanged("RenderList"); }
        }

        /// <summary>
        /// Management list for moveable objects
        /// </summary>
        private List<IMovableObject> _UpdateList = new List<IMovableObject>();
        public List<IMovableObject> UpdateList
        {
            get { return _UpdateList; }
            set { _UpdateList = value; NotifyPropertyChanged("UpdateList"); }
        }

        private List<IReceivesInput> _InputList = new List<IReceivesInput>();
        public List<IReceivesInput> InputList
        {
            get { return _InputList; }
            set { _InputList = value; NotifyPropertyChanged("InputList"); }
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

        #endregion WorldObject Members


        public WorldObject(String WorldName = "NewWorld", Boolean DebugMode = false)
        {
            Name = WorldName;
            IsDebugModeEnabled = DebugMode;

            Initialize();

            CurrentWorld = this;
        }

        public void Initialize()
        {
            InitializeTimer();
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

            PlayerOneInput.Update();

            if (IsPaused == true)
                return;

            foreach (IMovableObject Item in UpdateList)
                Item.Update(ElapsedTime);
            LastTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Adds a new object to the necessary management lists
        /// </summary>
        /// <param name="NewObject">The object to add</param>
        public void AddObject(GameObjectBase NewObject)
        {
            RenderList = RenderList ?? new List<GameObjectBase>();
            if (NewObject is GameObjectBase && RenderList.Contains(NewObject as GameObjectBase) == false)
                RenderList.Add(NewObject as GameObjectBase);

            UpdateList = UpdateList ?? new List<IMovableObject>();
            if (NewObject.DataContext is IMovableObject && UpdateList.Contains(NewObject.DataContext as IMovableObject) == false)
                UpdateList.Add(NewObject.DataContext as IMovableObject);

            InputList = InputList ?? new List<IReceivesInput>();
            if (NewObject.DataContext is IReceivesInput && InputList.Contains(NewObject.DataContext as IReceivesInput) == false)
                InputList.Add(NewObject.DataContext as IReceivesInput);
        }

        /// <summary>
        /// Removes an object from all management lists
        /// </summary>
        /// <param name="NewObject"></param>
        public void RemoveObject(GameObjectBase NewObject)
        {
            RenderList = RenderList ?? new List<GameObjectBase>();
            if (NewObject is GameObjectBase)
                RenderList.Remove(NewObject as GameObjectBase);

            UpdateList = UpdateList ?? new List<IMovableObject>();
            if (NewObject.DataContext is IMovableObject)
                UpdateList.Remove(NewObject.DataContext as IMovableObject);

            InputList = InputList ?? new List<IReceivesInput>();
            if (NewObject.DataContext is IReceivesInput)
                InputList.Remove(NewObject.DataContext as IReceivesInput);
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
    }
}
