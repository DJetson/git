using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace XnaControls.Classes
{
    public class UIBrush : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        //#region ICloneable Implementation
        //public object Clone()
        //{
        //    UIBrush NewBrush = new UIBrush();
        //    NewBrush.Tiles = Tiles;
        //    return NewBrush;
        //}

        //public UIBrush Copy
        //{
        //    get { return Clone() as UIBrush; }
        //}
        //#endregion ICloneable Implementation

        private UILayer _Tiles;
        public UILayer Tiles
        {
            get { return _Tiles; }
            set { _Tiles = value; NotifyPropertyChanged("Tiles"); }
        }

        private double _TileRows;
        public double TileRows
        {
            get { return _TileRows; }
            set { _TileRows = value; NotifyPropertyChanged("TileRows"); }
        }

        private double _TileColumns;
        public double TileColumns
        {
            get { return _TileColumns; }
            set { _TileColumns = value; NotifyPropertyChanged("TileColumns"); }
        }

        private Size _TileSize;
        public Size TileSize
        {
            get { return _TileSize; }
            set { _TileSize = value; NotifyPropertyChanged("TileSize"); }
        }
    }
}
