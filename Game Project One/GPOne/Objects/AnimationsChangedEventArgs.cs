using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPOne.Objects
{
    public class AnimationsChangedEventArgs : EventArgs
    {
        public String Key { get; set; }
        public AnimatedClip Added { get; set; }
        public AnimatedClip Removed { get; set; }
    }
}
