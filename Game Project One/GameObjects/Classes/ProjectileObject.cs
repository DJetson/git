using GameObjects.Controls;
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
    public class ProjectileObject : GameObject
    {

        public ProjectileControl Control;
        public double Damage = 2;

        public override void Update(long ElapsedTime)
        {
           // NotifyPropertyChanged("ElapsedTime");

            Adjustment = new Vector(0, 0);
            Acceleration.Current = NextAcceleration;
            Velocity.Current = NextVelocity;
            Position.Current = NextPosition;

            Angle = NextAngle;

            if (Position.Current.X > WorldObject.CurrentWorld.Bounds.Right ||
                Position.Current.X < 0 ||
                Position.Current.Y > WorldObject.CurrentWorld.Bounds.Bottom ||
                Position.Current.Y < 0)
                WorldObject.CurrentWorld.RemoveObject(Control);

           // LastTime = DateTime.Now.Ticks;
        }

        public ProjectileObject(Vector StartingPosition, Vector Trajectory)
        {
            Position = new BoundedVector(StartingPosition.X, StartingPosition.Y);
            Velocity = new BoundedVector(Trajectory.X, Trajectory.Y);
            Fill = new SolidColorBrush(Colors.Beige);
            Size = new Vector(10, 10);
            LastTime = DateTime.Now.Ticks;

        }

        public void Remove()
        {
            WorldObject.CurrentWorld.RemoveObject(Control);
        }

        public override string ToString()
        {
            return String.Format("Position - [X:{0}, Y:{1}]\nVelocity - [X:{2}, Y:{3}]", Position.Current.X, Position.Current.Y, Velocity.Current.X, Velocity.Current.Y);
        }
    }
}
