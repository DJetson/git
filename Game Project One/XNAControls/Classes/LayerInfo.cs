using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace XnaControls.Classes
{
    public class LayerInfo : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        private Boolean _IsActive;
        public Boolean IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; NotifyPropertyChanged("IsActive"); }
        }

        private Boolean _IsVisible;
        public Boolean IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; NotifyPropertyChanged("IsVisible"); }
        }

        private Boolean _IsLocked;
        public Boolean IsLocked
        {
            get { return _IsLocked; }
            set { _IsLocked = value; NotifyPropertyChanged("IsLocked"); }
        }

        private String _LayerName;
        public String LayerName
        {
            get { return _LayerName; }
            set { _LayerName = value; NotifyPropertyChanged("LayerName"); }
        }

        private int _LayerDepth;
        public int LayerDepth
        {
            get { return _LayerDepth; }
            set { _LayerDepth = value; NotifyPropertyChanged("LayerDepth"); }
        }

        private Size _TileSize;
        public Size TileSize
        {
            get { return _TileSize; }
            set { _TileSize = value; NotifyPropertyChanged("TileSize"); NotifyPropertyChanged("LayerHeight"); NotifyPropertyChanged("LayerWidth"); }
        }

        private Size _LayerSize;
        public Size LayerSize
        {
            get { return _LayerSize; }
            set { _LayerSize = value; NotifyPropertyChanged("LayerSize"); NotifyPropertyChanged("LayerHeight"); NotifyPropertyChanged("LayerWidth"); }
        }

        public double LayerHeight { get { return _LayerSize.Height * _TileSize.Height; } }
        public double LayerWidth { get { return _LayerSize.Width * _TileSize.Width; } }
    }

}
