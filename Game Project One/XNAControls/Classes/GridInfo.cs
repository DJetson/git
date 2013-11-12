using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace XnaControls.Classes
{
    public class GridInfo : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        private Size _TileSize;
        public Size TileSize
        {
            get { return _TileSize; }
            set { _TileSize = value; NotifyPropertyChanged("TileSize"); }
        }

        private Size _LevelSize;
        public Size LevelSize
        {
            get { return _LevelSize; }
            set { _LevelSize = value; NotifyPropertyChanged("LevelSize"); }
        }

        private double _LineThickness;
        public double LineThickness
        {
            get { return _LineThickness; }
            set { _LineThickness = value; NotifyPropertyChanged("LineThickness"); }
        }

        private Color _GridColor;
        public Color GridColor
        {
            get { return _GridColor; }
            set { _GridColor = value; NotifyPropertyChanged("GridColor"); }
        }

        private double _Opacity;
        public double Opacity
        {
            get { return _Opacity; }
            set { _Opacity = value; NotifyPropertyChanged("Opacity"); }
        }
    }
}
