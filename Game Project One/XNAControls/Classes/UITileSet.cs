using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace XnaControls.Classes
{
    public class UITileSet : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region Properties
        /// <summary>
        /// A collection of the tiles which belong to this Tileset
        /// </summary>
        private ObservableCollection<UITile> _Tiles;
        public ObservableCollection<UITile> Tiles
        {
            get { return _Tiles; }
            set { _Tiles = value; NotifyPropertyChanged("Tiles"); }
        }

        /// <summary>
        /// The height of the individual tiles in this set
        /// </summary>
        private double _TileHeight;
        public double TileHeight
        {
            get { return _TileHeight; }
            set { _TileHeight = value; NotifyPropertyChanged("TileHeight"); }
        }

        /// <summary>
        /// The width of the individual tiles in this set
        /// </summary>
        private double _TileWidth;
        public double TileWidth
        {
            get { return _TileWidth; }
            set { _TileWidth = value; NotifyPropertyChanged("TileWidth"); }
        }

        /// <summary>
        /// The number of tiles in each row of the tile set
        /// </summary>
        private int _TileCountX;
        public int TileCountX
        {
            get { return _TileCountX; }
            set { _TileCountX = value; NotifyPropertyChanged("TileCountX"); }
        }

        /// <summary>
        /// The number of tiles in each column of the tile set
        /// </summary>
        private int _TileCountY;
        public int TileCountY
        {
            get { return _TileCountY; }
            set { _TileCountY = value; NotifyPropertyChanged("TileCountY"); }
        }

        /// <summary>
        /// The Id for this tileset
        /// </summary>
        private Guid _Id;
        public Guid Id
        {
            get { return _Id; }
        }

        /// <summary>
        /// The Image file used by the tile set
        /// </summary>
        private BitmapImage _Image;
        public BitmapImage Image
        {
            get { return _Image; }
            set { _Image = value; NotifyPropertyChanged("Image"); }
        }

        /// <summary>
        /// Generates a new Id for this tile set. If the tile set already has a 
        /// valid Id, it's existing Id will remain
        /// </summary>
        public void GenerateId()
        {
            while (_Id == null || _Id == Guid.Empty)
                _Id = Guid.NewGuid();
        }
        #endregion Properties

        public void GenerateTiles()
        {
            Tiles = Tiles ?? new ObservableCollection<UITile>();

            for (int j = 0; j < TileCountY; j++)
            {
                for (int i = 0; i < TileCountX; i++)
                {
                    Tiles.Add(new UITile()
                    {
                        TileId = (i * TileCountX) + j,
                        TileSetId = this.Id,
                        Image = this.Image,
                        Scale = 1.0f,
                        TileSize = new Size(TileWidth, TileHeight),
                        ViewBox = new System.Windows.Rect(i * TileWidth, j * TileHeight, TileWidth, TileHeight)
                    });
                }
            }
        }
    }
}
