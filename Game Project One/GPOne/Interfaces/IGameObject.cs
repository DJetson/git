using GPOne.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOne.Interfaces
{
    /// <summary>
    /// All game objects must implement this interface to be compatible with various object
    /// managers
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// This is a reference to the world in which the object resides
        /// </summary>
        WorldObject World
        {
            set;
            get;
        }
    }
}
