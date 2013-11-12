using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Interfaces
{
    /// <summary>
    /// This interface allows an object to be positioned. It provides information about
    /// the object bounds. It imposes implementation of IMoveableObject and IScalableObject
    /// </summary>
    public interface IPositionInfo : IMovableObject, IScalableObject
    {
        /// <summary>
        /// The location of the left edge of the object
        /// </summary>
        double Left { get; }

        /// <summary>
        /// The location of the top edge of the object
        /// </summary>
        double Top { get; }

        /// <summary>
        /// The location of the right edge of the object
        /// </summary>
        double Right { get; }

        /// <summary>
        /// The location of the bottom edge of the object
        /// </summary>
        double Bottom { get; }
    }
}
