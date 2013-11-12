using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XnaControls.Controls;

namespace XnaControls.Classes
{
    public class UITile : INotifyPropertyChanged, ICloneable
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region ICloneable Implementation
        public object Clone()
        {
            UITile NewTile = new UITile();
            NewTile._Image = this.Image;
            NewTile._Scale = this.Scale;
            NewTile._TileId = this.TileId;
            NewTile._TileSetId = this.TileSetId;
            NewTile._TileSize = this.TileSize;
            NewTile._ViewBox = this.ViewBox;
            NewTile._Layer = this.Layer;
            return NewTile;
        }

        public UITile Copy
        {
            get { return Clone() as UITile; }
        }

        #endregion ICloneable Implementation

        #region Properties
        private Point _Position;
        public Point Position
        {
            get { return _Position; }
            set { _Position = value; NotifyPropertyChanged("Position"); }
        }

        private Size _TileSize;
        public Size TileSize
        {
            get { return _TileSize; }
            set { _TileSize = value; NotifyPropertyChanged("TileSize"); }
        }

        private Rect _ViewBox;
        public Rect ViewBox
        {
            get { return _ViewBox; }
            set { _ViewBox = value; NotifyPropertyChanged("Source"); }
        }

        private int _TileId;
        public int TileId
        {
            get { return _TileId; }
            set { _TileId = value; NotifyPropertyChanged("TileId"); }
        }

        private Guid _TileSetId;
        public Guid TileSetId
        {
            get { return _TileSetId; }
            set { _TileSetId = value; NotifyPropertyChanged("TileSetId"); NotifyPropertyChanged("TileSetString"); }
        }

        private double _Scale;
        public double Scale
        {
            get { return _Scale; }
            set { _Scale = value; NotifyPropertyChanged("Scale"); }
        }

        private BitmapImage _Image;
        public BitmapImage Image
        {
            get { return _Image; }
            set { _Image = value; NotifyPropertyChanged("Image"); }
        }

        private LayerInfo _Layer;
        public LayerInfo Layer
        {
            get { return _Layer; }
            set { _Layer = value; NotifyPropertyChanged("Layer"); }
        }

        public String TileSetString
        {
            get { return _TileSetId.ToString(); }
        }
        #endregion Properties

        
    }
}
