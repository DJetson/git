using GPOne.BaseClasses;
using GPOne.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GPOne.Objects
{
    public class PlaceHolderObject : INotifyPropertyChanged, IGameObject, IColorObject, IScalableObject, IMovableObject, IReceivesInput, IHasGravity
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region IGameObject Implementation
        private WorldObject _Parent;
        public WorldObject World
        {
            get { return _Parent; }
            set
            {
                if (value == null)
                    throw new Exception("SetWorld failed. World cannot be null.");

                _Parent = value;
                NotifyPropertyChanged("World");
            }
        }
        #endregion IGameObject Implementation

        #region IMovableObject Implementation
        private BoundedVector _Position;
        public BoundedVector Position
        {
            get { return _Position; }
            set { _Position = value; NotifyPositionChanged(); }
        }

        private BoundedVector _Velocity = new BoundedVector();
        public BoundedVector Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; NotifyVelocityChanged(); }
        }

        private BoundedVector _Acceleration = new BoundedVector();
        public BoundedVector Acceleration
        {
            get { return _Acceleration; }
            set { _Acceleration = value; NotifyAccelerationChanged(); }
        }

        private double _Angle;
        public double Angle
        {
            get { return _Angle; }
            set { _Angle = value; NotifyPropertyChanged("Angle"); }
        }


        private double _AngularVelocity;
        public double AngularVelocity
        {
            get { return _AngularVelocity; }
            set { _AngularVelocity = value; NotifyPropertyChanged("AngularVelocity"); }
        }

        private double _AngularAcceleration;
        public double AngularAcceleration
        {
            get { return _AngularAcceleration; }
            set { _AngularAcceleration = value; NotifyPropertyChanged("AngularAcceleration"); }
        }

        //private long _ElapsedTime;
        public long ElapsedTime
        {
            get { return DateTime.Now.Ticks - LastTime; }
        }

        public Vector NextPosition { get { return (Position.Current + (Velocity.Current * ElapsedTime)); } }
        public Vector NextVelocity { get { return (Velocity.Current + (Acceleration.Current * ElapsedTime)); } }

        private Vector _NextAcceleration;
        public Vector NextAcceleration
        {
            get { return _NextAcceleration; }
            set { _NextAcceleration = value; NotifyPropertyChanged("NextAcceleration"); }
        }

        public double NextAngle { get { return (Angle + (AngularVelocity * ElapsedTime)); } }
        public double NextAngularVelocity { get { return (AngularVelocity + (AngularAcceleration * ElapsedTime)); } }

        private double _NextAngularAcceleration;
        public double NextAngularAcceleration
        {
            get { return _NextAngularAcceleration; }
            set { _NextAngularAcceleration = value; NotifyPropertyChanged("NextAngularAcceleration"); }
        }

        private long _LastTime = 0;
        public long LastTime
        {
            get { return _LastTime; }
            set { _LastTime = value; NotifyLastTimeChanged(); }
        }

        public void NotifyPositionChanged()
        {
            NotifyPropertyChanged("Position");
            NotifyPropertyChanged("Center");
            NotifyPropertyChanged("Left");
            NotifyPropertyChanged("Top");
            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("Bottom");
        }

        public void NotifyVelocityChanged()
        {
            NotifyPropertyChanged("Velocity");
            NotifyPropertyChanged("NextPosition");
        }

        public void NotifyAccelerationChanged()
        {
            NotifyPropertyChanged("Acceleration");
            NotifyPropertyChanged("NextVelocity");
        }

        public void NotifyAngleChanged()
        {
            NotifyPropertyChanged("Angle");
        }

        public void NotifyAngularVelocityChanged()
        {
            NotifyPropertyChanged("AngularVelocity");
            NotifyPropertyChanged("NextAngle");
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
        }
        #endregion IMovableObject Implementation

        #region IScalableObject Implementation
        private Vector _Size;
        public Vector Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
                NotifySizeChanged();
            }
        }

        private Vector _Scale;
        public Vector Scale
        {
            get { return _Scale; }
            set
            {
                _Scale = value;
                NotifyScaleChanged();
            }
        }

        /// <summary>
        /// Object Width
        /// </summary>
        public double Width { get { return Size.X * Scale.X; } }

        /// <summary>
        /// Object Height
        /// </summary>
        public double Height { get { return Size.Y * Scale.Y; } }

        /// <summary>
        /// The Transformation Point for this object. Given in local space
        /// </summary>
        public Vector Center { get { return new Vector(Width / 2, Height / 2); } }

        /// <summary>
        /// The The X-Axis value of the Left Edge in global space
        /// </summary>
        public double Left { get { return Position.Current.X - Center.X; } }

        /// <summary>
        /// The Y-Axis value of the Top edge in global space
        /// </summary>
        public double Top { get { return Position.Current.Y - Center.Y; } }

        /// <summary>
        /// The X-Axis value of the right edge in global space
        /// </summary>
        public double Right { get { return Position.Current.X + Center.X; } }

        /// <summary>
        /// The Y-Axis value of the bottom edge in global space
        /// </summary>
        public double Bottom { get { return Position.Current.Y + Center.Y; } }

        public void NotifySizeChanged()
        {
            NotifyPropertyChanged("Size");
            NotifyPropertyChanged("Width");
            NotifyPropertyChanged("Height");
            NotifyPropertyChanged("Center");
            NotifyPropertyChanged("Left");
            NotifyPropertyChanged("Top");
            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("Bottom");
        }

        public void NotifyScaleChanged()
        {
            NotifyPropertyChanged("Scale");
            NotifyPropertyChanged("Width");
            NotifyPropertyChanged("Height");
            NotifyPropertyChanged("Center");
            NotifyPropertyChanged("Left");
            NotifyPropertyChanged("Top");
            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("Bottom");
        }

        #endregion IScalableObject Implementation

        #region IColorObject Implementation
        private Brush _Fill;
        public Brush Fill
        {
            get { return _Fill; }
            set { _Fill = value; NotifyPropertyChanged("Fill"); }
        }

        private Brush _Stroke;
        public Brush Stroke
        {
            get { return _Stroke; }
            set { _Stroke = value; NotifyPropertyChanged("Stroke"); }
        }

        #endregion IColorObject Implementation


        /// <summary>
        /// The approximate position that this object will reside at after the next update
        /// </summary>
        public double NextX { get { return (Position.Current.X + (Velocity.Current.X * ElapsedTime)); } }

        /// <summary>
        /// The approximate position that this object will reside at after the next update
        /// </summary>
        public double NextY { get { return (Position.Current.Y + (Velocity.Current.Y * ElapsedTime)); } }

        private double _MaxAcceleration = 1.0e-11;
        //private double FrictionCoefficient = 0.4;

        public PlaceHolderObject()
        {
        }

        public void BeginUpdate(long ElapsedTime)
        {
            NotifyPropertyChanged("ElapsedTime");
            Update(ElapsedTime);
            LastTime = DateTime.Now.Ticks;
        }

        public void Update(long ElapsedTime)
        {
            Acceleration.Current = NextAcceleration;
            Velocity.Current = NextVelocity;
            Position.Current = NextPosition;
            
            AngularVelocity = NextAngularVelocity;
            Angle = NextAngle;

            //Vector Friction = NextVelocity;
            //if (Friction.Length != 0)
            //    Friction.Negate();

            //Friction = Friction * FrictionCoefficient;

//            Velocity.Current = NextVelocity + ApplyGravity(ElapsedTime);
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            Vector NewAcceleration = new Vector(Acceleration.Current.X, Acceleration.Current.Y);
            if (e.Key == Key.A)
                NewAcceleration.X = -_MaxAcceleration;
            if (e.Key == Key.D)
                NewAcceleration.X = +_MaxAcceleration;
            if (e.Key == Key.W)
                NewAcceleration.Y = -_MaxAcceleration;
            if (e.Key == Key.S)
                NewAcceleration.Y = +_MaxAcceleration;
            /// Set Acceleration to Maximum

//            Acceleration.Current = NewAcceleration;
            NextAcceleration = NewAcceleration;
        }


        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            Vector NewAcceleration = new Vector(Acceleration.Current.X, Acceleration.Current.Y);
            if ((e.Key == Key.A) || (e.Key == Key.D))
                NewAcceleration.X = 0;
            if ((e.Key == Key.W) || (e.Key == Key.S))
                NewAcceleration.Y = 0;
            /// Set Acceleration to Maximum

            //Acceleration.Current = NewAcceleration;
            NextAcceleration = NewAcceleration;
            /// Set Acceleration to Friction
        }

        public Vector ApplyGravity(long ElapsedTime)
        {
            return WorldObject.Gravity * ElapsedTime;
        }







        
    }
}
