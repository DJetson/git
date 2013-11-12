using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPOne.Objects
{
    public class AnimationStateChangedEventArgs : EventArgs
    {
        public PlayMode OldState { get; set; }
        public PlayMode NewState { get; set; }
    }
}
