using GPOne.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GPOne.Objects
{
    public class ZombieObject : INotifyPropertyChanged, IColorObject, ISensoryInfo
    {
        #region ZombieObject Enumerations
        public enum LocomotiveStates { Idle, Wander, Stalk };
        #endregion ZombieObject Enumerations

        #region ZombieObject Members
        private WorldObject _Parent;
        private Brush _Fill = new SolidColorBrush(Colors.Green);
        private Brush _Stroke = new SolidColorBrush(Colors.Black);
        private Vector _Size = new Vector(5, 5);
        private Vector _Scale = new Vector(1.0, 1.0);
        private BoundedVector _Position;
        private BoundedVector _Velocity;
        private BoundedVector _Acceleration;
        private Vector _NextAcceleration;
        private double _Angle;
        private double _AngularVelocity;
        private double _AngularAcceleration;
        private double _NextAngularAcceleration;
        private long _LastTime = 0;
        private double _MaxAcceleration = 1.0e-11;
        private double _RadiusOfAwareness = 200;
        private double _VisualAcuity = 1;
        #endregion ZombieObject Members

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        #region IGameObject Implementation
        public WorldObject World
        {
            get { return _Parent; }
            set { _Parent = value; NotifyPropertyChanged("World"); }
        }
        #endregion IGameObject Implementation

        #region ZombieObject Properties
        public double MaxAcceleration
        {
            get { return _MaxAcceleration; }
        }
        #endregion ZombieObject Properties

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

        #region ISensoryInfo Implementation
        public double RadiusOfAwareness
        {
            get { return _RadiusOfAwareness; }
            set { _RadiusOfAwareness = value; NotifyPropertyChanged("RadiusOfAwareness"); }
        }

        public double VisualAcuity
        {
            get { return _VisualAcuity; }
            set { _VisualAcuity = value; NotifyPropertyChanged("VisualAcuity"); }
        }
        #endregion ISensoryInfo Implementation

        #region IPositionInfo Implementation
        public double Left { get { return Position.Current.X - Width / 2; } }
        public double Top { get { return Position.Current.Y - Height / 2; } }
        public double Right { get { return Position.Current.X + Width / 2; } }
        public double Bottom { get { return Position.Current.Y + Height / 2; } }
        public Vector NextPosition { get { return (Position.Current + (Velocity.Current * ElapsedTime)) + Adjustment; } }
        public Vector NextVelocity { get { return (Velocity.Current + (Acceleration.Current * ElapsedTime)); } }
        public double NextAngle { get { return (Angle + (AngularVelocity * ElapsedTime)); } }
        public double NextAngularVelocity { get { return (AngularVelocity + (AngularAcceleration * ElapsedTime)); } }
        public long ElapsedTime { get { return DateTime.Now.Ticks - LastTime; } }
        public double Width { get { return Size.X * Scale.X; } }
        public double Height { get { return Size.Y * Scale.Y; } }
        public double CenterX { get { return Width / 2; } }
        public double CenterY { get { return Height / 2; } }
        public Vector Adjustment;

        public BoundedVector Position
        {
            get { return _Position; }
            set { _Position = value; NotifyPropertyChanged("Position"); }
        }

        public BoundedVector Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; NotifyPropertyChanged("Velocity"); }
        }

        public BoundedVector Acceleration
        {
            get { return _Acceleration; }
            set { _Acceleration = value; NotifyPropertyChanged("Acceleration"); }
        }

        public Vector NextAcceleration
        {
            get { return _NextAcceleration; }
            set { _NextAcceleration = value; NotifyPropertyChanged("NextAcceleration"); }
        }

        public double Angle
        {
            get { return _Angle; }
            set { _Angle = value; NotifyPropertyChanged("Angle"); }
        }

        public double AngularVelocity
        {
            get { return _AngularVelocity; }
            set { _AngularVelocity = value; NotifyPropertyChanged("AngularVelocity"); }
        }

        public double AngularAcceleration
        {
            get { return _AngularAcceleration; }
            set { _AngularAcceleration = value; NotifyPropertyChanged("AngularAcceleration"); }
        }

        public double NextAngularAcceleration
        {
            get { return _NextAngularAcceleration; }
            set { _NextAngularAcceleration = value; NotifyPropertyChanged("NextAngularAcceleration"); }
        }

        public long LastTime
        {
            get { return _LastTime; }
            set { _LastTime = value; NotifyPropertyChanged("LastTime"); }
        }

        public void NotifyPositionChanged()
        {
            NotifyPropertyChanged("Position");
            NotifyPropertyChanged("NextPosition");
            NotifyPropertyChanged("Left");
            NotifyPropertyChanged("Top");
            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("Bottom");
        }

        public void NotifyVelocityChanged()
        {
            NotifyPropertyChanged("Velocity");
            NotifyPropertyChanged("NextPosition");
            NotifyPropertyChanged("NextVelocity");
        }

        public void NotifyAccelerationChanged()
        {
            NotifyPropertyChanged("Acceleration");
            NotifyPropertyChanged("NextVelocity");
        }

        public void NotifyAngleChanged()
        {
            NotifyPropertyChanged("Angle");
            NotifyPropertyChanged("NextAngle");
        }

        public void NotifyAngularVelocityChanged()
        {
            NotifyPropertyChanged("AngularVelocity");
            NotifyPropertyChanged("NextAngle");
            NotifyPropertyChanged("NextAngularVelocity");
        }

        public void NotifyAngularAccelerationChanged()
        {
            NotifyPropertyChanged("AngularAcceleration");
            NotifyPropertyChanged("NextAngularVelocity");
        }

        public void NotifyLastTimeChanged()
        {
            NotifyPropertyChanged("LastTime");
            NotifyPropertyChanged("ElapsedTime");
            NotifyPropertyChanged("NextPosition");
            NotifyPropertyChanged("NextVelocity");
            NotifyPropertyChanged("NextAngle");
            NotifyPropertyChanged("NextAngularVelocity");
        }

        public Vector Size
        {
            get { return _Size; }
            set { _Size = value; NotifyPropertyChanged("Size"); }
        }

        public Vector Scale
        {
            get { return _Scale; }
            set { _Scale = value; NotifyPropertyChanged("Scale"); }
        }

        public void NotifySizeChanged()
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
        }

        public void NotifyScaleChanged()
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
        }

        public void BeginUpdate(long ElapsedTime)
        {
            NotifyPropertyChanged("ElapsedTime");
            Update(ElapsedTime);
            LastTime = DateTime.Now.Ticks;
        }

        public void Update(long ElapsedTime)
        {
            Adjustment = new Vector(0, 0);
            Acceleration.Current = NextAcceleration;
            Velocity.Current = NextVelocity;
            Position.Current = NextPosition;

            AngularVelocity = Velocity.Current.Length * 1.25;
            Angle = NextAngle;

        }
        #endregion IPositionInfo Implementation

        public ZombieObject()
        {
            Vector Start = LaunchConditions.GetRandomPosition();
            Position = new BoundedVector(Start.X,Start.Y);
            Velocity = new BoundedVector(0, 0);
            Acceleration = new BoundedVector(0, 0);
        }
    }
}
