using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameObjects.Classes
{
    /// <summary>
    /// Bounded Vector is a group of 3 vectors representing the current value, as well as an upper and lower bound for
    /// the vector
    /// </summary>
    public class BoundedVector : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        private Vector _Current;
        public Vector Current
        {
            get { return _Current; }
            set
            {
                if (value.X < Minimum.X)
                    _Current.X = Minimum.X;
                else if (value.X > Maximum.X)
                    _Current.X = Maximum.X;
                else
                    _Current.X = value.X;

                if (value.Y < Minimum.Y)
                    _Current.Y = Minimum.Y;
                else if (value.Y > Maximum.Y)
                    _Current.Y = Maximum.Y;
                else
                    _Current.Y = value.Y;

                NotifyPropertyChanged("Current");
            }
        }

        private Vector _Minimum;
        public Vector Minimum
        {
            get { return _Minimum; }
            set { _Minimum = value; NotifyPropertyChanged("Minimum"); Current = Current; }
        }

        private Vector _Maximum;
        public Vector Maximum
        {
            get { return _Maximum; }
            set { _Maximum = value; NotifyPropertyChanged("Maximum"); Current = Current; }
        }

        public BoundedVector()
        {
            Current = new Vector(0, 0);
            Minimum = new Vector(double.MinValue, double.MinValue);
            Maximum = new Vector(double.MaxValue, double.MaxValue);
        }

        public BoundedVector(double x, double y)
        {
            Minimum = new Vector(double.MinValue, double.MinValue);
            Maximum = new Vector(double.MaxValue, double.MaxValue);
            Current = new Vector(x, y);
        }

        public BoundedVector(Vector current)
        {
            Minimum = new Vector(double.MinValue, double.MinValue);
            Maximum = new Vector(double.MaxValue, double.MaxValue);
            Current = current;
        }

        public BoundedVector(double x, double y, double minX, double minY, double maxX, double maxY)
        {
            Minimum = new Vector(minX, minY);
            Maximum = new Vector(maxX, maxY);
            Current = new Vector(x, y);
        }

        public BoundedVector(Vector current, Vector Min, Vector Max)
        {
            Minimum = Min;
            Maximum = Max;
            Current = current;
        }
    }
}
