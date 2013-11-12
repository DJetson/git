using GPOne.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GPOne.Interfaces
{
    /// <summary>
    /// IAnimated enforces a set of properties which allow a game object to be animated.
    /// INotifyPropertyChanged is also enforced so that animated objects can raise property
    /// changed events.
    /// </summary>
    public interface IAnimated : INotifyPropertyChanged
    {
        /// <summary>
        /// A collection of all animations employed by an object
        /// </summary>
        AnimationSet AnimationSet { get; set; }

        /// <summary>
        /// The current active animation
        /// </summary>
        AnimatedClip CurrentAnimation { get; set; }

        /// <summary>
        /// Initialize animations for this object and set the initial active animation.
        /// </summary>
        void InitializeAnimations();
    }
}
