using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GPOne.Interfaces
{
    /// <summary>
    /// This interface allows an objects size to be altered
    /// </summary>
    public interface IScalableObject
    {
        /// <summary>
        /// The Unit Size of the object. It should not be changed after being set.
        /// </summary>
        Vector Size { get; set; }

        /// <summary>
        /// The Scale of the object. Any scaling of the object should be done through this property
        /// </summary>
        Vector Scale { get; set; }

        /// <summary>
        /// The Scaled Width of the object
        /// </summary>
        double Width { get; }

        /// <summary>
        /// The Scaled Height of the object
        /// </summary>
        double Height { get; }

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in Size
        /// </summary>
        void NotifySizeChanged();

        /// <summary>
        /// Executes NotifyPropertyChanged on all properties affected by a change in Scale
        /// </summary>
        void NotifyScaleChanged();
    }
}
