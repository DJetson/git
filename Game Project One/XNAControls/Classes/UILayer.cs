using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using XnaControls.Controls;
using XnaGameObjects.Classes;

namespace XnaControls.Classes
{
    public class UILayer : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region UILayer Properties
        private Hashtable TileTable = new Hashtable();
        private ObservableCollection<TileControl> _Items = new ObservableCollection<TileControl>();
        public ObservableCollection<TileControl> Items
        {
            get { return _Items; }
            set { _Items = value; NotifyPropertyChanged("Items"); }
        }

        private LayerInfo _LayerInfo;
        public LayerInfo LayerInfo
        {
            get { return _LayerInfo; }
            set { _LayerInfo = value; NotifyPropertyChanged("LayerInfo"); }
        }
        #endregion UILayer Properties

        public void AddTile(UITile NewTile)
        {
            if (IsPositionOccupied(NewTile.Position))
                return;

            TileControl t = new TileControl() { DataContext = NewTile };
            Canvas.SetLeft(t, NewTile.Position.X);
            Canvas.SetTop(t, NewTile.Position.Y);
            TileTable.Add(NewTile.Position, t);
            Items.Add(t);
        }

        public UITile IsCellOccupied(Point MousePosition)
        {
            TileControl tile = (TileControl)TileTable[MousePosition];
    
            if(tile != null)
                return tile.DataContext as UITile;
            else 
                return null;
            //Point GridLocation = new Point((int)(MousePosition.X / LayerInfo.TileSize.Width), (int)(MousePosition.Y / LayerInfo.TileSize.Height));
            //Point TilePosition = new Point(GridLocation.X * LayerInfo.TileSize.Width, GridLocation.Y * LayerInfo.TileSize.Height);
            //foreach (TileControl control in Items)
            //{
            //    UITile tile = control.DataContext as UITile;
            //    if (tile == null)
            //        continue;

            //    if (tile.Position.X == TilePosition.X && tile.Position.Y == TilePosition.Y)
            //        return tile;
            //}
            //return null;
        }

        public TileControl GetCollocatedTileControl(Point MousePosition)
        {
            return (TileControl)TileTable[MousePosition];

            //Point GridLocation = new Point((int)(MousePosition.X / LayerInfo.TileSize.Width), (int)(MousePosition.Y / LayerInfo.TileSize.Height));
            //Point TilePosition = new Point(GridLocation.X * LayerInfo.TileSize.Width, GridLocation.Y * LayerInfo.TileSize.Height);
            //foreach (TileControl control in Items)
            //{
            //    UITile tile = control.DataContext as UITile;
            //    if (tile == null)
            //        continue;

            //    if (tile.Position.X == TilePosition.X && tile.Position.Y == TilePosition.Y)
            //        return control;
            //}
            //return null;
        }

        public void AddTile(TileControl NewTile)
        {
            if (IsPositionOccupied((NewTile.DataContext as UITile).Position))
                return;
            TileTable.Add((NewTile.DataContext as UITile).Position, NewTile);
            Items.Add(NewTile);
        }

        public Boolean IsPositionOccupied(Point Position)
        {
            if (TileTable.ContainsKey(Position))
                return true;
            return false;
        }

        public void RemoveTile(Point Position)
        {
            TileControl ctrl = TileTable[Position] as TileControl;
            if (ctrl == null)
                return;
            Items.Remove(ctrl);
            TileTable.Remove(Position);
        }

        public void RemoveTile(TileControl Remove)
        {
            Items.Remove(TileTable[(Remove.DataContext as UITile).Position] as TileControl);
            TileTable.Remove((Remove.DataContext as UITile).Position);
        }

        public void RemoveTile(UITile Remove)
        {
            //TileControl ctrl = null;
            //foreach (TileControl t in Items)
            //{
            //    if (t.DataContext == Remove)
            //        ctrl = t;
            //}

            //if (ctrl != null)
            Items.Remove(TileTable[Remove.Position] as TileControl);
        }
    }
}
