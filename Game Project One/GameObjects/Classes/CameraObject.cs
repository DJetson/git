using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameObjects.Classes
{
    public class CameraObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        /// <summary>
        /// The center of the camera view in World Space
        /// </summary>
        private Vector _Position;
        public Vector Position
        {
            get { return _Position; }
            set { _Position = value; NotifyPropertyChanged("Position"); NotifyPropertyChanged("Bounds"); }
        }

        /// <summary>
        /// The size of the viewport in screen space. (Example: If the display resolution is
        /// 1024 x 768 and the game window consists of the viewport and a vertical HUD against
        /// the left edge of the screen with dimensions measuring 100 x 768, then the 
        /// ViewSize would be 924 x 768
        /// </summary>
        private Size _ViewportSize;
        public Size ViewportSize
        {
            get { return _ViewportSize; }
            set { _ViewportSize = value; NotifyPropertyChanged("ViewportSize"); NotifyPropertyChanged("Bounds"); }
        }

        /// <summary>
        /// The scale of the of the world inside the viewport. Effectively, this is the 
        /// size of the area that is visible in World Space. This is used to make the camera
        /// zoom in and out.
        /// </summary>
        private Vector _Zoom = new Vector(1, 1);
        public Vector Zoom
        {
            get { return _Zoom; }
            set { _Zoom = value; NotifyPropertyChanged("Zoom"); }
        }

        /// <summary>
        /// Gets the camera rectangle
        /// </summary>
        public Rect Bounds
        {
            get { return new Rect(Position.X - ViewportSize.Width / 2, Position.Y - ViewportSize.Height / 2, ViewportSize.Width, ViewportSize.Height); }
        }

        public CameraObject()
        {
        }
    }
}
