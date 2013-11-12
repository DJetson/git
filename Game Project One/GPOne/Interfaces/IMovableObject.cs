using GPOne.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GPOne.Interfaces
{
    /// <summary>
    /// This interface enforces a set of properties which facilitate movement.
    /// </summary>
    public interface IMovableObject
    {
        /// <summary>
        /// The current position
        /// </summary>
        BoundedVector Position { get; set; }

        /// <summary>
        /// The Position to which the object will be moved at the end of the current update cycle
        /// </summary>
        Vector NextPosition { get; }

        /// <summary>
        /// The current velocity
        /// </summary>
        BoundedVector Velocity { get; set; }

        /// <summary>
        /// The Velocity that will be applied at the end of the current update cycle
        /// </summary>
        Vector NextVelocity { get; }

        /// <summary>
        /// The current acceleration
        /// </summary>
        BoundedVector Acceleration { get; set; }

        /// <summary>
        /// The Accleration that will be applied at the end of the current update cycle
        /// </summary>
        Vector NextAcceleration { get; set; }

        /// <summary>
        /// The objects current rotation
        /// </summary>
        double Angle { get; set; }

        /// <summary>
        /// The rotation that will be applied at the end of the current update cycle
        /// </summary>
        double NextAngle { get; }

        /// <summary>
        /// The objects current angular velocity
        /// </summary>
        double AngularVelocity { get; set; }

        /// <summary>
        /// The Angular Velocity that will be applied at the end of the current update cycle
        /// </summary>
        double NextAngularVelocity { get; }

        /// <summary>
        /// The objects current angular acceleration
        /// </summary>
        double AngularAcceleration { get; set; }

        /// <summary>
        /// The Angular Acceleration that will be applied at the end of the current update cycle
        /// </summary>
        double NextAngularAcceleration { get; set; }

        /// <summary>
        /// The value of DateTime.Now.Ticks at the end of the last update cycle
        /// </summary>
        long LastTime { get; set; }

        /// <summary>
        /// The amount of time that has transpired since the last update cycle
        /// </summary>
        long ElapsedTime { get; }

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in Position
        /// </summary>
        void NotifyPositionChanged();

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in Velocity
        /// </summary>
        void NotifyVelocityChanged();

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in Acceleration
        /// </summary>
        void NotifyAccelerationChanged();

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in Rotation
        /// </summary>
        void NotifyAngleChanged();

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in AngularVelocity
        /// </summary>
        void NotifyAngularVelocityChanged();

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in AngularAcceleration
        /// </summary>
        void NotifyAngularAccelerationChanged();

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in LastTime
        /// </summary>
        void NotifyLastTimeChanged();

        /// <summary>
        /// Begins an update cycle
        /// </summary>
        /// <param name="ElapsedTime">The amount of time that has transpired since the last update</param>
        void BeginUpdate(long ElapsedTime);

        /// <summary>
        /// Updates all Position and Orientation values
        /// </summary>
        /// <param name="ElapsedTime">The amount of time that has transpired since the last update</param>
        void Update(long ElapsedTime);
    }
}
