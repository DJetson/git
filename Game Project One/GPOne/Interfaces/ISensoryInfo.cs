using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOne.Interfaces
{
    /// <summary>
    /// This interface contains various properties related to an objects sensory information.
    /// </summary>
    public interface ISensoryInfo : IPositionInfo, IGameObject
    {
        /// <summary>
        /// Definition for a circle whose center point is located at the objects position
        /// This circle represents the limits of this objects awareness, regardless of the type of awareness
        /// </summary>
        double RadiusOfAwareness
        {
            get;
            set;
        }

        /// <summary>
        /// A fractional value between 0 and 1, that defines the maximum effective range of visual awareness.
        /// 1 defines a field of view that is triangular, oriented so that the base of the triangle is perpendicular to the 
        /// direction of travel (Velocity), and whose points are situated as follows: 2 points located
        /// on a circle of radius (r=(RadiusOfAwareness * VisualAcuity)) 1 point located at the objects position.
        /// </summary>
        double VisualAcuity
        {
            get;
            set;
        }
    }
}
