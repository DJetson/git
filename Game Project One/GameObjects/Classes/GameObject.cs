using GameObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GameObjects.Classes
{
    public abstract class GameObject : INotifyPropertyChanged, IColorObject, IPositionInfo//ISensoryInfo
    {

        #region Abstract Methods
        /// <summary>
        /// This function must be overridden by all game objects according to their specific needs
        /// NOTE: The very first line of every override must be:
        /// 
        /// NotifyPropertyChanged("ElapsedTime");
        /// 
        /// This raises a property changed event for the elapsed time property of the game object
        /// NOTE: In addition to the above, the last line of every override should be: 
        /// 
        /// LastTime = DateTime.Now.Ticks;
        /// 
        /// This ensures that the next update cycle will be calculated properly.
        /// </summary>
        /// <param name="ElapsedTime"></param>
        public abstract void Update(long ElapsedTime);
        #endregion Abstract Methods

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        #region CharacterBase Members
        protected WorldObject _Parent;
        protected Brush _Fill = new SolidColorBrush(Colors.Black);
        protected Brush _Stroke = new SolidColorBrush(Colors.Black);
        protected Vector _Size = new Vector(25, 25);
        protected Vector _Scale = new Vector(1, 1);
        protected BoundedVector _Position = new BoundedVector(100, 100);
        protected BoundedVector _Velocity = new BoundedVector(0, 0);
        protected BoundedVector _Acceleration = new BoundedVector(0, 0);
        protected Vector _NextAcceleration;
        protected double _Angle;
        protected double _AngularVelocity;
        protected double _AngularAcceleration;
        protected double _NextAngularAcceleration;
        protected long _LastTime = DateTime.Now.Ticks;
        protected double _MaxAcceleration = 1.0e-11;
        //private double _RadiusOfAwareness = 200;
        //private double _VisualAcuity = 1;
        #endregion CharacterBase Members

        #region IGameObject Implementation
        public WorldObject World
        {
            get { return _Parent; }
            set { _Parent = value; NotifyPropertyChanged("World"); }
        }
        #endregion IGameObject Implementation

        #region IColorObject Implementation
        public Brush Fill
        {
            get { return _Fill; }
            set { _Fill = value; NotifyPropertyChanged("Fill"); }
        }

        public Brush Stroke
        {
            get { return _Stroke; }
            set { _Stroke = value; NotifyPropertyChanged("Stroke"); }
        }
        #endregion IColorObject Implementation

        #region IPositionInfo Implementation
        public double Left { get { return Position.Current.X - Width / 2; } }
        public double Top { get { return Position.Current.Y - Height / 2; } }
        public double Right { get { return Position.Current.X + Width / 2; } }
        public double Bottom { get { return Position.Current.Y + Height / 2; } }
        public Rect Bounds { get { return new Rect(new Point(Left, Top), new Point(Right, Bottom)); } }
        public Vector NextPosition { get { return (Position.Current + (Velocity.Current * ElapsedTime)) + Adjustment; } }
        public Vector NextVelocity { get { return (Velocity.Current + (Acceleration.Current * ElapsedTime)); } }
        public double NextAngle { get { return (Angle + (AngularVelocity * ElapsedTime)); } }
        public double NextAngularVelocity { get { return (AngularVelocity + (AngularAcceleration * ElapsedTime)); } }
        public long ElapsedTime { get { return DateTime.Now.Ticks - LastTime; } }
        public double Width { get { return Size.X * WorldObject.CurrentWorld.Camera.Zoom.X; } }//Scale.X; } }
        public double Height { get { return Size.Y * WorldObject.CurrentWorld.Camera.Zoom.Y; } }//Scale.Y; } }
        public double CenterX { get { return Width / 2; } }
        public double CenterY { get { return Height / 2; } }
        public Vector Adjustment;

        private int _ZIndex = 1;
        public int ZIndex
        {
            get { return _ZIndex; }
            set { _ZIndex = value; NotifyPropertyChanged("ZIndex"); }
        }

        public BoundedVector Position
        {
            get { return _Position; }
            set { _Position = value; NotifyPositionChanged(); }
        }

        public Vector DrawPosition
        {
            get
            {

                Vector pos = new Vector(((WorldObject.CurrentWorld.Camera.ViewportSize.Width / 2) * -WorldObject.CurrentWorld.Camera.Zoom.X),
                                        ((WorldObject.CurrentWorld.Camera.ViewportSize.Height/ 2) * -WorldObject.CurrentWorld.Camera.Zoom.Y));
                //Vector pos = new Vector(Position.Current.X * WorldObject.CurrentWorld.Camera.Zoom.X,Position.Current.Y * WorldObject.CurrentWorld.Camera.Zoom.Y);
                return Position.Current - WorldObject.CurrentWorld.Camera.Position;// new Vector(pos.X * WorldObject.CurrentWorld.Camera.Zoom.X, pos.Y * WorldObject.CurrentWorld.Camera.Zoom.Y);
            }
        }

        public BoundedVector Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; NotifyVelocityChanged(); }
        }

        public BoundedVector Acceleration
        {
            get { return _Acceleration; }
            set { _Acceleration = value; NotifyAccelerationChanged(); }
        }

        public Vector NextAcceleration
        {
            get { return _NextAcceleration; }
            set { _NextAcceleration = value; NotifyPropertyChanged("NextAcceleration"); }
        }

        public double Angle
        {
            get { return _Angle; }
            set { _Angle = value; NotifyAngleChanged(); }
        }

        public double AngularVelocity
        {
            get { return _AngularVelocity; }
            set { _AngularVelocity = value; NotifyAngularVelocityChanged(); }
        }

        public double AngularAcceleration
        {
            get { return _AngularAcceleration; }
            set { _AngularAcceleration = value; NotifyAngularAccelerationChanged(); }
        }

        public double NextAngularAcceleration
        {
            get { return _NextAngularAcceleration; }
            set { _NextAngularAcceleration = value; NotifyPropertyChanged("NextAngularAcceleration"); }
        }

        public long LastTime
        {
            get { return _LastTime; }
            set { _LastTime = value; NotifyLastTimeChanged(); }
        }

        public virtual void NotifyPositionChanged()
        {
            NotifyPropertyChanged("Position");
            NotifyPropertyChanged("NextPosition");
            NotifyPropertyChanged("DrawPosition");
            NotifyPropertyChanged("Left");
            NotifyPropertyChanged("Top");
            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("Bottom");
            NotifyPropertyChanged("Bounds");
        }

        public virtual void NotifyVelocityChanged()
        {
            NotifyPropertyChanged("Velocity");
            NotifyPropertyChanged("NextPosition");
            NotifyPropertyChanged("DrawPosition");
            NotifyPropertyChanged("NextVelocity");
        }

        public virtual void NotifyAccelerationChanged()
        {
            NotifyPropertyChanged("Acceleration");
            NotifyPropertyChanged("NextVelocity");
        }

        public virtual void NotifyAngleChanged()
        {
            NotifyPropertyChanged("Angle");
            NotifyPropertyChanged("NextAngle");
        }

        public virtual void NotifyAngularVelocityChanged()
        {
            NotifyPropertyChanged("AngularVelocity");
            NotifyPropertyChanged("NextAngle");
            NotifyPropertyChanged("NextAngularVelocity");
        }

        public virtual void NotifyAngularAccelerationChanged()
        {
            NotifyPropertyChanged("AngularAcceleration");
            NotifyPropertyChanged("NextAngularVelocity");
        }

        public virtual void NotifyLastTimeChanged()
        {
            NotifyPropertyChanged("LastTime");
            NotifyPropertyChanged("ElapsedTime");
            NotifyPropertyChanged("NextPosition");
            NotifyPropertyChanged("DrawPosition");
            NotifyPropertyChanged("NextVelocity");
            NotifyPropertyChanged("NextAngle");
            NotifyPropertyChanged("NextAngularVelocity");
        }

        public Vector Size
        {
            get { return _Size; }
            set { _Size = value; NotifySizeChanged(); }
        }

        public Vector Scale
        {
            get { return _Scale; }
            set { _Scale = value; NotifyScaleChanged(); }
        }

        public virtual void NotifySizeChanged()
        {
            NotifyPropertyChanged("Size");
            NotifyPropertyChanged("Width");
            NotifyPropertyChanged("Height");
            NotifyPropertyChanged("Left");
            NotifyPropertyChanged("Top");
            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("Bottom");
            NotifyPropertyChanged("CenterX");
            NotifyPropertyChanged("CenterY");
            NotifyPropertyChanged("Bounds");

        }

        public virtual void NotifyScaleChanged()
        {
            NotifyPropertyChanged("Scale");
            NotifyPropertyChanged("Width");
            NotifyPropertyChanged("Height");
            NotifyPropertyChanged("Left");
            NotifyPropertyChanged("Top");
            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("Bottom");
            NotifyPropertyChanged("CenterX");
            NotifyPropertyChanged("CenterY");
            NotifyPropertyChanged("Bounds");

        }

        public void NotifyCameraSettingsChanged()
        {
            NotifyScaleChanged();
        }

        public void BeginUpdate(long ElapsedTime)
        {
            NotifyPropertyChanged("ElapsedTime");
            NotifyScaleChanged();
            Update(ElapsedTime);

            LastTime = DateTime.Now.Ticks;
        }
        #endregion IPositionInfo Implementation

        #region ISensoryInfo Implementation !NOT USED!
        //public double RadiusOfAwareness
        //{
        //    get { return _RadiusOfAwareness; }
        //    set { _RadiusOfAwareness = value; NotifyPropertyChanged("RadiusOfAwareness"); }
        //}

        //public double VisualAcuity
        //{
        //    get { return _VisualAcuity; }
        //    set { _VisualAcuity = value; NotifyPropertyChanged("VisualAcuity"); }
        //}
        #endregion ISensoryInfo Implementation



        public GameObject()
        {
        }

    }
}
