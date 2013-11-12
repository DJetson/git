using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGameOne.Classes
{
    /// <summary>
    /// This is the base class for all UIObjects such as menu items, popup texts, etc
    /// </summary>
    public class UIObject : VisualObject
    {
        /// <summary>
        /// Determines the active UIObject. The precise meaning of that depends on the individual
        /// type of UIObject. This more or less will function exactly like the Focusing system
        /// in Windows
        /// </summary>
        protected Boolean _HasFocus;
        public virtual Boolean HasFocus
        {
            get { return _HasFocus; }
        }

        /// <summary>
        /// This objects parent UIObject
        /// </summary>
        protected UIObject _Parent;
        public virtual UIObject Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        /// <summary>
        /// Indicates whether this element responds to camera movement
        /// </summary>
        protected Boolean _IsFixed = true;
        public virtual Boolean IsFixed
        {
            get { return _IsFixed; }
            set { _IsFixed = value; }
        }

        /// <summary>
        /// A collection of child UIObjects
        /// </summary>
        protected List<UIObject> _Children;
        public virtual List<UIObject> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }
    }
}
