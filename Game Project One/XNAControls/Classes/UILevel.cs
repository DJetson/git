using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using XnaControls.Controls;

namespace XnaControls.Classes
{

    public enum Layer { PopupForeground, PopupBackground, UIForeground, UIBackground, ViewEffects, Player, ViewForeground, ViewBackground };


    public class UILevel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region UILevel Properties

        private ObservableCollection<UILayer> _Layers;
        public ObservableCollection<UILayer> Layers
        {
            get { return _Layers; }
            set { _Layers = value; NotifyPropertyChanged("Layers"); }
        }

        private Size _LevelSize;
        public Size LevelSize
        {
            get { return _LevelSize; }
            set { _LevelSize = value; NotifyPropertyChanged("LevelSize"); }
        }

        private Size _TileSize;
        public Size TileSize
        {
            get { return _TileSize; }
            set { _TileSize = value; NotifyPropertyChanged("TileSize"); }
        }

        private UILayer _ActiveLayer;
        public UILayer ActiveLayer
        {
            get { return _ActiveLayer; }
            set { _ActiveLayer = value; NotifyPropertyChanged("ActiveLayer"); }
        }

        private Color _BackgroundColor;
        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; NotifyPropertyChanged("BackgroundColor"); }
        }

        private double _Scale;
        public double Scale
        {
            get { return _Scale; }
            set { _Scale = value; NotifyPropertyChanged("Scale"); }
        }

        public double LevelWidth { get { return _TileSize.Width * _LevelSize.Width; } }
        public double LevelHeight { get { return _TileSize.Height * _LevelSize.Height; } }

        #endregion UILevel Properties

        public UILevel()
        {
            GenerateLayers();
        }

        public UILevel(Size tileSize, Size levelSize)
        {
            TileSize = tileSize;
            LevelSize = levelSize;

            GenerateLayers();
        }

        private void GenerateLayers()
        {
            Layers = Layers ?? new ObservableCollection<UILayer>();

            foreach (String s in Enum.GetNames(typeof(Layer)))
            {
                int Depth = (int)Enum.Parse(typeof(Layer), s);
                Layers.Add(new UILayer()
                {
                    LayerInfo = new LayerInfo()
                        {
                            IsLocked = false,
                            IsVisible = true,
                            LayerDepth = Depth,
                            LayerName = s,
                            TileSize = this.TileSize
                        }
                });
            }
        }
    }
}
